using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubongo3dMax
{
    internal sealed class Repository
    {
        internal IEnumerable<Piece> Pieces { get; }
        public IEnumerable<string> Usage => this.counter
            .SelectMany(it => Enumerable.Range(0, piecesPerGame - it.Value).Select(_ => it.Key));

        internal readonly Dictionary<string, int> counter;
        private readonly int piecesPerGame;

        public Repository(int piecesPerGame, IReadOnlyList<Piece> pieces)
        {
            counter = pieces.ToDictionary(it => it.Label, _ => piecesPerGame); 
            this.Pieces = pieces.SelectMany(it => it.Rotations()).ToArray();
            this.piecesPerGame = piecesPerGame;
        }

        internal bool IsAvailable(Piece piece)
        {
            return counter[piece.Label] > 0;
        }

        internal void Rent(Piece piece)
        {
            --counter[piece.Label];
        }

        internal void Return(Piece piece)
        {
            ++counter[piece.Label];
        }
    }
}