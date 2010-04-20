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

        public Texture2D imgBackgroundHelpScence;
        public Texture2D imgForegroundHelpScence;

        public ResourceManager(ContentManager contenManager)
        {
            this.m_contentManager = contenManager;
        }

        public void LoadAllResource()
        {
            /*
            imgBackgroundHelpScence = this.m_contentManager.Load<Texture2D>("background.jpg");
            imgForegroundHelpScence = this.m_contentManager.Load<Texture2D>("foreground.jpg");
             */
        }
    }
}
