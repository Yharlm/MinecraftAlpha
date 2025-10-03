using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;


namespace MinecraftAlpha
{
    public class UserInterfaceManager

    {

        public List<WindowFrame> windows = new List<WindowFrame>();
        public List<Button> Buttons = new List<Button>();
        public List<ItemSlot> ItemSlots = new List<ItemSlot>();


        public Block selectedItem = null;
        public int amount = 0;

        public UserInterfaceManager()
        {
            Buttons = LoadButtons();
            
        }

        public void DrawUI(SpriteBatch spriteBatch, ContentManager Contnet)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (var button in Buttons)
            {
                var color = button.Hovered ? Color.Gray : Color.White;
                if (button.Background != null)
                {

                    spriteBatch.Draw(button.Background, new Rectangle((int)button.Position.X, (int)button.Position.Y, (int)button.Scale.X, (int)button.Scale.Y), color);

                }
            }
            foreach (var Window in windows)
            {
                foreach (var item in Window.ItemsSlots)
                {
                    string AmmountInSlot = "";
                    spriteBatch.Draw(item.Texture, new Rectangle((int)item.Position.X, (int)item.Position.Y, 32, 32), Color.White);
                    if (item.Item != null)
                    {
                        spriteBatch.Draw(item.Item.Texture, item.Position + Vector2.One * 4, null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);

                    }
                    if (item.Count > 1)
                    {
                        AmmountInSlot = item.Count.ToString();
                    }
                    spriteBatch.DrawString(Contnet.Load<SpriteFont>("Font"), AmmountInSlot, item.Position + new Vector2(10, 14), Color.White);
                }
            }
            spriteBatch.End();
            DrawItemToMouse(spriteBatch, Contnet.Load<SpriteFont>("Font"));
        }
        public void DrawItemToMouse(SpriteBatch spriteBatch, SpriteFont Text)
        {
            if (selectedItem == null)
            {
                return;
            }
            Vector2 mouse = Mouse.GetState().Position.ToVector2();
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(selectedItem.Texture, mouse - Vector2.One * selectedItem.Texture.Width, null, Color.White);
            spriteBatch.DrawString(Text, amount.ToString(), mouse, Color.White);
            spriteBatch.End();
        }
        public static List<Button> LoadButtons()
        {
            var list = new List<Button>()
            {
                //new Button()
                //{
                //    Name = "Test",
                //    Position = new Vector2(100, 100),
                //    Scale = new Vector2(200, 50),
                //    Action = "KIll",
                //},
                new Button()
                {
                    Name = "Test",
                    Position = new Vector2(100, 100),
                    Scale = new Vector2(30, 50),
                    Action = "Rot",
                }
            };
            return list;
        }
        
        public void LoadTextures(ContentManager Content)
        {
            foreach (var button in Buttons)
            {
                button.Background = Content.Load<Texture2D>("dirt");
            }
            foreach(var window in windows)
            {
                foreach (var ItemSlot in window.ItemsSlots)
                {
                    ItemSlot.Texture = Content.Load<Texture2D>("UIelements/ItemSlot");
                }
            }
        }
        public void HoverAction(Vector2 Mouse, ActionManager AM)
        {
            foreach (var button in Buttons)
            {
                if (button.IsInBounds(Mouse))
                {
                    button.Hovered = true;
                    //AM.GetAction(button.Action);

                }
                else
                {
                    button.Hovered = false;
                }
            }
            foreach (var ItemSlot in ItemSlots)
            {

            }
        }
        public void ClickAction(Vector2 Mouse, ActionManager AM, bool Mouse1)
        {
            if (Mouse1)
            {
                foreach (var button in Buttons)
                {
                    if (button.IsInBounds(Mouse))
                    {
                        AM.GetAction(button.Action);
                    }
                }
                foreach (var win in this.windows)
                {
                    foreach (var ItemSlot in win.ItemsSlots)
                    {
                        if (ItemSlot.IsInBounds(Mouse))
                        {
                            if (selectedItem == null)
                            {
                                ItemSlot.TakeItem(-1, this);

                            }
                            else if (selectedItem != null && ItemSlot.Count < 64)
                            {
                                ItemSlot.AddItem(-1, this);
                            }
                        }
                        //Thread.Sleep(100);
                    }
                }
            }
            else
            {
                foreach (var win in this.windows)
                {
                    foreach (var ItemSlot in win.ItemsSlots)
                    {
                        if (ItemSlot.IsInBounds(Mouse))
                        {
                            if (selectedItem == null)
                            {
                                ItemSlot.TakeItem(1, this);

                            }
                            else if (selectedItem != null && ItemSlot.Count < 64)
                            {
                                ItemSlot.AddItem(1, this);
                            }
                        }
                        //Thread.Sleep(100);
                    }
                }
            }

        }

    }

    public class WindowFrame
    {
        public string Name { get; set; }
        public Vector2 Position;
        public List<ItemSlot> ItemsSlots;

        public static List<WindowFrame> LoadGUI()
        {

            List<WindowFrame> windows;
            windows = new List<WindowFrame>();

            var list = new List<ItemSlot>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    list.Add(new ItemSlot()
                    {
                        Position = Vector2.One * 32 * (new Vector2(j, i) + Vector2.One),
                    }
                    );
                }
            }
            var window = new WindowFrame()
            {
                Name = "Inventory",
                Position = new Vector2(300, 0),
                ItemsSlots = list
            };
            
            windows.Add(window);
            

            return windows;
        }
    }
    public class ItemSlot
    {
        public Vector2 Position;
        Vector2 Scale = Vector2.One * 32;
        public Block Item;
        public int Count = 0;

        public Texture2D Texture;
        public bool IsInBounds(Vector2 Pos)
        {

            if (Pos.X >= Position.X && Pos.X <= Position.X + Scale.X)
                if (Pos.Y > Position.Y && Pos.Y <= Position.Y + Scale.Y)
                {
                    return true;
                    //Place Design shit here
                }
            return false;
        }

        public void TakeItem(int Amount, UserInterfaceManager UI)
        {
            if (Amount == -1)
            {
                Amount = Count;
            }
            UI.selectedItem = Item;
            UI.amount = Amount;
            this.Count -= Amount;
            if (this.Count == 0)
            {
                Item = null;
            }

        }

        public void AddItem(int Amount, UserInterfaceManager UI)
        {
            if (UI.selectedItem == null)
            {
                return;
            }
            if (Amount == -1)
            {
                Amount = UI.amount;
            }
            if (this.Item == UI.selectedItem || this.Item == null)
            {
                this.Item = UI.selectedItem;
                UI.amount -= Amount;

                this.Count += Amount;
            }
            if (UI.amount <= 0)
            {
                UI.selectedItem = null;
            }

        }

    }
    public class Button
    {
        public Vector2 Position = new Vector2(0, 0);
        public Vector2 Scale = new Vector2(0, 0);
        public bool Hovered = false;
        public Action HoverAction;


        public bool Visible = true;
        public bool Clicked = false;

        public Texture2D Background;
        public string Action = "None";
        public string Name = "Button";
        public string Text = "TextButton";

        public bool IsInBounds(Vector2 Pos)
        {

            if (Pos.X >= Position.X && Pos.X <= Position.X + Scale.X)
                if (Pos.Y > Position.Y && Pos.Y <= Position.Y + Scale.Y)
                {
                    return true;
                    //Place Design shit here
                }
            return false;
        }




    }
}
