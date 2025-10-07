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
        public bool Jumping = false;
    }

    public class Cammera
    {
        public Vector2 position = new Vector2(530, -500);
        public Vector2 size { get; set; } = new Vector2(800, 600);

        public void RenderLayer(BlockManager blockManager,SpriteBatch _spriteBatch, TileGrid[,] Map,float layer,Vector2 pos)
        {
            var BlockSize = blockManager.BlockSize;
            var Camera = this;

            // Render the world based on the position and size
            _spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            for (int i = (int)pos.Y - 20; i < Map.GetLength(0) && i < (int)pos.Y + 20; i++)
            {
                for (int j = (int)pos.X - 20; j < Map.GetLength(1) && j< (int)pos.X + 20; j++)
                {
                    if (Map[i, j].ID != 0)
                    {
                        float Light = Map[i, j].brightness;
                        var block = blockManager.Blocks[Map[i, j].ID];
                        
                        _spriteBatch.Draw(block.Texture, new Vector2(j * BlockSize, i * BlockSize) + Camera.position, null, Color.White, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, 0f);
                        
                    }
                }
            }
            _spriteBatch.End();

        }
    }
}
