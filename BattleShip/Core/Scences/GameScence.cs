using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleShip.Core.Scences
{
    public class GameScence : DrawableGameComponent
    {
        /// <summary>
        /// list of child game components
        /// </summary>
        protected List<GameComponent> m_lstGameComponent;
        public List<GameComponent> ListGameComponent
        {
            get { return this.m_lstGameComponent; }
        }

        public GameScence(Game game)
            : base(game)
        {
            this.m_lstGameComponent = new List<GameComponent>();
            Enabled = false;
            Visible = false;
        }

        /// <summary>
        /// show the screen
        /// </summary>
        public virtual void ShowScreen()
        {
            Enabled = true;
            Visible = true;
        }

        /// <summary>
        /// hide the screen
        /// </summary>
        public virtual void HideScreen()
        {
            Enabled = false;
            Visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent component in this.m_lstGameComponent)
            {
                if (component.Enabled)
                {
                    component.Update(gameTime);
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameComponent component in this.m_lstGameComponent)
            {
                if (component is DrawableGameComponent && component.Enabled)
                {
                    (component as DrawableGameComponent).Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }
    }
}
