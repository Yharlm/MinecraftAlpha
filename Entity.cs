using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinecraftAlpha
{
    public class Joint
    {
        public float orientation = 0f;

        public Sprite A_Sprite;
        public Sprite B_Sprite;
        public Vector2 A = Vector2.Zero;
        public Vector2 B = Vector2.Zero;




        public int A_Index = -1;
        public int B_Index = -1;
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

        public void UpdateCollision(Entity entity, TileGrid[,] World)
        {
            if (entity.position.X < 0 || entity.position.X >= World.GetLength(1) || entity.position.Y < 0 || entity.position.Y >= World.GetLength(0))
            {
                return; // Skip if the entity is out of bounds
            }
            entity.collisionBox = new CollisionBox(); // Reset collision box for each update
            float Collision_quality = 0.5f;
            //World[(int)(entity.position.Y), (int)(entity.position.X)] = 1;

            if (World[(int)(entity.position.Y - Size.Y), (int)(entity.position.X)].ID != 0)
            {
                entity.collisionBox.Top = true;
            }
            if (World[(int)(entity.position.Y + Size.Y), (int)(entity.position.X)].ID != 0)
            {
                entity.collisionBox.Bottom = true;
            }
            if (World[(int)(entity.position.Y - Size.Y * 0.8), (int)(entity.position.X - Size.X)].ID != 0 || World[(int)(entity.position.Y + Size.Y * 0.8), (int)(entity.position.X - Size.X)].ID != 0)
            {
                entity.collisionBox.Left = true;
            }
            if (World[(int)(entity.position.Y - Size.Y * 0.8), (int)(entity.position.X + Size.X)].ID != 0 || World[(int)(entity.position.Y + Size.Y * 0.8), (int)(entity.position.X + Size.X)].ID != 0)
            {
                entity.collisionBox.Right = true;
            }


        }

        public void CheckCollision(Entity entity, TileGrid[,] World)
        {
            foreach (var block in World)
            {
                if (LogicsClass.BlockCollide(entity.position, block.pos, 32 * Vector2.One))
                {
                    block.MinedHealth += 10;
                }
                   
            }


        }
    }

    public class Entity
    {

        public static Entity CloneEntity(Entity Example, Vector2 Position)
        {
            Entity Clone = new Entity(Example.ID, Example.name, Example.TextureName, Example.MaxHealth);
            {
                Clone.ID = Example.ID;
                Clone.Ractangles = Example.Ractangles;
                Clone.position = Position;
                Clone.Joints = Example.Joints;
                Clone.collisionBox = new CollisionBox();
                Clone.Animations = Example.Animations;
               
                Clone.Texture = Example.Texture;
                Clone.Fall_damage = Example.Fall_damage;
                Clone.Mass = Example.Mass;
                Clone.Fliped = Example.Fliped;
                Clone.paused = Example.paused;
                //Events
                Clone.Model3D = Example.Model3D;

                
                Clone.Interaction = Example.Interaction;
                Clone.Update = Example.Update;

            }
            Clone.Sprites = Sprite.LoadSprites(Example);
            EntityManager.LoadJoins(Clone);
            return Clone;
        }

        //public Entity CloneEntity(Entity Example, Vector2 Position, string Data)
        //{
        //    Entity Clone = new Entity(Example.ID, Example.name, Example.TextureName, Example.MaxHealth);
        //    {
        //        Clone.Ractangles = Example.Ractangles;
        //        Clone.position = Position;
        //        Clone.Joints = Example.Joints;
        //        Clone.collisionBox = new CollisionBox() { Size = Example.collisionBox.Size };
        //        Clone.Animations = Example.Animations;
        //        Clone.Sprites = Example.Sprites;
        //        Clone.Texture = Example.Texture;
        //        Clone.Fall_damage = Example.Fall_damage;
        //        Clone.Mass = Example.Mass;
        //        Clone.Fliped = Example.Fliped;
        //        Clone.paused = Example.paused;
        //        Clone.Data = Data;
        //        //Events

                
        //        Clone.Interaction = Example.Interaction;
        //        Clone.Update = Example.Update;

        //    }
        //    return Clone;
        //}


        public List<EntityAnimation> Animations = new List<EntityAnimation>();
        public bool paused = false;
        public bool Fliped = true;


        //public List<PotionEffects> = new List<PotionEffects>()
        public string Data = ""; // armor, items, if Itemdrop ammount, 
        public int Health;
        public int MaxHealth;
        public int ID = 0;


        public string name { get; set; } = "nullEntity";
        public List<Joint> Joints = new List<Joint>(); // used to connect limbs together
        public Texture2D Texture; // Main texture where all the limbs will originate from!
        public string TextureName = "null"; // Name of the texture to be loaded
        public List<Vector4> Ractangles = new List<Vector4>();
        public List<Sprite> Sprites = new List<Sprite>();
        public Sprite3D Model3D = null;
        public CollisionBox collisionBox = new CollisionBox();


        //public List<EntityAnimation> CurrentAnimations = new List<EntityAnimation>();


        public Vector2 position { get; set; } = Vector2.One * 50;
        public Velocity velocity = new Velocity();
        public int Fall_damage = 0;
        public float Mass = 1f;
        public float IFrame = 3f; // Invincibility Frames
        public bool CanDamage = true;

        public Entity(int id, string Name, string TextureName, int Health)
        {
            ID = id;
            name = Name;
            this.TextureName = TextureName;
            this.Health = Health;
            this.MaxHealth = Health;

        } // Constructor

        

        public Event Interaction;

        public Event Update;

        public Event Damaged;

        public static void CollisionEventCollision(Entity A,Entity B,Game1 game1)
        {
            if (A.ID == -1 && B.name == "Player") // ItemDrop
            {
                if ((A.position - B.position).Length() > 1f)
                {
                    A.velocity.velocity = (B.position - A.position) / 20;
                }
                int id = int.Parse(A.Data);

                game1.Player.PickupItem(game1._blockManager.Blocks[id], 1, game1._userInterfaceManager.windows[0]);
                A.Health = 0;
            }
        }

        static public Entity GetentityAtPosition(Vector2 Pos, List<Entity> Entities)
        {
            foreach (var entity in Entities)
            {
                Rectangle A = new Rectangle((int)(entity.position.X * 32), (int)(entity.position.Y * 32), (int)(entity.collisionBox.Size.X * 32), (int)(entity.collisionBox.Size.Y * 32));
                if (A.Contains((int)(Pos.X * 32), (int)(Pos.Y * 32)))
                {
                   return entity;
                }
            }
            return null;
        }
        public void ResetIframes()
        {
            if (IFrame <= 0) return;
            IFrame -= 0.1f;
        }

        public void DrawEntity(SpriteBatch SB, float BlockSize, Vector2 Cam)
        {

            //SB.Begin();
            //SB.Draw(Texture, BlockSize * position + Cam, null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, BlockSize SpriteEffects.None, 0f);

            //SB.End();
            if (this.Model3D != null)
            {
                this.Model3D.Draw(SB, BlockSize * position + Cam, BlockSize / 18);
                return;
            }


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

                s.DrawSprite(SB, BlockSize * position + Cam, BlockSize / 18, 0, Fliped);

            }
        }
        
        public bool CheckCollisionEntity(Entity entity)
        {
            Rectangle A = new Rectangle((int)(entity.position.X * 32), (int)(entity.position.Y * 32), (int)(entity.collisionBox.Size.X * 32), (int)(entity.collisionBox.Size.Y * 32));
            Rectangle B = new Rectangle((int)(position.X * 32), (int)(position.Y * 32), (int)(collisionBox.Size.X * 32), (int)(collisionBox.Size.Y * 32));
            if (A.Intersects(B))
            {
                //entity.velocity.velocity = (this.position - entity.position) / entity.Mass;

                

                return true; // invokes?
            }
            return false;
        }
        public void UpdateAnimation()
        {
            if (Animations.Count < 0) return;
            bool idle = true;
            foreach (var anim in Animations)
            {
                if (!anim.Paused && anim.name != "idle")
                {
                    idle = false;
                }

            }
            if (idle)
            {
                Animations[0].ResetAnim();
                return;
            }


            
        }
        public void Attack(Entity entity)
        {
            entity.Health -= 1;

        }





    }

    public class Velocity
    {
        public Vector2 Gravity = new Vector2(0, 0.1f);
        public Vector2 velocity { get; set; } = Vector2.Zero;

        public void apply_velocity(Entity entity)
        {
            var Acceleration = 0.1f;
            var Vel = Vector2.Zero;

            if (!entity.collisionBox.Left && velocity.X < 0)
            {
                Vel.X -= Acceleration;
                velocity -= Acceleration * Vector2.UnitX;
            }
            if (!entity.collisionBox.Right && velocity.X > 0)
            {
                Vel.X += Acceleration;
                velocity += Acceleration * Vector2.UnitX;
            }
            if (!entity.collisionBox.Top && velocity.Y < 0)
            {
                Vel.Y -= Acceleration;
                velocity -= Acceleration * Vector2.UnitY;
            }
            if (!entity.collisionBox.Bottom && velocity.Y > 0)
            {

                Vel.Y += Acceleration;
                velocity += Acceleration * Vector2.UnitY;
            }


            if (entity.collisionBox.Bottom)
            {
                Gravity.Y = 0;
            }
            else
            {
                Gravity.Y += Acceleration / 20;
            }



            entity.position += Vel + Gravity;
        }


    }
}
