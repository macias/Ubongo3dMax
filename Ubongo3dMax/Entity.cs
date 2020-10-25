using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubongo3dMax
{
    internal abstract class Entity
    {
        protected static bool[,,] Parse(IEnumerable<string>[] layers)
        {
            int len_x = layers.SelectMany(it => it).Select(it => it.Length).Max();
            int len_z = layers.Select(it => it.Count()).Max();
            int len_y = layers.Length;

            var data = new bool[len_z, len_y, len_x];
            int y = -1;
            foreach (var layer in layers)
            {
                ++y;
                int z = -1;
                foreach (var line in layer)
                {
                    ++z;
                    int x = -1;
                    foreach (var elem in line)
                    {
                        ++x;
                        if (elem == 'x')
                            data[z, y, x] = true;
                    }

                }
            }

            return data;
        }

        public readonly bool[,,] Data;

        protected int LengthZ => this.Data.GetLength(0);
        protected int LengthY => this.Data.GetLength(1);
        protected int LengthX => this.Data.GetLength(2);

        public bool this[int z, int y, int x]
        {
            get
            {
                if (x >= 0 && y >= 0 && z >= 0 && x < LengthX && y < LengthY && z < LengthZ)
                    return this.Data[z, y, x];
                else
                    return false;
            }
            protected set
            {
                this.Data[z , y , x ] = value;
            }
        }

        protected Entity(bool[,,] data)
        {
            this.Data = data;
        }

    }
}
