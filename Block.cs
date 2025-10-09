using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MinecraftAlpha
{
    public class Block
    {
        public float Health = 30;
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Placable = true;

        public int DefaultState = 0; // Default state of the block


        public Texture2D Texture { get; set; }
        public string TexturePath { get; set; }

        public Action<TileGrid> Interaction = null; 
        public Action<TileGrid> Update = null;
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
               new Block { Name = "Dirt", TexturePath = "dirt",Health = 30 },
               new Block { Name = "Grass", TexturePath = "grass_block_side",Health = 30 },
               new Block { Name = "Stone", TexturePath = "stone" ,Health = 100},
               new Block { Name = "Wood", TexturePath = "oak_planks" ,Health = 50},
               //new Block { Name = "Wood", TexturePath = "oak_planks" },
               //new Block { Name = "Wood", TexturePath = "oak_planks" },
               new Block { Name = "Chest", TexturePath = "ChestTesting" ,Interaction = null},
            };
            return list;
        }
        public Block getBlock(string Name)
        {
            return Blocks.Find(x => x.Name == "");
        }
        public void LoadActions()
        {
            Blocks[5].Interaction = (Pos) =>
            {
                
                string Items = Pos.Data;
                var Window = Game._userInterfaceManager.windows[1];
                Window.Visible = !Window.Visible;
                foreach (var slot in Window.ItemsSlots)
                {
                    slot.Item = null;
                    slot.Count = 0;
                }
                foreach (var item in Items.Split(','))
                {
                    if (item == "")
                    {  continue; }
                    var a = item.Split(':');
                    var Slot = Window.ItemsSlots[int.Parse(a[0])];
                    Slot.Item = Blocks[int.Parse(a[1])];
                    Slot.Count = int.Parse(a[2]);
                    
                }
            };
            Blocks[5].Update = (Pos) =>
            {
                var Window = Game._userInterfaceManager.windows[1];
                string Data = "";

                foreach (var slot in Window.ItemsSlots)
                {
                    if (slot.Item != null)
                    {
                        Data += $"{slot.ID}:{GetBlockID(slot.Item)}:{slot.Count},";
                    }
                }
                Pos.Data = Data;


            };

        }

        public int GetBlockID(Block block)
        {
            return Blocks.IndexOf(block);
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

    }

    public class TileGrid
    {
        public TileGrid() { }
        public int ID = 0;
        public int state = 0;
        public float brightness =0;
        public float LightSource = 0;
        public float MinedHealth = 0; // How much health has been mined from this block
        public string Data { get; set; } = string.Empty;

    }


    public class Structure()
    {
        public int Width = 0;
        public int Height = 0;

        public TileGrid[,] Layout;

    }

    
}
