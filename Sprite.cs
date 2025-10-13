using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Numerics;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;


namespace MinecraftAlpha
{
    public class SpriteManager
    {
        public List<Sprite> Sprites = new List<Sprite>();
        public List<Sprite3D> Sprites3D = new List<Sprite3D>();
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
    public class Sprite3D
    {
        public class Sides2_3D
        {
            public Texture2D SideA;
            public Texture2D SideB;

            public Sides2_3D(Texture2D sideA, Texture2D sideB)
            {
                SideA = sideA;
                SideB = sideB;
            }

        }
        public Sides2_3D[] Sides = new Sides2_3D[4]; // 0 = front/back, 1 = left/right

        public Vector2 Position = new Vector2(100,100);

        public float Size = 1f;

        public float Orientation = 0f; // 0,90,180,270
        public void Draw(SpriteBatch spritebatch)
        {
            Sides2_3D Visible = null;
            for (int i = 0; i < Sides.Length; i++)
            {
                float Angle = (i * 90);
                if (Orientation >= Angle - 45 && Orientation <= Angle + 45)
                {
                    Visible = Sides[i];
                }
            }



            float sizeRatio = Orientation%90/90;

            Color lightingA = Color.FromNonPremultiplied(new Microsoft.Xna.Framework.Vector4(sizeRatio, sizeRatio, sizeRatio, 1));
            Color lightingB = Color.FromNonPremultiplied(new Microsoft.Xna.Framework.Vector4(1 - sizeRatio, 1 - sizeRatio, 1 - sizeRatio, 1));

            Vector2 SizeA = new Vector2(sizeRatio, 1);
            Vector2 SizeB = new Vector2((1 - sizeRatio), 1);

            if (Visible == null)
            {
                if (Orientation > 180 + 45 || Orientation < 45)
                {
                    Visible = Sides[0];
                }
                
                    //return;
            }
            spritebatch.Begin();



            //spritebatch.Draw(Visible.SideA, new Rectangle(0, 0, (int)(Visible.SideA.Width * SizeA.X), (int)(Visible.SideA.Height * SizeA.Y)), lightingA);
            //spritebatch.Draw(Visible.SideB, new Rectangle((int)(Visible.SideA.Width * SizeA.X), 0, (int)(Visible.SideB.Width * SizeB.X), (int)(Visible.SideB.Height * SizeB.Y)), lightingB);


            spritebatch.Draw(
                Visible.SideA,
                Position + new Vector2(sizeRatio*16, 0),
                null,
                lightingA,
                0f, // Orientation
                new Vector2(Visible.SideA.Width, Visible.SideA.Height) / 2 , //
                SizeA,
                SpriteEffects.None,
                1f
                );

            // Draws the second side

            spritebatch.Draw(
                Visible.SideB,
                Position + new Vector2(sizeRatio*16, 0),
                null,
                lightingB,
                0f, // Orientation
                new Vector2(Visible.SideB.Width, Visible.SideB.Height) / 2, //
                SizeB,
                SpriteEffects.None,
                1f
                );
            spritebatch.End();


        }

        public Sprite3D(Texture2D sideA, Texture2D sideB, Texture2D sideC, Texture2D sideD)
        {

            Sides[0] = new Sides2_3D(sideA, sideB);
            Sides[1] = new Sides2_3D(sideB, sideC);
            Sides[2] = new Sides2_3D(sideC, sideD);
            Sides[3] = new Sides2_3D(sideD, sideA);

        }

        public void Update()
        {
            Orientation += 1f;
            if (Orientation >= 360f)
            {
                Orientation = 0f;
            }
        }





    }
    public class Sprite
    {
        public float Layer = 0f;

        public List<Vector2> Joints = new List<Vector2>();
        public Rectangle Margin;
        public Texture2D texture;

        public float ParentOrianetation = 0f;
        public Vector2 Attachment = new Vector2(0, 0);
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
        public void DrawSprite(SpriteBatch spriteBatch, Vector2 Pos, float size, float Rotation, bool Flip) // Pos is the Position of the Parent Attachment, it will be calculated with Joint, meanwhile Attachment gets joint's A attachment

        {
            SpriteEffects spriteEffect = SpriteEffects.None;
            if (Flip)
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
            }
            //Draws the sprite where the attachment of its parent is
            //Vector2 Attachment = this.Attachment / new Vector2(Margin.Width,Margin.Height);


            float Angle = (MathF.PI / 180 * (Orientation + ParentOrianetation));
            float ParentAngle = (MathF.PI / 180 * (ParentOrianetation));
            var ract = Margin;
            Matrix4x4 AnglePos = Matrix4x4.CreateRotationZ(ParentAngle);

            var ParentPos = Vector2.Transform(Parent, AnglePos);
            var attachmentPos = Vector2.Transform(Attachment, AnglePos);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(
                texture,
                Pos - ParentPos * size,
                ract,
                Microsoft.Xna.Framework.Color.White,
                Angle + JointOrientation, // Orientation
                new Vector2(ract.Width, ract.Height) / 2 + Attachment, //
                size,
                spriteEffect,
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
