using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace MinecraftAlpha
{
    
    internal class LogicsClass
    {
        public static bool BlockCollide(Vector2 point,Vector2 Position,Vector2 size)
        {
            Vector2 disfc = Position - point;
            if (disfc.X + disfc.Y <= size.X)
            { return true; }
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
