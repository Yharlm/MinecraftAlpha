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
        public Vector2 Size = new Vector2(1f, 1);

        public bool Left { get; set; } = false;
        public bool Right { get; set; } = false;
        public bool Top { get; set; } = false;
        public bool Bottom { get; set; } = false;

        public bool center { get; set; } = false;

        public void UpdateCollision(Entity entity, List<Chunk> World,Game1 game1)
        {
            //if (entity.position.X < 0 || entity.position.X >= World.GetLength(1) || entity.position.Y < 0 || entity.position.Y >= World.GetLength(0))
            //{
            //    return; // Skip if the entity is out of bounds
            //}
            entity.collisionBox = new CollisionBox(); // Reset collision box for each update
            //float Collision_quality = 0.5f;
            //World[(int)(entity.position.Y), (int)(entity.position.X)] = 1;

            //if (World[(int)(entity.position.Y - Size.Y), (int)(entity.position.X)].ID != 0)
            //{
            //    entity.collisionBox.Top = true;
            //}
            //if (World[(int)(entity.position.Y + Size.Y), (int)(entity.position.X)].ID != 0)
            //{
            //    entity.collisionBox.Bottom = true;
            //}
            //if (World[(int)(entity.position.Y - Size.Y * 0.8), (int)(entity.position.X - Size.X)].ID != 0 || World[(int)(entity.position.Y + Size.Y * 0.8), (int)(entity.position.X - Size.X)].ID != 0)
            //{
            //    entity.collisionBox.Left = true;
            //}
            //if (World[(int)(entity.position.Y - Size.Y * 0.8), (int)(entity.position.X + Size.X)].ID != 0 || World[(int)(entity.position.Y + Size.Y * 0.8), (int)(entity.position.X + Size.X)].ID != 0)
            //{
            //    entity.collisionBox.Right = true;
            //}
            Vector2 Offset = new Vector2(Size.X/2, 0);
            Vector2 Bottom = entity.position + new Vector2(entity.collisionBox.Size.X / 2, entity.collisionBox.Size.Y) - Offset;
            Vector2 Left = entity.position + new Vector2(0, entity.collisionBox.Size.Y / 2f) - Offset;
            Vector2 Right = entity.position + new Vector2(entity.collisionBox.Size.X, entity.collisionBox.Size.Y / 2f) - Offset;
            Vector2 Top = entity.position + new Vector2(entity.collisionBox.Size.X / 2, 0) - Offset;
            Vector2 Center = entity.position + new Vector2(entity.collisionBox.Size.X / 2, entity.collisionBox.Size.Y / 2f) - Offset;
            int z = (int)entity.Layer;
            
            
                game1._spriteBatch.Begin();
                Debuging.DebugPosWOrld(game1._spriteBatch, Bottom, game1);
                Debuging.DebugPosWOrld(game1._spriteBatch, Left, game1);
                Debuging.DebugPosWOrld(game1._spriteBatch, Right, game1);
                Debuging.DebugPosWOrld(game1._spriteBatch, Top, game1);
                Debuging.DebugPosWOrld(game1._spriteBatch, Center, game1);
                game1._spriteBatch.End();
            


            if (BlockManager.GetBlockAtPos(Bottom, z, World) != null && BlockManager.GetBlockAtPos(Bottom, z, World).ID != 0)
            {
                entity.collisionBox.Bottom = true;
            }
            if (BlockManager.GetBlockAtPos(Left, z, World) != null && BlockManager.GetBlockAtPos(Left, z, World).ID != 0)
            {
                entity.collisionBox.Left = true;
            }
            if (BlockManager.GetBlockAtPos(Right, z, World) != null && BlockManager.GetBlockAtPos(Right, z, World).ID != 0)
            {
                entity.collisionBox.Right = true;
            }
            if (BlockManager.GetBlockAtPos(Top, z, World) != null && BlockManager.GetBlockAtPos(Top, z, World).ID != 0)
            {
                entity.collisionBox.Top = true;
            }
            if (BlockManager.GetBlockAtPos(Center, z, World) != null)
            {
                entity.collisionBox.center = true;
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
                //Clone.Joints = Example.Joints;
                Clone.collisionBox = new CollisionBox();
                Clone.Animations = EntityAnimation.LoadAnimation(Clone, Example.Animations);
                Clone.velocity = new Velocity();
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
            EntityManager.LoadJoins(Example, Clone);
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
        public float Speed =0.7f;


        public string name { get; set; } = "nullEntity";
        public List<Joint> Joints = new List<Joint>(); // used to connect limbs together
        public Texture2D Texture; // Main texture where all the limbs will originate from!
        public string TextureName = "null"; // Name of the texture to be loaded
        public List<Vector4> Ractangles = new List<Vector4>();
        public List<Sprite> Sprites = new List<Sprite>();
        public Sprite3D Model3D = null;
        public CollisionBox collisionBox = new CollisionBox();


        //public List<EntityAnimation> CurrentAnimations = new List<EntityAnimation>();


        public Entity Target = null;


        public bool Grounded = false;
        public Vector2 position { get; set; } = Vector2.Zero;
        public Velocity velocity = new Velocity();
        public int Fall_damage = 0;
        public float Mass = 1f;
        public float IFrame = 20f; // Invincibility Frames
        public bool CanDamage = true;
        public float Layer = 8;

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

        public static void CollisionEventCollision(Entity A, Entity B, Game1 game1)
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
                
                if (LogicsClass.IsInBounds(Pos,entity.position,entity.collisionBox.Size))
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
        public void Iframes()
        {

            IFrame = 4f;


        }

        public void DrawEntity(SpriteBatch SB, float BlockSize, Vector2 Cam, Game1 game1)
        {

            
            //SB.Begin();
            //SB.Draw(Texture, BlockSize * position + Cam, null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, BlockSize SpriteEffects.None, 0f);
            //SB.End();
            if (this.Model3D != null)
            {
                this.Model3D.Draw(SB, BlockSize * position + Cam, BlockSize / 18, 0);
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
                int i = Sprites.IndexOf(s);
                s.DrawSprite(SB, BlockSize * position + Cam, BlockSize / 18, 0, Fliped, float.Floor(Layer) / 10 + (float)i / 60, float.Floor(Layer), (IFrame > 0));

            }
            if(game1.DebugMode) SB.DrawString(game1.Content.Load<SpriteFont>("Font"), $"{Health}/{MaxHealth}", BlockSize * position + Cam - new Vector2(4, 80), Color.Red);

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

        
        public void Punch(Entity Target, Game1 game)
        {

            if ("Item" == Target.name) return;
            if (!collisionBox.Bottom && IFrame <= 0f)
            {
                Random random = new Random();
                Target.TakeDamage(this, 3, 17);
                var part = new Particle()
                {
                    
                    Position = Target.position + new Vector2((float)random.NextDouble()-0.5f, -(float)random.NextDouble()/2)*2,
                    TextureName = "BlockMineEffect",
                    Texture = game._particleSystem.sprites[0],
                    lifeTime = 3f,
                    size = 1f,
                    Color = Color.Beige,
                    
                    Velocity = new Vector2(0,0),
                    

                };

                IFrame = 2f;

                game._particleSystem.Particles.Add(part);

            }
            else
            {
                Target.TakeDamage(this, 1,10);
            }
            
            Target.Target = this;
            game._entityAnimationService.Play(2, this);

        }
        public void TakeDamage(Entity source, int DMG,float knockback)
        {
            if (DMG < 1) return;
            //velocity.velocity += new Vector2(0, -1f);
            Vector2 Knockback = new Vector2(0,1);
            if (IFrame > 0) return;
            if (source != null) Knockback = (position - source.position);

            if(Knockback == Vector2.Zero)
            {
                Knockback = new Vector2(0, 1);
            }

            velocity.velocity += Vector2.Normalize(Knockback) * knockback;
            Jump();
            Health -= DMG;
            Iframes();
        }

        public void WalkTo(Vector2 pos,bool can_jump)
        {
            float lspeed = Speed;
            if (can_jump) {
                if (!collisionBox.Bottom || collisionBox.Left || collisionBox.Right)
                {
                    Jump();
                }
            }
            if(IFrame >1)
            {
                lspeed = Speed*0.6f;
            }
            //if (!collisionBox.Bottom)
            //{
            //    lspeed = Speed * 0.5f;
            //}

            velocity.velocity += pos * Vector2.UnitX * lspeed;

        }



        public void Jump()
        {
            if (Grounded) return;

            velocity.velocity += new Vector2(0, -3f);
            Grounded = true;
            //position += new Vector2(0, -0.2f);

        }

    }

    public class Velocity
    {
        public bool flying = false;

        public float Gravity = 0;
        public Vector2 velocity = new(0, 0);
        public float Drag = 7f;


        public void apply_velocity(Entity entity)
        {
            Vector2 vel = Vector2.One;
            float Drag = this.Drag;
           

            if(!entity.Grounded)
            {
                Gravity = 0;
                
            }
            else
            {
                Gravity += 0.02f;
            }


            if(float.Abs(velocity.X) < 0.4f)
            {
                velocity.X = 0;
            }


            if (entity.collisionBox.Bottom && velocity.Y > 0 || entity.collisionBox.Top && velocity.Y < 0)
            {
                vel.Y = 0;
                velocity.Y = 0;
            }
            if (entity.collisionBox.Right && velocity.X > 0 || entity.collisionBox.Left && velocity.X < 0)
            {
                vel.X = 0;
                velocity.X = 0;
            }
            if(vel.Length() > 30)
            {
                Drag = 20f;
            }
            vel *= velocity;
            velocity -= vel / Drag;

            if (flying)
            {
                Gravity = 0;
            }
            
            entity.position += vel/ 30;
            velocity += new Vector2(0, Gravity*2.25f);
        }

        
    }
}
