using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MinecraftAlpha
{
    public class EntityManager : Game1
    {
        public EntityManager() { }
        
        public List<Entity> Workspace = new List<Entity>();

        

        public static List<Mob> LoadMobs()
        {
            int id = 0;
            List<Mob> Mobs = new List<Mob>()
            {
                new Mob(id++,"Player","steve")
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





                },
                new Mob(id++,"Zombie","steve")
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





                },
                new Mob(-1,"Item","null")
                {

                    
                    collisionBox = new CollisionBox() { Size = new Vector2(2f,2f) },

                }



            };

            return Mobs;




        }

        static public Entity Spawn(Mob Template)
        {
            Entity mobClone = new Entity()
            {
                Texture = Template.Texture,
                position = Vector2.Zero,
                PlayingAnimations = new List<EntityAnimation>(),
                 = Template.collisionBox

            };
            return mobClone;

        }

        public void ItemDrop(Vector2 position, Block item)
        {
            Entity entity = new Entity(-1, item.Name, item.TexturePath, 1000)
            {

                position = position,
                collisionBox = new CollisionBox(),



            };
        }
        public void UpdateAll()
        {
            foreach (var entity in Workspace)
            {

            }
        }
        public void RenderAll(SpriteBatch SB, float Size, Vector2 Pos)
        {
            foreach (var entity in Workspace)
            {
                entity.DrawEntity(SB, Size, Pos);
            }
        }
        public void LoadEntities(Game1 game)
        {
            entities = EntityManager.Spawn();
            //foreach (var entity in entities)
            //{

            //    Sprite.LoadSprites(contentManager, entity);

            //}
        }
        public void LoadSprites(ContentManager contentManager)
        {
            foreach (var entity in entities)
            {
                Sprite.LoadSprites(contentManager, entity);
            }
        }
        public void LoadJoints()
        {
            var Entities = entities;

            Entities[0].Joints.Add(
                new Joint() //Head
                {

                    A = new Vector2(0, 8f),
                    B = new Vector2(0f, 2f),
                    A_Sprite = Entities[0].Sprites[1],
                    B_Sprite = Entities[0].Sprites[2]
                });
            Entities[0].Joints.Add(//LeftArm
                new Joint()
                {
                    orientation = 180f,
                    A = new Vector2(0, 4f),
                    B = new Vector2(0f, 4f),
                    A_Sprite = Entities[0].Sprites[1],
                    B_Sprite = Entities[0].Sprites[3]
                });
            Entities[0].Joints.Add(//Lleg
                new Joint()
                {
                    A = new Vector2(0, -8f),
                    B = new Vector2(0f, -4f),
                    A_Sprite = Entities[0].Sprites[1],
                    B_Sprite = Entities[0].Sprites[5]
                });
            Entities[0].Joints.Add(//rightArm
                new Joint()
                {
                    orientation = 0,
                    A = new Vector2(0, 4f),
                    B = new Vector2(0f, -4f),
                    A_Sprite = Entities[0].Sprites[1],
                    B_Sprite = Entities[0].Sprites[0]
                });
            Entities[0].Joints.Add(//Rleg
                new Joint()
                {
                    A = new Vector2(0, -8f),
                    B = new Vector2(0f, -4f),
                    A_Sprite = Entities[0].Sprites[1],
                    B_Sprite = Entities[0].Sprites[4]
                });
        }



    }
}
