using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ubongo3dMax
{
    class Program
    {
        private const int PIECES_PER_GAME = 2;
        private const int BOARD_HEIGHT = 3;

        static void Main(string[] args)
        {
            var layouts1 = getLayouts1();
            var layouts2 = getLayouts2();
            var layouts3 = getLayouts3();
            var layouts4 = getLayouts4();

            var repository = new Repository(PIECES_PER_GAME, buildPieces()
                .ToArray());
            var board = Board.Parse(repository, BOARD_HEIGHT, layouts1[0]);

            long start = Stopwatch.GetTimestamp();
            IEnumerable<Snapshot> solutions = board.Play().ToArray();
            System.IO.TextWriter writer = Console.Out;
            int c = 0;
            int count = solutions.Count();
            foreach (Snapshot snap in solutions)
            {
                ++c; // make it human-friendly
                writer.Write(c.ToString().PadLeft(count.ToString().Length) + ". ");
                snap.Print(writer);
            }

            Console.WriteLine();
            Console.WriteLine($"Found {count} in {TimeSpan.FromSeconds((Stopwatch.GetTimestamp() - start - 0.0) / Stopwatch.Frequency)}");
        }

        private static IEnumerable<Piece> buildPieces()
        {
            byte id = 0;
            yield return Piece.Parse("gS", id++, 0, 0, 0, new[] {
                "xx",
                "x"
            });
            yield return Piece.Parse("gB", id++, 0, 0, 0, new[] {
                "xx",
                " x",
                " x"
            }, new[] {
                "x",
            });
            yield return Piece.Parse("bL", id++, 0, 0, 0, new[] {
                "xx",
                "x",
                "x"
            });
            yield return Piece.Parse("bZ", id++, 0, 0, 0, new[] {
                "x",
                "xx",
                " x"
            }, new[] {
                "x",
            });
            yield return Piece.Parse("rS", id++, 0, 0, 0, new[] {
                "xx",
            });
            yield return Piece.Parse("rT", id++, 0, 0, 0, new[] {
                "xx",
                " x"
            }, new[] {
                "x",
            });
            yield return Piece.Parse("yT", id++, 0, 0, 0, new[] {
                "x",
                "xx"
            }, new[] {
                "x",
            });
            yield return Piece.Parse("yF", id++, 0, 0, 0, new[] {
                "xx",
                "x",
                "x"
            }, new[] {
                " ",
                "x",
            });
        }

        private static string[][] getLayouts3()
        {
            return new[] {
                new[]
                {
                    "    x",
                    "x xxx",
                    "xxx",
                },
                new[]
                {
                    "   x",
                    " xxxx",
                    "xx",
                    " x"
                },
                new[]
                {
                    "    x",
                    " xxxx",
                    "xx  x",
                },
                new[]
                {
                    "xx",
                    " x",
                    " xxxx",
                    "   x"
                },
                new[]
                {
                    "x x",
                    "xxx",
                    "x xx",
                },
                new[]
                {
                    " xxx",
                    " x x",
                    "xx",
                    " x"
                },
                new[]
                {
                    "  x",
                    "xxx",
                    "x xx",
                    "   x"
                },
                new[]
                {
                    "xxx",
                    "x xxx",
                    "   x",
                },
                new[]
                {
                    "   xx",
                    " xxx",
                    "xx",
                    " x"
                },
                new[]
                {
                    "xxxx",
                    "x  x",
                    "  xx"
                },
                new[]
                {
                    "xxx",
                    "x x",
                    " xxx",
                },
                new[]
                {
                    "  x",
                    "  xxx",
                    "  x",
                    "xxx"
                },
                new[]
                {
                    "  x x",
                    "xxxxx",
                    "    x",
                },
                new[]
                {
                    " xxxx",
                    "xx x",
                    "   x",
                },
                new[]
                {
                    "   x",
                    "xxxxx",
                    "x",
                    "x"
                },
                new[]
                {
                    " x",
                    "xxx",
                    "x xx",
                    "  x"
                },
                new[]
                {
                    "x  xx",
                    "xxxx",
                    "  x",
                },
                new[]
                {
                    "   x",
                    " xxx",
                    "xx",
                    " xx"
                },
                new[]
                {
                    "  x",
                    "x x",
                    "xxxx",
                    " x"
                },
                new[]
                {
                    "xx",
                    " x",
                    " xxx",
                    "   xx"
                },
                new[]
                {
                    "  xx",
                    "x x",
                    "xxx",
                    "x"
                },
                new[]
                {
                    "x",
                    "xxx",
                    "x xx",
                    "  x"
                },
                new[]
                {
                    " xxx",
                    "xx x",
                    "   xx",
                },
                new[]
                {
                    " xx",
                    " x",
                    "xxxx",
                    "  x"
                },
                new[]
                {
                    " x",
                    "xx xx",
                    " xxx",
                },
                new[]
                {
                    " x",
                    "xxxxx",
                    "   x",
                    "   x"
                },
                new[]
                {
                    "xx",
                    "x",
                    "xxx",
                    "  xx"
                },
                new[]
                {
                    "xx  x",
                    " xxxx",
                    "   x",
                },
                new[]
                {
                    "  xxx",
                    "  x",
                    "xxx",
                    " x"
                },
                new[]
                {
                    " x x",
                    "xxxx",
                    "x  x",
                },
                new[]
                {
                    "x x",
                    "xxx",
                    "  xx",
                    "   x"
                },
                new[]
                {
                    " x",
                    "xxxx",
                    "  x",
                    "  xx"
                },
                new[]
                {
                    "xx x",
                    " xxxx",
                    " x",
                },
                new[]
                {
                    "  x",
                    "xxx",
                    "  xxx",
                    "   x"
                },
                new[]
                {
                    "x",
                    "xx",
                    " xxxx",
                    " x"
                },
                new[]
                {
                    "    x",
                    "  xxx",
                    "  x",
                    "xxx"
                },
            };
        }

        private static string[][] getLayouts4()
        {
            return new[] {
                new[]
                {
                    "x",
                    "x",
                    "xxx",
                    " xxx"
                },
                new[]
                {
                    "  xxx",
                    "xxxx",
                    " x",
                },
                new[]
                {
                    "xxxx",
                    "x xx",
                    "   x",
                },
                new[]
                {
                    "  x",
                    "xxxx",
                    "  xx",
                    "   x"
                },
                new[]
                {
                    "xxxx",
                    "x xx",
                    "  x",
                },
                new[]
                {
                    " xx",
                    "xx",
                    "xxx",
                    "x"
                },
                new[]
                {
                    "  x",
                    " xx",
                    "xxxx",
                    " x"
                },
                new[]
                {
                    "  x",
                    "  x",
                    "xxxx",
                    " xx"
                },
                new[]
                {
                    "   x",
                    "xxxxx",
                    "  xx",
                },
                new[]
                {
                    " xx",
                    "xxxx",
                    " x x",
                },
                new[]
                {
                    "  xx",
                    "xxxxx",
                    "  x",
                },
                new[]
                {
                    " x",
                    " xx",
                    "xxxx",
                    " x"
                },
                new[]
                {
                    "  xx",
                    "xxxx",
                    "x  x",
                },
                new[]
                {
                    "  xx",
                    "xxxx",
                    " x x",
                },
                new[]
                {
                    "   x",
                    "  xx",
                    "xxxx",
                    "   x"
                },
                new[]
                {
                    "  x",
                    " xx",
                    "xxx",
                    "x x"
                },
                new[]
                {
                    "  x",
                    " xx",
                    "xx",
                    "xxx"
                },
                new[]
                {
                    "xxx",
                    " xxxx",
                    "    x",
                },
                new[]
                {
                    "  xxx",
                    "xxxx",
                    "  x",
                },
                new[]
                {
                    "   x",
                    "xxxx",
                    "  xxx",
                },
                new[]
                {
                    "   x",
                    "xxxx",
                    "x xx",
                },
                new[]
                {
                    " xxxx",
                    "xxx x",
                },
                new[]
                {
                    " xx",
                    "xxx",
                    " x",
                    " xx"
                },
                new[]
                {
                    " xx x",
                    "xxxxx",
                },
                new[]
                {
                    "x",
                    "xxxx",
                    "  xx",
                    "  x"
                },
                new[]
                {
                    "xx",
                    " x",
                    " xx",
                    " xxx"
                },
                new[]
                {
                    "x",
                    "xxxxx",
                    "  xx",
                },
                new[]
                {
                    "x x",
                    "xxxx",
                    "  xx",
                },
                new[]
                {
                    " x",
                    "xx",
                    " xxx",
                    " xx"
                },
                new[]
                {
                    "x",
                    "xxxx",
                    "xx",
                    " x"
                },
                new[]
                {
                    "  xx",
                    "xxx",
                    " xx",
                    " x"
                },
                new[]
                {
                    "xxxx",
                    "  xxx",
                    "   x",
                },
                new[]
                {
                    "x",
                    "xxx",
                    " xx",
                    "xx"
                },
                new[]
                {
                    "xx",
                    "xxxx",
                    "   xx",
                },
                new[]
                {
                    " xx",
                    "  x",
                    "xxx",
                    "xx"
                },
                new[]
                {
                    "xx xx",
                    " xxxx",
                },
            };
        }

        private static string[][] getLayouts2()
        {
            return new[] {
                new[]
                {
                    "x",
                    "xxxxx",
                    "   x "
                },
                new[]
                {
                    "  xx",
                    "  x",
                    "  x",
                    "xxx"
                },
                new[]
                {
                    "  x",
                    "x xx",
                    "xxx"
                },
                new[]
                {
                    "xxxx",
                    "x  xx"
                },
                new[]
                {
                    "x  x",
                    "xxxx",
                    " x"
                },
                new[]
                {
                    "   x",
                    "xxxx",
                    "x x"
                },
                new[]
                {
                    " xx",
                    "xx",
                    " xx",
                    " x"
                },
                new[]
                {
                    " x",
                    " x x",
                    "xxxx"
                },
                new[]
                {
                    "  x x",
                    "xxxxx"
                },
                new[]
                {
                    "  xx",
                    "xxx",
                    "x",
                    "x"
                },
                new[]
                {
                    "x",
                    "xxx",
                    "  xxx"
                },
                new[]
                {
                    "   x",
                    "xxxx",
                    "  x",
                    "  x"
                },
                new[]
                {
                    " x",
                    "xxxxx",
                    "  x"
                },
                new[]
                {
                    " xx",
                    "xx",
                    "x",
                    "xx"
                },
                new[]
                {
                    "    x",
                    "xxxxx",
                    "  x"
                },
                new[]
                {
                    " x x",
                    "xxxx",
                    " x"
                },
                new[]
                {
                    "  x",
                    " xxx",
                    "xx x"
                },
                new[]
                {
                    " xxx",
                    "xx x",
                    "   x"
                },
                new[]
                {
                    "xxxx",
                    "x  x",
                    "   x"
                },
                new[]
                {
                    "xxx",
                    " x",
                    "xx",
                    " x"
                },
                new[]
                {
                    "  x",
                    "x x",
                    "xxx",
                    "  x"
                },
                new[]
                {
                    "xx",
                    " xx",
                    "  xx",
                    "  x"
                },
                new[]
                {
                    "  xx",
                    "xxx",
                    "x x",
                },
                new[]
                {
                    " x",
                    " x",
                    "xxx",
                    "  xx"
                },
                new[]
                {
                    " x",
                    " xxx",
                    "xx",
                    " x"
                },
                new[]
                {
                    "xx",
                    " xxxx",
                    "   x",
                },
                new[]
                {
                    "x  x",
                    "xxxxx",
                },
                new[]
                {
                    " xxxx",
                    "xx x",
                },
                new[]
                {
                    "   x",
                    "xxxx",
                    " x",
                    " x"
                },
                new[]
                {
                    "  x",
                    " xxxx",
                    "xx",
                },
                new[]
                {
                    "x",
                    "xx",
                    "x",
                    "xxx"
                },
                new[]
                {
                    "xxx",
                    "  xx",
                    "  x",
                    "  x"
                },
                new[]
                {
                    "   x",
                    "  xxx",
                    "xxx",
                },
                new[]
                {
                    "  x",
                    "xxxx",
                    "x",
                    "x"
                },
                new[]
                {
                    "xxx",
                    "x xxx",
                },
                new[]
                {
                    "    x",
                    " xxxx",
                    "xx",
                },
            };
        }

        private static string[][] getLayouts1()
        {
            return new[] {
                new[]
                {
                    " x",
                    "xxxxx",
                },
                new[]
                {
                    " x",
                    "xx",
                    "xx",
                    "x"
                },
                new[]
                {
                    " xx",
                    "xxxx",
                },
                new[]
                {
                    "xxxx",
                    "x",
                    "x",
                },
                new[]
                {
                    " xx",
                    " x ",
                    "xxx",
                },
                new[]
                {
                    "  x",
                    "  x",
                    "xxx",
                    " x"
                },
                new[]
                {
                    "  xx",
                    "xxxx",
                },
                new[]
                {
                    "xx",
                    " xxx",
                    "   x",
                },
                new[]
                {
                    "x",
                    "x x",
                    "xxx",
                },
                new[]
                {
                    "xx",
                    "xx",
                    "xx",
                },
                new[]
                {
                    "x",
                    "xx",
                    "x",
                    "xx"
                },
                new[]
                {
                    " x",
                    "xxx",
                    "x x",
                },
                new[]
                {
                    " xxx",
                    "xx x",
                },
                new[]
                {
                    " xx",
                    "xx",
                    " x",
                    " x"
                },
                new[]
                {
                    "  x",
                    "xxxx",
                    "x",
                },
                new[]
                {
                    " x",
                    "xx",
                    " xx",
                    " x"
                },
                new[]
                {
                    " x",
                    " x",
                    "xxx",
                    "x"
                },
                new[]
                {
                    "xxxx",
                    " x x",
                },
                new[]
                {
                    "x",
                    "xxx",
                    "x",
                    "x"
                },
                new[]
                {
                    " x",
                    "xxxx",
                    " x",
                },
                new[]
                {
                    "xx",
                    "xx",
                    "x",
                    "x"
                },
                new[]
                {
                    " xxx",
                    " x",
                    "xx",
                },
                new[]
                {
                    " x",
                    "xx",
                    "xxx",
                },
                new[]
                {
                    " x",
                    "xxx",
                    "  xx",
                },
                new[]
                {
                    " xx",
                    "xxx",
                    "x",
                },
                new[]
                {
                    "xx",
                    "xxx",
                    " x",
                },
                new[]
                {
                    " x",
                    " xxx",
                    "xx",
                },
                new[]
                {
                    "xx",
                    "xx",
                    " x",
                },
                new[]
                {
                    "  x",
                    "  x",
                    "xxx",
                    "x"
                },
                new[]
                {
                    "x",
                    "xx",
                    " xx",
                    " x"
                },
                new[]
                {
                    "xxx",
                    "xx",
                    "x",
                },
                new[]
                {
                    "x",
                    "xxx",
                    "xx",
                },
                new[]
                {
                    "x",
                    "xx",
                    " x",
                    " xx"
                },
                new[]
                {
                    " xx",
                    "xx",
                    "xx",
                },
                new[]
                {
                    "xx",
                    " x",
                    "xx",
                    "x"
                },
                new[]
                {
                    " xx",
                    " x",
                    "xx",
                    " x"
                },
            };
        }
    }
}
