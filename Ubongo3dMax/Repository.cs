using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubongo3dMax
{
    internal sealed class Repository
    {
        internal IEnumerable<Piece> Pieces { get; }

        internal readonly Dictionary<string, int> counter;
        private readonly int piecesPerGame;

        private Repository(int piecesPerGame, IEnumerable<Piece> pieces, object _)
        {
            this.piecesPerGame = piecesPerGame;
            this.Pieces = pieces.ToArray();
            this.counter = this.Pieces.Select(it => it.Label).Distinct().ToDictionary(s => s, s => piecesPerGame);
        }

        // we check if given piece can be divided into two parts (smaller pieces)
        public Repository(Repository source, string excludedLabel) : this(2,
            source.Pieces.Where(it => it.Label != excludedLabel),
            default)
        {
        }

        public Repository(int piecesPerGame, IEnumerable<Piece> pieces) : this(piecesPerGame,
            pieces.SelectMany(it => it.Rotations()),
            default)
        {
        }

        internal bool IsAvailable(Piece piece)
        {
            return counter[piece.Label] > 0;
        }

        internal Piece Rent(Piece piece)
        {
            --counter[piece.Label];
            return piece.Clone();
        }

        internal void Return(Piece piece)
        {
            ++counter[piece.Label];
        }
    }
}