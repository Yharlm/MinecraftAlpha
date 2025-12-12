using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;

namespace MinecraftAlpha
{
    public class Particle
    {

        

        public Vector2 Position { get; set; }

        public Microsoft.Xna.Framework.Rectangle Rectangle = new Microsoft.Xna.Framework.Rectangle(0,0,0,0);
        public float lifeTime { get; set; }

        public float timeElapsed { get; set; }
        public float Changespeed { get; set; }

        public float size = 1f;

        //static void Main(string[] args) // Tween color to another color
        //{
        //    Vector3 A = new Vector3(0, 255, 0);
        //    Vector3 B = new Vector3(255, 0, 255);
        //    Console.WriteLine(A);
        //    int div = 255;
        //    var RGB = (B - A) / div;
        //    for (int i = 0; i < div; i++)
        //    {


        //        A += RGB;

        //        Console.WriteLine(A);

        //    }

        //}
        public Microsoft.Xna.Framework.Color Color { get; set; }

        public string TextureName = "ParticleSmokeEffect";

        public Texture2D Texture = null;

        public int Index = 0;

        public Vector2 Velocity;

        public Vector2 Acceleration;
         public float gravity = 0.01f;

        public bool ParticleRactangleCHose = false;

        public void Update()
        {

            timeElapsed += 0.01f;
            Index = (int)(timeElapsed /lifeTime);
            Position += (Acceleration+ Velocity) / /*lifeTime/*/20 + gravity * Vector2.UnitY;
            
        }
        Random rnd = new Random();
        public void DrawParticles(SpriteBatch spriteBatch, Vector2 Camera, float Size, Texture2D Preset)
        {
            if(Texture == null)
            {
                

                Texture = Preset;
            }

            var Ractangle = new Microsoft.Xna.Framework.Rectangle(Index*8, 0, 8, 8);

            //if (TextureName == "BlockMineEffect")
            //{
            //    if (!ParticleRactangleCHose)
            //    {
            //        return;
            //    }
            //    int x = rnd.Next(0, Texture.Width);
            //    int y = rnd.Next(0, Texture.Height);
            //    Ractangle = new Microsoft.Xna.Framework.Rectangle(x,y,x+3,y+3);
            //    ParticleRactangleCHose = true;
            //}

            if (!this.Rectangle.IsEmpty)
            {
                Ractangle = this.Rectangle;

            }

            //spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            spriteBatch.Draw(
                Texture,
                Size * Position + Camera,
                Ractangle,
                Color,
                0f, // Orientation
                Vector2.Zero, //
                size *Size/8,
                SpriteEffects.None,
                1f
                );
            //spriteBatch.End();
        }
    }
    public class ParticleSystem
    {
        public ContentManager Content;
        public ParticleSystem() { }
        public List<Texture2D> sprites = new List<Texture2D>();
        

        public List<Particle> Particles = new List<Particle>()
        {   

        };
        public void Load()
        {
            
            sprites.Add(Content.Load<Texture2D>("ParticleSmokeEffect"));
            

        }

       
    }


}
