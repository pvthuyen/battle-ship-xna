using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Specialized;
using Microsoft.Xna.Framework.Input;
using System.Xml;
using BattleShip.Core.Managers;

namespace BattleShip.Core.GameComponents
{
    public class TextMenuComponent:DrawableGameComponent
    {
        protected XmlDocument m_XmlMenu;

        protected SpriteBatch m_spriteBatch;

        protected SpriteFont m_regularFont;
        protected SpriteFont m_selectedFont;
        
        protected Color m_selectedColor;
        protected Color m_regularColor;
        protected Vector2 m_menuPosition;
        
        protected int m_selectedIndex;
        private List<string> m_menuItems;
        
        protected int m_widthMenu;
        protected int m_heightMenu;

        protected string m_OldSelectedMenu;

        protected Texture2D m_textureButton;
        protected Rectangle m_ButtonRect;

        //handle input
        protected InputManager m_inputManager;
        
        private List<string> GetMenuItems(XmlNodeList nodeList)
        {
            List<string> lstItem = new List<string>();
            foreach (XmlNode node in nodeList)
            {
                XmlAttribute att = node.Attributes[0];
                if (att.Name.CompareTo("name") == 0)
                    lstItem.Add(att.Value);
            }
            return lstItem;
        }

        public SpriteFont RegularFont
        {
            get { return m_regularFont; }
            set { m_regularFont = value; }
        }
        
        public SpriteFont SelectedFont
        {
            get { return m_selectedFont; }
            set { m_selectedFont = value; }
        }               

        public Color RegularColor
        {
            get { return m_regularColor; }
            set { m_regularColor = value; }
        }
        
        public Color SelectedColor
        {
            get { return m_selectedColor; }
            set { m_selectedColor = value; }
        }
        
        public Vector2 MenuPosition
        {
            get { return m_menuPosition; }
            set { m_menuPosition = value; }
        }

        public int SelectedIndex
        {
            get { return m_selectedIndex; }
            set { m_selectedIndex = value; }
        }

        public int WidthMenu
        {
            get { return m_widthMenu; }
            set { m_widthMenu = value; }
        }

        public int HeightMenu
        {
            get { return m_heightMenu; }
            set { m_heightMenu = value; }
        }

        public void SetMenuItems(string[] items)
        {
            this.m_menuItems.Clear();
            this.m_menuItems.AddRange(items);
            CalculateBounds();
        }

