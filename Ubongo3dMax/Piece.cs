﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubongo3dMax
{
    internal sealed class Piece : Entity
    {
        private readonly byte id;
        private readonly byte rotZ;
        private readonly byte rotY;
        private readonly byte rotX;

        public IEnumerable<(int z, int y, int x)> Positions
        {
            get
            {
                for (int z = 0; z < LengthZ; ++z)
                    for (int y = 0; y < LengthY; ++y)
                        for (int x = 0; x < LengthX; ++x)
                            if (this.Data[z, y, x])
                                yield return (z, y, x);

            }
        }

        // do NOT use it as hash code, it is pretty legal that piece with high rotational symmetry will get
        // different indices while being the same form which would lead to incorrect comparison via GetHashCode
        public int Index { get; }
        public string Label { get; }

        public int Volume { get; }

        public static Piece Parse(string label, byte id, byte rotZ, byte rotY, byte rotX, params IEnumerable<string>[] layers)
        {
            return new Piece(label, id, rotZ, rotY, rotX, Parse(layers));
        }

        private Piece(string label, byte id, byte rotZ, byte rotY, byte rotX, bool[,,] data) : base(data)
        {
            this.Volume = data.Cast<bool>().Count(x => x);
            this.Label = label;
            this.id = id;
            this.rotZ = (byte)(rotZ % 4);
            this.rotY = (byte)(rotY % 4);
            this.rotX = (byte)(rotX % 4);
            this.Index = ((int)id) << 8 | ((int)rotZ) << 4 | ((int)rotY) << 2 | ((int)rotX);
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
                return new Piece(Label, this.id, (byte)(rotZ + 1), rotY, rotX, output);
            }
            else if (yAxis)
            {
                var output = new bool[LengthX, LengthY, LengthZ];
                foreach ((int z, int y, int x) in Positions)
                    output[x, y, LengthZ - 1 - z] = Data[z, y, x];
                return new Piece(Label, id, rotZ, (byte)(rotY + 1), rotX, output);
            }
            else
            {
                var output = new bool[LengthY, LengthZ, LengthX];
                foreach ((int z, int y, int x) in Positions)
                    output[LengthY - 1 - y, z, x] = Data[z, y, x];
                return new Piece(Label, id, rotZ, rotY, (byte)(rotX + 1), output);
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
