using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;
//using Vector4 = Microsoft.Xna.Framework.Vector4;

namespace MinecraftAlpha
{
    public class Player
    {
        public Vector2 SpawnPoint = new Vector2(0, 0);
        public Game1 game;
        public WindowFrame Inventory = null;
        public Entity Plr;
        public Cammera cam = new();
        public bool Jumping = false;
        public float respawnTimer = 60f;
        public int DisplayID = -1;


        public ItemSlot FindItem(string name, string tag)
        {
            var itemSlot = Inventory.ItemSlots.Find(s => s.Item != null && s.Item.Name == name);
            return itemSlot;
        }
        public void Respawn()
        {
            if (respawnTimer > 0)
            {
                respawnTimer -= 0.4f;
                return;
            }
            Inventory = game._userInterfaceManager.windows.Find(w => w.Name == "Inventory");

            if (!game.KeepInventory)
            {
                

                foreach (var item in Inventory.ItemSlots)
                {
                    if (item.Item != null)
                    {
                        DropItem(item.Item, new Vector3(Plr.position, Plr.Layer), game.WorldMousePos, item.Count);
                    }
                }
                foreach (var slot in Inventory.ItemSlots)
                {
                    slot.Item = null;
                    slot.Count = 0;
                }
            }







            var plr = Entity.CloneEntity(game._entityManager.entities[0], SpawnPoint);
            Plr = plr;

            game._entityManager.Workspace.Add(Plr);
            respawnTimer = 60f;
        }
        public void PickupItem(Block item, int amount, WindowFrame inventory)
        {
            foreach (var slot in inventory.ItemSlots)
            {
                if (slot.Item == null)
                {
                    slot.Item = item;
                    slot.Count = amount;
                    break;
                }
                if (slot.Item.Name == item.Name && slot.Count < 64)
                {
                    slot.Count += amount;
                    break;
                }

            }
        }

        public void DropItem(Block item, Vector3 origin, Vector2 Dir, int amount)
        {
            Entity drop;
            if (item == null) return;

            drop = game._entityManager.SpawnItem(new Vector2(origin.X, origin.Y) + Vector2.Normalize(Dir - new Vector2(origin.X, origin.Y)), (int)origin.Z, item, amount);
            drop.IFrame = 5f;

            drop.velocity.velocity = Dir - new Vector2(origin.X, origin.Y);
            if (drop != null)
            {
                game._entityManager.Workspace.Add(drop);
            }
        }

        public static void DrawStats(SpriteBatch SB, Texture2D Main, string name, Vector2 pos)
        {


            if (name == "heart")
            {

                SB.Draw(Main, pos, new Rectangle(0, 0, 9, 9), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);

            }
            if (name == "heart.5")
            {
                SB.Draw(Main, pos, new Rectangle(0, 0, 5, 9), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);

            }
            if (name == "heart.0")
            {
                SB.Draw(Main, pos, new Rectangle(9, 0, 9, 9), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);

            }




        }

        public void Update()
        {
            Plr.Item = game._userInterfaceManager.selectedItem;
            if (Plr.Item != null)
            {
                if (Plr.Item.ChargeMax < Plr.Item.Charge && game._inputManager.WasMouseDown(Mouse.GetState()))
                {
                    //
                    Plr.Item.CanFire = true;
                    game._actionManager.PlaceBlock(game.WorldMousePos, Plr.Item);
                    //Plr.Item.Charge = 0;

                }
            }
            Inventory = game._userInterfaceManager.windows.Find(w => w.Name == "Inventory");
            foreach (var slot in Inventory.ItemSlots)
            {
                if (slot.Item == null) continue;
                if (slot.Item != null && slot.Count <= 0)
                {
                    slot.Item = null;
                }
                if (slot.Item.CooldownMax > 0 && slot.Item.Cooldown < slot.Item.CooldownMax)
                {
                    slot.Item.Cooldown += 1f;
                }
            }

        }
    }


    public class Cammera
    {

