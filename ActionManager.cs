using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography;
using System.Linq;

namespace MinecraftAlpha
{
    public class Event
    {



        public string Name = "New Event";
        public string Description = "This is a new event";
        public Action Action;

        public Event(string name, string description, Action action)
        {
            Name = name;
            Description = description;
            Action = action;
        }
    }
    public class ActionManager
    {
        public Random random = new Random();
        public Game1 Game;
        public List<Event> Actions = new List<Event>();
        public ActionManager() { Actions = LoadActions(); }

        public List<Event> LoadActions()
        {
            var list = new List<Event>()
            {

                //Kills all entities
                new Event("KIll","", () => { Game.Entities.Clear(); }),
                // 
                new Event("LeftInteract","",() => {  })


            };
            return list;
        }

        public bool CheckAround(int x, int y, TileGrid[,] Map)
        {
            bool Placable = true;
            if (Map[y, x].ID == 0) Placable = false;
            if (Map[y, x + 1].ID == 0) Placable = false;
            if (Map[y, x - 1].ID == 0) Placable = false;
            if (Map[y + 1, x].ID == 0) Placable = false;
            if (Map[y - 1, x].ID == 0) Placable = false;
            return Placable;
        }
        public void PlaceBlock(int X, int Y)
        {
            var Grid = Game.World;
            //if(!CheckAround(X,Y,Grid)) continue;
            var ItemSelected = Game._userInterfaceManager.selectedItem;
            if (ItemSelected == null) return;
            if (Game._userInterfaceManager.amount > 0 && Grid[Y, X].ID == 0)
            {
                var BlockId = Game._blockManager.Blocks.IndexOf(ItemSelected); // Set to air

                Grid[Y, X].ID = BlockId;
                Grid[Y,X].MinedHealth = 0;
                Game._userInterfaceManager.amount -= 1;
            }
            if (Game._userInterfaceManager.amount <= 0)
            {
                Game._userInterfaceManager.selectedItem = null;

            }


        }
        public Block Lastblock = null;
        public void Interact(Vector2 WorldPos)
        {
            foreach (Entity entity in Game._entityManager.Workspace)
            {
                if (LogicsClass.IsInBounds(WorldPos, entity.collisionBox.Size))
                {
                    entity.Interaction.Action.Invoke();
                }
            }

            return;
            //Game.World[(int)WorldPos.Y,(int)WorldPos.X].ID = 0;
            TileGrid Grid = Game.World[(int)WorldPos.Y, (int)WorldPos.X];
            var block = Game._blockManager.Blocks[Game.World[(int)WorldPos.Y, (int)WorldPos.X].ID];
            if (block.Interaction != null)
            {
                
                block.Interaction.Invoke(Grid);
                Game._userInterfaceManager.LastUsedBlock = Grid;

            }
            
        }

        
        public void BreakBlock(int X, int Y)
        {
            var block = Game._blockManager.Blocks[Game.World[Y, X].ID];

            if (Game.World[Y, X].ID != 0)
            {
                if (block.Health > Game.World[Y, X].MinedHealth)
                {
                    
                    int x = random.Next(0, block.Texture.Width);
                    int y = random.Next(0, block.Texture.Height);
                    Game.World[Y, X].MinedHealth += 0.5f;


                    if (block.Health %0.2f == 0) return;



                    var part = new Particle() 
                    {
                        Position = (new Vector2(X + (float)x/ block.Texture.Width, Y + (float)y / block.Texture.Height)),
                        TextureName = "BlockMineEffect",
                        Texture = block.Texture,
                        lifeTime = 0.2f,
                        size = 0.4f,
                        Color = Color.White,
                        Rectangle = new Microsoft.Xna.Framework.Rectangle(x, y, 3, 3),
                        Velocity = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f),
                        Acceleration = new Vector2(0, -1f),
                        gravity = 0.1f

                    };



                    Game._particleSystem.Particles.Add(part);
                    return;
                }

                //foreach (var islot in Game._userInterfaceManager.windows[0].ItemsSlots)
                //{
                //    if (islot.Item != null && islot.Item != Game._blockManager.Blocks[Game.World[Y, X].ID])
                //    { continue; }
                //    else
                //    {
                //        islot.Item = Game._blockManager.Blocks[Game.World[Y, X].ID];
                //        Game.World[Y, X].MinedHealth = 0;
                //        for (int i = 0; i < 50;i++)
                //        {

                //            // Item Entity
                            
                //            int x = random.Next(0, block.Texture.Width);
                //            int y = random.Next(0, block.Texture.Height);
                //            //    Ractangle = new Microsoft.Xna.Framework.Rectangle(x,y,x+3,y+3);
                //            //    ParticleRactangleCHose = true;
                //            Game.World[Y, X].MinedHealth += 0.5f;
                //            if (block.Health % 0.2f == 0) return;
                //            var part = new Particle()
                //            {
                //                Position = (new Vector2(X + (float)x / block.Texture.Width, (float)Y)),
                //                TextureName = "BlockMineEffect",
                //                Texture = block.Texture,
                //                lifeTime = 0.5f,
                //                size = 0.4f,
                //                Color = Color.White,
                //                Rectangle = new Microsoft.Xna.Framework.Rectangle(x, y, 3, 3),
                //                Velocity = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f),
                //                Acceleration = new Vector2(0, -0f),
                //                gravity = 0.02f

                //            };
                //            Game._particleSystem.Particles.Add(part);

                //        }
                //        islot.Count += 1; break;


                //    }
                //}


                Game.World[Y, X].ID = 0;
                Game._entityManager.Workspace.Add(Entity.CloneEntity(Game._entityManager.entities[1], new Vector2((float)X , (float)Y )  + Vector2.One * 0.5f));
                Game._entityManager.Workspace.Last().TextureName = "null";
var drop = block;
                if(block.ItemDrop != null) drop = block.ItemDrop;
                Game._entityManager.Workspace.Last().Data =Game._blockManager.GetBlockID(block).ToString();
                
                Game._entityManager.Workspace.Last().Model3D = new Sprite3D(block.Texture, block.Texture, block.Texture, block.Texture);

            }

        }
        public void GetAction(string action)
        {
            foreach (var act in Actions)
            {
                if (act.Name == action)
                {
                    act.Action.Invoke();
                }
            }
        }
    }
}
