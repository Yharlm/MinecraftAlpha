using System;
using System.Collections.Generic;
using System.Linq;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

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
        public void PlaceBlock(Vector2 pos, Block block)
        {
            int Zindex = (int)Game.Player.Plr.Layer;
            var Chunks = Game.Chunks;

            if(Zindex < 0 || Zindex > 9) { return; }
            if (block == null) return;
            TileGrid Tile = BlockManager.GetBlockAtPos(pos, Zindex, Chunks);
            if (Tile == null)
            {
                
                var ChunkNot = BlockManager.GetChunkAtPos(pos);
                Chunks.Add(new(ChunkNot[0], ChunkNot[1]));
                Tile = BlockManager.GetBlockAtPos(pos, Zindex, Chunks);
            }




            if (Tile.ID != 0)
            {
                return;
            }

            Tile.ID = Game._blockManager.Blocks.IndexOf(block);
            Tile.MinedHealth = 0;
            if (!Game.creativeMode)
            {
                Game._userInterfaceManager.amount -= 1;
                if (Game._userInterfaceManager.amount <= 0)
                {
                    Game._userInterfaceManager.selectedItem = null;

                }
            }


            //if(!CheckAround(X,Y,Grid)) continue;
            //var ItemSelected = Game._userInterfaceManager.selectedItem;
            //if (ItemSelected == null) return;
            //if (Game._userInterfaceManager.amount > 0 && Grid[Y, X].ID == 0)
            //{
            //    var BlockId = Game._blockManager.Blocks.IndexOf(ItemSelected); // Set to air

            //    Grid[Y, X].ID = BlockId;
            //    Grid[Y,X].MinedHealth = 0;
            //    Game._userInterfaceManager.amount -= 1;
            //}
            //if (Game._userInterfaceManager.amount <= 0)
            //{
            //    Game._userInterfaceManager.selectedItem = null;

            //}


        }

        public Block Lastblock = null;
        public void Interact(Vector2 Pos)
        {
            foreach (Entity entity in Game._entityManager.Workspace)
            {
                if (LogicsClass.IsInBounds(Pos, entity.collisionBox.Size))
                {
                    entity.Interaction.Action.Invoke();
                }
            }


            //Game.World[(int)WorldPos.Y,(int)WorldPos.X].ID = 0;
            TileGrid Tile = BlockManager.GetLastBlockAtPos(Pos, Game.Chunks);
            if (Tile == null) return;
            var block = Game._blockManager.Blocks[Tile.ID];
            if (block.Interaction != null)
            {

                block.Interaction.Invoke(Tile);
                Game._userInterfaceManager.LastUsedBlock = Tile;

            }

        }


        public void BreakBlock(Vector2 Pos)
        {
            int Zindex = (int)Game.Player.Plr.Layer;
            var Tile = BlockManager.GetBlockAtPos(Pos, Zindex, Game.Chunks);
            if (Tile == null) { return; }
            var block = Game._blockManager.Blocks[Tile.ID];

            if (Tile.ID != 0)
            {
                if (block.Health > Tile.MinedHealth)
                {

                    int x = random.Next(0, block.Texture.Width);
                    int y = random.Next(0, block.Texture.Height);
                    if(Game.creativeMode) { Tile.MinedHealth += 0.5f; }
                    Tile.MinedHealth += 0.5f;


                    if (block.Health % 0.2f == 0 ) return;



                    var part = new Particle()
                    {
                        Position = (new Vector2(float.Floor(Pos.X) + (float)x / block.Texture.Width, float.Floor(Pos.Y) + (float)y / block.Texture.Height)),
                        TextureName = "BlockMineEffect",
                        Texture = block.Texture,
                        lifeTime = 0.2f,
                        size = 0.4f,
                        Color = block.Color,
                        Rectangle = new Microsoft.Xna.Framework.Rectangle(x, y, 3, 3),
                        Velocity = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f),
                        Acceleration = new Vector2(0, -1f),
                        gravity = 0.1f

                    };



                    Game._particleSystem.Particles.Add(part);
                    return;
                }
                Tile.MinedHealth = 0;
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


                Tile.ID = 0;
                Game._entityManager.Workspace.Add(Entity.CloneEntity(Game._entityManager.entities[1], Vector2.Floor(Pos) + Vector2.One * 0.5f));
                Game._entityManager.Workspace.Last().TextureName = "null";

                var drop = block;
                if (block.ItemDrop != null) drop = block.ItemDrop;
                Game._entityManager.Workspace.Last().Data = Game._blockManager.GetBlockID(drop).ToString();
                Game._entityManager.Workspace.Last().Layer = Zindex;
                Game._entityManager.Workspace.Last().Model3D = new Sprite3D(drop.Texture, drop.Texture, drop.Texture, drop.Texture);

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