        public Vector2 position = new Vector2(0, 0);
        public Vector2 size { get; set; } = new Vector2(0, 0);

        //public void RenderLayer(BlockManager blockManager, SpriteBatch _spriteBatch, TileGrid[,] Map, float layer, Vector2 pos,Texture2D BreakingTexture)
        //{
        //    var Grid = Map;





        //    var BlockSize = blockManager.BlockSize;
        //    var Camera = this;

        //    // Render the world based on the position and size
        //    _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        //    for (int i = (int)pos.Y - 20; i < Map.GetLength(0) ; i++)
        //    {
        //        for (int j = (int)pos.X - 20; j < Map.GetLength(1) ; j++)
        //        {
        //            if (Map[i, j].ID != 0)
        //            {


        //                float Light = Map[i, j].brightness;
        //                float Light01 = Light - layer;
        //                var block = blockManager.Blocks[Map[i, j].ID];
        //                var color = Color.FromNonPremultiplied(new Vector4(Light01, Light01, Light01, 1)) * block.Color;

        //                int healthPercent = (int)Map[i, j].MinedHealth / 10;
        //                Rectangle sourceRectangle = new Rectangle(healthPercent* BreakingTexture.Height, 0, BreakingTexture.Height, BreakingTexture.Height);
        //                if ((int)Map[i, j].MinedHealth <= 0)
        //                {
        //                    sourceRectangle = new Rectangle(0, 0, 0, 0);
        //                }
        //                Rectangle BlockState = new Rectangle(0, 0, block.Texture.Width, block.Texture.Height);

        //                int State = block.DefaultState;
        //                if (Map[i, j].state > 0)
        //                {
        //                    BlockState = new Rectangle(State, 0, block.Texture.Width, block.Texture.Height);
        //                }

        //                _spriteBatch.Draw(block.Texture, new Vector2(j * BlockSize, i * BlockSize) + Camera.position, BlockState, color, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, 0f);
        //                _spriteBatch.Draw(BreakingTexture, new Vector2(j * BlockSize, i * BlockSize) + Camera.position, sourceRectangle, Color.White, 0f, Vector2.Zero, BlockSize / block.Texture.Width, SpriteEffects.None, 0f);

        //            }
        //        }
        //    }
        //    _spriteBatch.End();

        //}



    }

    public class InputManager
    {
        public List<Keys> KeyHistory = new List<Keys>();
        public float TimeSinceLastKeyPress = 0f;
        public MouseState LastMouseState;

        // to stop key repeat
        public bool IsKeyUp_Now(Keys key)
        {
            foreach (var k in KeyHistory)
            {
                if (k == key)
                {

                    return false;

                }
            }

            return true;
        }

        public bool WasMouseDown(MouseState state)
        {

            if (state.RightButton == ButtonState.Released && LastMouseState.RightButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        public bool IsKeyDown_Now(Keys key)
        {

            foreach (var k in KeyHistory)
            {
                if (k == key)
                {
                    TimeSinceLastKeyPress = 0f;
                    return false;

                }
            }
            if (Keyboard.GetState().IsKeyDown(key))
            {
                return true;
            }
            return false;
        }

        public void UpdateKeyHistory(KeyboardState keyboardState)
        {
            KeyHistory.Clear();
            foreach (var key in keyboardState.GetPressedKeys())
            {
                KeyHistory.Add(key);
            }
            LastMouseState = Mouse.GetState();
        }

        public bool KeyCombo(Keys key1, Keys key2, bool Order)
        {
            bool succes = false;

            if (Order)
            {
                if (KeyHistory.Count >= 2)
                {
                    if (KeyHistory[KeyHistory.Count - 2] == key1 && KeyHistory[KeyHistory.Count - 1] == key2)
                    {
                        succes = true;
                    }
                }
            }
            else
            {
                if (KeyHistory.Contains(key1) && KeyHistory.Contains(key2))
                {
                    succes = true;
                }
            }

            return succes;
        }
    }
}
