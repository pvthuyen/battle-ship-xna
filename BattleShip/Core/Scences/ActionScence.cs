using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleShip.Core.GameComponents;
using Microsoft.Xna.Framework.Graphics;
using BattleShip.Core.Managers;
using Microsoft.Xna.Framework;

namespace BattleShip.Core.Scences
{
    class ActionScence: GameScence
    {
        private MapComponent m_MapComponent;

        private SpriteBatch m_SpriteBatch;
        private SoundManager m_SoundManager;

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void ShowScreen()
        {
            //play sound

            base.ShowScreen();
        }
        public override void HideScreen()
        {
            //stop sound
            base.HideScreen();
        }

        public ActionScence(Game game)
            : base(game)
        {
            m_MapComponent = new MapComponent(game);
            
            m_SpriteBatch = game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            m_SoundManager = game.Services.GetService(typeof(SoundManager)) as SoundManager;
        }
    }
}
