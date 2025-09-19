using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftAlpha
{
    // animation thing but also can be used for Sprite Selection
    public class Sprite
    {
        


        public Rectangle Margin;
        public Texture2D texture;
        public Vector2 Attachment = new Vector2(0,1);
        public float Orientation = 0f;
        public Sprite()
        {
            
        }

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
        }

        public int Index = 1;

        public Sprite(Texture2D texture, int index) : this(texture)
        {
            Index = index;
        }
        // The Pos will be Pos of Parent + Attachments, Then here it gets offset to fit the orientation
        public void DrawSprite(SpriteBatch spriteBatch,Vector2 Pos,float size)
        {
            var ract = Ractangle;
            Matrix4x4 AnglePos = Matrix4x4.CreateRotationZ(MathF.PI/180 * Orientation);
            var AttachemtPos = Vector2.Transform(Attachment * size, AnglePos);
            spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            spriteBatch.Draw(
                texture,
                new Vector2(ract.Width, ract.Height) * size/2 + AttachemtPos,
                ract,
                Microsoft.Xna.Framework.Color.White,
                Orientation, // Orientation
                new Vector2.Zero(), //
                size,
                SpriteEffects.None,
                1f
                );
            spriteBatch.End();
        }

        public static void LoadSprites(ContentManager Content, Entity mob)
        {
            if (mob.TextureName != "null")
            {
                var texture = Content.Load(mob.TextureName);
                // adds a new Sprite for each limb,
                foreach(var R in mob.Ractangles)
                {
                    Sprite sprite = new Sprite()
                    {
                        Margin = R;
                    }
                    mob.Sprites.Add(sprite);
                }
                
            }
                
        }
    }
}
