using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Emit;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;


namespace MinecraftAlpha
{
    public class SpriteManager
    {
        public List<Sprite> Sprites = new List<Sprite>();
        public List<Sprite3D> Sprites3D = new List<Sprite3D>();

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

        public Vector2 Position = new Vector2(100, 100);

        public float Size = 1f;

        public float Orientation = 0f; // 0,90,180,270
        
        public void Draw(SpriteBatch spritebatch, Vector2 Pos, float size, Vector2 offset,Game1 game)
        {
            Sides2_3D Visible = null;
            float Orientation = this.Orientation + offset.Y * 180;
            for (int i = 0; i < Sides.Length; i++)
            {
                float Angle = (i * 90);
                if (Orientation >= Angle && Orientation <= Angle + 90)
                {
                    Visible = Sides[i];
                }
            }
            Pos += new Vector2(offset.X * 2, 0);


            float sizeRatio = (Orientation % 90) / 90;

            Color lightingA = Color.FromNonPremultiplied(new Microsoft.Xna.Framework.Vector4(1 - sizeRatio, 1 - sizeRatio, 1 - sizeRatio, 1));
            Color lightingB = Color.FromNonPremultiplied(new Microsoft.Xna.Framework.Vector4(0.3f + sizeRatio, 0.3f + sizeRatio, 0.3f + sizeRatio, 1));


            Vector2 SizeA = new Vector2(1 - sizeRatio, 1) * Size * size / 2.3f;
            Vector2 SizeB = new Vector2((sizeRatio), 1) * Size * size / 2.3f;

            if (Visible == null)
            {

                return;
            }
            //spritebatch.Begin();

            float Floating = float.Cos(Orientation * MathF.PI / 180) * 10;

            //spritebatch.Draw(Visible.SideA, new Rectangle(0, 0, (int)(Visible.SideA.Width * SizeA.X), (int)(Visible.SideA.Height * SizeA.Y)), lightingA);
            //spritebatch.Draw(Visible.SideB, new Rectangle((int)(Visible.SideA.Width * SizeA.X), 0, (int)(Visible.SideB.Width * SizeB.X), (int)(Visible.SideB.Height * SizeB.Y)), lightingB);

            game.DrawBlock(Visible.SideA, 0, SizeA, Pos + new Vector2(0, Floating), 1f, 0f, Vector2.Zero, lightingA, SpriteEffects.None);
            game.DrawBlock(Visible.SideB, 0, SizeB, Pos + new Vector2(SizeA.X * 16, Floating), 1f, 0f, Vector2.Zero, lightingB, SpriteEffects.None);
            //public void DrawBlock(Visible., int state, float size, Vector2 position, float layer, float Orientation, Vector2 Origin, Color color, SpriteEffects spriteEffect);



            //spritebatch.Draw(
            //    Visible.SideA,
            //    Pos + new Vector2(0, Floating),
            //    null,
            //    lightingA,
            //    0f, // Orientation
            //    Vector2.Zero, //
            //    SizeA,
            //    SpriteEffects.None,
            //    1f
            //    );

            //// Draws the second side

            //spritebatch.Draw(
            //    Visible.SideB,
            //    Pos + new Vector2(SizeA.X * 16, Floating),
            //    null,
            //    lightingB,
            //    0f, // Orientation
            //    Vector2.Zero, //
            //    SizeB,
            //    SpriteEffects.None,
            //    1
            //    );
            //spritebatch.End();


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
            Orientation -= 1f;
            if (Orientation >= 360f)
            {
                Orientation = 0f;
            }
            if (Orientation < 0f)
            {
                Orientation = 360f;
            }
        }





    }
    public class Sprite
    {
        public float Layer = 0f;


        public bool isGrip = false;
        public Vector2 GripOffset = new Vector2(0, 0);
        //public List<Vector2> Joints = new List<Vector2>();
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
        public void DrawSprite(Game1 game1, Vector2 Pos, float size, float Rotation, bool Flip, float Z, float Shadow, bool Iframe, Entity mob) // Pos is the Position of the Parent Attachment, it will be calculated with Joint, meanwhile Attachment gets joint's A attachment

