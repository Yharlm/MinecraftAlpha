using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace MinecraftAlpha
{
    internal class LogicsClass
    {
        public static bool BlockCollide(Entity target, TileGrid[,] Chunk)
        {
            for (int i = 0;i < Chunk.Length;i++)
            {
                for(int j = 0;j < Chunk.GetLength(1);j++)
                {
                    var grid = Chunk[i, j];
                    if (grid.ID != 0)
                    {
                        Vector2 blockPos = new Vector2(j * 32, i * 32)/2;
                        Vector2 blockSize = new Vector2(32, 32);
                        if (IsInBounds(new Vector2(target.position.X, target.position.Y), blockPos))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static bool IsInBounds(Vector2 Pos, Vector2 size)
        {

            if (Pos.X >= size.X && Pos.X <= size.X + size.X)
                if (Pos.Y > size.Y && Pos.Y <= size.Y + size.Y)
                {
                    return true;
                    //Place Design shit here
                }
            return false;
        }
    }
}
