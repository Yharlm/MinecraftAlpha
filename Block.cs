using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MinecraftAlpha
{
    

    public class Block
    {
        public int ID = 0;
        public float Health = 30;
        public string Tag = "";
        public string Name { get; set; }
        public string Description { get; set; }

        public Block ItemDrop = null;

        public bool Transparent = false;

        public int DefaultState = 0; // Default state of the block


        //Tool stuff
        public float Durrability = -1; // How much damage the block can take before breaking
        public int ItemID = 0; // Id ot the item in atlas
        public bool Placable = true;
        




        


        public Color Color = Color.White;
        public Texture2D Texture { get; set; }
        public string TexturePath { get; set; }


        public bool Item = false;
        public int MineLevel = 0; // How fast the block can be mined
        public float Damage = 2; // How fast the block can be mined
        public float Grip = 0f;
        
        public bool consumable = true;
        public Block ammo = null;
        public string ammoTag = "";
        //Constants for use types
        public float ChargeMax = 0f; // Time taken to charge the item
        //public float ChargeMin = 0f; // Time taken to charge the item

        public float CooldownMax = 0f;//cooldown after using the item
        public float UseTimeMax = 0f; // Time taken to use/interact with the block
        //Dynamic use variables
        public float Charge = 0f;
        public bool CanFire = false;
        public float Cooldown = 0f;
        public float UseTime = 0f;

        public Action<TileGrid,Entity,Block> Interaction = null;
        
        





        public Action<TileGrid> Update = (tile) => { };
        public Action<TileGrid> OnCollide = (Grid) => { };
    }

    public class BlockManager
    {
        public Game1 Game;
        public float BlockSize = 32;
        public List<Block> Blocks = new List<Block>();
        public BlockManager(Game1 game1)
        {
            Game = game1;
            Blocks = LoadBlocks();
        }
        public List<int[,]> World; // List of different layers of the world
        public static List<Block> LoadBlocks()
        {
            //Create Blocks here
            var list = new List<Block>()
            {

                new Block { Name = "Air", TexturePath = "air" ,Health = 1000},
                new Block { Name = "Dirt", TexturePath = "dirt",Health = 30, Tag="dirt"},
                new Block { Name = "Grass", TexturePath = "grass_block_side",Health = 30 },
                new Block { Name = "Cobblestone", TexturePath = "cobblestone", Health = 100,},
                new Block { Name = "Stone", TexturePath = "stone" ,Health = 100},
                new Block { Name = "Wood", TexturePath = "oak_planks" ,Health = 60},
                new Block { Name = "Sand", TexturePath = "sand" ,Health = 30},
                new Block { Name = "Chest", TexturePath = "ChestTesting" ,Interaction = null,Transparent = true},
                new Block { Name = "Crafting Table", TexturePath = "crafting_table_front" ,Health = 60, Interaction = null},
                new Block { Name = "Log", TexturePath = "oak_log", Health = 60},
                new Block { Name = "Leaves", TexturePath = "oak_leaves", Health = 13,Color = Color.SeaGreen,Transparent = true},
                new Block { Name = "Glass block", TexturePath = "glass", Health = 4,Transparent = true},
                new Block { Name = "Tnt block", TexturePath = "tnt_side", Health = 2,Transparent = false},
                new Block { Name = "Apple", TexturePath = "_item", Item = true,Placable = false,ItemID = 0,UseTimeMax = 3},
                new Block { Name = "Stick", TexturePath = "_item", Item = true,Placable = false,ItemID = 199},
                new Block { Name = "Wooden Pickaxe", TexturePath = "_item", Item = true,Placable = false,ItemID = 210,Damage = 0.4f,Tag="Pickaxe",MineLevel = 1},
                new Block { Name = "Stone Pickaxe", TexturePath = "_item", Item = true,Placable = false,ItemID = 202,Damage = 0.6f,Tag="Pickaxe",MineLevel = 2},
                new Block { Name = "Iron Pickaxe", TexturePath = "_item", Item = true,Placable = false,ItemID = 63,Damage = 0.7f,Tag="Pickaxe",MineLevel = 3},
                new Block { Name = "Diamond Pickaxe", TexturePath = "_item", Item = true,Placable = false,ItemID = 101,Damage = 0.8f,Tag="Pickaxe",MineLevel = 4},
                new Block { Name = "Bow", TexturePath = "_item", Item = true,Placable = false,ItemID = 36,Damage = 3f,Tag="Bow",Grip = 90f,ChargeMax = 4},

            };

            for (int i = 0; i < list.Count; i++)
            {
                list[i].ID = i;
            }



            return list;
        }
        public Block getBlock(string Name)
        {
            var blocks = Blocks;
            return blocks.Find(x => x.Name == Name);
        }

        public static Block getBlocks(string Name)
        {
            var blocks = LoadBlocks();
            return blocks.Find(x => x.Name == Name);
        }
        
        public void LoadActions()
        {

            getBlock("Bow").Interaction = (Pos, ent,item) =>
            {

                int[] sprites = { 36, 51, 4,20};
                if (!item.CanFire && item.ChargeMax < item.Charge)
                {
                    if(item.Charge > 6)
                    {
                        return;
                    }
                    int I = (int)item.Charge-2;
                    item.Charge += 0.03f;
                    Game.Player.DisplayID = sprites[I];//make this change itemslot isntead



                    return;
                }
                Entity Arrow = new Entity(-3,"Arrow", "_projectile", 3) { Texture = Game.Content.Load<Texture2D>("Projectiles/Arrow"), };
                Arrow.collisionBox = new CollisionBox() { Size = new Vector2(0.5f) };
                Arrow.position = Game.Player.Plr.position;
                Arrow.Mass = 0.4f;
                Arrow.velocity.velocity = Vector2.Normalize(Game.WorldMousePos - Game.Player.Plr.position)*item.Charge* item.Charge/2;
                Arrow.Target = ent;
                Game._entityManager.Workspace.Add(Arrow);
                Game.Player.DisplayID = -1;
                item.Charge = 0f;
                item.CanFire = false;

            };

            getBlock("Apple").Interaction = (Pos,ent,item) =>
            {
                // Heal Player
                Game.Player.Plr.Health += 3;
                // Consume the apple

                if (Game.Player.Plr.Health > Game.Player.Plr.MaxHealth)
                {
                    Game.Player.Plr.Health = Game.Player.Plr.MaxHealth;
                }

            };

            getBlock("Stone").ItemDrop = getBlock("Cobblestone");
            getBlock("Leaves").ItemDrop = getBlock("Air");
            getBlock("Grass").ItemDrop = getBlock("Dirt");

            getBlock("Sand").Update = (Pos) =>
            {
                var pos = GetPosAtBlock(Pos, Game.Chunks);
                
                
                Game._entityManager.GravityBlock(new Vector2(pos.X,pos.Y),(int)pos.Z,true);
                   




            };
            getBlock("Chest").Interaction = (Pos, user,Item) =>
            {

                string Items = Pos.Data;
                var Window = Game._userInterfaceManager.windows[1];
                Window.Visible = !Window.Visible;
                foreach (var slot in Window.ItemSlots)
                {
                    slot.Item = null;
                    slot.Count = 0;
                }
                foreach (var item in Items.Split(','))
                {
                    if (item == "")
                    { continue; }
                    var a = item.Split(':');
                    var Slot = Window.ItemSlots[int.Parse(a[0])];
                    Slot.Item = Blocks[int.Parse(a[1])];
                    Slot.Count = int.Parse(a[2]);

                }
            };
            getBlock("Chest").Update = (Pos) =>
            {
                var Window = Game._userInterfaceManager.windows[1];
                string Data = "";

                var block = Pos;


                foreach (var slot in Window.ItemSlots)
                {
                    if (slot.Item != null)
                    {
                        Data += $"{slot.ID}:{GetBlockID(slot.Item)}:{slot.Count},";
                    }
                }
                block.Data = Data;


            };
            getBlock("Crafting Table").Interaction = (Pos, user,item) =>
            {

                var Window = Game._userInterfaceManager.windows[4];
                Window.Visible = !Window.Visible;

            };
            //getBlock("Crafting Table").Update = (Pos) =>
            //{

            //    var Window = Game._userInterfaceManager.windows[4];
            //    var ItemSlots = Window.ItemSlots;
            //    foreach (CraftingRecipe Recipe in Game._RecipeManager.Recipes)
            //    {

            //        if (Recipe.RecipeGrid.GetLength(0) == 3)
            //        {
            //            if (Recipe.CheckRecipe(new ItemSlot[,]
            //        {
            //                { ItemSlots[0], ItemSlots[1],ItemSlots[2] },
            //                { ItemSlots[3], ItemSlots[4],ItemSlots[5] },
            //                { ItemSlots[6], ItemSlots[7],ItemSlots[8] },
            //        }))
            //            {
            //                ItemSlots[9].Item = Recipe.item.Item;
            //                ItemSlots[9].Count = Recipe.item.Count;
            //                break;

            //            }
            //            else
            //            {
            //                ItemSlots[9].Count = 0;
            //                ItemSlots[9].Item = null;
            //            }
            //        }
            //        else
            //        {

            //        }
            //        //if (ItemSlots[4].Item != null) break;


            //    }


            //};


        }

        public int GetBlockID(Block block)
        {
            return Blocks.IndexOf(block);
        }
        public Block GetBlockByName(string name)
        {
            return Blocks.Find(x => x.Name == name);
        }

        public ItemSlot[,] RandomiseLoot()
        {
            Random random = new Random();
            ItemSlot[,] Chest = new ItemSlot[3, 9];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (random.Next(0, 100) < 20) // 20% chance to spawn an item
                    {
                        int itemId = random.Next(1, 6); // Assuming item IDs range from 1 to 5
                        int amount = random.Next(1, 65); // Random amount between 1 and 64
                        Chest[i, j] = new ItemSlot { Item = Blocks[itemId], Count = amount };
                    }
                }
            }
            return Chest;
        }
        public static void Makechunk(Vector2 pos, List<Chunk> Chunks)
        {



            TileGrid Tile = BlockManager.GetBlockAtPos(pos, Chunks);
            if (Tile == null)
            {

                var ChunkNot = BlockManager.GetChunkAtPos(pos);
                Chunks.Add(new(ChunkNot[0], ChunkNot[1]));
                Tile = BlockManager.GetBlockAtPos(pos, Chunks);
            }
        }
        public static TileGrid GetBlockAtPos(Vector2 pos, List<Chunk> Chunks)
        {
            return GetBlockAtPos(pos, 9, Chunks);
        }

        public static Vector3 GetPosAtBlock(TileGrid tile, List<Chunk> Chunks)
        {
            foreach (Chunk C in Chunks)
            {
                
                
                for (int z = 0; z < 10; z++)
                {
                    for (int y = 0; y < C.Tiles.GetLength(1); y++)
                    {
                        for (int x = 0; x < C.Tiles.GetLength(2); x++)
                        {
                            if (C.Tiles[z, y, x] == tile)
                            {
                                int worldX = C.x + x;
                                int worldY = C.y + y;
                                if (C.x < 0)
                                {
                                    worldX = (C.x + 1) * C.Tiles.GetLength(2) - (C.Tiles.GetLength(2) - x);
                                   
                                }
                                if (C.y < 0)
                                {
                                    worldY = (C.y + 1) * C.Tiles.GetLength(1) - (C.Tiles.GetLength(1) - y);

                                }



                                return new Vector3(worldX, worldY, z);


                            }
                        }
                    }
                }
            }
            return Vector3.Zero; // Return zero vector if tile not found
        
        }
        public Block GetBlockAtTile(TileGrid tile)
        {
            if (tile == null) return null;
            return Blocks[tile.ID];
        }


        public static TileGrid GetBlockAtPos(Vector2 pos, int z, List<Chunk> Chunks)
        {
            if (z < 0 || z > 9) return null;
            TileGrid Tile = null;
            if (Chunks.Count == 0) return null;

            int size = Chunks[0].Tiles.GetLength(1);
            int ChunkX = (int)Math.Ceiling((pos.X / size));
            int ChunkY = (int)Math.Ceiling((pos.Y / size));





            foreach (Chunk C in Chunks)
            {
                if (C.x == ChunkX && C.y == ChunkY)
                {
                    int x = (int)(pos.X % size);
                    int y = (int)(pos.Y % size);

                    if (pos.X < 0)
                    {
                        x = size - 1 - Math.Abs(x);
                    }
                    if (pos.Y < 0)
                    {
                        y = size - 1 - Math.Abs(y);
                    }

                    Tile = C.Tiles[z, y, x];
                }

            }


            return Tile;
        }

        public static TileGrid GetLastBlockAtPos(Vector2 pos, List<Chunk> Chunks)
        {

            TileGrid Tile = GetBlockAtPos(pos, Chunks);
            if (Tile == null) return null;
            for (int i = 0; i < 10; i++)
            {

                if (GetBlockAtPos(pos, i, Chunks).ID != 0)
                {
                    Tile = GetBlockAtPos(pos, i, Chunks);
                }
            }

            return Tile;
        }



        public static int[] GetChunkAtPos(Vector2 pos)
        {
            int size = 32;
            int ChunkX = (int)Math.Ceiling((pos.X / size));
            int ChunkY = (int)Math.Ceiling((pos.Y / size));
            return [ChunkX, ChunkY];
        }

    }

    public class TileGrid
    {
        public Vector3 pos;
        public TileGrid() { }
        public int ID = 0;
        public int state = 0;
        public float brightness = 0;
        public float LightSource = 0;
        public float MinedHealth = 0; // How much health has been mined from this block
        public string Data { get; set; } = string.Empty;

        public 
    }

    public class Items
    {
        public Texture2D Atlas;
        public int itemSize = 16; // Size of each item in the atlas


        public void DrawItem(SpriteBatch _spriteBatch, Vector2 position, int itemID, float scale)
        {
            int itemSize = this.itemSize;
            int itemsPerRow = Atlas.Width / itemSize;

            int row = itemID / itemsPerRow;
            int column = itemID % itemsPerRow;

            Rectangle sourceRectangle = new Rectangle(column * itemSize, row * itemSize, itemSize, itemSize);
            _spriteBatch.Draw(Atlas, position, sourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public Rectangle GetRactangle(int itemID)
        {
            int itemSize = this.itemSize;
            int itemsPerRow = Atlas.Width / itemSize;

            int row = itemID / itemsPerRow;
            int column = itemID % itemsPerRow;

            return new Rectangle(column * itemSize, row * itemSize, itemSize, itemSize);

        }

    }





}
