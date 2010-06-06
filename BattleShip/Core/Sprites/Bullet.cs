using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Xml;
using BattleShip.Core.Managers;
using Microsoft.Xna.Framework.Content;

namespace BattleShip.Core.Sprites
{
    public class Bullet : AnimateEffect
    {
        protected Rectangle rectPlayField;
        
        protected int m_iNumBullet;             //so luong dan        
        protected int m_iMaxNumBullet;

        protected int m_iAttack;                //suc tan cong cua dan
        protected SoundEffect m_SoundGun;       //am thanh phat ra khi ban
        
        protected Vector2 v2Target;
        
        protected int iSpeed;
        protected BattleShip.Core.Utils.Common.Direction m_CurrDirection;
                        
        public BattleShip.Core.Utils.Common.Direction CurrDirection
        {
            set { m_CurrDirection = value; }
        }

        public int MaxNumBullet
        {
            get { return m_iMaxNumBullet; }
        }

        public int NumBullet
        {
            set { m_iNumBullet = value; }
            get { return m_iNumBullet; }
        }
                
        public int Attack
        {
            get { return m_iAttack; }
        }

        public SpriteAnimation Sprite
        {
            get { return asSprite; }
            set { asSprite = value; }
        }
        
        public Vector2 Target
        {
            get { return v2Target; }
            set { v2Target = value; }
        }
                
        public Bullet(Texture2D t2dTexture)
        {
            asSprite = new SpriteAnimation(t2dTexture);
            iSpeed = 1;
            asSprite.AutoRotate = true;
            bActive = false;
            bVisible = false;
            m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Up;
        }

        public Bullet()
        {
            iSpeed = 1;
            bActive = false;
            bVisible = false;
            m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Up;
        }

        public void LoadFromXml(XmlNode node, ContentManager contentManager)
        {            
            int iTileWidth = 0;
            int iTileHeight = 0;
            int iFrameCount = 0;
            string strSound = string.Empty;

            foreach (XmlAttribute att in node.Attributes)
            {
                switch (att.Name)
                {
                    case "Name":
                        m_strName = att.Value;
                        break;
                    case "TileWidth":
                        iTileWidth = int.Parse(att.Value);
                        break;
                    case "TileHeight":
                        iTileHeight = int.Parse(att.Value);
                        break;
                    case "FrameCount":
                        iFrameCount = int.Parse(att.Value);
                        break;
                    case "Speed":
                        iSpeed = int.Parse(att.Value);
                        break;
                    case "NumBullet":
                        m_iMaxNumBullet = m_iNumBullet = int.Parse(att.Value);
                        break;
                    case "CollisionBufferX":
                        iCollisionBufferX = int.Parse(att.Value);
                        break;
                    case "CollisionBufferY":
                        iCollisionBufferY = int.Parse(att.Value);
                        break;
                    case "SoundEffect":
                        strSound = att.Value;
                        break;
                    case "Attack":
                        m_iAttack = int.Parse(att.Value);
                        break;
                }
            }

            Texture2D t2d = contentManager.Load<Texture2D>(@"Resource/Sprite/Bullet/" + m_strName);
            asSprite = new SpriteAnimation(t2d);
            asSprite.AddAnimation("bullet", 0, 0, iTileWidth, iTileHeight, iFrameCount, 0.1f, true);

            m_SoundGun = contentManager.Load<SoundEffect>(@"Resource/Sound/" + strSound);
        }

        public Bullet Clone()
        {
            Bullet tmp = new Bullet();

            tmp.asSprite = this.asSprite.Clone();
            tmp.bActive = this.bActive;
            tmp.bVisible = this.bVisible;
            tmp.iSpeed = this.iSpeed;
            tmp.iCollisionBufferX = this.iCollisionBufferX;
            tmp.iCollisionBufferY = this.iCollisionBufferY;
            tmp.m_CurrDirection = this.m_CurrDirection;
            tmp.v2Target = this.v2Target;
            tmp.m_iNumBullet = this.m_iNumBullet;
            tmp.m_SoundGun = this.m_SoundGun;
            tmp.m_iAttack = this.m_iAttack;
            tmp.m_strName = this.m_strName;
            tmp.rectPlayField = this.rectPlayField;
            tmp.m_strName = this.m_strName;
            tmp.m_iPoint = this.m_iPoint;
            tmp.m_fTimeToLive = this.m_fTimeToLive;
            tmp.m_fCurrTime = this.m_fCurrTime;
            tmp.bLoop = this.bLoop;
            tmp.m_iMaxNumBullet = this.m_iMaxNumBullet;
            return tmp;
        }

        public void Fire(int x, int y, BattleShip.Core.Utils.Common.Direction direction, Rectangle rectPlayField)
        {
            m_SoundGun.Play();

            bActive = true;
            bVisible = true;
            asSprite.X = x;
            asSprite.Y = y;
            m_CurrDirection = direction;
            this.rectPlayField = rectPlayField;
        }

        public override void Update(GameTime gameTime)
        {
            if (bActive)
            {
                if (m_fTimeToLive != -1)
                {
                    float ellapse = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    m_fCurrTime += ellapse;
                    if (m_fCurrTime > m_fTimeToLive)
                    {
                        m_fCurrTime = 0;
                        bVisible = false;
                        bActive = false;
                    }
                }
                else
                {
                    switch (m_CurrDirection)
                    {
                        case BattleShip.Core.Utils.Common.Direction.Left:
                            asSprite.X -= iSpeed;
                            break;
                        case BattleShip.Core.Utils.Common.Direction.Right:
                            asSprite.X += iSpeed;
                            break;
                        case BattleShip.Core.Utils.Common.Direction.Up:
                            asSprite.Y -= iSpeed;
                            break;
                        case BattleShip.Core.Utils.Common.Direction.Down:
                            asSprite.Y += iSpeed;
                            break;
                        case BattleShip.Core.Utils.Common.Direction.None:
                            bActive = false;
                            bVisible = false;
                            break;
                        default:
                            break;
                    }
                }

                asSprite.Update(gameTime);

                if (m_fTimeToLive == -1)
                {
                    if ((asSprite.X > rectPlayField.Width) || (asSprite.X < 0)
                        || asSprite.Y > rectPlayField.Height || asSprite.Y < 0)
                    {
                        bActive = false;
                        bVisible = false;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (bActive)
            {
                switch (m_CurrDirection)
                {
                    case BattleShip.Core.Utils.Common.Direction.Left:
                        asSprite.Draw(spriteBatch, 0, 0, (float)(Math.PI / 2));
                        break;
                    case BattleShip.Core.Utils.Common.Direction.Right:
                        asSprite.Draw(spriteBatch, 0, 0, (float)(3 * Math.PI / 2));
                        break;
                    case BattleShip.Core.Utils.Common.Direction.Up:
                        asSprite.Draw(spriteBatch, 0, 0, (float)Math.PI);
                        break;
                    case BattleShip.Core.Utils.Common.Direction.Down:
                        asSprite.Draw(spriteBatch, 0, 0, 0);
                        break;
                    case BattleShip.Core.Utils.Common.Direction.None:
                        asSprite.Draw(spriteBatch, 0, 0, 0);
                        break;
                    default:
                        break;
                }
            }
        }        
    }
}
