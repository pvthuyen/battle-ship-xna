using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleShip.Core.Sprites
{
    class Explosion
    {
        static int iMapWidth = 1920;
        
        SpriteAnimation asSprite;

        int iX = 0;

        int iY = -100;

        bool bActive = true;

        int iBackgroundOffset = 0;

        Vector2 v2motion = new Vector2(0f, 0f);

        float fSpeed = 1f;

        public int X
        {

            get { return iX; }

            set { iX = value; }

        }



        public int Y
        {

            get { return iY; }

            set { iY = value; }

        }



        public bool IsActive
        {

            get { return bActive; }

        }



        public int Offset
        {

            get { return iBackgroundOffset; }

            set { iBackgroundOffset = value; }

        }



        public float Speed
        {

            get { return fSpeed; }

            set { fSpeed = value; }

        }



        public Vector2 Motion
        {

            get { return v2motion; }

            set { v2motion = value; }

        }

        public Explosion(Texture2D texture,

                         int X, int Y, int W, int H, int Frames)
        {

            asSprite = new SpriteAnimation(texture);

            //asSprite.FrameLength = 0.05f;

        }

        public void Activate(int x, int y, Vector2 motion,

                             float speed, int offset)
        {

            iX = x;

            iY = y;

            v2motion = motion;

            fSpeed = speed;

            iBackgroundOffset = offset;

        //    asSprite.Frame = 0;

            bActive = true;

        }

        private int GetDrawX()
        {

            int X = iX - iBackgroundOffset;

            if (X > iMapWidth)

                X -= iMapWidth;

            if (X < 0)

                X += iMapWidth;



            return X;

        }

        public void Update(GameTime gametime, int iOffset)
        {

            if (bActive)
            {

                iBackgroundOffset = iOffset;



                iX += (int)((float)v2motion.X * fSpeed);

                iY += (int)((float)v2motion.Y * fSpeed);

                asSprite.Update(gametime);

               // if (asSprite.Frame >= 15)
                {

                    bActive = false;

                }

            }

        }

        public void Draw(SpriteBatch sb, bool bAbsolute)
        {

            if (bActive)
            {

                if (!bAbsolute)

                    asSprite.Draw(sb, GetDrawX(), iY);

                else

                    asSprite.Draw(sb, iX, iY);

            }

        }


    }
}
