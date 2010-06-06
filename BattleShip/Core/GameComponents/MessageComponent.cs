using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace BattleShip.Core.GameComponents
{
    class MessageComponent: DrawableGameComponent
    {
        private string m_strTitle = string.Empty;
        private string[] m_arrName;
        private string[] m_arrValue;
        
        private Rectangle m_rectOkBtnSRC;
        private Rectangle m_rectOkBtnDES;

        private Rectangle m_boundingRectSRC;
        private Rectangle m_boundingRectDES;
        private Texture2D m_t2d;
        private SpriteBatch m_spriteBatch;
                        

        public MessageComponent(Game game, Texture2D t2dTexture, Rectangle rectOkSRC, Rectangle rectBoundingSRC, Rectangle rectPlayField)
            : base(game)
        {
            m_t2d = t2dTexture;
            m_rectOkBtnSRC = rectOkSRC;
            m_boundingRectSRC = rectBoundingSRC;

            m_boundingRectDES = new Rectangle(rectPlayField.X + rectPlayField.Width / 3, rectPlayField.Y + rectPlayField.Height / 5, rectPlayField.Width / 3, rectPlayField.Height / 3);
            m_rectOkBtnDES = new Rectangle(m_boundingRectDES.X + m_boundingRectDES.Width / 2 - m_rectOkBtnSRC.Width / 2, m_boundingRectDES.Y + m_boundingRectDES.Height - m_rectOkBtnSRC.Height - 20, m_rectOkBtnSRC.Width, m_rectOkBtnSRC.Height);

            m_spriteBatch = game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            Enabled = false;
            Visible = false;
        }

        public void ShowMessage(string strTitle, string[] arrName, string[] arrValue)
        {
            Enabled = true;
            Visible = true;

            m_strTitle = strTitle;
            m_arrName = arrName;
            m_arrValue = arrValue;
        }

        public void Close()
        {
            Enabled = false;
            Visible = false;
        }

        public override void Draw(GameTime gameTime)
        {
            //draw interface of message
            m_spriteBatch.Draw(m_t2d, m_boundingRectDES, m_boundingRectSRC, Color.White);
            m_spriteBatch.Draw(m_t2d, m_rectOkBtnDES, m_rectOkBtnSRC, Color.White);

            //draw content
            WriteText(m_strTitle, m_boundingRectDES.X + m_boundingRectDES.Width / 2 - 12*m_strTitle.Length / 2, m_boundingRectDES.Y + 5, Color.White, 15, 12);

            base.Draw(gameTime);
        }

        public MessageResult SetMouseListener(MouseState ms)
        {
            if (ms.LeftButton == ButtonState.Pressed)
            {
                if (m_rectOkBtnDES.Contains(new Point(ms.X, ms.Y)))
                {
                    Close();
                    return MessageResult.OK;
                }
            }
            return MessageResult.None;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        void WriteText(string sTextOut, int x, int y, Color colorTint, int iHeight, int iWidth)
        {
            int iFontX = 0;
            int iFontY = 450;
            int iFontHeight = 12;
            int iFontWidth = 9;
            int iFontAsciiStart = 32;
            int iFontAsciiEnd = 90;
            int iOutChar;

            int iFontWidthDES = iWidth;
            int iFontHeightHES = iHeight;

            for (int i = 0; i < sTextOut.Length; i++)
            {
                iOutChar = (int)sTextOut[i];
                if ((iOutChar >= iFontAsciiStart) & (iOutChar <= iFontAsciiEnd))
                {

                    m_spriteBatch.Draw(m_t2d,
                                     new Rectangle(x + (iFontWidthDES * i), y, iFontWidthDES, iFontHeightHES),
                                     new Rectangle(iFontX + ((iOutChar - iFontAsciiStart) * iFontWidth),
                                     iFontY, iFontWidth, iFontHeight),
                                     colorTint);
                }
            }
        }

        public enum MessageResult
        {
            OK,
            Cancel,
            None
        }
    }
}
