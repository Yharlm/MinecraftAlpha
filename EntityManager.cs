using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MinecraftAlpha
{
    public class EntityManager
    {
        public Game1 game;
        public EntityManager() { }
        public List<Entity> entities = new List<Entity>();
        public List<Entity> Workspace = new List<Entity>();

        //public void Spawn(Entity entity)
        //{
        //    Entity mobClone = new Entity(entity.ID,entity.name, entity.TextureName, entity.MaxHealth)
        //    { 
        //        Ractangles = entity.Ractangles,
        //        position = Vector2.One * 50,
        //        Joints = entity.Joints,
        //        collisionBox = new CollisionBox() { Size = entity.collisionBox.Size },
        //    };
        //    Workspace.Add(mobClone);

        //}


        public void Attract(float r,Vector2 position)
        {
            Random random = new Random();
            foreach (var entity in Workspace)
            {
                if (entity == game.Player.Plr) continue;
                if (Vector2.Distance(entity.position, position) < r)
                {
                    Vector2 direction = position - entity.position;
                    entity.velocity.velocity +=(direction+ Vector2.UnitY)*1 + new Vector2((float)random.NextDouble()-0.5f, (float)random.NextDouble() - 0.5f) *3; // Adjust the multiplier for speed
                    entity.velocity.Gravity = 0;
                }
            }
        }

        public void AI(Entity mob)
        {


            Vector2 Pos = mob.position;

            if (mob.Target != null)
            {
                Pos = mob.Target.position;
            }




            if (mob == game.Player.Plr) return;
            foreach (var entity in Workspace)
            {
                if (entity != mob)
                {
                    if (Vector2.Distance(mob.position, entity.position) < 10f)
                    {
                        //mob.Target = entity;
                    }

                }
            }
            

            
            if (mob.Target != null)
            {
                mob.WalkTo(Vector2.Normalize(Pos - mob.position),true);
                var Targ = mob.Target;
                if (Vector2.Distance(mob.position, Targ.position) < 1.5f)
                {
                    mob.Punch(Targ, game);
                }
            }

        }
        public Entity SpawnItem(Vector2 position,int Z, Block item)
        {
            var Drop = Entity.CloneEntity(game._entityManager.entities[1], Vector2.Floor(position) + Vector2.One * 0.5f);
            Drop.TextureName = "null";
            
            

            var drop = item;
            Drop.Data = game._blockManager.GetBlockID(drop).ToString();


            Drop.Data = game._blockManager.GetBlockID(drop).ToString() + ";1";
            
            Drop.Layer = Z;
            Drop.Model3D = new Sprite3D(drop.Texture, drop.Texture, drop.Texture, drop.Texture);
            return Drop;
        }
        public void UpdateAll()
        {
            foreach (var entity in Workspace)
            {

            }
        }

        public void Die(Entity entity,List<Entity> list)
        {
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                var part = new Particle()
                {
                    Position = entity.position+ new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f) ,
                    TextureName = "Dust",
                    Texture = game._particleSystem.sprites[0],
                    lifeTime = (float)random.NextDouble()*2,
                    size = (float)random.NextDouble() * 2f,
                    Color = Color.LightGray,
                    Velocity = new Vector2((float)random.NextDouble()-0.5f, (float)random.NextDouble()-0.5f)*0.3f,
                    Acceleration = Vector2.Zero,
                    gravity = -0.02f

                };

                game._particleSystem.Particles.Add(part);
            }
            list.Add(SpawnItem(entity.position, (int)entity.Layer, game._blockManager.Blocks[2]));
        }
        public void RenderAll(SpriteBatch SB, float Size, Vector2 Pos)
        {
            foreach (var entity in Workspace)
            {

                entity.DrawEntity(SB, Size, Pos,game);
            }
        }
        public static List<Entity> LoadEntites(Game1 game1)
        {
            int id = 0;
            List<Entity> Entities = new List<Entity>();



            var Plr = new Entity(id++, "Player", "steve", 20)
            {

                Ractangles = new List<Vector4>() // LimbShapes
                {
                        // Replace Vector4 with a Object that can hold the widths of all 4 sides of an entity
                       
                        new Vector4(12,8,4,12), // Right Arm
                        new Vector4(24,8,4,12),// Body
                        new Vector4(8,0,8,8), // Head
                        new Vector4(12,12,4,12), // Left Arm
                        new Vector4(12,44,4,12), // Right Leg
                        new Vector4(12,32,4,12) // Left Leg
                },
                Joints = new List<Joint>()
                {
                    new Joint() //Head
                    {

                        A = new Vector2(0, 8f),
                        B = new Vector2(0f, 2f),
                        A_Index = 1,
                        B_Index = 2,
                    },

                    new Joint()
                    {
                        orientation = 180f,
                        A = new Vector2(0, 4f),
                        B = new Vector2(0f, 4f),
                        A_Index = 1,
                        B_Index = 3
                    },

                    new Joint()
                    {
                        A = new Vector2(0, -8f),
                        B = new Vector2(0f, -4f),
                        A_Index = 1,
                        B_Index = 5
                    },

                    new Joint()
                    {
                        orientation = 0,
                        A = new Vector2(0, 4f),
                        B = new Vector2(0f, -4f),
                        A_Index = 1,
                        B_Index = 0
                    },

                    new Joint()
                    {
                        A = new Vector2(0, -8f),
                        B = new Vector2(0f, -4f),
                        A_Index = 1,
                        B_Index = 4
                    },
                    
                },
                Animations = new()
                {
                    EntityAnimationService.LoadAnimations()[0],
                    EntityAnimationService.LoadAnimations()[1],
                    EntityAnimationService.LoadAnimations()[2],
                }





            };
            Plr.Texture = game1.Content.Load<Texture2D>(Plr.TextureName);
            Entities.Add(Plr);


            var item = new Entity(-1, "Item", "null", 100)
            {

                position = Vector2.Zero,
                collisionBox = new CollisionBox() { Size = new Vector2(2f, 2f) },

            };
            Entities.Add(item);






            return Entities;



        }

        public List<EntityAnimation> LoadEntityAnimations(int id)
        {
            var animations = new List<EntityAnimation>();
            //animations.Add();
            return animations;
        }
        public void LoadSprites(ContentManager contentManager)
        {
            foreach (var entity in entities)
            {
                entity.Sprites = Sprite.LoadSprites(entity);
            }
        }
        public void LoadJoints()
        {
            foreach (var entity in entities)
            {
                foreach (var joint in entity.Joints)
                {

                    joint.A_Sprite = entity.Sprites[joint.A_Index];
                    joint.B_Sprite = entity.Sprites[joint.B_Index];
                }
            }

        }
        public static void LoadJoins(Entity example, Entity clone)
        {
            for (var i = 0; i < example.Joints.Count; i++)
            {
                var Ex = example.Joints[i];
                var Cl = new Joint()
                {
                    A_Index = Ex.A_Index,
                    B_Index = Ex.B_Index,
                    A = Ex.A,
                    B = Ex.B,
                    A_Sprite = clone.Sprites[Ex.A_Index],
                    B_Sprite = clone.Sprites[Ex.B_Index],

                };
                clone.Joints.Add(Cl);



            }
        }
    }




}
