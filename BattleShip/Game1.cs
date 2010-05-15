using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using BattleShip.Core.Scences;
using BattleShip.Core.Managers;
using BattleShip.Core.GameComponents;

namespace BattleShip
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ResourceManager m_ResourceManager;
        SoundManager m_SoundManager;

        GameScence m_ActiveScence;
        HelpScence m_HelpScence;
        StartScence m_StartScence;
        ActionScence m_ActionScence;

        InputManager m_InputManager;

       

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), spriteBatch);
            // TODO: use this.Content to load your game content here
            
            //load all sounds
            m_SoundManager = new SoundManager();
            m_SoundManager.LoadContent(this.Content);
            this.Services.AddService(typeof(SoundManager), m_SoundManager);

            //load all resource
            m_ResourceManager = new ResourceManager(this.Content);
            m_ResourceManager.LoadAllResource();
            this.Services.AddService(typeof(ResourceManager), m_ResourceManager);

            m_InputManager = new InputManager();
            
            //load scences
            m_HelpScence = new HelpScence(this, m_ResourceManager.imgBackgroundHelpScence, m_ResourceManager.imgForegroundHelpScence);                        
            m_StartScence = new StartScence(this, m_ResourceManager.smallFont, m_ResourceManager.largeFont, m_ResourceManager.imgBackgroundStartScence, m_ResourceManager.imgForegroundStartScence, m_ResourceManager.imgButton);
            m_ActionScence = new ActionScence(this);

            this.Components.Add(m_HelpScence);
            this.Components.Add(m_StartScence);            
            this.Components.Add(m_ActionScence);

            //begin at start scence
            m_StartScence.ShowScreen();
            this.m_ActiveScence = m_StartScence;            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
                        
            HandleScenceInput();
            base.Update(gameTime);
        }

        private void HandleScenceInput()
        {            
            if (m_ActiveScence == m_StartScence)
            {
                HandleStartScenceInput();
            }
            else if (m_ActiveScence == m_HelpScence)
            {
                m_InputManager.BeginHandler();
                bool isPressEsc = m_InputManager.IsKeyboardPress(Keys.Escape);
                m_InputManager.EndHandler();

                if (isPressEsc)
                {
                    ShowScence(m_StartScence);
                }
            }
            else if (m_ActiveScence == m_ActionScence)
            {
                HandleActionInput();
            }             
        }

        private void HandleActionInput()
        {
            m_InputManager.BeginHandler();
            bool isPressEsc = m_InputManager.IsKeyboardPress(Keys.Escape);
            m_InputManager.EndHandler();

            if (isPressEsc)
            {
                ShowScence(m_StartScence);
            }
        }

        private void ShowScence(GameScence scence)
        {
            m_ActiveScence.HideScreen();
            m_ActiveScence = scence;
            scence.ShowScreen();
        }

        private void HandleStartScenceInput()
        {
            switch (m_StartScence.MainMenu.SelectedMenuItem)
            {
                case BattleShip.Core.GameComponents.TextMenuComponent.GameMenuItem.StartGame:
                    m_ActionScence.GameMode = ActionScence.PlayMode.Play;
                    ShowScence(m_ActionScence);                    
                    break;
                case BattleShip.Core.GameComponents.TextMenuComponent.GameMenuItem.EditMap:
                    m_ActionScence.GameMode = ActionScence.PlayMode.Edit;
                    ShowScence(m_ActionScence);
                    break;
                case BattleShip.Core.GameComponents.TextMenuComponent.GameMenuItem.Option:                    
                    break;
                case BattleShip.Core.GameComponents.TextMenuComponent.GameMenuItem.Help:
                    ShowScence(m_HelpScence);                    
                    break;
                case BattleShip.Core.GameComponents.TextMenuComponent.GameMenuItem.Exit:
                    Exit();
                    break;
                default:
                    break;
            }

            m_StartScence.MainMenu.SelectedMenuItem = BattleShip.Core.GameComponents.TextMenuComponent.GameMenuItem.None;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
