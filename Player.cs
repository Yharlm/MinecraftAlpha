using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector4 = Microsoft.Xna.Framework.Vector4;

namespace MinecraftAlpha
{
    public class Player
    {
        public Vector2 SpawnPoint = new Vector2(0, 0);
        public Game1 game;
        public WindowFrame Inventory = null;
        public Entity Plr;
        public Cammera cam = new Cammera();
        public bool Jumping = false;
        public Vector2
        
        
        public void Respawn()
        {
            var plr = Entity.CloneEntity(game._entityManager.entities[0], SpawnPoint);
            Plr = plr;

            game._entityManager.Workspace.Add(Plr);
        }
        public void PickupItem(Block item,int amount,WindowFrame inventory)
        {
            foreach (var slot in inventory.ItemSlots)
            {
                if (slot.Item == null)
                {
                    slot.Item = item;
                    slot.Count = amount;
                    break;
                }
                if (slot.Item.Name == item.Name && slot.Count <64)
                {
                    slot.Count += amount;
                    break;
                }

            }
        }

    }

    public class Cammera
    {
        public Vector2 position = new Vector2(530, -500);
        public Vector2 size { get; set; } = new Vector2(800, 600);

        public void RenderLayer(BlockManager blockManager, SpriteBatch _spriteBatch, TileGrid[,] Map, float layer, Vector2 pos,Texture2D BreakingTexture)
        {
            var Grid = Map;
            
            
           


            var BlockSize = blockManager.BlockSize;
            var Camera = this;

            // Render the world based on the position and size
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            for (int i = (int)pos.Y - 20; i < Map.GetLength(0) ; i++)
            {
                for (int j = (int)pos.X - 20; j < Map.GetLength(1) ; j++)
                {
                    if (Map[i, j].ID != 0)
                    {
                        

                        float Light = Map[i, j].brightness;
                        float Light01 = Light - layer;
                        var block = blockManager.Blocks[Map[i, j].ID];
                        var color = Color.FromNonPremultiplied(new Vector4(Light01, Light01, Light01, 1)) * block.Color;

                        int healthPercent = (int)Map[i, j].MinedHealth / 10;
                        Rectangle sourceRectangle = new Rectangle(healthPercent* BreakingTexture.Height, 0, BreakingTexture.Height, BreakingTexture.Height);
                        if ((int)Map[i, j].MinedHealth <= 0)
                        {
                            sourceRectangle = new Rectangle(0, 0, 0, 0);
                        }
                        Rectangle BlockState = new Rectangle(0, 0, block.Texture.Width, block.Texture.Height);

                        int State = block.DefaultState;
                        if (Map[i, j].state > 0)
                        {
                            BlockState = new Rectangle(State, 0, block.Texture.Width, block.Texture.Height);
                        }
                        
                        _spriteBatch.Draw(block.Texture, new Vector2(j * BlockSize, i * BlockSize) + Camera.position, BlockState, color, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, 0f);
                        _spriteBatch.Draw(BreakingTexture, new Vector2(j * BlockSize, i * BlockSize) + Camera.position, sourceRectangle, Color.White, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, 0f);

                    }
                }
            }
            _spriteBatch.End();

        }

        

    }
}
