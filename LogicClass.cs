using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace MinecraftAlpha
{

    public class Debuging
    {
        public static void DrawBlockLayout(TileGrid Tile,int Position)
        {

        }

        public static void DebugPos(SpriteBatch sb, Vector2 pos, Game1 game)
        {
            if (!game.DebugMode) return;
            sb.Draw(game._blockManager.Blocks[2].Texture,new Rectangle((int)pos.X, (int)pos.Y,10,10),Color.Red);
        }
        public static void DebugPosWOrld(SpriteBatch sb, Vector2 pos, Game1 game)
        {
            if (!game.DebugMode) return;
            var part = new Particle()
            {
                Position = pos,
                TextureName = "BlockMineEffect",
                Texture = game._particleSystem.sprites[0],
                lifeTime = 0.2f,
                size = 0.1f,
                Color = Color.Red,
                
                
                
                

            };



           game._particleSystem.Particles.Add(part);
        }
    }
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
