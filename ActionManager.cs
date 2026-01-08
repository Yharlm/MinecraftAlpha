using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
            if (block == null) return;
            if (block.Item)
            {
                if (block.Interaction == null) return;

                if (block.ChargeMax > block.Charge)
                {
                    block.Charge += 0.1f;
                    
                    return;
                }
                if (!block.CanFire && block.ChargeMax < block.Charge)
                {
                    block.Interaction.Invoke(null, Game.Player.Plr, block);
                    return;
                }
                if (block.UseTimeMax > block.UseTime)
                {
                    block.UseTime += 0.1f;
                    return;
                }
                if (block.CooldownMax > block.Cooldown)
                {
                    return;
                }
                //block.Charge = 0;
                block.UseTime = 0;
                block.Cooldown = 0;
                //block.Charge = 0;
                //block.CanFire = false;


                if (!Game.creativeMode) // Use cost
                {
                    



                    if(block.ammo == null && block.ammoTag == "") // Doesn't use ammo
                    {
                        Game._userInterfaceManager.amount -= 1;
                        if (Game._userInterfaceManager.amount <= 0)
                        {
                            Game._userInterfaceManager.selectedItem = null;

                        }
                    }
                    else 
                    {
                        
                        var ammo = Game.Player.FindItem(block.ammo.Name,block.ammoTag);

                        if(ammo != null && ammo.Count > 0)
                        {
                            ammo.Count -= 1;
                            if(ammo.Count <= 0)
                            {
                                ammo.Item = null;
                            }
                        }
                        else
                        {
                            return; // no ammo found
                        }
                    }
                    
                }
                block.Interaction.Invoke(null, Game.Player.Plr, block);
                
                return;
            }
            int Zindex = (int)Game.Player.Plr.Layer;
            var Chunks = Game.Chunks;

            if (Zindex < 0 || Zindex > 9) { return; }
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
                if (entity.Interaction == null) continue;
                if (LogicsClass.IsInBounds(Pos, entity.position, entity.collisionBox.Size))
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

                block.Interaction.Invoke(Tile, Game.Player.Plr,block);
                Game._userInterfaceManager.LastUsedBlock = Tile;

            }

        }

        public void DeleteBlocksSphere(Vector2 Pos, float Z, float radius)
        {





            float radiusSquared = radius * radius;

            // Determine the bounding box to minimize iterations
            int minX = (int)Math.Floor(Pos.X - radius);
            int maxX = (int)Math.Ceiling(Pos.X + radius);
            int minY = (int)Math.Floor(Pos.Y - radius);
            int maxY = (int)Math.Ceiling(Pos.Y + radius);
            int minZ = (int)Math.Floor(Z - radius);
            int maxZ = (int)Math.Ceiling(Z + radius);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        // Calculate distance from center to current point
                        float dx = x - Pos.X;
                        float dy = y - Pos.Y;
                        float dz = z - Z;
                        float distance = dx * dx + dy * dy + dz * dz;
                        // If distance squared is <= radius squared, it's inside the sphere
                        if (distance <= radiusSquared)
                        {
                            BreakBlock(new Vector2(x, y) + Vector2.One / 2, z, 3);
                        }
                    }
                }
            }

        }
        public void BreakBlock(Vector2 Pos, float Z, float Dmg)
        {







            int Zindex = (int)Z;
            var Tile = BlockManager.GetBlockAtPos(Pos, Zindex, Game.Chunks);
            if (Tile == null) { return; }
            var block = Game._blockManager.Blocks[Tile.ID];
            int x = random.Next(0, block.Texture.Width);
            int y = random.Next(0, block.Texture.Height);
            var pos = (new Vector2(float.Floor(Pos.X) + (float)x / block.Texture.Width, float.Floor(Pos.Y) + (float)y / block.Texture.Height));
            if (Tile.ID != 0)
            {
                if (block.Health > Tile.MinedHealth)
                {


                    if (Game.creativeMode) { Tile.MinedHealth += 2f; } // Fix this later when you want Creative to not matter for explosives

                    Tile.MinedHealth += Dmg;


                    if (block.Health % 0.2f == 0) return;



                    var part = new Particle()
                    {
                        Position = pos,
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

                for (int i = 0; i < 15; i++)
                {
                    x = random.Next(0, block.Texture.Width);
                    y = random.Next(0, block.Texture.Height);
                    var part = new Particle()
                    {
                        Position = new Vector2(float.Floor(Pos.X) + (float)x / block.Texture.Width, Pos.Y),
                        TextureName = "BlockMineEffect",
                        Texture = block.Texture,
                        lifeTime = 0.6f,
                        size = 0.4f,
                        Color = block.Color,
                        Rectangle = new Microsoft.Xna.Framework.Rectangle(x, y, 4, 4),
                        Velocity = new Vector2((float)random.NextDouble() - 0.5f, -(float)random.NextDouble() - 1),
                        Acceleration = new Vector2(0, 0.1f),
                        gravity = 0.1f

                    };



                    Game._particleSystem.Particles.Add(part);
                }


                Tile.MinedHealth = 0;

                Tile.ID = 0;
                Entity drop = null;
                if (block.ItemDrop != null) drop = Game._entityManager.SpawnItem(pos, Zindex, block.ItemDrop,1);
                else drop = Game._entityManager.SpawnItem(pos, Zindex, block,1);
                if (drop != null)
                {
                    drop.IFrame = 0.1f;
                    Game._entityManager.Workspace.Add(drop);
                }


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
