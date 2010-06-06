using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BattleShip.Core.Managers;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace BattleShip.Core.Sprites
{
    public class Sprite
    {
        protected BattleShip.Core.Utils.Common.Direction m_CurrDirection;
        protected BattleShip.Core.Utils.Common.Direction m_ScrollMapDirection = BattleShip.Core.Utils.Common.Direction.None;

        protected SpriteAnimation asSprite;

        protected Queue<Vector2> queuePath = new Queue<Vector2>();

        protected Vector2 v2Target;

        protected float fSpeed = 1f;

        protected int iCollisionBufferX = 0;
        protected int iCollisionBufferY = 0;

        protected int m_iDeltaX = 5;
        protected int m_iDeltaY = 5;

        protected bool bActive = true;
        protected bool bMovingTowardsTarget = true;
        protected bool bPathing = true;
        protected bool bLoopPath = true;
        protected bool bVisible = true;
        protected bool bDeactivateAtEndOfPath = false;
        protected bool bHideAtEndOfPath = false;
        protected bool bAutoCheckDirection = true;
        protected bool bIsAttack = false;
        protected string sEndPathAnimation = null;
                
        /// <summary>
        /// //////////////////////////////////////////////
        /// </summary>
        protected int collisionOffset;

        protected bool bIsSail;      //tau co dang cang buom hay khong
                
        protected int m_iAttack;
        protected int m_iDefence;
        protected int m_iCurrHP;
        protected int m_iMaxHP;
        protected int m_iLevel;
        protected string m_strName;
        protected int m_iPoint;
        //protected List<Bullet> m_lstBullet;
        protected int m_iSelectedBullet;

        protected Color m_ColorOnMap;

        //thoi gian xu ly input
        protected float fKeyPressCheckDelay = 0;//0.25f;                
        protected float fTotalElapsedTimeKeyPress = 0;
        protected Vector2 v2DirectionSpeed;

        protected float m_fTimerAttack = 1.5f;       //thoi gian tan cong tu dong
        protected float m_iCurrTimeAttack = 0;
                

        /// <summary>
        /// ///////////////////////////////////////////////
        /// </summary>
        public BattleShip.Core.Utils.Common.Direction ScrollMap
        {
            get { return m_ScrollMapDirection; }
        }
        public Vector2 DirectionSpeed
        {
            get { return v2DirectionSpeed; }
            set { v2DirectionSpeed = value; }
        }
        public int Point
        {
            get { return m_iPoint; }
            set { m_iPoint = value; }
        }

        public bool IsCollision
        {
            set { asSprite.IsCollision = value; }
        }

        public Color ColorOnMap
        {
            get { return m_ColorOnMap; }
            set { m_ColorOnMap = value; }
        }

        public bool IsSail
        {
            get { return bIsSail; }
            set { bIsSail = value; }
        }

        public bool IsAttack
        {
            get { return bIsAttack; }
            set { bIsAttack = value; }
        }

        public string Name
        {
            get { return this.m_strName; }
        }
        public int Attack
        {
            get { return m_iAttack; }
            set { m_iAttack = value; }
        }

        public int Defence
        {
            get { return m_iDefence; }
            set { m_iDefence = value; }
        }

        public int MaxHP
        {
            get { return m_iMaxHP; }
            set { m_iMaxHP = value; }
        }

        public int CurrHP
        {
            get { return m_iCurrHP; }
            set { m_iCurrHP = value; }
        }
                
        public bool IsResetAnimation
        {
            set { asSprite.CurrentFrameAnimation.IsResetAnimation = value; }            
        }

        public bool AutoCheckDirection
        {
            set { bAutoCheckDirection = value; }
            get { return bAutoCheckDirection; }
        }

        public SpriteAnimation MySprite
        {
            get { return asSprite; }
        }

        public BattleShip.Core.Utils.Common.Direction SpriteDirection
        {
            set { m_CurrDirection = value; }
            get { return m_CurrDirection; }
        }
        
        public Vector2 Position
        {
            get { return asSprite.Position; }
            set { asSprite.Position = value; }
        }

        public Vector2 CenterPosition
        {
            get { return asSprite.CenterPosition; }
        }

        public Vector2 Target
        {
            get { return v2Target; }
            set { v2Target = value; }
        }

        public int HorizontalCollisionBuffer
        {
            get { return iCollisionBufferX; }
            set { iCollisionBufferX = value; }
        }

        public int VerticalCollisionBuffer
        {
            get { return iCollisionBufferY; }
            set { iCollisionBufferY = value; }
        }

        public bool IsPathing
        {
            get { return bPathing; }
            set { bPathing = value; }
        }

        public bool DeactivateAfterPathing
        {
            get { return bDeactivateAtEndOfPath; }
            set { bDeactivateAtEndOfPath = value; }
        }

        public bool LoopPath
        {
            get { return bLoopPath; }
            set { bLoopPath = value; }
        }

        public string EndPathAnimation
        {
            get { return sEndPathAnimation; }
            set { sEndPathAnimation = value; }
        }

        public bool HideAtEndOfPath
        {
            get { return bHideAtEndOfPath; }
            set { bHideAtEndOfPath = value; }
        }

        public bool IsVisible
        {
            get { return bVisible; }
            set { bVisible = value; }
        }

        public float Speed
        {
            get { return fSpeed; }
            set { fSpeed = value; }
        }

        public bool IsActive
        {
            get { return bActive; }
            set { bActive = value; }
        }

        public bool IsMoving
        {
            get { return bMovingTowardsTarget; }
            set { bMovingTowardsTarget = value; }
        }
                
        public Rectangle BoundingBox
        {
            get { return asSprite.BoundingBox; }
        }

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(asSprite.BoundingBox.X + iCollisionBufferX, asSprite.BoundingBox.Y + iCollisionBufferY, asSprite.Width - (2 * iCollisionBufferX), asSprite.Height - (2 * iCollisionBufferY));
            }
        }

        public Sprite()
        {
        }

        public Sprite(Texture2D texture)
        {
            asSprite = new SpriteAnimation(texture);
            m_CurrDirection = BattleShip.Core.Utils.Common.Direction.None;
            m_ColorOnMap = Color.Red;
        }

        public void AddPathNode(Vector2 node)
        {
            queuePath.Enqueue(node);
        }

        public void AddPathNode(int X, int Y)
        {
            queuePath.Enqueue(new Vector2(X, Y));
        }

        public void ClearPathNodes()
        {
            queuePath.Clear();
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {            
            if (bActive && bMovingTowardsTarget)
            {
                if (!(v2Target == null))
                {
                    // Get a vector pointing from the current location of the sprite
                    // to the destination.
                    Vector2 Delta = new Vector2(v2Target.X - asSprite.X, v2Target.Y - asSprite.Y);

                    if (bAutoCheckDirection)
                    {
                        double angle = Math.Atan2(asSprite.Position.Y - v2Target.Y, asSprite.Position.X - v2Target.X);
                        angle = 1.0 * angle * 180 / 3.14;

                        int result = (int)angle;
                        switch (result)
                        {
                            case 0:
                                m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Left;
                                break;
                            case 90:
                                m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Up;
                                break;
                            case 180:
                                m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Right;
                                break;
                            case -90:
                                m_CurrDirection = BattleShip.Core.Utils.Common.Direction.Down;
                                break;
                            default:
                                m_CurrDirection = BattleShip.Core.Utils.Common.Direction.None;
                                break;
                        }
                    }

                    if (Delta.Length() > Speed)
                    {
                        Delta.Normalize();
                        Delta *= Speed;
                        Position += Delta;
                    }
                    else
                    {
                        if (v2Target == asSprite.Position)
                        {
                            if (bPathing)
                            {
                                if (queuePath.Count > 0)
                                {
                                    v2Target = queuePath.Dequeue();
                                    if (bLoopPath)
                                    {
                                        queuePath.Enqueue(v2Target);
                                    }
                                }
                                else
                                {
                                    if (!(sEndPathAnimation == null))
                                    {
                                        if (!(asSprite.CurrentAnimation == sEndPathAnimation))
                                        {
                                            asSprite.CurrentAnimation = sEndPathAnimation;
                                        }
                                    }
                                    if (bDeactivateAtEndOfPath)
                                    {
                                        IsActive = false;
                                    }
                                    if (bHideAtEndOfPath)
                                    {
                                        IsVisible = false;                                        
                                    }
                                }
                            }
                        }
                        else
                        {
                            asSprite.Position = v2Target;
                        }
                    }
                }
            }
            if (bActive)
            {
                asSprite.Update(gameTime);                
            }
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            if (bVisible)
            {
                string animation = string.Empty;
                if (m_iCurrHP <= 0)
                {
                    bMovingTowardsTarget = false;
                    switch (m_CurrDirection)
                    {
                        case BattleShip.Core.Utils.Common.Direction.Left:
                            animation = "sink_left";
                            break;
                        case BattleShip.Core.Utils.Common.Direction.Right:
                            animation = "sink_right";
                            break;
                        case BattleShip.Core.Utils.Common.Direction.Up:
                            animation = "sink_up";
                            break;
                        case BattleShip.Core.Utils.Common.Direction.Down:
                            animation = "sink_down";
                            break;
                        case BattleShip.Core.Utils.Common.Direction.None:
                            break;
                        default:
                            break;
                    }                                       
                }
                else
                {
                    switch (m_CurrDirection)
                    {
                        case BattleShip.Core.Utils.Common.Direction.Left:
                            animation = "left";
                            break;
                        case BattleShip.Core.Utils.Common.Direction.Right:
                            animation = "right";
                            break;
                        case BattleShip.Core.Utils.Common.Direction.Up:
                            animation = "up";
                            break;
                        case BattleShip.Core.Utils.Common.Direction.Down:
                            animation = "down";
                            break;
                        default:                            
                            break;
                    }
                }
                if (animation != asSprite.CurrentAnimation)
                {
                    asSprite.CurrentAnimation = animation;                  
                }

                if (m_iCurrHP <= 0 && asSprite.CurrentFrameAnimation.PlayCount > 0)
                {
                    bActive = false;                    
                    bVisible = false;
                    return;
                }

                asSprite.Draw(spriteBatch, 0, 0);
                 
            }
        }

        virtual public void LoadFromXml(XmlNode node, ContentManager contentManager)
        {
            Point initPosition = new Point();    //tile hien thi up
            
            Texture2D textureImage = null; //texture cua sprite            
            Point frameSize = new Point();  //kich thuot 1 tile        

            int frameCount;

            XmlNodeList nodeList = node.ChildNodes;
            foreach (XmlNode tmp in nodeList)
            {
                switch (tmp.Name)
                {
                    case "TileSet":                        
                        textureImage = contentManager.Load<Texture2D>(@"Resource/Sprite/Computer/" + tmp.Attributes["FileName"].Value);
                        asSprite = new SpriteAnimation(textureImage);
                        break;
                    case "Position":
                        XmlNodeList childs = tmp.ChildNodes;
                        foreach (XmlNode child in childs)
                        {
                            switch (child.Name)
                            {
                                case "Up":
                                    initPosition.X = int.Parse(child.Attributes["X"].Value);
                                    initPosition.Y = int.Parse(child.Attributes["Y"].Value);
                                    frameSize.X = int.Parse(child.Attributes["TileWidth"].Value);
                                    frameSize.Y = int.Parse(child.Attributes["TileHeight"].Value);
                                    frameCount = int.Parse(child.Attributes["FrameCount"].Value);
                                    asSprite.AddAnimation("up", initPosition.X, initPosition.Y, frameSize.X, frameSize.Y, frameCount, 0.1f, true);
                                    break;
                                case "Down":
                                    initPosition.X = int.Parse(child.Attributes["X"].Value);
                                    initPosition.Y = int.Parse(child.Attributes["Y"].Value);
                                    frameSize.X = int.Parse(child.Attributes["TileWidth"].Value);
                                    frameSize.Y = int.Parse(child.Attributes["TileHeight"].Value);
                                    frameCount = int.Parse(child.Attributes["FrameCount"].Value);
                                    asSprite.AddAnimation("down", initPosition.X, initPosition.Y, frameSize.X, frameSize.Y, frameCount, 0.1f, true);                                    
                                    break;
                                case "Left":
                                    initPosition.X = int.Parse(child.Attributes["X"].Value);
                                    initPosition.Y = int.Parse(child.Attributes["Y"].Value);
                                    frameSize.X = int.Parse(child.Attributes["TileWidth"].Value);
                                    frameSize.Y = int.Parse(child.Attributes["TileHeight"].Value);
                                    frameCount = int.Parse(child.Attributes["FrameCount"].Value);
                                    asSprite.AddAnimation("left", initPosition.X, initPosition.Y, frameSize.X, frameSize.Y, frameCount, 0.1f, true);
                                    break;
                                case "Right":
                                    initPosition.X = int.Parse(child.Attributes["X"].Value);
                                    initPosition.Y = int.Parse(child.Attributes["Y"].Value);
                                    frameSize.X = int.Parse(child.Attributes["TileWidth"].Value);
                                    frameSize.Y = int.Parse(child.Attributes["TileHeight"].Value);
                                    frameCount = int.Parse(child.Attributes["FrameCount"].Value);
                                    asSprite.AddAnimation("right", initPosition.X, initPosition.Y, frameSize.X, frameSize.Y, frameCount, 0.1f, true);
                                    break;
                            }
                        }
                        break;
                    case "Sink":
                        childs = tmp.ChildNodes;
                        foreach (XmlNode child in childs)
                        {
                            switch (child.Name)
                            {
                                case "Up":
                                    initPosition.X = int.Parse(child.Attributes["X"].Value);
                                    initPosition.Y = int.Parse(child.Attributes["Y"].Value);
                                    frameSize.X = int.Parse(child.Attributes["TileWidth"].Value);
                                    frameSize.Y = int.Parse(child.Attributes["TileHeight"].Value);
                                    frameCount = int.Parse(child.Attributes["FrameCount"].Value);
                                    asSprite.AddAnimation("sink_up", initPosition.X, initPosition.Y, frameSize.X, frameSize.Y, frameCount, 0.1f, false);
                                    break;
                                case "Down":
                                    initPosition.X = int.Parse(child.Attributes["X"].Value);
                                    initPosition.Y = int.Parse(child.Attributes["Y"].Value);
                                    frameSize.X = int.Parse(child.Attributes["TileWidth"].Value);
                                    frameSize.Y = int.Parse(child.Attributes["TileHeight"].Value);
                                    frameCount = int.Parse(child.Attributes["FrameCount"].Value);
                                    asSprite.AddAnimation("sink_down", initPosition.X, initPosition.Y, frameSize.X, frameSize.Y, frameCount, 0.1f, false);
                                    break;
                                case "Left":
                                    initPosition.X = int.Parse(child.Attributes["X"].Value);
                                    initPosition.Y = int.Parse(child.Attributes["Y"].Value);
                                    frameSize.X = int.Parse(child.Attributes["TileWidth"].Value);
                                    frameSize.Y = int.Parse(child.Attributes["TileHeight"].Value);
                                    frameCount = int.Parse(child.Attributes["FrameCount"].Value);
                                    asSprite.AddAnimation("sink_left", initPosition.X, initPosition.Y, frameSize.X, frameSize.Y, frameCount, 0.1f, false);
                                    break;
                                case "Right":
                                    initPosition.X = int.Parse(child.Attributes["X"].Value);
                                    initPosition.Y = int.Parse(child.Attributes["Y"].Value);
                                    frameSize.X = int.Parse(child.Attributes["TileWidth"].Value);
                                    frameSize.Y = int.Parse(child.Attributes["TileHeight"].Value);
                                    frameCount = int.Parse(child.Attributes["FrameCount"].Value);
                                    asSprite.AddAnimation("sink_right", initPosition.X, initPosition.Y, frameSize.X, frameSize.Y, frameCount, 0.1f, false);
                                    break;
                            }
                        }
                        break;
                    case "Properties":
                        m_iAttack = int.Parse(tmp.Attributes["Attack"].Value);
                        m_iDefence = int.Parse(tmp.Attributes["Defence"].Value);
                        m_iMaxHP = m_iCurrHP = int.Parse(tmp.Attributes["HP"].Value);
                        fSpeed = int.Parse(tmp.Attributes["Speed"].Value);
                        m_iPoint = int.Parse(tmp.Attributes["Point"].Value);
                        m_fTimerAttack = float.Parse(tmp.Attributes["TimeDelayAttack"].Value);
                        break;
                }
            }

            asSprite.CurrentAnimation = "down";

            /*
            asSprite = new SpriteAnimation(textureImage);

            asSprite.AddAnimation("sink_left", leftShink.X * frameSize.X, leftShink.Y * frameSize.Y, frameSize.X, frameSize.Y, sheetSize.X / 2, 0.1f, false);
            asSprite.AddAnimation("sink_right", rightShink.X * frameSize.X, rightShink.Y * frameSize.Y, frameSize.X, frameSize.Y, sheetSize.X / 2, 0.1f, false);
            asSprite.AddAnimation("sink_up", upShink.X * frameSize.X, upShink.Y * frameSize.Y, frameSize.X, frameSize.Y, sheetSize.X / 2, 0.1f, false);
            asSprite.AddAnimation("sink_down", downShink.X * frameSize.X, downShink.Y * frameSize.Y, frameSize.X, frameSize.Y, sheetSize.X / 2, 0.1f, false);

            asSprite.AddAnimation("left", leftFrame.X * frameSize.X, leftFrame.Y * frameSize.Y, frameSize.X, frameSize.Y, 1, 0.1f, true);
            asSprite.AddAnimation("right", rightFrame.X * frameSize.X, rightFrame.Y * frameSize.Y, frameSize.X, frameSize.Y, 1, 0.1f, true);
            
            asSprite.AddAnimation("down", downFrame.X * frameSize.X, downFrame.Y * frameSize.Y, frameSize.X, frameSize.Y, 1, 0.1f, true);
             */
        }

        virtual public Sprite Clone()
        {
            Sprite tmp = new Sprite();

            tmp.asSprite = this.asSprite;
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

            return tmp;
        }
        
        public enum SpriteState
        {
            Died,
            Playing
        }                
    }
}
