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
    class EntityManager
    {
        public EntityManager() { }
        public List<Entity> entities = new List<Entity>();
        public List<Entity> Workspace = new List<Entity>();
        
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
        public void LoadEntities()
        {
            entities = Entity.LoadEntites();
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
                new Joint()
                {

                    A = new Vector2(0, 8f),
                    B = new Vector2(0f, 4f),
                    A_Sprite = Entities[0].Sprites[1],
                    B_Sprite = Entities[0].Sprites[2]
                });
            Entities[0].Joints.Add(
                new Joint()
                {
                    A = new Vector2(0, 0f),
                    B = new Vector2(0f, 0f),
                    A_Sprite = Entities[0].Sprites[0],
                    B_Sprite = Entities[0].Sprites[1]
                });
            Entities[0].Joints.Add(
                new Joint()
                {
                    A = new Vector2(0, 0f),
                    B = new Vector2(0f, 0f),
                    A_Sprite = Entities[0].Sprites[0],
                    B_Sprite = Entities[0].Sprites[3]
                });
            Entities[0].Joints.Add(
                new Joint()
                {
                    A = new Vector2(0, 0f),
                    B = new Vector2(0f, 0f),
                    A_Sprite = Entities[0].Sprites[0],
                    B_Sprite = Entities[0].Sprites[4]
                });
            Entities[0].Joints.Add(
                new Joint()
                {
                    A = new Vector2(0, 0f),
                    B = new Vector2(0f, 0f),
                    A_Sprite = Entities[0].Sprites[0],
                    B_Sprite = Entities[0].Sprites[5]
                });
        }

        

    }
}
