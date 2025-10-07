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

        public float lifeTime { get; set; }
        public float Changespeed { get; set; }

        public Microsoft.Xna.Framework.Color Color { get; set; }

        public string TextureName;

        public Texture2D Texture = null;

        public int Index = 0;

        public Vector2 Velocity;

        public void Update()
        {
            
            lifeTime += 0.01f;
            Index = (int)lifeTime;
            Position += Velocity/lifeTime/20;
            
        }
        public void DrawParticles(SpriteBatch spriteBatch, Vector2 Camera, float Size, Texture2D Preset)
        {
            if(Texture == null)
            {
                Texture = Preset;
            }
            var Ractangle = new Microsoft.Xna.Framework.Rectangle(Index*8, 0, 8, 8);
            spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            spriteBatch.Draw(
                Texture,
                Size * Position + Camera,
                Ractangle,
                Color,
                0f, // Orientation
                Vector2.Zero, //
                Size/8,
                SpriteEffects.None,
                1f
                );
            spriteBatch.End();
        }
    }
    public class ParticleSystem
    {
        public ContentManager Content;
        public ParticleSystem() { }

        public List<Particle> Particles = new List<Particle>()
        {   new Particle
            {
                TextureName = "ParticleSmokeEffect"
            }

        };
        public void Load()
        {
            foreach (var item in Particles)
            {
                item.Texture = Content.Load<Texture2D>(item.TextureName);
            }

        }
    }


}
