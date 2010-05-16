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
        private ContentManager m_contentManager;

        public Texture2D imgPlayer;

        //texture for help scence
        public Texture2D imgBackgroundHelpScence;
        public Texture2D imgForegroundHelpScence;

        //texture for start scence
        public Texture2D imgBackgroundStartScence;
        public Texture2D imgForegroundStartScence;

        public Texture2D imgButton;

        public Texture2D imgGameScreen;
        public Texture2D imgGameEditor;

        public Texture2D imgPoint;

        //sprite fonts
        public SpriteFont smallFont;
        public SpriteFont largeFont;

        public ResourceManager(ContentManager contenManager)
        {
            this.m_contentManager = contenManager;
        }

        public void LoadAllResource()
        {            
            imgBackgroundHelpScence = this.m_contentManager.Load<Texture2D>(@"Resource/Menu/1");
            imgForegroundHelpScence = this.m_contentManager.Load<Texture2D>(@"Resource/Menu/main_1260140920_happy_aquarium_cheats");

            imgBackgroundStartScence = this.m_contentManager.Load<Texture2D>(@"Resource/Menu/1");
            imgForegroundStartScence = this.m_contentManager.Load<Texture2D>(@"Resource/Menu/intro");

            imgButton = this.m_contentManager.Load<Texture2D>(@"Resource/Menu/Button");

            imgGameScreen = this.m_contentManager.Load<Texture2D>(@"Resource/Background/gamescreen");
            imgGameEditor = this.m_contentManager.Load<Texture2D>(@"Resource/Background/editorimages");

            smallFont = this.m_contentManager.Load<SpriteFont>(@"Font/Arial");
            largeFont = this.m_contentManager.Load<SpriteFont>(@"Font/Arial");

            imgPlayer = this.m_contentManager.Load<Texture2D>(@"Resource/Sprite/Computer/Tboot_Trans");
            imgPoint = this.m_contentManager.Load<Texture2D>(@"Resource/Background/point");
        }

        internal Texture2D LoadTexture(string sFileName)
        {
            return this.m_contentManager.Load<Texture2D>(sFileName);
        }
    }
}
