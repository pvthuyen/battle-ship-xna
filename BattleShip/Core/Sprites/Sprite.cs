using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BattleShip.Core.Sprites
{
    public abstract class Sprite
    {                
        protected Point m_FrameSize;
        protected int m_CollisionOffset;
        protected Point m_CurrentFrame;
        protected Point m_SheetSize;
        
        protected const int m_DefaultMillisecondsPerFrame = 16;
        /// <summary>
        /// /////////////
        /// </summary>
        protected int m_iHeightScreen;
        protected int m_iWidthScreen;

        protected Vector2 m_Position;
        
        protected Rectangle m_rectBounding;

        protected Vector2 m_Speed;

        protected int m_iCurrIndex;
        protected int m_iTotalFrame;

        protected bool m_bIsCollision;

        protected Texture2D[] m_arrFrame;
                
        //frame rate
        protected int m_iTimeSinceLastFrame = 0;
        protected int m_iMillisecondsPerFrame;

        protected int m_iHP;
        protected int m_iDefence;
        protected int m_iLevel;
        protected string m_strName;

        public Texture2D[] ArrFrame
        {
            set
            {
                this.m_arrFrame = value;
                this.m_iCurrIndex = 0;
                this.m_iTotalFrame = this.m_arrFrame.Length;
                this.m_rectBounding.Width = this.m_arrFrame[0].Width;
                this.m_rectBounding.Height = this.m_arrFrame[0].Height;
            }
        }
        
        public string Name
        {
            get { return this.m_strName; }
            set { this.m_strName = value; }
        }

        public Rectangle BoundingRect
        {
            get { return this.m_rectBounding; }
            set { this.m_rectBounding = value; }
        }
        public Sprite()
        {
            this.m_iHeightScreen = 0;
            this.m_iWidthScreen = 0;
            this.m_iCurrIndex = 0;
            this.m_iTotalFrame = 0;
            this.m_rectBounding = new Rectangle();
            this.m_bIsCollision = false;
        }

        public bool CheckCollision(Sprite[] arrSprite)
        {
            int i;
            for (i = 0; i < arrSprite.Length; i++)
            {
                if (this.CheckCollision(arrSprite[i]))
                {
                    this.m_bIsCollision = true;
                    arrSprite[i].m_bIsCollision = true;
                    return true;
                }
            }

            this.m_bIsCollision = false;
            return false;
        }

        public bool CheckCollision(Sprite sprite)
        {
            this.m_bIsCollision = this.m_rectBounding.Intersects(sprite.BoundingRect);
            if (this.m_bIsCollision)
            {
                sprite.m_bIsCollision = true;
                return true;
            }
            else
            {
                sprite.m_bIsCollision = false;
                return false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.m_bIsCollision)
            {
                spriteBatch.Draw(this.m_arrFrame[this.m_iCurrIndex], this.m_Position, Color.Red);
            }
            else
            {
                spriteBatch.Draw(this.m_arrFrame[this.m_iCurrIndex], this.m_Position, Color.White);
            }
        }

        public void Update(GameTime gameTime)
        {
            this.m_iTimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (this.m_iTimeSinceLastFrame > this.m_iMillisecondsPerFrame)
            {
                this.m_iTimeSinceLastFrame -= this.m_iMillisecondsPerFrame;

                Go();
                                
                this.m_rectBounding.X = (int)this.m_Position.X;
                this.m_rectBounding.Y = (int)this.m_Position.Y;

                this.m_iCurrIndex = (this.m_iCurrIndex + 1) % this.m_iTotalFrame;
            }
        }

        abstract public void Go();
        public abstract Sprite Clone();                  
    }
}
