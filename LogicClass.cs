using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = System.Drawing.Rectangle;

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
            
            sb.Draw(
                game._blockManager.Blocks[2].Texture,
                pos,
                null,
                Color.Red,
                0f, // Orientation
                Vector2.Zero, //
                0.2f,
                SpriteEffects.None,
                1
                );
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
        public static bool IsInBounds(Vector2 Pos,Vector2 Origin, Vector2 size)
        {

            if (Pos.X > Origin.X - size.X / 2 && Pos.X < Origin.X + size.X / 2 &&
                Pos.Y > Origin.Y - size.Y / 2 && Pos.Y < Origin.Y + size.Y / 2)
            {
                return true;
            }
            return false;
        }


    }
}
