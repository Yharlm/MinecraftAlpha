using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MinecraftAlpha
{
    public class EntityManager
    {
        public Game1 game;

        public EntityManager()
        { }

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

        public void Attract(float r, Vector2 position)
        {
            Random random = new Random();
            foreach (var entity in Workspace)
            {
                if (entity.ID <= -1)
                {


                    if (entity == game.Player.Plr) continue;
                    if (Vector2.Distance(entity.position, position) < r)
                    {
                        Vector2 direction = position - entity.position;
                        entity.velocity.velocity += (direction + Vector2.UnitY) * 1 + new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f) * 3; // Adjust the multiplier for speed
                        entity.velocity.Gravity = 0;
                    }
                }
            }
        }

        public void AI(Entity mob)
        {
            Vector2 Pos = mob.position;
            if (mob.ID <= -1) return; // Lol no ur an object
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
                mob.WalkTo(Vector2.Normalize(Pos - mob.position), true);
                var Targ = mob.Target;
                if (Vector2.Distance(mob.position, Targ.position) < 1.5f)
                {
                    mob.Punch(Targ, game);
                }
            }
        }

        public Entity SpawnItem(Vector2 position, int Z, Block item,int c)
        {
            var Drop = Entity.CloneEntity(game._entityManager.entities[1], Vector2.Floor(position) + Vector2.One * 0.5f);
            Drop.TextureName = "null";

            var drop = item;
            Drop.Data = game._blockManager.GetBlockID(drop).ToString();

            if (item.Item)
            {
                Drop.Data = item.ID.ToString() + ";"+ c;
                Drop.name = "item";
            }
            else
            {
                Drop.name = "item3D";
                Drop.Data = item.ID.ToString() + ";1"+ c;
            }

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

        public void Die(Entity entity, List<Entity> list)
        {
            Random random = new Random();
            if (entity.ID <= -1) return; // Lol no ur an object
            for (int i = 0; i < 7; i++)
            {
                var part = new Particle()
                {
                    Position = entity.position + new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f),
                    TextureName = "Dust",
                    Texture = game._particleSystem.sprites[0],
                    lifeTime = (float)random.NextDouble() * 2,
                    size = (float)random.NextDouble() * 2f,
                    Color = Color.LightGray,
                    Velocity = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f) * 0.3f,
                    Acceleration = Vector2.Zero,
                    gravity = -0.02f
                };

                game._particleSystem.Particles.Add(part);
            }
            list.Add(SpawnItem(entity.position, (int)entity.Layer, game._blockManager.Blocks[2],1));
        }

        public void RenderAll(SpriteBatch SB, float Size, Vector2 Pos)
        {
            foreach (var entity in Workspace)
            {
                //if (Workspace.Count > 200)
                //{
                //    continue;
                //}
                entity.DrawEntity(SB, Size, Pos, game);
            }
        }

        public static List<Entity> LoadEntites(Game1 game1)
        {
            int id = 0;
            List<Entity> Entities = new List<Entity>();

            

            var Plr = new Entity(id++, "Player", "Mobs/Steve", 20)
            {
                gripIndex = 3,
                GripOffset = new Vector2(4, 12f),
                Ractangles = new List<Vector4>() // LimbShapes
                {
                    // Replace Vector4 with a Object that can hold the widths of all 4 sides of an entity

                    new Vector4(12, 8, 4, 12), // Right Arm
                    new Vector4(24, 8, 4, 12),// Body
                    new Vector4(8, 0, 8, 8), // Head
                    new Vector4(12, 20, 4, 12), // Left Arm
                    new Vector4(12, 44, 4, 12), // Right Leg
                    new Vector4(12, 32, 4, 12) // Left Leg
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

                    new Joint() //Left Arm
                    {

                        A = new Vector2(0, 4f),
                        B = new Vector2(0f, -4f),
                        A_Index = 1,
                        B_Index = 3
                    },

                    new Joint()// right leg
                    {
                        A = new Vector2(0, -8f),
                        B = new Vector2(0f, -4f),
                        A_Index = 1,
                        B_Index = 5
                    },

                    new Joint() // Right Arm
                    {

                        A = new Vector2(0, 4f),
                        B = new Vector2(0f, -4f),
                        A_Index = 1,
                        B_Index = 0
                    },

                    new Joint() //left arm
                    {
                        A = new Vector2(0, -8f),
                        B = new Vector2(0f, -4f),
                        A_Index = 1,
                        B_Index = 4
                    },
                },
                Animations = new()
                {
                    EntityAnimationService.GetAnimation("idle",0),
                    EntityAnimationService.GetAnimation("Running",0),
                    EntityAnimationService.LoadAnimations()[2],
                }
            };
            Plr.collisionBox = new CollisionBox() { Size = new Vector2(0.6f, 1.8f) };
            Plr.Texture = game1.Content.Load<Texture2D>(Plr.TextureName);
            Entities.Add(Plr);

            var item = new Entity(-1, "Item", "null", 100)
            {
                position = Vector2.Zero,
                collisionBox = new CollisionBox() { Size = new Vector2(2f, 2f) },
            };
            Entities.Add(item);

            var Zombie = new Entity(id++, "Zombie", "Mobs/Zombie", 20)
            {
                gripIndex = 3,
                GripOffset = new Vector2(4, 12f),
                Ractangles = new List<Vector4>() // LimbShapes
                {
                        // Replace Vector4 with a Object that can hold the widths of all 4 sides of an entity

                    new Vector4(12, 8, 4, 12), // Right Arm
                    new Vector4(24, 8, 4, 12),// Body
                    new Vector4(8, 0, 8, 8), // Head
                    new Vector4(12, 20, 4, 12), // Left Arm
                    new Vector4(12, 44, 4, 12), // Right Leg
                    new Vector4(12, 32, 4, 12) // Left Leg
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

                    new Joint() //Left Arm
                    {

                        A = new Vector2(0, 4f),
                        B = new Vector2(0f, -4f),
                        A_Index = 1,
                        B_Index = 3
                    },

                    new Joint()// right leg
                    {
                        A = new Vector2(0, -8f),
                        B = new Vector2(0f, -4f),
                        A_Index = 1,
                        B_Index = 5
                    },

                    new Joint() // Right Arm
                    {

                        A = new Vector2(0, 4f),
                        B = new Vector2(0f, -4f),
                        A_Index = 1,
                        B_Index = 0
                    },

                    new Joint() //left arm
                    {
                        A = new Vector2(0, -8f),
                        B = new Vector2(0f, -4f),
                        A_Index = 1,
                        B_Index = 4
                    },
                },
                Animations = new()
                {
                    EntityAnimationService.GetAnimation("idle_zombie",1),
                    EntityAnimationService.GetAnimation("Running_zombie",1),
                    EntityAnimationService.LoadAnimations()[2],
                }
            };
            Zombie.collisionBox = new CollisionBox() { Size = new Vector2(0.6f, 1.8f) };
            Zombie.Texture = game1.Content.Load<Texture2D>(Zombie.TextureName);
            Entities.Add(Zombie);
            var Pig = new Entity(id++, "Pig", "Mobs/Pig", 10)
            {
                gripIndex = -1,
                GripOffset = new Vector2(4, 12f),
                Ractangles = new List<Vector4>() // LimbShapes
                {
                        // Replace Vector4 with a Object that can hold the widths of all 4 sides of an entity
                          
                        /* 1,2
                           3,4*/


                        new Vector4(0,16,4,6), // Leg1
                        new Vector4(0,16,4,6), // Leg2
                        new Vector4(0,16,4,6), // Leg3
                        new Vector4(0,16,4,6), // Leg4
                        new Vector4(0,8,16,8), // Torso
                        new Vector4(17,0,9,8), // Head
                },
                Joints = new List<Joint>()
                {
                    new Joint() //1
                    {
                        A = new Vector2(6f, -7f),
                        B = new Vector2(0, 0),
                        A_Index = 4,
                        B_Index = 0,
                    },
                    new Joint() //2
                    {
                        A = new Vector2(6f, -7f),
                        B = new Vector2(0, 0),
                        A_Index = 4,
                        B_Index = 1,
                    },
                    new Joint() //3
                    {
                        A = new Vector2(-6f, -7f),
                        B = new Vector2(0, 0),
                        A_Index = 4,
                        B_Index = 2,
                    },
                    new Joint() //4
                    {
                        A = new Vector2(-6f, -7f),
                        B = new Vector2(0, 0),
                        A_Index = 4,
                        B_Index = 3,
                    },
                    new Joint() //Head
                    {
                        A = new Vector2(-10, 2f),
                        B = new Vector2(0f, 0f),
                        A_Index = 4,
                        B_Index = 5,
                    },

                    
                },
                Animations = new()
                {
                    EntityAnimationService.GetAnimation("idle",2),
                    EntityAnimationService.GetAnimation("pig_walk",2),
                }
            };
            Pig.collisionBox = new CollisionBox() { Size = new Vector2(0.6f, 1f) };
            Pig.Texture = game1.Content.Load<Texture2D>(Pig.TextureName);
            Entities.Add(Pig);

            var Falling_block = new Entity(-2, "Falling Block", "_block", 100)
            {
                position = Vector2.Zero,
                collisionBox = new CollisionBox() { Size = new Vector2(1f, 1f) },
                
            };


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
