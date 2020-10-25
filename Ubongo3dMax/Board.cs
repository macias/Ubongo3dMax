using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubongo3dMax
{
    internal sealed class Board : Entity
    {
        public int Volume { get; private set; }

        public static Board Parse(Repository repository, int height, params string[] lines)
        {
            return new Board(repository, Parse(Enumerable.Range(0, height).Select(_ => lines).ToArray()));
        }

        private readonly IEnumerable<(int z, int y, int x, IEnumerable<Piece> pieces)> fitters;
        private readonly Piece[,,] configuration;
        private readonly Repository repository;

        public Board(Repository repository, bool[,,] data) : base(data)
        {
            this.Volume = data.Cast<bool>().Count(x => x);
            this.repository = repository;
            this.fitters = initialFitting();
            this.configuration = new Piece[LengthZ, LengthY, LengthX];
        }

        private bool canFitAt(Piece piece, int z, int y, int x)
        {
            foreach ((int pz, int py, int px) in piece.Positions)
                if (!this[z + pz, y + py, x + px])
                    return false;

            return true;
        }


        private IEnumerable<(int z, int y, int x, IEnumerable<Piece> pieces)> initialFitting()
        {
            var fitters = new List<(int z, int y, int x, IEnumerable<Piece> pieces)>();
            // we have to extend anchor positions because (0,0,0) of the board is not the same as (0,0,0)
            // of the piece and besides piece can be rotated such way that its (0,0,0) falls into empty space anyway
            for (int z = -LengthZ + 1; z < LengthZ * 2 - 1; ++z)
                for (int y = -LengthY + 1; y < LengthY * 2 - 1; ++y)
                    for (int x = -LengthX + 1; x < LengthX * 2 - 1; ++x)
                    {
                        var list = new List<Piece>();
                        foreach (Piece piece in repository.Pieces)
                            if (canFitAt(piece, z, y, x))
                            {
                                list.Add(piece);
                            }

                        if (list.Any())
                            fitters.Add((z, y, x, list));
                    }

            return fitters;
        }

        private IEnumerable<(Piece piece, int z, int y, int x)> getMoves(int usedZ, int usedY, int usedX, Piece usedPiece)
        {
            foreach ((int z, int y, int x, IEnumerable<Piece> pieces) in this.fitters)
            {
                // since time/order in ubongo does not matter every following "move" has to hit coordinates
                // "greater" than previous move used. Otherwise we would check given solutions multiple times
                // "equal" part is tricky here -- in theory we could put multiple pieces at the same position
                // because of their current rotation
                int pos_comparison = compare(z, y, x, usedZ, usedY, usedX);
                // if current position is not greater or equal to used --> continue
                if (pos_comparison < 0)
                    continue;

                foreach (Piece p in pieces)
                    if (p.Volume <= this.Volume
                        // piece index is just the fourth coordinate
                        && (pos_comparison > 0 || p.Index > (usedPiece?.Index ?? -1))
                        && repository.IsAvailable(p)
                        // we have to check if we can fit at this particular time, because some space might be
                        // taken already by some other piece
                        && canFitAt(p, z, y, x))
                    {
                        yield return (p, z, y, x);
                    }
            }
        }

        private static int compare(int z, int y, int x, int usedZ, int usedY, int usedX)
        {
            int result;
            result = z.CompareTo(usedZ);
            if (result != 0)
                return result;
            result = y.CompareTo(usedY);
            if (result != 0)
                return result;
            result = x.CompareTo(usedX);
            return result;
        }

        private List<Snapshot> solve(List<Snapshot> solutions, int usedZ, int usedY, int usedX, Piece usedPiece)
        {
            foreach ((Piece template, int z, int y, int x) in getMoves(usedZ, usedY, usedX, usedPiece).ToArray())
            {
                Piece piece = repository.Rent(template);
                add(piece, z, y, x);
                if (Volume == 0)
                    solutions.Add(takeSnapshot());
                else
                    solve(solutions, z, y, x, piece);
                remove(piece, z, y, x);
                repository.Return(piece);
            }

            return solutions;
        }

        public IEnumerable<Snapshot> Solve(bool allowSeparable)
        {
            // do NOT use -1, because such small values can be offseted by piece size and can turn out to be legal
            var solutions = solve(new List<Snapshot>(), int.MinValue, int.MinValue, int.MinValue, null)
                .GroupBy(sln => sln, Snapshot.SameLabelsComparer)
                // if the given solution consists in fact of two smaller ones there is nothing wrong with it
                // buch such setups are less interesting
                .Where(grp => allowSeparable || grp.All(sln => !sln.IsSeparable))
                .Select(grp => (sln: grp.Key, nonAtomic: true))
                .ToList();

            // and one more step to eliminate boring solutions -- let's say you have one solution which involves
            // piece L (blue one) and another pretty same, but instead of "L" it contains two small reds (-)
            // here we break down pieces to their atomic equivalences and if we found such solutions we
            // remove the ones constructed with more atomic pieces (please note we don't rely on simply
            // counting, because small reds can be used in "legimate" scenarios, when they are for example separated) 

            // hint: A can invalidates B, and solution B can invalidates C, so don't remove B too quickly
            foreach (Snapshot current_solution in solutions.Select(it => it.sln).ToArray())
            {
                foreach (Piece piece in current_solution.Pieces.Where(p => p.Compounds.Any()))
                {
                    foreach (Snapshot compounds in piece.Compounds)
                    {
                        IEnumerable<string> alt_labels = current_solution.ExchangeLabels(piece, compounds).ToArray();
                        for (int i = 0; i < solutions.Count; ++i)
                        {
                            if (solutions[i].sln.HasSameLabels(alt_labels))
                                solutions[i] = (solutions[i].sln, false);
                        }
                    }
                }
            }

            return solutions
                .Where(it => it.nonAtomic)
                .Select(it => it.sln)
                .OrderBy(it => it, Snapshot.LabelOrderComparer);
        }


        private void add(Piece piece, int z, int y, int x)
        {
            set(piece, z, y, x, false);
        }

        private void remove(Piece piece, int z, int y, int x)
        {
            set(piece, z, y, x, true);
        }

        private Snapshot takeSnapshot()
        {
            return new Snapshot((Piece[,,])this.configuration.Clone());
        }

        private void set(Piece piece, int z, int y, int x, bool freeingSpace)
        {
            foreach ((int pz, int py, int px) in piece.Positions)
            {
                this[z + pz, y + py, x + px] = freeingSpace;
                this.configuration[z + pz, y + py, x + px] = freeingSpace ? null : piece;
            }

            if (freeingSpace)
                Volume += piece.Volume;
            else
                Volume -= piece.Volume;
        }
    }

}
