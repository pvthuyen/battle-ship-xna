using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BattleShip.Core.GameComponents;
using BattleShip.Core.Managers;
using Microsoft.Xna.Framework.Media;

namespace BattleShip.Core.Scences
{
    public class StartScence : GameScence
    {        
        protected TextMenuComponent m_MainMenu;
        protected Texture2D m_textureElements;

        protected SoundManager m_SoundManager;
        protected SpriteBatch m_SpriterBatch;

        protected Rectangle m_BattleRect;
        protected Vector2 m_BattlePosition;

        protected Rectangle m_ShipRect;
        protected Vector2 m_ShipPosition;

        protected Rectangle m_flashEnhanceRect;
        protected Vector2 m_flashEnhancePosition;

        protected bool m_bShowEnhanced;
        protected TimeSpan m_ElapsedTime;

        public TextMenuComponent MainMenu
        {
            get { return this.m_MainMenu; }
        }

        public override void Update(GameTime gameTime)
        {
            if (!this.m_MainMenu.Visible)
            {
                Rectangle clientBounds = Game.Window.ClientBounds;
                if (this.m_ShipPosition.X >= (clientBounds.Width) / 2)
                {
                    this.m_ShipPosition.X -= 15;
                }
                if (m_BattlePosition.X <= (clientBounds.Width - 500) / 2)
                {
                    m_BattlePosition.X += 15;
                }
                else
                {
                    this.m_MainMenu.Visible = true;
                    this.m_MainMenu.Enabled = true;

                    //m_SoundManager.StartMusic
                    
                    m_flashEnhancePosition = new Vector2((m_ShipPosition.X + m_ShipRect.Width - m_flashEnhanceRect.Width / 2) - 80, m_ShipPosition.Y + m_ShipRect.Height + 10);

                    m_bShowEnhanced = true;
                }
            }
            else
            {
                this.m_ElapsedTime += gameTime.ElapsedGameTime;
                if (this.m_ElapsedTime > TimeSpan.FromSeconds(1))
                {
                    this.m_ElapsedTime -= TimeSpan.FromSeconds(1);
                    this.m_bShowEnhanced = !this.m_bShowEnhanced;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            m_SpriterBatch.Draw(this.m_textureElements, m_BattlePosition, m_BattleRect, Color.White);
            m_SpriterBatch.Draw(this.m_textureElements, m_ShipPosition, m_ShipRect, Color.White);
            if (this.m_bShowEnhanced)
            {
                m_SpriterBatch.Draw(this.m_textureElements, m_flashEnhancePosition, m_flashEnhanceRect, Color.White);
            }
        }

        public override void ShowScreen()
        {            
            //play sound ....
            //MediaPlayer.Play(SoundManager.m_startMusic);

            m_BattlePosition.X = -1 * m_BattleRect.Width;
            m_BattlePosition.Y = 40;

            m_ShipPosition.X = Game.Window.ClientBounds.Width;
            m_ShipPosition.Y = m_BattleRect.Height;

            this.m_MainMenu.MenuPosition = new Vector2((Game.Window.ClientBounds.Width - m_MainMenu.WidthMenu) / 2, 330);
            this.m_MainMenu.Visible = false;
            this.m_MainMenu.Enabled = false;
            this.m_bShowEnhanced = false;
            
            base.ShowScreen();
        }

        public override void HideScreen()
        {
            //stop sound
            //MediaPlayer.Stop();

            base.HideScreen();
        }
                
        public StartScence(Game game, SpriteFont smallFont, SpriteFont largeFont, Texture2D background, Texture2D elements, Texture2D button)
            : base(game)
        {
            this.m_textureElements = elements;
            this.m_lstGameComponent.Add(new ImageComponent(game, background, ImageComponent.DrawMode.Stretch));

            m_BattleRect = new Rectangle(0, 0, 394, 96);
            m_ShipRect = new Rectangle(208, 97, 186, 65);
            m_flashEnhanceRect = new Rectangle(70, 154, 100, 45);
            m_ElapsedTime = TimeSpan.Zero;

            //load main menu from xml
            m_MainMenu = new TextMenuComponent(game, smallFont, largeFont, button);
            m_MainMenu.SetXmlMenu("GameMenu.xml");
            m_MainMenu.SelectedColor = Color.Red;
            m_MainMenu.RegularColor = Color.Blue;            
            this.m_lstGameComponent.Add(m_MainMenu);

            this.m_SpriterBatch = game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            this.m_SoundManager = game.Services.GetService(typeof(SoundManager)) as SoundManager;
        }
    }
}
