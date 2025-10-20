using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;


namespace MinecraftAlpha
{
    public class UserInterfaceManager

    {
        
        public TileGrid LastUsedBlock = null;
        public List<WindowFrame> windows = new List<WindowFrame>();
        public List<Button> Buttons = new List<Button>();
        public List<ItemSlot> ItemSlots = new List<ItemSlot>();
        public List<UIFrame> Frames = new List<UIFrame>();

        public bool Clicked = false;

        public Block selectedItem = null;
        public int amount = 0;

        public UserInterfaceManager()
        {
            Buttons = LoadButtons();
            Frames = UIFrame.Load();
        }

        public void DrawUI(SpriteBatch spriteBatch, ContentManager Contnet)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            foreach (var Frame in Frames)
            {
                Frame.Render(spriteBatch);
            }
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
                if (Window.Visible == false)
                {
                    continue;
                }
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
            string Amounts = amount.ToString();
            if (amount <= 1)
            {
                Amounts = "";
            }
            if (selectedItem == null)
            {
                return;
            }
            Vector2 mouse = Mouse.GetState().Position.ToVector2();
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(selectedItem.Texture, mouse - Vector2.One * selectedItem.Texture.Width, null, Color.White);
            spriteBatch.DrawString(Text, Amounts, mouse, Color.White);
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
            foreach (var window in windows)
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
            bool In_interface = false;
            foreach (var button in Buttons)
            {
                if (button.IsInBounds(Mouse))
                {
                    In_interface = true;
                    AM.GetAction(button.Action);

                }

            }
            foreach (var win in this.windows)
            {
                if (win.Visible != true)
                {
                    continue;
                }


                foreach (var ItemSlot in win.ItemsSlots)
                {
                    if (ItemSlot.IsInBounds(Mouse))
                    {
                        In_interface = true;
                        Clicked = true;
                        if (Mouse1)
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
                        else
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
                        if (LastUsedBlock != null)
                        {

                            AM.Game._blockManager.Blocks[LastUsedBlock.ID].Update.Invoke(LastUsedBlock);
                        }
                    }

                }


            }

            if (!In_interface)
            {
                //windows.Find(x => x.Name == "Crafting").Visible = false;
                windows.Find(x => x.Name == "Chest").Visible = false;

            }
        }

    }

    public class WindowFrame
    {
        

        public bool Visible = false;
        public string Name { get; set; }
        public Vector2 Position;
        public List<ItemSlot> ItemsSlots;
        public Action Update = () => { };

        

        public static List<WindowFrame> LoadGUI(Game1 Game)
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
                Position = new Vector2(100, 100),
                ItemsSlots = list
            };


            windows.Add(window);
            list = new List<ItemSlot>();
            int id = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    list.Add(new ItemSlot()
                    {
                        Position = Vector2.One * 32 * (new Vector2(j, i) + Vector2.One) + new Vector2(400, 0),
                        ID = id++,
                    }
                    );
                }
            }
            window = new WindowFrame()
            {

                Name = "Chest",
                Position = new Vector2(2200, 400),
                ItemsSlots = list
            };
            windows.Add(window);
            list = new List<ItemSlot>();
            id = 1;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    list.Add(new ItemSlot()
                    {
                        Position = Vector2.One * 32 * (new Vector2(j, i) + Vector2.One) + new Vector2(400, 0),
                        ID = id++,
                    }
                    );
                }
            }

            list.Add(new ItemSlot()
            {
                Position = Vector2.One * 32 * (new Vector2(2.5f, 0.5f) + Vector2.One) + new Vector2(400, 0),
                ID = id++,

            }

                    );
            list.Last().Clicked = () =>
            {
                for (int i = 0; i < 4; i++)
                { list[i].TakeItem(1); }
            };
            window = new WindowFrame()
            {
                Visible = true,
                Name = "Crafting",
                Position = new Vector2(1300, 400),
                ItemsSlots = list,
                Update = () =>
                {
                    var blocks = Game._blockManager.Blocks;
                    var Window = window;

                    if (!Window.Visible) return;
                    var item =blocks[0];

                    var grid = Game._userInterfaceManager.windows[2].ItemsSlots;

                    ItemSlot[,] Grid2x2 = new ItemSlot[2, 2]
                    {
                    { grid[0], grid[1] },
                    { grid[2], grid[3] }
                    };
                    foreach (var Recipe in Game._RecipeManager.Recipes)
                    {
                        if (Recipe.CheckRecipe(Grid2x2))
                        {

                            Window.ItemsSlots[4].Item = Recipe.item.Item;
                            Window.ItemsSlots[4].Count = Recipe.item.Count;
                            return;
                        }
                        else
                        {
                            Window.ItemsSlots[4].TakeItem(64);
                        }
                    }
                }
            };


            windows.Add(window);

            return windows;
        }
    }
    public class ItemSlot : WindowFrame
    {
        public int ID = 0;
        public Vector2 Position;
        public Vector2 Scale = Vector2.One * 32;
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

        public Action Clicked = () => { };

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
            Clicked.Invoke();
        }

        public void TakeItem(int Amount)
        {
       
            this.Count -= Amount;
            if (this.Count <= 0)
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
            Clicked.Invoke();
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

    public class textLabel
    {
        public Vector2 Position = new Vector2(300, 100);
        public string Text = "TextLabel";

        public string ID = "Key";

    }

    public class UIFrame
    {
        public Vector2 Position = new Vector2(300, 100);
        public Vector2 Size = new Vector2(30, 50);
        public string textureName = "UIelements/WindowFrame";
        public Texture2D Window = null;

        public void loadContent(ContentManager Content) 
        {
            Window = Content.Load<Texture2D>(this.textureName);
        }
        
        public static List<UIFrame> Load()
        {
            List<UIFrame> uIFrames =
            [
                new UIFrame()
                {textureName = "UIelements/WindowFrame",
                    Position = new Vector2(100,100) - new Vector2(50,50), Size = new Vector2(100,100) + new Vector2(50, 50),
                },
            ];



            return uIFrames;
        }
        public void Render(SpriteBatch Spritebatch)
        {
            Vector2 CornerSize = Size;
            int Tx = Window.Width;
            int Ty = Window.Height;
            int Cx = (int)CornerSize.X;
            int Cy = (int)CornerSize.Y;
            int Sizex = (int)Size.X;
            int Sizey = (int)Size.Y;


            Rectangle[] Corners = {
                new Rectangle(0,0,Cx,Cy),
                new Rectangle(Tx-Cx,0,Cx,Cy),
                
                new Rectangle(0,Ty-Cy,Cx,Cy),
                new Rectangle(Tx-Cx,Ty-Cy,Cx,Cy),
            };
            
            
            //Spritebatch.Draw(Window, Position, Background, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 1f);

            Spritebatch.Draw(Window, Position, Corners[0], Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            Spritebatch.Draw(Window, Position + new Vector2(Size.X + CornerSize.X, 0), Corners[1], Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            Spritebatch.Draw(Window, Position + new Vector2(0, Size.Y + CornerSize.Y), Corners[2], Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            Spritebatch.Draw(Window, Position + new Vector2(Size.X + CornerSize.X, Size.Y + CornerSize.Y), Corners[3], Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            

            
        }
    }
}
