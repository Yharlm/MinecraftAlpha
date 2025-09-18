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
        


        public List<Vector4> Ractangles = new List<Vector4>();
        public Texture2D texture;
        public Vector2 Attachment = new Vector2(0,8);
        public Vector2 Joint = new Vector2(0,0);
        public float Orientation = MathF.PI/180*0f;
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
        public void DrawSprite(int index,SpriteBatch spriteBatch,Vector2 Pos,float size)
        {
            var R = Rectangles[index];
            var ract = new Microsoft.Xna.Framework.Rectangle(R.X,R.Y,R.Z,R.W);

            var Matrix = Matrix4x4.CreateRotationZ(Orientation);
            var Offset = Vector2.Transform(Attachment, Matrix);

            spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            spriteBatch.Draw(
                texture,
                 Pos - Offset * size,
                ract,
                Microsoft.Xna.Framework.Color.White,
                Orientation,
                new Vector2(ract.Width, ract.Height) / 2,
                size,
                SpriteEffects.None,
                0f
                );
            spriteBatch.End();
        }
    }
}
