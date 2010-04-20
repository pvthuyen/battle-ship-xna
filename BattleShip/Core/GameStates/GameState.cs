using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BattleShip.Core.Managers;
using BattleShip.Core.Utils;

namespace BattleShip.Core.GameStates
{
    public abstract class GameState : DrawableGameComponent, IGameState
    {
        protected IGameStateManager m_GameManager;
        protected InputManager m_Input;
        protected Rectangle m_TitleSafeArea;

        public GameState(Game game)
            : base(game)
        {
            this.m_GameManager = (IGameStateManager)game.Services.GetService(typeof(IGameStateManager));
            this.m_Input = (InputManager)game.Services.GetService(typeof(InputManager));
        }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                this.m_TitleSafeArea = Utility.GetTitleSafeArea(GraphicsDevice, 0.85f);
            }
            base.LoadGraphicsContent(loadAllContent);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal protected virtual void StateChange(object sender, EventArgs e)
        {
            if (this.m_GameManager.State == this.Value)
                Visible = Enabled = true;
            else
                Visible = Enabled = false;
        }
        #region IGameState Members
        public GameState Value
        {
            get { return (this); }
        }
        #endregion
    }
}
