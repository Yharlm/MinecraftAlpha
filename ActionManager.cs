using System;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

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

            if (Zindex < 0 || Zindex > 9) { return; }
            if (block == null) return;
            TileGrid Tile = BlockManager.GetBlockAtPos(pos, Zindex, Chunks);

            if (block == null) return;
            if (block.Item)
            {
                if (Tile != null)
                {
                    if (Tile.ID != 0)
                    {
                        Block B = Game._blockManager.GetBlockAtTile(Tile);
                        if (B == null) return;
                        if (B.Interaction == null) return;
                        B.Interaction.Invoke(Tile, Game.Player.Plr, block);
                    }
                }
                


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




                    if (block.ammo == null && block.ammoTag == "") // Doesn't use ammo
                    {
                        Game._userInterfaceManager.amount -= 1;
                        if (Game._userInterfaceManager.amount <= 0)
                        {
                            Game._userInterfaceManager.selectedItem = null;

                        }
                    }
                    else
                    {

                        var ammo = Game.Player.FindItem(block.ammo.Name, block.ammoTag);

                        if (ammo != null && ammo.Count > 0)
                        {
                            ammo.Count -= 1;
                            if (ammo.Count <= 0)
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
            //Places block here
            block.Data = Game._blockManager.GetBlockByName(block.Name).Data;
            SetTile(Tile,block.Name,block.Data);
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

                block.Interaction.Invoke(Tile, Game.Player.Plr, block);
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

        public void SetTile(TileGrid tile,string name)
        {
            SetTile(tile, name, "");
        }
        public void SetTile(TileGrid tile, string name,string Data)
        {
            
            tile.ID = Game._blockManager.Blocks.FindIndex(b => b.Name == name);
            tile.Data = Data;
            tile.MinedHealth = 0;
            tile.MarkedForUpdate = true;
        }
        public void SpawnEntity(Vector2 Pos, string EntityName)
        {
            var ent = Game._entityManager.entities.Find(e => e.name.ToUpper() == EntityName.ToUpper());

            if (ent == null) { return; }
            ent = Entity.CloneEntity(ent,Pos);
            ent.position = Pos;
            

            Game._entityManager.Workspace.Add(ent);
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
                if (block.ItemDrop != null) drop = Game._entityManager.SpawnItem(pos, Zindex, block.ItemDrop, 1);
                else drop = Game._entityManager.SpawnItem(pos, Zindex, block, 1);
                if (drop != null)
                {
                    drop.IFrame = 0.1f;
                    Game._entityManager.Workspace.Add(drop);
                }


            }

        }
        public void Explosion(Vector3 pos, float power, bool GravityBlocks)
        {
            Vector2 Pos = new Vector2(pos.X, pos.Y);
            float Z = pos.Z;
            float radius = power;


            float radiusSquared = radius * radius;

            // Determine the bounding box to minimize iterations
            int minX = (int)Math.Floor(Pos.X - radius);
            int maxX = (int)Math.Ceiling(Pos.X + radius);
            int minY = (int)Math.Floor(Pos.Y - radius);
            int maxY = (int)Math.Ceiling(Pos.Y + radius);
            int minZ = (int)Math.Floor(Z - radius);
            int maxZ = (int)Math.Ceiling(Z + radius);

            int max = 1000;
            int count = 0;

            for (int n = 0; n < 40; n++)
            {
                Vector2 Point = new Vector2((float)random.NextDouble(), (float)random.NextDouble()) - Vector2.One / 2;





                var Smoke = new Particle()
                {
                    Position = Vector2.Normalize(Point) * power * (float)random.NextDouble() + new Vector2(pos.X, pos.Y),
                    TextureName = "ParticleSmokeEffect",
                    Texture = Game._particleSystem.sprites[0],
                    lifeTime = 1.4f + (float)random.NextDouble(),
                    size = (float)random.NextDouble() * 7,
                    Color = Color.White,
                    Velocity = Vector2.Zero,
                    Acceleration = new Vector2(0, -0.1f),
                    gravity = 0.0f
                };
                Game._particleSystem.Particles.Add(Smoke);
            }

            foreach (Entity ent in Game._entityManager.Workspace)
            {
                float dis = Vector2.Distance(Pos, ent.position);
                if (dis < radius + 2)
                {
                    ent.velocity.velocity = Vector2.Normalize(Pos - ent.position) * -radius * 10;
                    ent.TakeDamage(null, (int)power, 3);
                }
            }




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
                        var BLOCK = BlockManager.GetBlockAtPos(new Vector2(x, y) + Vector2.One / 2,z, Game.Chunks);
                        if (BLOCK == null) continue;
                        if (Game._blockManager.GetBlockAtTile(BLOCK).ID == 13)
                        {
                            Game._blockManager.GetBlockAtTile(BLOCK).Interaction.Invoke(BLOCK, null, Game._blockManager.getBlock("Flint and Steel"));
                        }


                        if (distance <= radiusSquared)
                        {
                            if (GravityBlocks)
                            {



                                if (max > count)
                                {
                                    
                                    var block = Game._entityManager.GravityBlock(new Vector2(x, y) + Vector2.One / 2, z, true);

                                    


                                    count++;
                                    if (block == null) continue;
                                    block.velocity.velocity = new Vector2(1, -2) * Vector2.Normalize(new Vector2(pos.X, pos.Y) - block.position) * -3 * radius;
                                    continue;
                                }

                            }
                            BreakBlock(new Vector2(x, y) + Vector2.One / 2, z, power * power);
                        }
                    }
                }
            }
        }
    }
}
