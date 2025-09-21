using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftAlpha
{
    public class Joint
    {
        public Sprite A_Sprite;
        public Sprite B_Sprite;
        public Vector2 A = Vector2.Zero;
        public Vector2 B = Vector2.Zero;
        public Joint() { }
        public Joint(Sprite a, Sprite b)
        {
            A_Sprite = a;
            B_Sprite = b;
        }
        public Joint(Sprite a, Vector2 a_attach, Sprite b, Vector2 b_attach)
        {
            A_Sprite = a;
            B_Sprite = b;
            A = a_attach;
            B = b_attach;
        }

        public Vector2 Connect() // returns the distance between A and B
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
        public Texture2D Texture; // Main texture where all the limbs will originate from!
        public string TextureName = "null"; // Name of the texture to be loaded
        public List<Vector4> Ractangles = new List<Vector4>();
        public List<Sprite> Sprites = new List<Sprite>();
        public CollisionBox collisionBox = new CollisionBox();
        public Velocity velocity = new Velocity();


        public List<Joint> Joints = new List<Joint>(); // used to connect limbs together
        public string name { get; set; } = "Entity";
        public Vector2 position { get; set; } = Vector2.Zero;
        public float Mass = 1f;

        public void DrawEntity(SpriteBatch SB, float BlockSize,Vector2 Cam)
        {
            foreach (Joint Joint in Joints)
            {

                Joint.A_Sprite.Joints.Add(Joint.A);
                Joint.B_Sprite.Attachment = Joint.B;
            }
            foreach (Sprite s in Sprites)
            {
                s.DrawSprite(SB, /*s.Attachment + */Cam , BlockSize);
                
            }
        }


        public static List<Entity> LoadEntites()
        {
            List<Entity> Entities = new List<Entity>()
            { 
                new Entity()
                {

                    name = "Player",
                    TextureName = "Steve",
                    Ractangles = new List<Vector4>()
                    {
                        new Vector4(24,8,4,12), // Body
                        new Vector4(8,0,8,8), // Head
                        //new Vector4(24,8,28,20) // Right Arm
                        //new Vector4(48,0,64,16), // Left Arm
                        //new Vector4(32,16,48,32), // Right Leg
                        //new Vector4(48,16,64,32) // Left Leg
                    },


                    
                }
            };

            return Entities;



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
                Vel.X = velocity.X- Acceleration;
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
