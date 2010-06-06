using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml;
using BattleShip.Core.GameComponents;
using BattleShip.Core.Managers;

namespace BattleShip.Core.Sprites
{
    public class UserControlledSprite : Sprite
    {
        protected InputManager m_InputManager;
        //bullets
        protected List<Bullet> m_lstBullets = new List<Bullet>();
        protected int m_iBulletUsing;

        protected int m_iTotalLives = 3;
        protected int m_iTotalStars = 0;
        protected int m_iTotalGolds = 0;

        protected bool m_bSwitchBullet = false;
                
        public bool IsSwitchBullet
        {
            get { return m_bSwitchBullet; }
        }

        public int TotalLives
        {
            get { return m_iTotalLives; }
            set { m_iTotalLives = value; }
        }
        
        public int TotalGolds
        {
            get { return m_iTotalGolds; }
            set { m_iTotalGolds = value; }
        }
        
        public int TotalStars
        {
            set { m_iTotalStars = value; }
            get { return m_iTotalStars; }
        }
        
        public Bullet BulletUsing
        {
            get { return m_lstBullets[m_iBulletUsing]; }
        }

        public void Restart()
        {
            m_iTotalGolds = 0;
            m_iTotalLives = 3;
            m_iTotalStars = 0;
            ResetBullets();
            m_iCurrHP = m_iMaxHP;
            m_iLevel = 0;
        }

        public UserControlledSprite()
        {
            m_ColorOnMap = Color.Blue;
            bActive = true;
            bMovingTowardsTarget = false;
            m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Down;
            m_InputManager = new InputManager();
            m_iDeltaY = m_iDeltaX = 5;
        }

        public UserControlledSprite(Texture2D t2dTexture)
            : base(t2dTexture)
        {
            m_ColorOnMap = Color.Blue;
            bActive = true;
            bMovingTowardsTarget = false;
            m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Down;
            m_InputManager = new InputManager();
            m_iDeltaY = m_iDeltaX = 5;
        }

        public void InitNextBullet(Bullet[] arrBullet)
        {
            int i;
            for (i = 0; i < arrBullet.Length; i++)
            {
                arrBullet[i] = m_lstBullets[m_iBulletUsing].Clone();
            }
        }
        
        private int SearchBullet(Bullet bullet)
        {
            for (int i = 0; i < m_lstBullets.Count; i++)
            {
                if (bullet.Name.Equals(m_lstBullets[i].Name))
                {
                    return i;
                }
            }
            return -1;
        }

        public void AddBullet(Bullet bullet)
        {
            int index = SearchBullet(bullet);
            if (index == -1)
            {
                bullet.TimeToLive = -1;
                m_lstBullets.Add(bullet);
            }
            else
            {
                m_lstBullets[index].NumBullet += bullet.NumBullet;
            }
        }

        override public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            float ellapse = (float)gameTime.ElapsedGameTime.TotalSeconds;
            m_iCurrTimeAttack += ellapse;

            UpdatePlayerProperties();

            fTotalElapsedTimeKeyPress = 0;
            bIsAttack = false;
            m_bSwitchBullet = false;

            m_InputManager.BeginHandler();
            bool enter = m_InputManager.IsKeyboardPress(Keys.Enter);
            m_bSwitchBullet = m_InputManager.IsKeyboardPress(Keys.LeftControl);
            m_InputManager.EndHandler();

            bool canWalk = true;

