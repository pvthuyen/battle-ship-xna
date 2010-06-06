using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BattleShip.Core.Sprites
{
    public class SpriteAnimation
    {
        // The texture that holds the images for this sprite
        Texture2D t2dTexture;

        // True if animations are being played
        bool bAnimating = true;

        // If set to anything other than Color.White, will colorize
        // the sprite with that color.
        Color colorTint = Color.White;

        // Screen Position of the Sprite
        Vector2 v2Position = new Vector2(0, 0);
        Vector2 v2LastPosition = new Vector2(0, 0);

        // Dictionary holding all of the FrameAnimation objects
        // associated with this sprite.
        Dictionary<string, FrameAnimation> faAnimations = new Dictionary<string, FrameAnimation>();

        // Which FrameAnimation from the dictionary above is playing
        string sCurrentAnimation = null;
        FrameAnimation faCurrAnimation;

        // If true, the sprite will automatically rotate to align itself
        // with the angle difference between it's new position and
        // it's previous position.  In this case, the 0 rotation point
        // is to the right (so the sprite should start out facing to
        // the right.
        bool bRotateByPosition = false;

        // How much the sprite should be rotated by when drawn
        // Value is in Radians, and 0 indicates no rotation.
        float fRotation = 0f;

        // Calcualted center of the sprite
        Vector2 v2Center;
                
        // width and height of the sprite
        int iWidth;
        int iHeight;

        bool bIsCollision = false;

        public bool IsCollision
        {
            set { bIsCollision = value; }
        }

        ///
        /// Vector2 representing the position of the sprite's upper left
        /// corner pixel.
        ///
        public Vector2 Position
        {
            get { return v2Position; }
            set
            {
                v2LastPosition = v2Position;
                v2Position = value;
                UpdateRotation();
            }
        }

        public Vector2 CenterPosition
        {
            get {
                v2Center.X = X + iWidth / 2;
                v2Center.Y = Y + iHeight / 2;
                return v2Center;
            }
        }

        ///
        /// The X position of the sprite's upper left corner pixel.
        ///
        public int X
        {
            get { return (int)v2Position.X; }
            set
            {
                v2LastPosition.X = v2Position.X;
                v2Position.X = value;
                UpdateRotation();
            }
        }

        ///
        /// The Y position of the sprite's upper left corner pixel.
        ///
        public int Y
        {
            get { return (int)v2Position.Y; }
            set
            {
                v2LastPosition.Y = v2Position.Y;
                v2Position.Y = value;
                UpdateRotation();
            }
        }

        ///
        /// Width (in pixels) of the sprite animation frames
        ///
        public int Width
        {
            get { return iWidth; }
        }

        ///
        /// Height (in pixels) of the sprite animation frames
        ///
        public int Height
        {
            get { return iHeight; }
        }

        ///
        /// If true, the sprite will automatically rotate in the direction
        /// of motion whenever the sprite's Position changes.
        ///
        public bool AutoRotate
        {
            get { return bRotateByPosition; }
            set { bRotateByPosition = value; }
        }

        ///
        /// The degree of rotation (in radians) to be applied to the
        /// sprite when drawn.
        ///
        public float Rotation
        {
            get { return fRotation; }
            set { fRotation = value; }
        }

        ///
        /// Screen coordinates of the bounding box surrounding this sprite
        ///
        public Rectangle BoundingBox
        {
            get { return new Rectangle(X, Y, iWidth, iHeight); }
        }

        ///
        /// The texture associated with this sprite.  All FrameAnimations will be
        /// relative to this texture.
        ///
        public Texture2D Texture
        {
            get { return t2dTexture; }
        }

        ///
        /// Color value to tint the sprite with when drawing.  Color.White
        /// (the default) indicates no tinting.
        ///
        public Color Tint
        {
            get { return colorTint; }
            set { colorTint = value; }
        }

        ///
        /// True if the sprite is (or should be) playing animation frames.  If this value is set
        /// to false, the sprite will not be drawn (a sprite needs at least 1 single frame animation
        /// in order to be displayed.
        ///
        public bool IsAnimating
        {
            get { return bAnimating; }
            set { bAnimating = value; }
        }

        ///
        /// The FrameAnimation object of the currently playing animation
        ///
        public FrameAnimation CurrentFrameAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(sCurrentAnimation))
                    return faAnimations[sCurrentAnimation];
                else
                    return null;
            }
        }

        ///
        /// The string name of the currently playing animaton.  Setting the animation
        /// resets the CurrentFrame and PlayCount properties to zero.
        ///
        public string CurrentAnimation
        {
            get { return sCurrentAnimation; }
            set
            {
                if (faAnimations.ContainsKey(value))
                {
                    if (sCurrentAnimation != string.Empty && faAnimations[sCurrentAnimation].PlayCount > 0)
                    {
                        sCurrentAnimation = value;
                        faAnimations[sCurrentAnimation].CurrentFrame = 0;
                        faAnimations[sCurrentAnimation].PlayCount = 0;
                    }                    
                }
            }
        }

        public SpriteAnimation(Texture2D Texture)
        {
            t2dTexture = Texture;
        }

        void UpdateRotation()
        {
            if (bRotateByPosition)
            {
                fRotation = (float)Math.Atan2(v2Position.Y - v2LastPosition.Y, v2Position.X - v2LastPosition.X);
            }
        }

        public void AddAnimation(string Name, int X, int Y, int Width, int Height, int Frames, float FrameLength, bool bIsLoopAnimation)
        {
            FrameAnimation fa = new FrameAnimation(X, Y, Width, Height, Frames, FrameLength, bIsLoopAnimation);
            faAnimations.Add(Name, fa);
            iWidth = Width;
            iHeight = Height;
            v2Center = new Vector2(iWidth / 2, iHeight / 2);
            sCurrentAnimation = Name;
            faCurrAnimation = fa;
        }

        public void AddAnimation(string Name, int X, int Y, int Width, int Height, int Frames, float FrameLength, bool bIsLoopAnimation, string NextAnimation)
        {
            FrameAnimation fa = new FrameAnimation(X, Y, Width, Height, Frames, FrameLength,bIsLoopAnimation, NextAnimation);
            faAnimations.Add(Name, fa);
            iWidth = Width;
            iHeight = Height;
            v2Center = new Vector2(iWidth / 2, iHeight / 2);
            sCurrentAnimation = Name;
            faCurrAnimation = fa;
        }

        public FrameAnimation GetAnimationByName(string Name)
        {
            if (faAnimations.ContainsKey(Name))
            {
                return faAnimations[Name];
            }
            else
            {
                return null;
            }
        }

        public void MoveBy(int x, int y)
        {
            v2LastPosition = v2Position;
            v2Position.X += x;
            v2Position.Y += y;
            UpdateRotation();
        }

        public void Update(GameTime gameTime)
        {
            // Don't do anything if the sprite is not animating
            if (bAnimating)
            {
                // If there is not a currently active animation
                if (CurrentFrameAnimation == null)
                {
                    // Make sure we have an animation associated with this sprite
                    if (faAnimations.Count > 0)
                    {
                        // Set the active animation to the first animation
                        // associated with this sprite
                        string[] sKeys = new string[faAnimations.Count];
                        faAnimations.Keys.CopyTo(sKeys, 0);
                        CurrentAnimation = sKeys[0];
                    }
                    else
                    {
                        return;
                    }
                }

                // Run the Animation's update method
                CurrentFrameAnimation.Update(gameTime);

                // Check to see if there is a "followup" animation named for this animation
                if (!String.IsNullOrEmpty(CurrentFrameAnimation.NextAnimation))
                {
                    // If there is, see if the currently playing animation has
                    // completed a full animation loop
                    if (CurrentFrameAnimation.PlayCount > 0)
                    {
                        // If it has, set up the next animation
                        CurrentAnimation = CurrentFrameAnimation.NextAnimation;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int XOffset, int YOffset)
        {
            if (CurrentFrameAnimation == null)
                return;
            if (bAnimating)
                spriteBatch.Draw(t2dTexture, (v2Position + new Vector2(XOffset, YOffset) + v2Center), CurrentFrameAnimation.FrameRectangle, colorTint, fRotation, v2Center, 1f, SpriteEffects.None, 0);
        }
        public void Draw(SpriteBatch spriteBatch, int XOffset, int YOffset, float fAngle)
        {
            if (CurrentFrameAnimation == null)
                return;
            if (bAnimating)
            {
                if (bIsCollision)
                {
                    colorTint = Color.Red;
                }
                else
                {
                    colorTint = Color.White;
                }
                spriteBatch.Draw(t2dTexture, (v2Position + new Vector2(XOffset, YOffset) + v2Center), CurrentFrameAnimation.FrameRectangle, colorTint, fAngle, v2Center, 1f, SpriteEffects.None, 0);
                bIsCollision = false;
            }
        }

        public SpriteAnimation Clone()
        {
            SpriteAnimation tmp = new SpriteAnimation(this.t2dTexture);
            
            tmp.sCurrentAnimation = this.sCurrentAnimation;
            tmp.t2dTexture = this.t2dTexture;
            tmp.v2Center = this.v2Center;
            tmp.v2LastPosition = this.v2LastPosition;
            tmp.v2Position = this.v2Position;
            tmp.iWidth = this.iWidth;
            tmp.iHeight = this.iHeight;
            tmp.fRotation = this.fRotation;
            tmp.faCurrAnimation = this.faCurrAnimation;
            tmp.faAnimations = this.faAnimations;
            tmp.colorTint = this.colorTint;
            tmp.bRotateByPosition = this.bRotateByPosition;
            tmp.bAnimating = this.bAnimating;

            return tmp;
        }
    }
}
