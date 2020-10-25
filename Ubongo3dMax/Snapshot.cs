using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ubongo3dMax
{
    internal sealed class Snapshot
    {
        private sealed class SameLabels : IEqualityComparer<Snapshot>
        {
            public bool Equals(Snapshot x, Snapshot y)
            {
                if (Object.ReferenceEquals(x, y))
                    return true;
                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;

                if (x.LengthZ != y.LengthZ || x.LengthY != y.LengthY || x.LengthX != y.LengthX)
                    return false;

                return x.HasSameLabels(y.labels);
            }

            public int GetHashCode(Snapshot obj)
            {
                return 0;
            }
        }

        private sealed class LabelOrder : IComparer<Snapshot>
        {
            public int Compare(Snapshot x, Snapshot y)
            {
                return String.Join("", x.labels).CompareTo(String.Join("", y.labels));
            }
        }

        public static IEqualityComparer<Snapshot> SameLabelsComparer { get; } = new SameLabels();
        public static IComparer<Snapshot> LabelOrderComparer { get; } = new LabelOrder();

        private readonly Piece[,,] data;

        public IReadOnlyList<Piece> Pieces { get; }

        private readonly IEnumerable<string> labels;

        public bool IsSeparable { get; }

        public int LengthZ => this.data.GetLength(0);
        public int LengthY => this.data.GetLength(1);
        public int LengthX => this.data.GetLength(2);

        public Snapshot(Piece[,,] data)
        {
            this.data = data;
            this.Pieces = data.Cast<Piece>()
                .Distinct()
                // we have empty spaces in the array
                .Where(p => p != null)
                .ToList();
            this.labels = getLabels(Pieces).ToArray();

            this.IsSeparable = zSeparable() || ySeparable() || xSeparable();
        }

        private static IOrderedEnumerable<string> getLabels(IEnumerable<Piece> pieces)
        {
            return pieces.Select(it => it.Label).OrderBy(s => s);
        }

        private bool zSeparable()
        {
            for (int z = 1; z < LengthZ; ++z)
            {
                for (int y = 0; y < LengthY; ++y)
                    for (int x = 0; x < LengthX; ++x)
                    {
                        if (data[z - 1, y, x] == data[z, y, x])
                            goto next_layer;
                    }

                return true;

            next_layer: { }
            }

            return false;
        }

        private bool ySeparable()
        {
            for (int y = 1; y < LengthY; ++y)
            {
                for (int z = 0; z < LengthZ; ++z)
                    for (int x = 0; x < LengthX; ++x)
                    {
                        if (data[z, y - 1, x] == data[z, y, x])
                            goto next_layer;
                    }

                return true;

            next_layer: { }
            }

            return false;
        }

        private bool xSeparable()
        {
            for (int x = 1; x < LengthX; ++x)
            {
                for (int z = 0; z < LengthZ; ++z)
                    for (int y = 0; y < LengthY; ++y)
                    {
                        if (data[z, y, x - 1] == data[z, y, x])
                            goto next_layer;
                    }

                return true;

            next_layer: { }
            }

            return false;
        }

        public void Print(TextWriter writer, bool solution = false)
        {
            if (solution)
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

            writer.WriteLine(string.Join(", ", labels.GroupBy(s => s).Select(it =>
            {
                int c = it.Count();
                return (c == 1 ? "" : $"{c} ") + it.Key;
            })));
        }

        public bool HasSameLabels(IEnumerable<string> other)
        {
            return this.labels.SequenceEqual(other);
        }

        internal IEnumerable<string> ExchangeLabels(Piece piece, Snapshot compounds)
        {
            return getLabels(this.Pieces.Where(it => it != piece).Concat(compounds.Pieces));
        }

        /*private bool isIdentical(Snapshot other)
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
        }*/
    }
}
