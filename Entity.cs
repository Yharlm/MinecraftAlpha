using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftAlpha
{
    public class Joint
    {
        public Vector2 A = Vector2.Zero;
        public Vector2 B = Vector2.Zero;

        public Vector2 Connect()
        {
            return Vector2.Max(A, B) - Vector2.Min(A,B);
        }
    }
    public class CollisionBox
    {

        public bool Left { get; set; } = false;
        public bool Right { get; set; } = false;
        public bool Top { get; set; } = false;
        public bool Bottom { get; set; } = false;


    }

    public class Entity
    {
        public List<Vector4> Ractangles = new List<Vector4>();
        public List<Sprite> Sprites = new List<Sprite>();
        public CollisionBox collisionBox;
        public Velocity velocity; 
        public string name { get; set; } = "Entity";
        public Vector2 position { get; set; } = Vector2.Zero;
        public float Mass = 1f;

        public void DrawEntity(SpriteBatch SB, float BlockSize)
        {
            foreach(Sprite s in Sprites)
            {
                s.DrawSprite(SB,BlockSize);
            }
        }

        
    }

    public class Velocity 
    {

        public Vector2 velocity { get; set; } = Vector2.Zero ;

        public void apply_velocity(Entity entity)
        {
            var Acceleration = 0.1f;
            var Vel = Vector2.Zero;

            if (!entity.collisionBox.Left && velocity.X < 0)
            {
                Vel.X = velocity.X- Accel
                    eration;
            }
            if (!entity.collisionBox.Right && velocity.X > 0)
            {
                Vel.X = velocity.X+ Acceleration;
            }
            if (!entity.collisionBox.Top && velocity.Y < 0)
            {
                Vel.Y = velocity.Y- Acceleration;
            }
            if (!entity.collisionBox.Bottom && velocity.Y > 0)
            {
                Vel.Y = velocity.Y + Acceleration;
            }





            entity.position += Vel;
        }
        

    }
}
