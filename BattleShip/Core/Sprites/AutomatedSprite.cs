using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BattleShip.Core.GameComponents;

namespace BattleShip.Core.Sprites
{
    class AutomatedSprite : Sprite
    {
        protected float m_iTimerChangeWay = 2.0f;    //thoi gian thay doi huong di chuyen
        protected float m_iCurrTimeChangeWay = 0;
        protected Random m_Random;
        protected bool bRandX;

        public AutomatedSprite():base()
        {
            m_ColorOnMap = Color.Red;
            bAutoCheckDirection = true;     //tu dong xac dinh huong di chuyen
            bMovingTowardsTarget = true;
            bActive = true;
            bVisible = true;
            m_Random = new Random();
            m_iDeltaY = m_iDeltaX = 20;
            m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Down;
        }

        public AutomatedSprite(Texture2D t2dTexture)
            : base(t2dTexture)
        {
            m_ColorOnMap = Color.Red;
            bAutoCheckDirection = true;     //tu dong xac dinh huong di chuyen
            bMovingTowardsTarget = true;
            bActive = true;
            bVisible = true;
            m_Random = new Random();
            m_iDeltaY = m_iDeltaX = 20;
            m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Down;
        }               

        public void ChangeWay(Rectangle clientBounds)
        {
            if (bRandX)
            {
                //random x
                int x = m_Random.Next(clientBounds.Width);
                bRandX = false;

                v2Target.X = x;
                v2Target.Y = asSprite.Y;
            }
            else
            {
                //random y
                int y = m_Random.Next(clientBounds.Height);
                bRandX = true;

                v2Target.X = asSprite.X;
                v2Target.Y = y;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {            
            float ellapse = (float)gameTime.ElapsedGameTime.TotalSeconds;
            fTotalElapsedTimeKeyPress += ellapse;
            if (fTotalElapsedTimeKeyPress >= fKeyPressCheckDelay)
            {
                fTotalElapsedTimeKeyPress = 0;
                if (bActive)
                {
                    Vector2 v2OldPos = Position;

                    base.Update(gameTime, clientBounds);

                    m_iCurrTimeChangeWay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    m_iCurrTimeAttack += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (m_iCurrTimeChangeWay >= m_iTimerChangeWay)
                    {
                        //change way
                        m_iCurrTimeChangeWay = 0;

                        ChangeWay(clientBounds);
                    }

                    //gap vat can thi chuyen huong di chuyen
                    int xMap;
                    int yMap;
                    int data;

                    bool canWalk = true;
                    switch (m_CurrDirection)
                    {
                        case BattleShip.Core.Utils.Common.Direction.Left:
                            
                            if (!MapComponent.CheckCanWalk((int)(asSprite.Position.X - asSprite.Width - m_iDeltaX), (int)asSprite.Position.Y))
                            {
                                canWalk = false;
                            }
                            break;
                        case BattleShip.Core.Utils.Common.Direction.Right:
                                                                                    
                            if (!MapComponent.CheckCanWalk(((int)(asSprite.Position.X + asSprite.Width + m_iDeltaX)), ((int)asSprite.Position.Y)))
                            {
                                canWalk = false;
                            }
                            break;
                        case BattleShip.Core.Utils.Common.Direction.Up:
                            
                            if (!MapComponent.CheckCanWalk(((int)asSprite.Position.X), ((int)asSprite.Position.Y - asSprite.Height - m_iDeltaY)))
                            {
                                canWalk = false;
                            }
                            break;
                        case BattleShip.Core.Utils.Common.Direction.Down:
                            
                            if (!MapComponent.CheckCanWalk(((int)asSprite.Position.X), ((int)(asSprite.Position.Y + asSprite.Height + m_iDeltaY))))
                            {
                                canWalk = false;
                            }
                            break;
                        case BattleShip.Core.Utils.Common.Direction.None:
                            break;
                        default:
                            break;
                    }
                    if (!canWalk)
                    {
                        Position = v2OldPos;
                        ChangeWay(clientBounds);
                    }

                    
                    //neu ket thuc di chuyen thi tao huong di moi
                    if (asSprite.Position == v2Target)
                    {
                        ChangeWay(clientBounds);
                    }

                    bIsAttack = false;
                    if (m_iCurrTimeAttack >= m_fTimerAttack)
                    {
                        //attack
                        m_iCurrTimeAttack = 0;

                        bIsAttack = true;
                    }
                    
                }
            }
        }

        public override Sprite Clone()
        {
            AutomatedSprite tmp = new AutomatedSprite();

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
            tmp.m_fTimerAttack = this.m_fTimerAttack;
            tmp.m_iTimerChangeWay = this.m_iTimerChangeWay;
            tmp.m_iCurrTimeAttack = this.m_iCurrTimeAttack;
            tmp.m_iCurrTimeChangeWay = this.m_iCurrTimeChangeWay;
            tmp.m_Random = this.m_Random;
            tmp.bRandX = this.bRandX;
            tmp.Point = this.Point;
            tmp.m_fTimerAttack = this.m_fTimerAttack;
            
            return tmp;
        }
                
    }
}
