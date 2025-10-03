using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MinecraftAlpha
{
    class Player
    {
        public Entity Plr;
        public Cammera cam = new Cammera();
    }

    public class Cammera
    {
        public Vector2 position = new Vector2(530, -500);
        public Vector2 size { get; set; } = new Vector2(800, 600);

        public void RenderLayer(BlockManager blockManager,SpriteBatch _spriteBatch, int[,] Map,float layer)
        {
            var BlockSize = blockManager.BlockSize;
            var Camera = this;

            // Render the world based on the position and size
            _spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] != 0)
                    {
                        
                        var block = blockManager.Blocks[Map[i, j]];
                        
                        _spriteBatch.Draw(block.Texture, new Vector2(j * BlockSize, i * BlockSize) + Camera.position, null, Color.LightGray, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, 0f);
                        
                    }
                }
            }
            _spriteBatch.End();

        }
    }
}
