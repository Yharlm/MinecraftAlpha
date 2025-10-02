using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MinecraftAlpha
{
    public class Particle
    {
        public Vector2 Position { get; set; }

        public float lifeTime { get; set; }
        public float Changespeed { get; set; }

        public Color Color { get; set; }

        public string TextureName;

        public Texture2D Texture = null;

        public int Index = 0;

        public Vector2 Velocity;

        public void DrawParticles(SpriteBatch spriteBatch, Vector2 Camera, float Size,Texture2D Preset)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(
                Preset,
                Camera - Position * Size,
                null,
                Microsoft.Xna.Framework.Color.White,
                0f, // Orientation
                Vector2.Zero, //
                Vector2.One,
                SpriteEffects.None,
                1f
                );
            spriteBatch.End();
        }
    }
    public class ParticleSystem
    {
        public ParticleSystem() { }

        public List<Particle> Particles = new List<Particle>()
        {   new Particle
            {
            
            }

        };


    }



}
