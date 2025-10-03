using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;


namespace MinecraftAlpha
{
    public class UserInterfaceManager

    {
        

        public List<Button> Buttons = new List<Button>();
        public List<ItemSlot> ItemSlots = new List<ItemSlot>();

        public Block selectedItem = null;
        public int amount = 0;

        public UserInterfaceManager()
        {
            Buttons = LoadButtons();
        }

        public void DrawUI(SpriteBatch spriteBatch)
        {
            foreach (var button in Buttons)
            {
                var color = button.Hovered ? Color.Gray : Color.White;
                if (button.Background != null)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(button.Background, new Rectangle((int)button.Position.X, (int)button.Position.Y, (int)button.Scale.X, (int)button.Scale.Y), color);
                    spriteBatch.End();
                }
            }
        }
        public void DrawItemToMouse(SpriteBatch spriteBatch)
        {
            Vector2 mouse = Mouse.GetState().Position.ToVector2();
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(selectedItem.Texture, mouse - Vector2.One * selectedItem.Texture.Width, null, Color.White);
            // drawstring thing here ok?
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
        }
        public void HoverAction(Vector2 Mouse,ActionManager AM)
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
        public void ClickAction(Vector2 Mouse, ActionManager AM)
        {
            foreach (var button in Buttons)
            {
                if (button.IsInBounds(Mouse))
                {
                    AM.GetAction(button.Action);
                }
            }
            foreach(var ItemSlot in ItemSlots)
            {
                if (ItemSlot.IsInBounds(Mouse))
                {
                    if (selectedItem == null)
                    {
                        ItemSlot.TakeItem(-1, this);

                    }
                    else if(selectedItem != null)
                    {
                        ItemSlot.AddItem(-1, this);
                    }
                }
            }
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

        public void TakeItem(int Amount,UserInterfaceManager UI)
        {
            if(Amount == -1)
            {
                Amount = Count;
            }
            UI.selectedItem = Item;
            UI.amount = Amount;
            this.Count -= Amount;

        }

        public void AddItem(int Amount, UserInterfaceManager UI)
        {
            if (Amount == -1)
            {
                Amount = Count;
            }
            if (this.Item.Name == UI.selectedItem.Name)
            {
                this.Item = UI.selectedItem;
                UI.amount -= Amount;
                this.Count+=Amount;
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
