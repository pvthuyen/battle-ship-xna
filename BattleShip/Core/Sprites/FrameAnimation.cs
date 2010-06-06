using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleShip.Core.Sprites
{
    public class FrameAnimation : ICloneable
    {
        // The first frame of the Animation.  We will calculate other
        // frames on the fly based on this frame.
        private Rectangle rectInitialFrame;

        // Number of frames in the Animation
        private int iFrameCount = 1;

        // The frame currently being displayed. 
        // This value ranges from 0 to iFrameCount-1
        private int iCurrentFrame = 0;

        // Amount of time (in seconds) to display each frame
        private float fFrameLength = 0.2f;

        // Amount of time that has passed since we last animated
        private float fFrameTimer = 0.0f;

        // The number of times this animation has been played
        private int iPlayCount = 0;

        // The animation that should be played after this animation
        private string sNextAnimation = null;

        private bool bIsLoopAnimation = true;

        private bool bIsResetAnimation = false;

        public bool IsResetAnimation
        {
            set { bIsResetAnimation = value; }
        }

        public bool IsLoopAnimation
        {
            set { bIsLoopAnimation = value; }
            get { return bIsLoopAnimation; }
        }

        /// <summary>
        /// The number of frames the animation contains
        /// </summary>
        public int FrameCount
        {
            get { return iFrameCount; }
            set { iFrameCount = value; }
        }

        /// <summary>
        /// The time (in seconds) to display each frame
        /// </summary>
        public float FrameLength
        {
            get { return fFrameLength; }
            set { fFrameLength = value; }
        }

        /// <summary>
        /// The frame number currently being displayed
        /// </summary>
        public int CurrentFrame
        {
            get { return iCurrentFrame; }
            set { iCurrentFrame = (int)MathHelper.Clamp(value, 0, iFrameCount - 1); }
        }

        public int FrameWidth
        {
            get { return rectInitialFrame.Width; }
        }

        public int FrameHeight
        {
            get { return rectInitialFrame.Height; }
        }

        /// <summary>
        /// The rectangle associated with the current
        /// animation frame.
        /// </summary>
        public Rectangle FrameRectangle
        {
            get
            {
                return new Rectangle(rectInitialFrame.X + (rectInitialFrame.Width * iCurrentFrame), rectInitialFrame.Y, rectInitialFrame.Width, rectInitialFrame.Height);
            }
        }

        public int PlayCount
        {
            get { return iPlayCount; }
            set { iPlayCount = value; }
        }

        public string NextAnimation
        {
            get { return sNextAnimation; }
            set { sNextAnimation = value; }
        }

        public FrameAnimation(Rectangle FirstFrame, int Frames)
        {
            rectInitialFrame = FirstFrame;
            iFrameCount = Frames;
        }

        public FrameAnimation(int X, int Y, int Width, int Height, int Frames)
        {
            rectInitialFrame = new Rectangle(X, Y, Width, Height);
            iFrameCount = Frames;
        }

        public FrameAnimation(int X, int Y, int Width, int Height, int Frames, float FrameLength, bool bLoopAnimation)
        {
            rectInitialFrame = new Rectangle(X, Y, Width, Height);
            iFrameCount = Frames;
            fFrameLength = FrameLength;
            this.bIsLoopAnimation = bLoopAnimation;
        }

        public FrameAnimation(int X, int Y, int Width, int Height, int Frames, float FrameLength, bool bLoopAnimation, string strNextAnimation)
        {
            rectInitialFrame = new Rectangle(X, Y, Width, Height);
            iFrameCount = Frames;
            fFrameLength = FrameLength;
            sNextAnimation = strNextAnimation;
            this.bIsLoopAnimation = bLoopAnimation;
        }

        public void Update(GameTime gameTime)
        {
            fFrameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fFrameTimer > fFrameLength)
            {
                fFrameTimer = 0.0f;                
                if (bIsLoopAnimation)
                {
                    iCurrentFrame = (iCurrentFrame + 1) % iFrameCount;
                    if (iCurrentFrame == 0)
                        iPlayCount = (int)MathHelper.Min(iPlayCount + 1, int.MaxValue);
                }
                else
                {
                    if (iCurrentFrame < iFrameCount)
                    {
                        iCurrentFrame++;
                    }
                    else
                    {                        
                        iPlayCount = (int)MathHelper.Min(iPlayCount + 1, int.MaxValue);
                        if (bIsResetAnimation)
                        {
                            iCurrentFrame = 0;
                            bIsResetAnimation = false;
                        }
                    }                    
                }
            }
        }

        object ICloneable.Clone()
        {
            return new FrameAnimation(this.rectInitialFrame.X, this.rectInitialFrame.Y, this.rectInitialFrame.Width, this.rectInitialFrame.Height, this.iFrameCount, this.fFrameLength, bIsLoopAnimation, sNextAnimation);
        }                
    }
}
