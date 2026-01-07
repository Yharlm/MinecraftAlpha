using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;

namespace MinecraftAlpha
{

    public class Debuging
    {
        public static void DrawBlockLayout(TileGrid Tile, int Position)
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
            DebugPosWOrld(sb, pos, game, Color.Red);
        }
        public static void DebugPosWOrld(SpriteBatch sb, Vector2 pos, Game1 game, Color color)
        {
            if (!game.DebugMode) return;
            var part = new Particle()
            {
                Position = pos,
                TextureName = "BlockMineEffect",
                Texture = game._particleSystem.sprites[0],
                lifeTime = 0.5f,
                size = 0.5f,
                Color = color,





            };



            game._particleSystem.Particles.Add(part);
        }
    }
    internal class LogicsClass
    {
        public static Vector2 Randomiser(float min, float max)
        {
            Random rand = new Random();
            float X = (float)(rand.NextDouble() * (max - min) + min);
            float Y = (float)(rand.NextDouble() * (max - min) + min);
            return new Vector2(X, Y);
        }
        public static bool BlockCollide(Vector2 point, Vector2 Position, Vector2 size)
        {
            Vector2 disfc = Position - point;
            if (disfc.X + disfc.Y <= size.X)
            { return true; }
            return false;
        }
        public static bool IsInBounds(Vector2 Pos, Vector2 Origin, Vector2 size)
        {

            if (Pos.X > Origin.X - size.X / 2 && Pos.X < Origin.X + size.X / 2 &&
                Pos.Y > Origin.Y - size.Y / 2 && Pos.Y < Origin.Y + size.Y / 2)
            {
                return true;
            }
            return false;
        }
        public static float F(float X,Vector2 dir,Vector2 Offset)
        {
            return (X-Offset.X)*dir.Y/dir.X + Offset.Y;
        }
        public static void RaycastPos(Vector2 origin,Vector2 Pos, Game1 game)
        {
            //entity ray

            for (float d = 0; d < (Pos-origin).Length(); d+= (Pos - origin).Length()/30)
            {

                Vector2 v0 = Pos;
                Vector2 v1 = origin;
                Vector2 v2 = v0 - v1;


                
                float X = origin.X + (Pos - origin).X*d;
                float Y = F(X, v2, v1);
                
                
                Debuging.DebugPosWOrld(game._spriteBatch, new Vector2(X, Y), game, Color.Blue);

            }

            foreach (var entity in game._entityManager.Workspace)
            {

                Vector2 v0 = Pos;
                Vector2 v1 = origin;
                Vector2 v2 = v0 - v1;

                Vector2 Hit = new(entity.position.X, F(entity.position.X, v2, v1));
                if (float.Abs(entity.position.Y-Hit.Y) < entity.collisionBox.Size.Y)
                {
                    var part = new Particle()
                    {
                        Position = entity.position,
                        TextureName = "BlockMineEffect",
                        Texture = game._particleSystem.sprites[0],
                        lifeTime = 2f,
                        Velocity = Randomiser(-2,2),
                        size = 0.65f,
                        Color = Color.Yellow,





                    };



                    game._particleSystem.Particles.Add(part);
                }

            }

        }
        public static Entity RaycastDir(Vector2 origin, Vector2 direction, Game1 game,List<Entity> ignore)
        {
            Entity Instance = null;
            for (float i = -30; i < 30; i++)
            {

                
                Vector2 v1 = origin;
                Vector2 v3 = direction;



                float X = origin.X + i;
                float Y = F(X, v3, v1);


                Debuging.DebugPosWOrld(game._spriteBatch, new Vector2(X, Y), game, Color.Blue);

            }

            foreach (var entity in game._entityManager.Workspace)
            {
                if (ignore.Contains(entity)) continue;
                Vector2 v1 = origin;
                Vector2 v3 = direction;



                float X = entity.position.X;
                float Y = F(X, v3, v1);

                Vector2 Hit = new(entity.position.X, F(X, v3, v1));
                if (float.Abs(entity.position.Y - Hit.Y) < entity.collisionBox.Size.Y)
                {
                    if(game.DebugMode)
                    {
                        var part = new Particle()
                        {
                            Position = entity.position,
                            TextureName = "BlockMineEffect",
                            Texture = game._particleSystem.sprites[0],
                            lifeTime = 1f,
                            Velocity = Randomiser(-1, 1),
                            size = 0.65f,
                            Color = Color.Yellow,
                        };
                        game._particleSystem.Particles.Add(part);
                    }
                    

                    Instance = entity;

                    
                }

            }

            return Instance;
        }

    }
}
