using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;


namespace MinecraftAlpha
{
    public class UserInterfaceManager
    {

        //Base UI elements
        public List<WindowFrame> windows = new List<WindowFrame>();
        public List<Button> Buttons = new List<Button>();
        public List<ItemSlot> ItemSlots = new List<ItemSlot>();
        public List<UIFrame> Frames = new List<UIFrame>();



        public TileGrid LastUsedBlock = null;
        public bool Clicked = false;

        public Block selectedItem = null;
        public int amount = 0;

        public UserInterfaceManager()
        {
            Buttons = LoadButtons();
            Frames = LoadFrames();
            ItemSlots = LoadItemSlots();


            
        }


        
        public void DrawUI(SpriteBatch spriteBatch, ContentManager Contnet)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp,effect: Contnet.Load<Effect>("Shaders/Shader"));
            //foreach (var Frame in Frames)
            //{
            //    if (Frame.Window == null) continue;
            //    if (Frame.Parent.Visible == false)
            //    {
            //        continue;
            //    }
            //    Frame.Render(spriteBatch);
            //}
            //foreach (var button in Buttons)
            //{
            //    var color = button.Hovered ? Color.Gray : Color.White;
            //    if (button.Background != null)
            //    {

            //        spriteBatch.Draw(button.Background, new Rectangle((int)button.Position.X, (int)button.Position.Y, (int)button.Scale.X, (int)button.Scale.Y), color);

            //    }
            //}
            //foreach (var Window in windows)
            //{
            //    if (Window.Visible == false)
            //    {
            //        continue;
            //    }
            //    foreach (var item in Window.ItemSlots)
            //    {

            //        string AmmountInSlot = "";
            //        spriteBatch.Draw(item.Texture, new Rectangle((int)item.Position.X, (int)item.Position.Y, 32, 32), Color.White);
            //        if (item.Item != null)
            //        {
            //            spriteBatch.Draw(item.Item.Texture, item.Position + Vector2.One * 4, null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);

            //        }
            //        if (item.Count > 1)
            //        {
            //            AmmountInSlot = item.Count.ToString();
            //        }
            //        spriteBatch.DrawString(Contnet.Load<SpriteFont>("Font"), AmmountInSlot, item.Position + new Vector2(10, 14), Color.White);
            //    }
            //}

