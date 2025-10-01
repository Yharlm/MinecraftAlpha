using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MinecraftAlpha
{
    public class Joint
    {
        public float orientation = 0f;

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

            return Vector2.Max(A, B) - Vector2.Min(A, B);
        }
    }
    public class CollisionBox
    {
        public Vector2 Size = new Vector2(0.4f, 1);

        public bool Left { get; set; } = false;
        public bool Right { get; set; } = false;
        public bool Top { get; set; } = false;
        public bool Bottom { get; set; } = false;

        /// <summary>
        /// Checks for the collision of an entity with the world
        /// </summary>

        public void UpdateCollision(Entity entity, int[,] World)
        {
            if (entity.position.X < 0 || entity.position.X >= World.GetLength(1) || entity.position.Y < 0 || entity.position.Y >= World.GetLength(0))
            {
                return; // Skip if the entity is out of bounds
            }
            entity.collisionBox = new CollisionBox(); // Reset collision box for each update
            float Collision_quality = 0.5f;
            //World[(int)(entity.position.Y), (int)(entity.position.X)] = 1;

            if (World[(int)(entity.position.Y - Size.Y), (int)(entity.position.X)] != 0)
            {
                entity.collisionBox.Top = true;
            }
            if (World[(int)(entity.position.Y + Size.Y), (int)(entity.position.X)] != 0)
            {
                entity.collisionBox.Bottom = true;
            }
            if (World[(int)(entity.position.Y - Size.Y * 0.8), (int)(entity.position.X - Size.X)] != 0 || World[(int)(entity.position.Y + Size.Y * 0.8), (int)(entity.position.X - Size.X)] != 0)
            {
                entity.collisionBox.Left = true;
            }
            if (World[(int)(entity.position.Y - Size.Y * 0.8), (int)(entity.position.X + Size.X)] != 0 || World[(int)(entity.position.Y + Size.Y * 0.8), (int)(entity.position.X + Size.X)] != 0)
            {
                entity.collisionBox.Right = true;
            }


        }
    }

    public class Entity
    {
        //public List<PotionEffects> = new List<PotionEffects>()
        public int Health;
        public int MaxHealth;
        public int ID = 0;
        public Texture2D Texture; // Main texture where all the limbs will originate from!
        public string TextureName = "null"; // Name of the texture to be loaded
        public List<Vector4> Ractangles = new List<Vector4>();
        public List<Sprite> Sprites = new List<Sprite>();
        public CollisionBox collisionBox = new CollisionBox();
        public Velocity velocity = new Velocity();
        public List<EntityAnimation> Animations = new List<EntityAnimation>();
        //public List<EntityAnimation> CurrentAnimations = new List<EntityAnimation>();

        public Entity(int id, string Name, string TextureName, int Health)
        {
            ID = id;
            name = Name;
            this.TextureName = TextureName;
            this.Health = Health;
            this.MaxHealth = Health;

        }


        public List<Joint> Joints = new List<Joint>(); // used to connect limbs together
        public string name { get; set; } = "nullEntity";
        public Vector2 position { get; set; } = Vector2.Zero;
        public float Mass = 1f;

        public void DrawEntity(SpriteBatch SB, float BlockSize, Vector2 Cam)
        {

            //SB.Begin();
            //SB.Draw(Texture, BlockSize * position + Cam, null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, BlockSize SpriteEffects.None, 0f);

            //SB.End();
            foreach (Joint Joint in Joints)
            {
                Joint.B_Sprite.ParentOrianetation = Joint.A_Sprite.Orientation;
                //Joint.A_Sprite.Joints.Add(Joint.A);
                Joint.B_Sprite.Attachment = Joint.B;
                Joint.B_Sprite.Parent = Joint.A;
                Joint.B_Sprite.JointOrientation = MathF.PI / 180 * Joint.orientation;
            }
            foreach (Sprite s in Sprites)
            {

                s.DrawSprite(SB, BlockSize * position + Cam, BlockSize / 18, 0);

            }
        }


        public static List<Entity> LoadEntites()
        {
            int id = 0;
            List<Entity> Entities = new List<Entity>()
            {
                new Entity(id++,"Player","steve",20)
                {
                    Ractangles = new List<Vector4>()
                    {
                        // Replace Vector4 with a Object that can hold the widths of all 4 sides of an entity
                       
                        new Vector4(12,8,4,12), // Right Arm
                        new Vector4(24,8,4,12),// Body
                        new Vector4(8,0,8,8), // Head
                        new Vector4(12,12,4,12), // Left Arm
                        new Vector4(12,44,4,12), // Right Leg
                        new Vector4(12,32,4,12) // Left Leg
                    },
                    




                }


            };

            return Entities;



        }

        public void UpdateAnimation()
        {
            foreach (EntityAnimation anim in Animations)
            {
                anim.Update();
            }
        }




    }

    public class Velocity
    {

        public Vector2 velocity { get; set; } = Vector2.Zero;

        public void apply_velocity(Entity entity)
        {
            var Acceleration = 0.1f;
            var Vel = Vector2.Zero;

            if (!entity.collisionBox.Left && velocity.X < 0)
            {
                Vel.X = velocity.X - Acceleration;
            }
            if (!entity.collisionBox.Right && velocity.X > 0)
            {
                Vel.X = velocity.X + Acceleration;
            }
            if (!entity.collisionBox.Top && velocity.Y < 0)
            {
                Vel.Y = velocity.Y - Acceleration;
            }
            if (!entity.collisionBox.Bottom && velocity.Y > 0)
            {
                Vel.Y = velocity.Y + Acceleration;
            }





            entity.position += Vel;
        }


    }
}
