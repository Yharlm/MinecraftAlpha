using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = System.Numerics.Vector2;

namespace MinecraftAlpha
{
    // animation thing but also can be used for Sprite Selection
    public class Sprite
    {


        public List<Vector2> Joints = new List<Vector2>(); 
        public Rectangle Margin;
        public Texture2D texture;
        public Vector2 Attachment = new Vector2(0,0);
        public float Orientation = 0f;
        public Sprite Parent = null; // Parent Sprite, if null then it is the root
        public Sprite()
        {
            
        }

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
        }

        

        
        // The Pos will be Pos of Parent + Attachments, Then here it gets offset to fit the orientation
        public void DrawSprite(SpriteBatch spriteBatch,Vector2 Pos,float size,float Rotation) // Pos is the Position of the Parent Attachment, it will be calculated with Joint, meanwhile Attachment gets joint's A attachment

        {

            //Draws the sprite where the attachment of its parent is



            var ract = Margin;
            Matrix4x4 AnglePos = Matrix4x4.CreateRotationZ(MathF.PI/180 * (Orientation/* + Rotation*/));
            var AttachemtPos = Vector2.Transform(Attachment * size, AnglePos);
            spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            spriteBatch.Draw(
                texture,
                size*AttachemtPos + Pos,
                ract,
                Microsoft.Xna.Framework.Color.White,
               MathF.PI / 180 * Orientation, // Orientation
                new Vector2(ract.Width,ract.Height)/2, //
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
                var texture = Content.Load<Texture2D>(mob.TextureName);
                mob.Texture = texture;
                // adds a new Sprite for each limb,
                foreach (var R in mob.Ractangles)
                {
                    var Ractangle = new Rectangle((int)R.X, (int)R.Y, (int)R.Z, (int)R.W);
                    Sprite sprite = new Sprite()
                    {
                        Margin = Ractangle,
                        texture = texture
                    };
                    mob.Sprites.Add(sprite);
                }
                
            }
            
                
        }
    }
}
