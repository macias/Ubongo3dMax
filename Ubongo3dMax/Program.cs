using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ubongo3dMax
{
    class Program
    {
        static void Main(string[] args)
        {
            var gS = Piece.Parse("gS", new[] {
                "xx",
                "x"
            });
            var gB = Piece.Parse("gB", new[] {
                "xx",
                " x",
                " x"
            }, new[] {
                "x",
            });
            var bL = Piece.Parse("bL", new[] {
                "xx",
                "x",
                "x"
            });
            var bZ = Piece.Parse("bZ", new[] {
                "x",
                "xx",
                " x"
            }, new[] {
                "x",
            });
            var rS = Piece.Parse("rS", new[] {
                "xx",
            });
            var rT = Piece.Parse("rT", new[] {
                "xx",
                " x"
            }, new[] {
                "x",
            });
            var yT = Piece.Parse("yT", new[] {
                "x",
                "xx"
            }, new[] {
                "x",
            });
            var yO = Piece.Parse("yO", new[] {
                "xx",
                "x",
                "x"
            }, new[] {
                " ",
                "x",
            });

            var layouts1 = new[] {
                new[]
                {
                    "x",
                "xxxxx",
                "   x "
                },
            };

            var repository = new Repository(piecePerGame: 1, gB, gS, bL, bZ, rS, rT, yT, yO);
            var board = Board.Parse(repository, height: 2, layouts1.First());

            long start = Stopwatch.GetTimestamp();
            IEnumerable<Snapshot> solutions = board.Play().ToArray();
            foreach (Snapshot snap in solutions)
                snap.Print(Console.Out);

            Console.WriteLine();
            Console.WriteLine($"Found {solutions.Count()} in {TimeSpan.FromSeconds((Stopwatch.GetTimestamp() - start - 0.0) / Stopwatch.Frequency)}");
            Console.ReadLine();
        }
    }
}
