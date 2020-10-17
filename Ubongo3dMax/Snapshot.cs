using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ubongo3dMax
{
    internal sealed class Snapshot
    {
        private readonly Piece[,,] data;
        private readonly IEnumerable<string> labels;

        public int LengthZ => this.data.GetLength(0);
        public int LengthY => this.data.GetLength(1);
        public int LengthX => this.data.GetLength(2);

        public Snapshot(Piece[,,] data, IEnumerable<string> labels)
        {
            this.data = data;
            this.labels = labels.OrderBy(s => s).ToArray();
        }

        public void Print(TextWriter writer)
        {
            if (false) // prints solution
            {
                for (int y = 0; y < LengthY; ++y)
                {
                    for (int z = 0; z < LengthZ; ++z)
                    {
                        for (int x = 0; x < LengthX; ++x)
                        {
                            Piece piece = data[z, y, x];
                            if (piece != null)
                                writer.Write($"{piece.Label} ");
                            else
                                writer.Write($"   ");
                        }
                        writer.WriteLine();
                    }
                    writer.WriteLine();
                }

                writer.WriteLine(new string('-', LengthX));
            }

            writer.WriteLine(String.Join(", ", labels.GroupBy(s => s).Select(it => $"{it.Key} {it.Count()}")));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Snapshot);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public bool Equals(Snapshot other)
        {
            if (Object.ReferenceEquals(other, this))
                return true;
            if (Object.ReferenceEquals(other, null))
                return false;

            if (LengthZ != other.LengthZ || LengthY != other.LengthY || LengthX != other.LengthX)
                return false;

            //return isIdentical(other);
            return samePieces(other);
        }

        private bool isIdentical(Snapshot other)
        {
            for (int z = 0; z < LengthZ; ++z)
                for (int y = 0; y < LengthY; ++y)
                    for (int x = 0; x < LengthX; ++x)
                    {
                        Piece this_piece = data[z, y, x];
                        Piece other_piece = other.data[z, y, x];
                        if (this_piece?.Label != other_piece?.Label)
                            return false;
                    }

            return true;
        }

        private bool samePieces(Snapshot other)
        {
            return this.labels.SequenceEqual(other.labels);
        }
    }
}
