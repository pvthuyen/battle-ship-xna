using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BattleShip.Core.Sprites
{
    public abstract class Sprite : DrawableGameComponent
    {               
        protected const int m_DefaultMillisecondsPerFrame = 16;
        
        //kich thuoc man hinh
        protected int m_iHeightScreen;
        protected int m_iWidthScreen;

        //kich thuoc cua 1 tile (frame)
        protected int m_iTileWidth;
        protected int m_iTileHeight;

        protected Vector2 m_Position;       //vi tri cua sprite        
        protected Rectangle m_rectBounding; //rect cua sprite
        protected Vector2 m_Speed;          //toc do di chuyen cua sprite

        protected int m_iCurrIndex;
        protected int m_iTotalFrame;

        protected bool m_bIsCollision;

        protected Texture2D m_Texture;
                
        //frame rate
        protected int m_iTimeSinceLastFrame = 0;
        protected int m_iMillisecondsPerFrame;

        protected int m_iHP;
        protected int m_iDefence;
        protected int m_iLevel;
        protected string m_strName;

        protected SpriteBatch m_SpriteBatch;

        public Texture2D ArrFrame
        {
            set
            {
                this.m_Texture = value;                
            }
        }
        
        public string Name
        {
            get { return this.m_strName; }            
        }

        public Rectangle BoundingRect
        {
            get { return this.m_rectBounding; }            
        }

        public void SetPosition(Vector2 pos)
        {
            this.m_Position = pos;
        }
        
        public Sprite(Game game)
            : base(game)
        {
            this.m_iHeightScreen = 0;
            this.m_iWidthScreen = 0;
            this.m_iCurrIndex = 0;
            this.m_iTotalFrame = 0;
            this.m_rectBounding = new Rectangle();
            this.m_bIsCollision = false;
            this.m_Speed = new Vector2(10, 10);
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
                
        public override void Update(GameTime gameTime)
        {            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.m_bIsCollision)
            {
                m_SpriteBatch.Draw(this.m_Texture, this.m_Position, new Rectangle(0,0,this.m_iTileWidth, this.m_iTileHeight), Color.Red);
            }
            else
            {
                m_SpriteBatch.Draw(this.m_Texture, this.m_Position, new Rectangle(0, 0, this.m_iTileWidth, this.m_iTileHeight), Color.Red);
            }

            base.Draw(gameTime);
        }
               
        abstract public void TurnLeft();
        abstract public void TurnRight();
        abstract public void GoUp();
        abstract public void GoDown();
        public abstract Sprite Clone();                  
    }
}
