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
        public Texture2D texture;
        public Vector2 Attachment = new Vector2(0,0);
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

        public void DrawSprite(int index,SpriteBatch spriteBatch,Vector2 Pos,float size)
        {
            var ract = new Microsoft.Xna.Framework.Rectangle(8 * index, 0, 8, 8);
            spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            spriteBatch.Draw(
                texture,
                Pos + new Vector2(ract.Width, ract.Height) * size/2,
                ract,
                Microsoft.Xna.Framework.Color.White,
                Orientation,
                new Vector2(),
                size,
                SpriteEffects.None,
                0f
                );
            spriteBatch.End();
        }
    }
}
