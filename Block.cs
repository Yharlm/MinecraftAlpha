using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MinecraftAlpha
{
    public class Block
    {

        public string Name { get; set; }

        public Texture2D Texture { get; set; }
        public string TexturePath { get; set; }
    }

    public class BlockManager
    {
        public float BlockSize = 32;
        public List<Block> Blocks = new List<Block>();
        public BlockManager()
        {
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
            };
            return list;
        }

        
    }

}
