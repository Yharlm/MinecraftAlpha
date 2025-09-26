using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace MinecraftAlpha
{
    public class UserInterfaceManager

    {
        

        public List<Button> Buttons = new List<Button>();
        public UserInterfaceManager()
        {
            Buttons = LoadButtons();
        }

        public void DrawUI(SpriteBatch spriteBatch)
        {
            foreach (var button in Buttons)
            {
                if (button.Background != null)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(button.Background, new Rectangle((int)button.Position.X, (int)button.Position.Y, (int)button.Scale.X, (int)button.Scale.Y), Color.White);
                    spriteBatch.End();
                }
            }
        }
        public static List<Button> LoadButtons()
        {
            var list = new List<Button>()
            {
                new Button()
                {
                    Name = "Test",
                    Position = new Vector2(100, 100),
                    Scale = new Vector2(200, 50),
                    Action = "KIll",
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
            foreach(var button in Buttons)
            {
                if (button.IsInBounds(Mouse))
                {
                    AM.GetAction(button.Action);
                }
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
        }

    }



    public class Button
    {
        public Vector2 Position = new Vector2(0, 0);
        public Vector2 Scale = new Vector2(0, 0);
        public bool Hovered = false;
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
