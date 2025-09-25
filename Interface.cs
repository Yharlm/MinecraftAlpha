using Microsoft.Xna.Framework;
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

        public static List<Button> LoadButtons()
        {
            var list = new List<Button>();
            {
                new Button()
                {
                    Name = "Test"
                };
            }
            return list;
        }

        public static void TriggerAction(string Action)
        {

        }
    }



    public class Button
    {
        public Vector2 Position = new Vector2(0, 0);
        public Vector2 Scale = new Vector2(0, 0);

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
