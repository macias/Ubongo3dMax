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

        private readonly IReadOnlyDictionary<(int z, int y, int x), IEnumerable<Piece>> fitters;
        private readonly Piece[,,] configuration;
        private readonly Repository repository;

        private Board(Repository repository, bool[,,] data) : base(data)
        {
            this.Volume = data.Cast<bool>().Count(x => x);
            this.repository = repository;
            this.fitters = Fit();
            this.configuration = new Piece[LengthZ, LengthY, LengthX];
        }

        private bool CanFitAt(Piece piece, int z, int y, int x)
        {
            foreach ((int pz, int py, int px) in piece.Positions)
                if (!this[z + pz, y + py, x + px])
                    return false;

            return true;
        }

        private Dictionary<(int z, int y, int x), IEnumerable<Piece>> Fit()
        {
            var fitters = new Dictionary<(int z, int y, int x), IEnumerable<Piece>>();
            foreach ((int z, int y, int x) in Positions)
            {
                var list = new List<Piece>();
                foreach (Piece piece in repository.Pieces)
                    if (CanFitAt(piece, z, y, x))
                    {
                        list.Add(piece);
                    }

                if (list.Any())
                    fitters.Add((z, y, x), list);
            }

            return fitters;
        }

        private IEnumerable<(Piece piece, int z, int y, int x)> getMoves(int usedZ, int usedY, int usedX)
        {
            foreach ((int z, int y, int x) in this.Positions)
            {
                if (!greaterThan(z, y, x, usedZ, usedY, usedX))
                    continue;
                if (!this.fitters.TryGetValue((z, y, x), out IEnumerable<Piece> pieces))
                    continue;

                foreach (Piece p in pieces)
                    if (p.Volume <= this.Volume
                        && repository.IsAvailable(p)
                        && CanFitAt(p, z, y, x))
                    {
                        yield return (p, z, y, x);
                    }
            }
        }

        private static bool greaterThan(int z, int y, int x, int usedZ, int usedY, int usedX)
        {
            return z > usedZ || (z == usedZ && (y > usedY || (y == usedY && x > usedX)));
        }

        public void Play(List<Snapshot> solutions, int usedZ, int usedY, int usedX)
        {
            foreach ((Piece piece, int z, int y, int x) in getMoves(usedZ, usedY, usedX).ToArray())
            {
                repository.Rent(piece);
                add(piece, z, y, x);
                if (Volume == 0)
                    solutions.Add(takeSnapshot());
                else
                    Play(solutions, z, y, x);
                remove(piece, z, y, x);
                repository.Return(piece);
            }
        }

        public IEnumerable<Snapshot> Play()
        {
            var solutions = new List<Snapshot>();
            Play(solutions, -1, -1, -1);
            return solutions.Distinct();
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
            return new Snapshot((Piece[,,])this.configuration.Clone(),repository.Usage);
        }

        private void set(Piece piece, int z, int y, int x, bool value)
        {
            foreach ((int pz, int py, int px) in piece.Positions)
            {
                this[z + pz, y + py, x + px] = value;
                this.configuration[z + pz, y + py, x + px] = value ? null : piece;
            }
            if (value)
                Volume += piece.Volume;
            else
                Volume -= piece.Volume;
        }
    }

}
