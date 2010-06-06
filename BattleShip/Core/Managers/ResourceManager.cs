using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleShip.Core.Managers
{
    public class ResourceManager
    {
        static private UnitManager m_UnitManager;

        private ContentManager m_contentManager;
                
        //texture for help scence
        static public Texture2D imgBackgroundHelpScence;
        static public Texture2D imgForegroundHelpScence;

        //texture for start scence
        static public Texture2D imgBackgroundStartScence;
        static public Texture2D imgForegroundStartScence;

        static public Texture2D imgButton;

        static public Texture2D imgGameScreen;
        static public Texture2D imgGameEditor;

        static public Texture2D imgPoint;

        static public Texture2D imgExplosion;

        static public Texture2D imgPlayerStatus;

        static public Texture2D imgStar;

        static public Texture2D imgBonus;

        //fishs
        static public Texture2D imgJumpingFish;
        static public Texture2D imgFish;

        //sprite fonts
        static public SpriteFont smallFont;
        static public SpriteFont largeFont;

        static public UnitManager Units
        {
            get { return m_UnitManager; }
        }

        public ResourceManager(ContentManager contenManager)
        {
            this.m_contentManager = contenManager;            
        }

        public void LoadAllResource()
        {            
            imgBackgroundHelpScence = this.m_contentManager.Load<Texture2D>(@"Resource/Menu/1");
            imgForegroundHelpScence = this.m_contentManager.Load<Texture2D>(@"Resource/Menu/BG3");

            imgBackgroundStartScence = this.m_contentManager.Load<Texture2D>(@"Resource/Menu/1");
            imgForegroundStartScence = this.m_contentManager.Load<Texture2D>(@"Resource/Menu/intro");

            imgButton = this.m_contentManager.Load<Texture2D>(@"Resource/Menu/Button");

            imgGameScreen = this.m_contentManager.Load<Texture2D>(@"Resource/Background/gamescreen");
            imgGameEditor = this.m_contentManager.Load<Texture2D>(@"Resource/Background/editorimages");

            smallFont = this.m_contentManager.Load<SpriteFont>(@"Font/Arial");
            largeFont = this.m_contentManager.Load<SpriteFont>(@"Font/Arial");
                        
            imgPoint = this.m_contentManager.Load<Texture2D>(@"Resource/Background/point");

            imgExplosion = this.m_contentManager.Load<Texture2D>(@"Resource/Sprite/Bullet/explosions");

            imgStar = this.m_contentManager.Load<Texture2D>(@"Resource/Sprite/Animations/Star_Trans");
            imgBonus = this.m_contentManager.Load<Texture2D>(@"Resource/Sprite/Animations/Bonus");

            imgJumpingFish = this.m_contentManager.Load<Texture2D>(@"Resource/Sprite/Animations/Jumping Fish_Trans");
            imgFish = this.m_contentManager.Load<Texture2D>(@"Resource/Sprite/Animations/Fish_Trans");

            imgPlayerStatus = this.m_contentManager.Load<Texture2D>(@"Resource/Background/PlayerStatus");
            m_UnitManager = new UnitManager(Utils.Utility.strSpritePath, Utils.Utility.strBulletPath, m_contentManager);
        }

        public Texture2D LoadTexture(string sFileName)
        {
            return this.m_contentManager.Load<Texture2D>(sFileName);
        }
    }
}
