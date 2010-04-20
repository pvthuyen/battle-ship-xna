using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleShip.Core.GameComponents
{
    public class ImageComponent:DrawableGameComponent
    {
        public enum DrawMode
        {
            Center = 1,
            Stretch
        }

        protected Texture2D m_texture2D;
        protected SpriteBatch m_spriteBatch;
        protected DrawMode m_drawMode;
        protected Rectangle m_imgRect;

        public ImageComponent(Game game, Texture2D texture, DrawMode drawMode)
            : base(game)
        {
            this.m_texture2D = texture;
            this.m_drawMode = drawMode;
            this.m_spriteBatch = game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            switch (this.m_drawMode)
            {
                case DrawMode.Center:
                    this.m_imgRect = new Rectangle((game.Window.ClientBounds.Width - this.m_texture2D.Width) / 2, (game.Window.ClientBounds.Height - this.m_texture2D.Height) / 2, this.m_texture2D.Width, this.m_texture2D.Height);
                    break;
                case DrawMode.Stretch:
                    this.m_imgRect = new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
                    break;
                default:
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.m_spriteBatch.Draw(this.m_texture2D, this.m_imgRect, Color.White);
            base.Draw(gameTime);
        }
    }
}