            v2DirectionSpeed = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                v2DirectionSpeed.X = -1;
                m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Left;
                if (!MapComponent.CheckCanWalk((int)(asSprite.CenterPosition.X + v2DirectionSpeed.X - m_iDeltaX), (int)(asSprite.CenterPosition.Y + v2DirectionSpeed.Y)))
                {
                    canWalk = false;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                v2DirectionSpeed.X = 1;
                m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Right;
                if (!MapComponent.CheckCanWalk((int)(asSprite.CenterPosition.X + asSprite.Width + v2DirectionSpeed.X + m_iDeltaX), (int)(asSprite.CenterPosition.Y + v2DirectionSpeed.Y)))
                {
                    canWalk = false;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                v2DirectionSpeed.Y = -1;
                m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Up;

                if (!MapComponent.CheckCanWalk((int)(asSprite.CenterPosition.X + v2DirectionSpeed.X), (int)(asSprite.CenterPosition.Y + v2DirectionSpeed.Y - m_iDeltaY)))
                {
                    canWalk = false;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                v2DirectionSpeed.Y = 1;
                m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Down;
                if (!MapComponent.CheckCanWalk((int)(asSprite.CenterPosition.X + v2DirectionSpeed.X), (int)(asSprite.CenterPosition.Y + asSprite.Height + v2DirectionSpeed.Y + m_iDeltaY)))
                {
                    canWalk = false;
                }
            }

            if (enter)
            {
                if (m_iCurrTimeAttack >= (m_fTimerAttack - m_iLevel * 1.0 / 3))
                {
                    m_iCurrTimeAttack = 0;
                    bIsAttack = true;
                }
            }

            if (m_bSwitchBullet)
            {
                SelectNextBullet();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                m_iCurrHP = -1;
                IsResetAnimation = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                m_iCurrHP = 50;
                bActive = true;
                bVisible = true;
            }

            if (canWalk)
            {
                asSprite.Position += v2DirectionSpeed * (fSpeed + m_iLevel);
            }

            m_ScrollMapDirection = BattleShip.Core.Utils.Common.Direction.None;

            // If the sprite is off the screen, put it back in play
            if (asSprite.X < 0)
            {
                asSprite.X = 0;
                m_ScrollMapDirection = BattleShip.Core.Utils.Common.Direction.Left;
            }
            if (asSprite.Y < 0)
            {
                asSprite.Y = 0;
                m_ScrollMapDirection = BattleShip.Core.Utils.Common.Direction.Up;
            }
            if (asSprite.X > clientBounds.Width - asSprite.Width)
            {
                asSprite.X = clientBounds.Width - asSprite.Width;
                m_ScrollMapDirection = BattleShip.Core.Utils.Common.Direction.Right;
            }
            if (asSprite.Y > clientBounds.Height - asSprite.Height)
            {
                asSprite.Y = clientBounds.Height - asSprite.Height;
                m_ScrollMapDirection = BattleShip.Core.Utils.Common.Direction.Down;
            }

            asSprite.Update(gameTime);
        }

        private void SelectNextBullet()
        {
            m_iBulletUsing++;
            if (m_iBulletUsing >= m_lstBullets.Count)
            {
                m_iBulletUsing = 0;
            }
        }

        public void NewLive()
        {
            m_iCurrHP = m_iMaxHP;
            m_iTotalLives--;

            bVisible = true;
            bActive = true;

            m_iLevel = 0;

            ResetBullets();
        }

        private void ResetBullets()
        {
            if (m_lstBullets.Count > 1)
            {
                int count = m_lstBullets.Count;
                m_lstBullets.RemoveRange(1, count - 1);
            }
            m_lstBullets[0].NumBullet = m_lstBullets[0].MaxNumBullet;
            m_iBulletUsing = 0;
        }

        private void UpdatePlayerProperties()
        {
            //update properties's player: speed, hp
            //collect three stars -> increase level
            if (m_iTotalStars >= 3)
            {
                m_iLevel++;
                m_iTotalStars = 0;
            }

            //max level = 3
            if (m_iLevel > 3)
                m_iLevel = 3;
        }

        override public Sprite Clone()
        {
            UserControlledSprite tmp = new UserControlledSprite();

            tmp.asSprite = this.asSprite.Clone();
            tmp.bActive = this.bActive;
            tmp.bAutoCheckDirection = this.bAutoCheckDirection;
            tmp.bDeactivateAtEndOfPath = this.bDeactivateAtEndOfPath;
            tmp.bHideAtEndOfPath = this.bHideAtEndOfPath;
            tmp.bLoopPath = this.bLoopPath;
            tmp.bMovingTowardsTarget = this.bMovingTowardsTarget;
            tmp.bPathing = this.bPathing;
            tmp.bVisible = this.bVisible;
            tmp.collisionOffset = this.collisionOffset;
            tmp.fSpeed = this.fSpeed;
            tmp.iCollisionBufferX = this.iCollisionBufferX;
            tmp.iCollisionBufferY = this.iCollisionBufferY;
            tmp.bIsSail = this.bIsSail;
            tmp.m_CurrDirection = this.m_CurrDirection;
            tmp.queuePath = this.queuePath;
            tmp.sEndPathAnimation = this.sEndPathAnimation;
            tmp.v2Target = this.v2Target;
            tmp.m_strName = this.m_strName;
            tmp.m_iLevel = this.m_iLevel;
            tmp.m_iCurrHP = this.m_iCurrHP;
            tmp.m_iMaxHP = this.m_iMaxHP;
            tmp.m_iDefence = this.m_iDefence;
            tmp.m_iAttack = this.m_iAttack;
            tmp.m_iSelectedBullet = this.m_iSelectedBullet;
            tmp.m_ColorOnMap = this.m_ColorOnMap;
            tmp.bIsAttack = this.bIsAttack;
            tmp.Point = this.Point;
            tmp.m_fTimerAttack = this.m_fTimerAttack;
            tmp.m_iTotalGolds = this.m_iTotalGolds;
            tmp.m_iTotalLives = this.m_iTotalLives;
            tmp.m_iTotalStars = this.m_iTotalStars;

            return tmp;
        }
    }
}
