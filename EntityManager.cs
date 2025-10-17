using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftAlpha
{
    public class EntityManager
    {
        public EntityManager() { }
        public List<Entity> entities = new List<Entity>();
        public List<Entity> Workspace = new List<Entity>();
        
        public void Spawn(Entity entity)
        {
            Entity mobClone = new Entity(entity.ID,entity.name, entity.TextureName, entity.MaxHealth)
            { 
                Ractangles = entity.Ractangles,
                position = Vector2.One * 50,
                Joints = entity.Joints,
                collisionBox = new CollisionBox() { Size = entity.collisionBox.Size },
            };
            Workspace.Add(mobClone);

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
        public void RenderAll(SpriteBatch SB,float Size,Vector2 Pos)
        {
            foreach (var entity in Workspace)
            {
                entity.DrawEntity(SB, Size, Pos);
            }
        }
        public void LoadEntities(Game1 game)
        {
            entities = Entity.LoadEntites(game);
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
