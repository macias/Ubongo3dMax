using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubongo3dMax
{
    internal sealed class Repository
    {
        internal IEnumerable<Piece> Pieces { get; }
        public IEnumerable<string> Usage => this.counter
            .SelectMany(it => Enumerable.Range(0, piecePerGame - it.Value).Select(_ => it.Key));

        internal readonly Dictionary<string, int> counter;
        private readonly int piecePerGame;

        public Repository(int piecePerGame, params Piece[] pieces)
        {
            counter = pieces.ToDictionary(it => it.Label, _ => piecePerGame); 
            this.Pieces = pieces.SelectMany(it => it.Rotations()).ToArray();
            this.piecePerGame = piecePerGame;
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