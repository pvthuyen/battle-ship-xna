using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleShip.Core.GameComponents;
using BattleShip.Core.Managers;
using Microsoft.Xna.Framework;
using BattleShip.Core.Utils;

namespace BattleShip.Core.Sprites
{
    class Mission : DrawableGameComponent
    {
        private SpriteManager spriteManager;
        private MapComponent m_MapComponent;

        private MissionState m_CurrMissionState = MissionState.Playing;

        public MissionState CurrMissionState
        {
            get { return m_CurrMissionState; }
        }

        public Mission(Game game, MapComponent map, Rectangle rectPlayField)
            : base(game)
        {
            m_MapComponent = map;
            spriteManager = new SpriteManager(game, rectPlayField);
        }

        public void Play(Sprite player, int iLevel)
        {
            m_CurrMissionState = MissionState.Playing;
            
            m_MapComponent.LoadMap(Utility.strMapPath, iLevel);
            
            spriteManager.Player = player;                       
            
            spriteManager.InitEnemies(iLevel);
            spriteManager.InitExplosions();
            spriteManager.InitBonus();
        }

        public enum MissionState
        {
            Playing,
            Completed,
            Fail
        }

        public override void Draw(GameTime gameTime)
        {
            m_MapComponent.Draw(gameTime);
            spriteManager.Draw(gameTime);
            m_MapComponent.DrawMiniMap(spriteManager.Player, spriteManager.Enemies);

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            m_CurrMissionState = MissionState.Playing;

            m_MapComponent.Update(gameTime);
            spriteManager.Update(gameTime);

            if (spriteManager.AllEnemiesDied)
                m_CurrMissionState = MissionState.Completed;

            UserControlledSprite player = spriteManager.Player as UserControlledSprite;

            if (player.TotalLives < 0)
                m_CurrMissionState = MissionState.Fail;
            
            base.Update(gameTime);
        }
    }
}