            foreach (WindowFrame Window in windows)
            {
                if (!Window.Visible) continue;
                foreach ( UIFrame frame in Window.Frames)
                {
                    frame.Render(spriteBatch);
                }
                foreach (ItemSlot Slot in Window.ItemSlots)
                {
                    Slot.Render(spriteBatch, Contnet);
                }
                foreach (Button button in Window.Buttons)
                {
                    
                }
                foreach (textLabel text in Window.TextLabels)
                {
                    
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

        //creates presets
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

        public static List<UIFrame> LoadFrames()
        {
            var list = new List<UIFrame>()
            {
                new UIFrame()
                {
                    Name = "InventoryFrame",
                    textureName = "UIelements/WindowFrame",
                    Position = new Vector2(100,100) - new Vector2(80,80), Size = new Vector2(30,70),
                },
            };
            return list;
        }

        public static List<ItemSlot> LoadItemSlots()
        {
            var list = new List<ItemSlot>()
            {
                new ItemSlot()
                {
                    Position = new Vector2(300,300),
                },
            };
            return list;
        }


        public void LoadTextures(ContentManager Content)
        {
            foreach (var button in Buttons)
            {
                button.Background = Content.Load<Texture2D>("dirt");
            }
            foreach (var ItemSlot in ItemSlots)
            {
                ItemSlot.Texture = Content.Load<Texture2D>("UIelements/ItemSlot");
            }
            foreach (var Frame in Frames)
            {
                Frame.loadContent(Content);
            }
        }

        public void LoadGUI()
        {
            //Create Windows here

            windows.Add(new WindowFrame()
            {
                Name = "Inventory",
                Visible = true,
                Frames = new() { new UIFrame(){Window = Frames[0].Window ,Size =new Vector2(40,40),Position = new Vector2(290, 20) } },
                Buttons = new() { },
                ItemSlots = new() { },
            });
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    ItemSlot slot = new()
                    { ItemPosition = new Vector2(32 * i, 32 * j) + new Vector2(300, 30), Texture = ItemSlots[0].Texture };
                    windows.Last().ItemSlots.Add(slot);
                }
                
            }
            windows.Add(new WindowFrame()
            {
                Name = "Chest",
                Visible = false,
                Frames = new() { },
                Buttons = new() { },
                ItemSlots = new() { },
            });
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    ItemSlot slot = new()
                    { ItemPosition = new Vector2(32 * i, 32 * j) + new Vector2(300, 130), Texture = ItemSlots[0].Texture };
                    windows.Last().ItemSlots.Add(slot);
                }

            }

            windows.Add(new WindowFrame()
            {
                Name = "Crafting2x2",
                Visible = true,
                Frames = new() { },
                Buttons = new() { },
                ItemSlots = new() { },
            });
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    ItemSlot slot = new()
                    { ItemPosition = new Vector2(32 * i, 32 * j) + new Vector2(100, 60), Texture = ItemSlots[0].Texture };
                    windows.Last().ItemSlots.Add(slot);
                }

            }
            ItemSlot result = new()
            { ItemPosition = new Vector2(32, 32) + new Vector2(140, 30), Texture = ItemSlots[0].Texture, canPlace = false, Clicked = () => { for (int i = 0; i < 4; i++) { windows[2].ItemSlots[i].TakeItem(1); }  } };
            windows.Last().ItemSlots.Add(result);
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


                foreach (var ItemSlot in win.ItemSlots)
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
                //windows.Find(x => x.Name == "Chest").Visible = false;

            }
        }

    }

    public class WindowFrame
    {


        public List<Button> Buttons = new List<Button>();
        public List<ItemSlot> ItemSlots = new List<ItemSlot>();
        public List<UIFrame> Frames = new List<UIFrame>();
        public List<textLabel> TextLabels = new List<textLabel>();


        public bool Visible = false;
        public string Name { get; set; }
        public Vector2 Position;


        public void Update(Game1 game)
        {
            //Update Logic Here

            if (Visible)
            {
                if (Name == "Crafting2x2")
                {
                    foreach (CraftingRecipe Recipe in game._RecipeManager.Recipes)
                    {
                        //if (ItemSlots[4].Item != null) break;
                        if (Recipe.CheckRecipe(new ItemSlot[,]
                        {
                            { ItemSlots[0], ItemSlots[1] },
                            { ItemSlots[2], ItemSlots[3] },
                        }))
                        {
                            ItemSlots[4].Item = Recipe.item.Item;
                            ItemSlots[4].Count = Recipe.item.Count;
                            break;
                            
                        }
                        else
                        {
                            ItemSlots[4].Count = 0;
                            ItemSlots[4].Item = null;
                        }

                    }
                }
            }

        }


    }
    public class ItemSlot : WindowFrame
    {
        public bool canTake = true;
        public bool canPlace = true;
        public string Tag = "";
        public int ID = 0;
        public Vector2 ItemPosition;
        public Vector2 Scale = Vector2.One * 32;
        public Block Item;
        public int Count = 0;

        public Texture2D Texture;
        public bool IsInBounds(Vector2 Pos)
        {

            if (Pos.X >= ItemPosition.X && Pos.X <= ItemPosition.X + Scale.X)
                if (Pos.Y > ItemPosition.Y && Pos.Y <= ItemPosition.Y + Scale.Y)
                {
                    return true;
                    //Place Design shit here
                }
            return false;
        }

        public Action Clicked = () => { };

        public void TakeItem(int Amount, UserInterfaceManager UI)
        {
            if (!canTake) return;
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
            if (!canPlace) return;
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

        public void Render(SpriteBatch Spritebatch, ContentManager Content)
        {
            
            string AmmountInSlot = "";
            Spritebatch.Draw(Texture, new Rectangle((int)ItemPosition.X, (int)ItemPosition.Y, 32, 32), Color.White);
            if (Item != null)
            {
                Spritebatch.Draw(Item.Texture, ItemPosition + Vector2.One * 4, null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);

            }
            if (Count > 1)
            {
                AmmountInSlot = Count.ToString();
            }
            Spritebatch.DrawString(Content.Load<SpriteFont>("Font"), AmmountInSlot, ItemPosition + new Vector2(10, 14), Color.White);

        }
    }
    public class Button : WindowFrame
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

    public class textLabel : WindowFrame
    {
        public Vector2 Position = new Vector2(300, 100);
        public string Text = "TextLabel";

        public string ID = "Key";

    }

    public class UIFrame : WindowFrame
    {
        public string Name = "default";
        public Vector2 Position = new Vector2(300, 100);
        public Vector2 Size = new Vector2(30, 50);
        public string textureName = "UIelements/WindowFrame";
        public Texture2D Window = null;
        public WindowFrame Parent = null;

        public void loadContent(ContentManager Content)
        {
            Window = Content.Load<Texture2D>(this.textureName);
        }

        public static List<UIFrame> Load()
        {
            List<UIFrame> uIFrames =
            [
                new UIFrame()
                {
                    Name = "Inventory",
                    textureName = "UIelements/WindowFrame",
                    Position = new Vector2(100,100) - new Vector2(80,80), Size = new Vector2(30,70),
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
