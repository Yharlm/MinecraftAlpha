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
using Vector2 = Microsoft.Xna.Framework.Vector2;


namespace MinecraftAlpha
{
    public class SpriteManager
    {
        public List<Sprite> Sprites = new List<Sprite>();
        public SpriteManager()
        {
            Sprites = LoadSprites();
        }
        public static List<Sprite> LoadSprites()
        {
            var list = new List<Sprite>();
            {
                new Sprite()
                {
                    Layer = 0f
                };
            }
            return list;
        }
    }
    // animation thing but also can be used for Sprite Selection
    public class Sprite
    {
        public float Layer = 0f;

        public List<Vector2> Joints = new List<Vector2>(); 
        public Rectangle Margin;
        public Texture2D texture;

        public float ParentOrianetation = 0f;
        public Vector2 Attachment = new Vector2(0,0);
        public Vector2 Parent = new Vector2(0, 0); // Position of the Parent Attachment
        public float JointOrientation = 0f; // Orientation of the Joint
        public float Orientation = 0f;
        //public Sprite Parent = null; // Parent Sprite, if null then it is the root
        
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
            //Vector2 Attachment = this.Attachment / new Vector2(Margin.Width,Margin.Height);
            

            float Angle = (MathF.PI / 180 * ( Orientation + ParentOrianetation));
            float ParentAngle = (MathF.PI / 180 * (ParentOrianetation));
            var ract = Margin;
            Matrix4x4 AnglePos = Matrix4x4.CreateRotationZ(ParentAngle);
            
            var ParentPos = Vector2.Transform(Parent, AnglePos);
            var attachmentPos = Vector2.Transform(Attachment, AnglePos);
            
            spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            spriteBatch.Draw(
                texture,
                Pos - ParentPos * size,
                ract,
                Microsoft.Xna.Framework.Color.White,
                Angle + JointOrientation, // Orientation
                new Vector2(ract.Width,ract.Height)/2 + Attachment, //
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
                        
                        Layer = mob.Ractangles.IndexOf(R),
                        Margin = Ractangle,
                        texture = texture
                    };
                    mob.Sprites.Add(sprite);
                }
                
            }
            
                
        }
    }
}
