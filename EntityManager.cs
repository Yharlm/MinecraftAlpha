using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MinecraftAlpha
{
    public class EntityManager
    {
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
                    }
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
                foreach(var joint in entity.Joints)
                {
                    
                    joint.A_Sprite = entity.Sprites[joint.A_Index];
                    joint.B_Sprite = entity.Sprites[joint.B_Index];
                }
            }

        }
        public static void LoadJoins(Entity example,Entity clone)
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
