using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleShip.Core.GameComponents;
using Microsoft.Xna.Framework.Graphics;
using BattleShip.Core.Managers;
using Microsoft.Xna.Framework;
using BattleShip.Core.Sprites;
using Microsoft.Xna.Framework.Input;

namespace BattleShip.Core.Scences
{
    class ActionScence : GameScence
    {
        private MapComponent m_MapComponent;
        private ImageComponent m_GameScreen;

        private SpriteBatch m_SpriteBatch;

        private SoundManager m_SoundManager;
        private ResourceManager m_ResourceManager;
        private InputManager m_InputManager;

        private Player m_Player;

        private PlayMode m_GameMode;
        public PlayMode GameMode
        {
            get { return m_GameMode; }
            set { m_GameMode = value; }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (m_GameMode)
            {
                case PlayMode.Play:
                    break;
                case PlayMode.Edit:
                    break;
                default:
                    break;
            }

            base.Draw(gameTime);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //scroll map
            m_MapComponent.ScrollByPixels(1, 1);

            m_InputManager.BeginHandler();
            bool up = m_InputManager.IsKeyboardPress(Keys.Up);
            bool down = m_InputManager.IsKeyboardPress(Keys.Down);
            bool left = m_InputManager.IsKeyboardPress(Keys.Left);
            bool right = m_InputManager.IsKeyboardPress(Keys.Right);
            m_InputManager.EndHandler();

            switch (m_GameMode)
            {
                case PlayMode.Play:
                    if (up)
                        m_Player.GoUp();
                    if (down)
                        m_Player.GoDown();
                    if (left)
                        m_Player.TurnLeft();
                    if (right)
                        m_Player.TurnRight();
                    break;

                case PlayMode.Edit:
                    break;
                default:
                    break;
            }

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
            m_SpriteBatch = game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            m_SoundManager = game.Services.GetService(typeof(SoundManager)) as SoundManager;
            m_ResourceManager = game.Services.GetService(typeof(ResourceManager)) as ResourceManager;
            m_InputManager = new InputManager();
                                    
            //load map
            m_MapComponent = new MapComponent(game);
            m_MapComponent.LoadMap("Maps.xml", 1);

            m_MapComponent.Left = 0;
            m_MapComponent.Top = 0;
            m_MapComponent.Width = 18;
            m_MapComponent.Height = 14;

            m_MapComponent.DrawFirstRowTilesOnTransLayers = false;

            m_MapComponent.Enabled = true;
            m_MapComponent.Visible = true;

            this.m_lstGameComponent.Add(m_MapComponent);

            //load player
            m_Player = new Player(game, m_ResourceManager.imgPlayer, 64, 64);
            m_Player.SetPosition(new Vector2(game.Window.ClientBounds.Width / 2, 0));
            this.m_lstGameComponent.Add(m_Player);

            //load game screen
            m_GameScreen = new ImageComponent(game, m_ResourceManager.imgGameScreen, ImageComponent.DrawMode.Stretch);
            this.m_lstGameComponent.Add(m_GameScreen);
        }
                
        public enum PlayMode
        {
            Play,
            Edit
        }
    }
}