        {
            SpriteBatch spriteBatch = game1._spriteBatch;
            var ract = Margin;
            var target = Color.White;

            



            SpriteEffects spriteEffect = SpriteEffects.None;
            
            //Draws the sprite where the attachment of its parent is
            //Vector2 Attachment = this.Attachment / new Vector2(Margin.Width,Margin.Height);


            float Angle = (MathF.PI / 180 * (Orientation + ParentOrianetation));
            float ParentAngle = (MathF.PI / 180 * (ParentOrianetation));
            //var ract = Margin;
            Matrix4x4 AnglePos = Matrix4x4.CreateRotationZ(ParentAngle);
            float a = Z / 2 + 0.4f;

            var Layer = Color.FromNonPremultiplied(new Vector4(a, a, a, 1));

            var ParentPos = Vector2.Transform(Parent, AnglePos);
            var attachmentPos = Vector2.Transform(Attachment, AnglePos);



            Vector2 A = Attachment;
            Vector2 B = ParentPos;
            if (Flip)
            {
                A *= new Vector2(-1, 1);
                B *= new Vector2(-1, 1);
                spriteEffect = SpriteEffects.FlipHorizontally;
            }
            if (Iframe)
            {
                Layer = Color.PaleVioletRed;
            }
            spriteBatch.Draw(
                texture,
                Pos - B * size,
                ract,
                Layer,
                Angle + JointOrientation, // Orientation
                new Vector2(ract.Width, ract.Height) / 2 + A, //
                size,
                spriteEffect,
                0
                );

            if (isGrip && mob.Item != null)
            {
                AnglePos = Matrix4x4.CreateRotationZ(ParentAngle - 45);
                
                var item = mob.Item;
                float rotation = MathF.PI * (45) / 180;
                Vector2 Offset = new Vector2(2, -12);
                if (Flip)
                {
                    Offset *= new Vector2(-1, 1);
                    rotation = -MathF.PI * (45) / 180;
                    
                    AnglePos = Matrix4x4.CreateRotationZ(ParentAngle - 45 + 90);
                }


                attachmentPos = Vector2.Transform(Offset, AnglePos);

                if (item.Item)
                {
                    spriteBatch.Draw(
                        game1.items.Atlas,
                        Pos - (B) * size,
                        game1.items.GetRactangle(item.ItemID),
                        Layer,
                        Angle + JointOrientation + rotation, // Orientation
                        new Vector2(game1.items.GetRactangle(item.ItemID).Width, game1.items.GetRactangle(item.ItemID).Height) / 2 + attachmentPos, //
                        size * 0.6f,
                        spriteEffect,
                        0
                    );
                    Debuging.DebugPos(spriteBatch, Pos - (B) * size, game1);
                }
                else
                {
                    //spriteBatch.Draw(
                    //    item.Texture,
                    //    Pos - (B) * size,
                    //    null,
                    //    Layer,
                    //    Angle + JointOrientation + rotation, // Orientation
                    //    new Vector2(item.Texture.Width, item.Texture.Height) / 2 + attachmentPos, //
                    //    size * 0.6f,
                    //    spriteEffect,
                    //    Z
                    //);
                    game1.DrawBlock(item, 0, size * 0.6f, Pos - (B) * size, Z, Angle + JointOrientation + rotation, new Vector2(item.Texture.Width, item.Texture.Width) / 2 + attachmentPos, Layer, spriteEffect);
                    Debuging.DebugPos(spriteBatch, Pos - (B) * size, game1);
                }

            }


            //Debuging.DebugPos(spriteBatch, Pos - ParentPos * size, game1);

        }

        public static List<Sprite> LoadSprites(Entity mob)
        {
            List<Sprite> sprites = new List<Sprite>();
            if (mob.TextureName != "null")
            {
                var texture = mob.Texture;
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
                    sprites.Add(sprite);
                }

            }
            if (mob.gripIndex != -1)
            {
                sprites[mob.gripIndex].isGrip = true;
                sprites[mob.gripIndex].GripOffset = mob.GripOffset;
            }

            return sprites;


        }
    }
}