        protected void CalculateBounds()
        {
            this.m_heightMenu = 0;
            this.m_widthMenu = 0;
            foreach (string item in this.m_menuItems)
            {
                Vector2 size = this.m_selectedFont.MeasureString(item);
                if (size.X > this.m_widthMenu)
                {
                    this.m_widthMenu = (int)size.X;
                }
                this.m_heightMenu += this.m_selectedFont.LineSpacing;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            float y = this.m_menuPosition.Y;
            for (int i = 0; i < this.m_menuItems.Count; i++)
            {
                SpriteFont font;
                Color color;
                if (i == this.m_selectedIndex)
                {
                    font = this.m_selectedFont;
                    color = this.m_selectedColor;
                }
                else
                {
                    font = this.m_regularFont;
                    color = this.m_regularColor;
                }
                //draw button
                int x = (int)this.m_menuPosition.X;                
                this.m_spriteBatch.Draw(this.m_textureButton, new Vector2(x - 30, y - this.m_ButtonRect.Height / 2 + 15), Color.White);
                                
                //draw text shadow
                this.m_spriteBatch.DrawString(font, this.m_menuItems[i], new Vector2(x + 1, y + 1), Color.Black);
                this.m_spriteBatch.DrawString(font, this.m_menuItems[i], new Vector2(x, y), color);

                y += this.m_ButtonRect.Height + 5;
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {            
            m_inputManager.BeginHandler();
            bool down = m_inputManager.IsKeyboardPress(Keys.Down);
            bool up = m_inputManager.IsKeyboardPress(Keys.Up);
            bool enter = m_inputManager.IsKeyboardPress(Keys.Enter);
            bool back = m_inputManager.IsKeyboardPress(Keys.Back);

            m_inputManager.EndHandler();
                        
            if (down || up)
            {                
                //SoundManager.m_SelectMenu.Play();
            }

            if (down)
            {
                m_selectedIndex++;
                if (m_selectedIndex == m_menuItems.Count)
                {
                    m_selectedIndex = 0;
                }
            }

            if (up)
            {
                m_selectedIndex--;
                if (m_selectedIndex == -1)
                {
                    m_selectedIndex = m_menuItems.Count - 1;
                }
            }

            if (enter)
            {
                string StrTmp = this.m_menuItems[this.m_selectedIndex];

                XmlNodeList nodeList = this.m_XmlMenu.GetElementsByTagName("Item");
                foreach (XmlNode node in nodeList)
                {
                    if (node.Attributes[0].Value.CompareTo(StrTmp) == 0)
                    {
                        List<string> tmp = this.GetMenuItems(node.ChildNodes);
                        if (tmp != null && tmp.Count > 0)
                        {
                            this.m_menuItems = tmp;
                            this.m_selectedIndex = 0;
                            m_OldSelectedMenu = StrTmp;
                        }
                    }
                }   
                
                //////////////////////
                switch (StrTmp)
                {
                    case "Start Game":
                        m_SelectedMenuItem = GameMenuItem.StartGame;
                        break;
                    case "Edit Map":
                        m_SelectedMenuItem = GameMenuItem.EditMap;
                        break;
                    case "Option":
                        m_SelectedMenuItem = GameMenuItem.Option;
                        break;
                    case "Help":
                        m_SelectedMenuItem = GameMenuItem.Help;
                        break;
                    case "Exit Game":
                        m_SelectedMenuItem = GameMenuItem.Exit;
                        break;
                }
             
            }

            if (back)
            {
                XmlNodeList nodeList = this.m_XmlMenu.GetElementsByTagName("Item");
                foreach (XmlNode node in nodeList)
                {
                    if (node.Attributes[0].Value.CompareTo(m_OldSelectedMenu) == 0)
                    {
                        List<string> tmp = this.GetMenuItems(node.ParentNode.ChildNodes);
                        if (tmp != null && tmp.Count > 0)
                        {
                            this.m_menuItems = tmp;
                            this.m_selectedIndex = 0;
                            if (node.ParentNode.Attributes.Count > 0)
                            {
                                this.m_OldSelectedMenu = node.ParentNode.Attributes[0].Value;
                            }
                            else
                            {
                                this.m_OldSelectedMenu = string.Empty;
                            }
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        public TextMenuComponent(Game game, SpriteFont normalFont, SpriteFont selectFont, Texture2D textureButton)
            : base(game)
        {
            m_textureButton = textureButton;
            m_ButtonRect = new Rectangle(0, 0, m_textureButton.Width, m_textureButton.Height);

            m_regularFont = normalFont;
            m_selectedFont = selectFont;
            this.m_menuItems = new List<string>();
            
            m_spriteBatch = game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            
            m_inputManager = new InputManager();

            m_SelectedMenuItem = GameMenuItem.None;
        }

        internal void SetXmlMenu(string p)
        {
            this.m_XmlMenu = new XmlDocument();
            this.m_XmlMenu.Load(p);
            this.m_menuItems = this.GetMenuItems(this.m_XmlMenu.DocumentElement.ChildNodes);
        }

        private GameMenuItem m_SelectedMenuItem;
        public GameMenuItem SelectedMenuItem
        {
            get { return m_SelectedMenuItem; }
            set { this.m_SelectedMenuItem = value; }
        }
        public enum GameMenuItem
        {
            StartGame,
            EditMap,
            Option,
            Help,
            Exit,
            None
        };
    }
}
