using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubongo3dMax
{
    internal sealed class Piece : Entity
    {
        public string Label { get; }

        public int Volume { get; }

        public static Piece Parse(string label, params IEnumerable<string>[] layers)
        {
            return new Piece(label, Parse(layers));
        }

        private Piece(string label, bool[,,] data) : base(data)
        {
            this.Volume = data.Cast<bool>().Count(x => x);
            this.Label = label;
        }

        public IEnumerable<Piece> Rotations()
        {
            IEnumerable<Piece> rotations(Piece piece, bool zAxis, bool yAxis, bool xAxis)
            {
                for (int i = 0; i < 4; ++i)
                {
                    piece = piece.Rotated(zAxis, yAxis, xAxis);
                    yield return piece;
                }
            }

            // I bet this is buggy (i.e. not optimized) for sure, because Equals does not check if the piece 
            // was shifted because of rotation. As the result we (can) get some duplicates
            return rotations(this, true, false, false)
                .SelectMany(p => rotations(p, false, true, false))
                .SelectMany(p => rotations(p, false, false, true))
                .Distinct();
        }

        // z-axis goes through the surface of the monitor
        private Piece Rotated(bool zAxis, bool yAxis, bool xAxis)
        {
            if ((zAxis ? 1 : 0) + (yAxis ? 1 : 0) + (xAxis ? 1 : 0) != 1)
                throw new ArgumentException();

            if (zAxis)
            {
                var output = new bool[LengthZ, LengthX, LengthY];
                foreach ((int z, int y, int x) in Positions)
                    output[z, LengthX - 1 - x, y] = Data[z, y, x];
                return new Piece(Label, output);
            }
            else if (yAxis)
            {
                var output = new bool[LengthX, LengthY, LengthZ];
                foreach ((int z, int y, int x) in Positions)
                    output[LengthX - 1 - x, y, z] = Data[z, y, x];
                return new Piece(Label, output);
            }
            else
            {
                var output = new bool[LengthY, LengthZ, LengthX];
                foreach ((int z, int y, int x) in Positions)
                    output[LengthY - 1 - y, z, x] = Data[z, y, x];
                return new Piece(Label, output);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Piece);
        }

        public bool Equals(Piece other)
        {
            if (Object.ReferenceEquals(other, this))
                return true;
            if (Object.ReferenceEquals(other, null))
                return false;

            return Enumerable.SequenceEqual(this.Positions, other.Positions);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return this.Label;
        }
    }
}
