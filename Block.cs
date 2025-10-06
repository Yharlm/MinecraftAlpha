using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MinecraftAlpha
{
    public class Block
    {

        public string Name { get; set; }
        public string Description { get; set; }

        public bool Placable = true;

        

        public Texture2D Texture { get; set; }
        public string TexturePath { get; set; }

        public Action Interaction = null; 
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
               new Block { Name = "Air", TexturePath = "air" },
               new Block { Name = "Dirt", TexturePath = "dirt" },
               new Block { Name = "Grass", TexturePath = "grass_block_side" },
               new Block { Name = "Stone", TexturePath = "stone" },
               new Block { Name = "Wood", TexturePath = "oak_planks" },
               //new Block { Name = "Chest", TexturePath = "chest" },
            };
            return list;
        }
        public Block getBlock(string Name)
        {
            return Blocks.Find(x => x.Name == "");
        }
        public void LoadActions()
        {
            getBlock("Chest").Interaction = () =>
            {
                Game._userInterfaceManager.windows[0].Visible = true;

            };

        }

        
    }

    public class TileGrid
    {
        public TileGrid() { }
        public int ID = 0;
        public int state = 0;
        public float brightness = 0;
        public string Data { get; set; } = string.Empty;

    }

}
