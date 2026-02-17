using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
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
        public Game1 Game;
        //Base UI elements
        public List<WindowFrame> windows = new List<WindowFrame>();
        public List<Button> Buttons = new List<Button>();
        public List<ItemSlot> ItemSlots = new List<ItemSlot>();
        public List<UIFrame> Frames = new List<UIFrame>();
        public bool In_interface = false;


        public TileGrid LastUsedBlock = null;
        //public bool Clicked = false;
        public float itemUsetimer = 0f;
        public Block selectedItem = null;
        public int amount = 0;

        public UserInterfaceManager(Game1 Game1)
        {
            Game = Game1;
            Buttons = LoadButtons();
            Frames = LoadFrames();
            ItemSlots = LoadItemSlots();



        }



        public void DrawUI(SpriteBatch spriteBatch, ContentManager Contnet)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp/*, effect: Contnet.Load<Effect>("Shaders/Shader")*/);
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
                foreach (UIFrame frame in Window.Frames)
                {
                    frame.Position = Window.Position;
                    frame.Render(spriteBatch);
                    Debuging.DebugPos(spriteBatch, frame.position + frame.Position, Game);
                }
                foreach (ItemSlot Slot in Window.ItemSlots)
                {
                    Slot.Position = Window.Position;
                    Slot.Render(spriteBatch, Game);
                    Debuging.DebugPos(spriteBatch, Slot.ItemPosition + Slot.Position, Game);
                }
                foreach (Button button in Window.Buttons)
                {
                    button.Position = Window.Position;
                    button.Render(spriteBatch);
                    Debuging.DebugPos(spriteBatch, button.position + button.Position, Game);
                }
                foreach (textLabel text in Window.TextLabels)
                {
                    text.Position = Window.Position;
                    text.Render(spriteBatch);
                    Debuging.DebugPos(spriteBatch, text.position + text.Position, Game);
                }

            }
            DrawItemToMouse(spriteBatch, Contnet.Load<SpriteFont>("Font"));

            spriteBatch.End();



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

            if (selectedItem.Item)
            {
                int id = selectedItem.ItemID;
                if (Game.Player.DisplayID != -1)
                {
                    id = Game.Player.DisplayID;
                }
                Game.items.DrawItem(spriteBatch, mouse - new Vector2(16, 16), id, 2.5f);
            }
            else
            {
                //spriteBatch.Draw(selectedItem.Texture, new Rectangle((int)mouse.X, (int)mouse.Y, 32, 32), Color.White);
                Game.DrawBlock(selectedItem, 0, 1.6f, mouse, 1, 0, Vector2.One * 16, selectedItem.Color, SpriteEffects.None);
            }
            spriteBatch.DrawString(Text, Amounts, mouse - new Vector2(16, 16), Color.White);

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
                    //Action = "Rot",
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
                    Position = new Vector2(100,100) - new Vector2(80,80), Size = new Vector2(30,30),
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
            int Middlex = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;


            //Create Windows here
            float w = ItemSlots[0].Texture.Width * 3;
            windows.Add(new WindowFrame()
            {
                Position = new Vector2(Middlex/2 - w * 9/2, 600),
                Name = "Inventory",
                Visible = false,
                Frames = new() { new UIFrame() {position = new Vector2(-43, -50), Window = Frames[0].Window, Size = new Vector2(130, 50) } },
                Buttons = new() { },
                ItemSlots = new() { },
            });
            int ID = 0;
            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    
                    ItemSlot slot = new()
                    { ItemPosition = new Vector2(w * i, w * j), Texture = ItemSlots[0].Texture, ID = ID };
                    windows.Last().ItemSlots.Add(slot);
                    ID++;
                }

            }

            windows.Add(new WindowFrame()
            {
                Position = new Vector2(Middlex / 2 - w * 9 / 2, 200),
                Name = "Chest",
                Visible = false,
                Frames = new() { new UIFrame() { position = new Vector2(-43, -50), Window = Frames[0].Window, Size = new Vector2(130, 60) } },
                Buttons = new() { },
                ItemSlots = new() { },
            });
            ID = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 3; j++)
                {

                    ItemSlot slot = new()
                    { ItemPosition = new Vector2(w * i, w * j), Texture = ItemSlots[0].Texture, ID = ID };
                    windows.Last().ItemSlots.Add(slot);
                    ID++;
                }

            }

            windows.Add(new WindowFrame()
            {
                Name = "Crafting2x2",
                Visible = false,
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
            { ItemPosition = new Vector2(32, 32) + new Vector2(140, 30), Texture = ItemSlots[0].Texture, canPlace = false, Clicked = () => { for (int i = 0; i < 4; i++) { windows[2].ItemSlots[i].TakeItem(1); } } };
            windows.Last().ItemSlots.Add(result);



            windows.Add(new WindowFrame()
            {
                Position = new Vector2(Game.windowWidth / 2, Game.windowHeight / 2),
                Name = "Menu",
                Visible = true,
                Frames = new() { new() { Window = Frames[0].Window, position = new Vector2(0, 0) } },
                Buttons = new() { new() { Background = null, position = new Vector2(0, 0), Scale = new Vector2(340, 40), Click = (game) => { game.GameStarted = true; } } },
                ItemSlots = new() { },

            });

            windows.Last().Frames[0].button = windows.Last().Buttons[0];

            windows.Add(new WindowFrame()
            {
                Name = "Crafting3x3",
                Visible = false,
                Frames = new() { },
                Buttons = new() { },
                ItemSlots = new() { },

            });
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    ItemSlot slot = new()
                    { ItemPosition = new Vector2(32 * j, 32 * i) + new Vector2(100, 60), Texture = ItemSlots[0].Texture };
                    windows.Last().ItemSlots.Add(slot);
                }

            }
            result = new()
            { ItemPosition = new Vector2(32, 32) + new Vector2(200, 60), Texture = ItemSlots[0].Texture, canPlace = false, Clicked = () => { for (int i = 0; i < 4; i++) { windows[2].ItemSlots[i].TakeItem(1); } } };
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

        }
        public void MouseAction(Vector2 Mouse, ActionManager AM, int Mouse1) // Mouse1 = 0 none, 1 left, 2 right
        {
            In_interface = false;


            foreach (var win in this.windows)
            {

                foreach (var button in win.Buttons)
                {
                    if (button.IsInBounds(Mouse))
                    {
                        In_interface = true;

                    }

                    button.Hovered = false;
                    if (button.IsInBounds(Mouse))
                    {
                        if (Mouse1 == 0)
                        {
                            button.Hovered = true;
                        }
                        if (Mouse1 == 1)
                        {
                            button.Click.Invoke(AM.Game);
                        }

                    }




                }


                if (win.Visible != true)
                {
                    continue;
                }


                foreach (var ItemSlot in win.ItemSlots)
                {
                    if (ItemSlot.IsInBounds(Mouse))
                    {
                        In_interface = true;
                        //Clicked = true;
                        if (Mouse1 == 1)
                        {
                            if (!ItemSlot.canPlace)
                            {
                                if (ItemSlot.Item != null)
                                {
                                    if (amount + ItemSlot.Count <= 64)
                                    {
                                        ItemSlot.TakeItem(-1, this);
                                        break;
                                    }

                                }


                            }
                            else
                            {
                                if (selectedItem == null)
                                {
                                    ItemSlot.TakeItem(-1, this);
                                }
                                else if (selectedItem != null && ItemSlot.Count + amount <= 64)
                                {
                                    ItemSlot.AddItem(-1, this);
                                }
                                else if (selectedItem == ItemSlot.Item && ItemSlot.Item != null && amount < 64)
                                {
                                    ItemSlot.TakeItem(64 - amount, this);
                                }
                            }

                        }
                        else if (Mouse1 == 2)
                        {
                            if (ItemSlot.canPlace)
                            {
                                if (selectedItem == null)
                                {
                                    ItemSlot.TakeItem(1, this);

                                }
                                else if (selectedItem != null && 1 + ItemSlot.Count <= 64)
                                {
                                    ItemSlot.AddItem(1, this);
                                }
                            }
                        }
                    }

                }


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
        public Vector2 Position = Vector2.Zero;


        public void Update(Game1 game)
        {
            //Update Logic Here

            if (Visible)
            {
                if (Name == "Crafting2x2")
                {

                    foreach (CraftingRecipe Recipe in game._RecipeManager.Recipes)
                    {
                        if (Recipe.RecipeGrid.GetLength(0) == 2)
                        {
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
                        else
                        {

                        }
                        //if (ItemSlots[4].Item != null) break;


                    }
                }
                if (Name == "Crafting3x3")
                {

                    var ItemSlots = this.ItemSlots;
                    foreach (CraftingRecipe Recipe in game._RecipeManager.Recipes)
                    {

                        if (Recipe.RecipeGrid.GetLength(0) == 3)
                        {
                            if (Recipe.CheckRecipe(new ItemSlot[,]
                        {
                            { ItemSlots[0], ItemSlots[1],ItemSlots[2] },
                            { ItemSlots[3], ItemSlots[4],ItemSlots[5] },
                            { ItemSlots[6], ItemSlots[7],ItemSlots[8] },
                        }))
                            {
                                ItemSlots[9].Item = Recipe.item.Item;
                                ItemSlots[9].Count = Recipe.item.Count;
                                break;

                            }
                            else
                            {
                                ItemSlots[9].Count = 0;
                                ItemSlots[9].Item = null;
                            }
                        }
                        else
                        {

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
        public Vector2 Scale = Vector2.One * 36;
        public Block Item;
        public int Count = 0;

        public Texture2D Texture;
        public bool IsInBounds(Vector2 Pos)
        {
            Vector2 PO = this.Position + this.ItemPosition - Scale / 2;
            if (Pos.X >= PO.X && Pos.X <= PO.X + Scale.X)
                if (Pos.Y > PO.Y && Pos.Y <= PO.Y + Scale.Y)
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

            if (Count < Amount) return;

            UI.selectedItem = Item;
            UI.amount += Amount;
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

        public void Render(SpriteBatch Spritebatch, Game1 game)
        {
            Vector2 SlotPos = Position + this.ItemPosition;
            string AmmountInSlot = "";
            Spritebatch.Draw(Texture,SlotPos - Texture.Width * Vector2.One * 3 / 2,null,Color.White,0f,Vector2.Zero, 3, SpriteEffects.None,0f);
            if (Item != null)
            {
                if (Item.Item)
                {
                    game.items.DrawItem(Spritebatch, SlotPos - Vector2.One * 23, Item.ItemID,3f);
                    //Spritebatch.Draw(Texture, SlotPos - Texture.Width * Vector2.One * 3 / 2, null, Color.White, 0f, Vector2.Zero, 3, SpriteEffects.None, 0f);
                }
                else
                {
                    //Spritebatch.Draw(Item.Texture, SlotPos + Vector2.One * 4, null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
                    game.DrawBlock(Item, 0, 3f, SlotPos - Vector2.One *23, 1, 0, Vector2.Zero, Item.Color, SpriteEffects.None);
                    //Spritebatch.Draw(Texture, SlotPos - Texture.Width * Vector2.One * 3 / 2, null, Color.White, 0f, Vector2.Zero, 3, SpriteEffects.None, 0f);
                }


            }
            if (Count > 1)
            {
                AmmountInSlot = Count.ToString();
            }
            Spritebatch.DrawString(game.font, AmmountInSlot, SlotPos+ Vector2.One * 3, Color.White);

        }
    }
    public class Button : WindowFrame
    {
        public Vector2 position = new Vector2(0, 0);
        public Vector2 Scale = new Vector2(0, 0);
        public bool Hovered = false;
        public Action HoverAction;


        public bool Visible = true;
        //public bool Clicked = false;

        public Texture2D Background;
        public Action<Game1> Click = (Game) => { };
        public string Name = "Button";
        public string Text = "TextButton";

        public bool IsInBounds(Vector2 Pos)
        {
            Vector2 position = this.position + Position - Scale / 2;
            if (Pos.X >= position.X && Pos.X <= position.X + Scale.X)
                if (Pos.Y > position.Y && Pos.Y <= position.Y + Scale.Y)
                {

                    return true;
                    //Place Design shit here
                }
            return false;
        }

        public void Render(SpriteBatch Spritebatch)
        {

            if (Background == null) return;
            var color = Color.White;
            if (Hovered) color = Color.CornflowerBlue;
            Spritebatch.Draw(Background, new Rectangle((int)position.X, (int)position.Y, (int)Scale.X, (int)Scale.Y), color);
        }



    }

    public class textLabel : WindowFrame
    {
        public Vector2 position = new Vector2(300, 100);
        public string Text = "TextLabel";
        public SpriteFont font;
        public Color Color;

        public void Render(SpriteBatch sb)
        {
            int l = Text.Length;
            Vector2 Position = this.position + this.Position - new Vector2(l / 2 * 9, 6);
            sb.DrawString(font, Text, Position, Color);
        }

    }

    public class UIFrame : WindowFrame
    {
        public Button button;
        public string Name = "default";
        public Vector2 position = new Vector2(300, 100);
        public Vector2 Size = new Vector2(30, 50);
        public string textureName = "UIelements/WindowFrame";
        public Texture2D Window = null;
        //public WindowFrame Parent = null;

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

            Vector2 Size = this.Size;
            Color color = Color.White;
            if (button != null)
            {
                Size = button.Scale / 4;
                if (button.Hovered)
                {
                    color = Color.Gray;
                }
            }
            Vector2 CornerSize = Size;
            int Tx = Window.Width;
            int Ty = Window.Height;
            int Cx = (int)CornerSize.X;
            int Cy = (int)CornerSize.Y;



            Rectangle[] Corners = {
                new Rectangle(0,0,Cx,Cy),
                new Rectangle(Tx-Cx,0,Cx,Cy),

                new Rectangle(0,Ty-Cy,Cx,Cy),
                new Rectangle(Tx-Cx,Ty-Cy,Cx,Cy),
            };
            Vector2 position = this.position + this.Position;


            //Spritebatch.Draw(Window, Position, Background, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 1f);

            Spritebatch.Draw(Window, position, Corners[0], color, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            Spritebatch.Draw(Window, position + new Vector2(Size.X + CornerSize.X, 0), Corners[1], color, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            Spritebatch.Draw(Window, position + new Vector2(0, Size.Y + CornerSize.Y), Corners[2], color, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            Spritebatch.Draw(Window, position + new Vector2(Size.X + CornerSize.X, Size.Y + CornerSize.Y), Corners[3], color, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);




        }
    }
}
