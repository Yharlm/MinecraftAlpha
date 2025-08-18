using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftAlpha
{
    public class CollisionBox
    {

        public bool Left { get; set; } = false;
        public bool Right { get; set; } = false;
        public bool Top { get; set; } = false;
        public bool Bottom { get; set; } = false;


    }

    public class Entity
    {
        public CollisionBox collisionBox;
        public Velocity velocity; 
        public string name { get; set; } = "Entity";
        public Vector2 position { get; set; } = Vector2.Zero;
        public float Mass = 1f;

    }

    public class Velocity 
    {

        public Vector2 velocity { get; set; } = Vector2.Zero ;

        public void apply_velocity(Entity entity)
        {
            var Vel = Vector2.Zero;

            if (!entity.collisionBox.Left && velocity.X < 0)
            {
                Vel.X = velocity.X;
            }
            if (!entity.collisionBox.Right && velocity.X > 0)
            {
                Vel.X = velocity.X;
            }
            if (!entity.collisionBox.Top && velocity.Y < 0)
            {
                Vel.Y = velocity.Y;
            }
            if (!entity.collisionBox.Bottom && velocity.Y > 0)
            {
                Vel.Y = velocity.Y;
            }





            entity.position += Vel;
        }
        

    }
}
