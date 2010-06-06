using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleShip.Core.Sprites
{    
    public class AnimateEffect
    {        
        protected SpriteAnimation asSprite;
        protected bool bActive;
        protected bool bVisible;
        protected bool bLoop = false;
        protected float m_fTimeToLive = -1;   //thoi gian sprite co hieu luc. neu bang -1 thi khong su dung field nay
        protected float m_fCurrTime = 0;
        protected string m_strName = string.Empty;
        protected int m_iPoint = -1;    //=-1 neu khong su dung
        protected int iCollisionBufferX;
        protected int iCollisionBufferY;
        public AnimateEffect(Texture2D t2dTexture)
        {
            asSprite = new SpriteAnimation(t2dTexture);
            bActive = false;
            bVisible = false;
            
        }

        public AnimateEffect()
        {            
            bActive = false;
            bVisible = false;
        }

        public int Point
        {
            get { return m_iPoint; }
            set { m_iPoint = value; }
        }

        public string Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }

        public bool IsLoop
        {
            set { bLoop = value; }
        }

        public float TimeToLive
        {
            set { m_fTimeToLive = value; }
            get { return m_fTimeToLive; }
        }

        public bool IsResetAnimation
        {
            set { asSprite.CurrentFrameAnimation.IsResetAnimation = value; }
        }

        public SpriteAnimation MySprite
        {
            get { return asSprite; }
        }

        public Vector2 Position
        {
            get { return asSprite.Position; }
            set { asSprite.Position = value; }
        }
                
        public bool IsVisible
        {
            get { return bVisible; }
            set { bVisible = value; }
        }

        public bool IsActive
        {
            get { return bActive; }
            set { bActive = value; }
        }
                
        public void Activate(int x, int y)
        {
            asSprite.Position = new Vector2(x, y);
            
            bActive = true;
            bVisible = true;
            m_fCurrTime = 0;
            asSprite.CurrentFrameAnimation.CurrentFrame = 0;
        }

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(asSprite.BoundingBox.X + iCollisionBufferX, asSprite.BoundingBox.Y + iCollisionBufferY, asSprite.Width - (2 * iCollisionBufferX), asSprite.Height - (2 * iCollisionBufferY));
            }
        }

        virtual public void Update(GameTime gameTime)
        {            
            if (bActive)
            {
                float ellapse = (float)gameTime.ElapsedGameTime.TotalSeconds;
                m_fCurrTime += ellapse;
                if (m_fCurrTime > m_fTimeToLive && m_fTimeToLive != -1)
                {
                    m_fCurrTime = 0;
                    bActive = false;
                    bVisible = false;
                    return;
                }

                asSprite.Update(gameTime);
                if (asSprite.CurrentFrameAnimation.PlayCount > 0 && !bLoop)
                {
                    bActive = false;
                    bVisible = false;
                }
            }            
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            if (bActive)
            {
                asSprite.Draw(spriteBatch, 0, 0);
            }
        }
    }
}
