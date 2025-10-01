using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
    }
    public class ParticleSystem
    {

    }

    public void DrawParticles(SpriteBatch spriteBatch)
        {

        }
}
