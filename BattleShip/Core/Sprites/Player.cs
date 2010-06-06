using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleShip.Core.Sprites
{
    class Player : Sprite
    {
        public Player(Game game, Texture2D texture2D, int iTileWidth, int iTileHeight)
            : base(game)
        {
            m_SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

            this.m_iTileHeight = iTileHeight;
            this.m_iTileWidth = iTileWidth;
            this.m_Texture = texture2D;
            this.m_bIsCollision = false;
            
        }

        public override Sprite Clone()
        {
            throw new NotImplementedException();
        }

        public override void GoDown()
        {
            this.m_Position.Y = this.m_Position.Y + this.m_Speed.Y;
            if (this.m_Position.Y > this.m_iHeightScreen)
            {
                this.m_Position.Y = this.m_iHeightScreen;
            }
        }

        public override void GoUp()
        {
            this.m_Position.Y = this.m_Position.Y - this.m_Speed.Y;
            if (this.m_Position.Y < 0)
            {
                this.m_Position.Y = 0;
            }
        }

        public override void TurnLeft()
        {
            this.m_Position.X = this.m_Position.X - this.m_Speed.X;
            if (this.m_Position.X < 0)
            {
                this.m_Position.X = 0;
            }
        }

        public override void TurnRight()
        {
            this.m_Position.X = this.m_Position.X - this.m_Speed.X;
            if (this.m_Position.X > this.m_iWidthScreen)
            {
                this.m_Position.X = this.m_iWidthScreen;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
