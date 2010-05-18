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
        private SpriteManager spriteManager;
        private MapComponent m_MapComponent;
        private ImageComponent m_GameScreen;

        private SpriteBatch m_SpriteBatch;

        private SoundManager m_SoundManager;
        private ResourceManager m_ResourceManager;
        private InputManager m_InputManager;

        
        private PlayMode m_GameMode;
        public PlayMode GameMode
        {
            get { return m_GameMode; }
            set { m_GameMode = value; }
        }

        #region editor
        private Texture2D t2dEditorImages;

        Rectangle rectEditorBaseButtonSRC;
        Rectangle rectEditorBaseButton;

        Rectangle rectEditorTransButtonSRC;
        Rectangle rectEditorTransButton;
        
        Rectangle rectEditorObjectButtonSRC;
        Rectangle rectEditorObjectButton;
        
        Rectangle rectEditorXOverlay;
        Rectangle rectEditorBoxOverlay;

        Rectangle rectSelectTileSRC;
        Rectangle rectSelectTileDest;

        Rectangle rectDrawTile;
        Rectangle rectPlayField;
            

        private int iEditorModeButtonPressedOffset = 45;
        private int iEditorCurrentTile;      
          
        //thoi gian xu ly input
        float fKeyPressCheckDelay = 0.25f;
        float fTotalElapsedTime = 0;

        private EditorLayerMode iEditorLayerMode = EditorLayerMode.Base;
        #endregion

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);

            switch (m_GameMode)
            {
                case PlayMode.Play:
                    WriteText("PLAY MODE", 550, 460, Color.White);
                    break;
                case PlayMode.Edit:
                    DrawEditorInterface();
                    WriteText("EDIT MODE", 550, 460, Color.White);
                    break;
                default:
                    break;
            }
                        
            m_MapComponent.DrawMiniMap(null);
        }

        void WriteText(string sTextOut, int x, int y, Color colorTint)
        {
            // Use our simple font rendering to write out a line of text.  The spriteBatch object should be
            // in a Begin mode with alpha blending BEFORE calling the routine.
            // Information about our "Font" :)
            int iFontX = 0;
            int iFontY = 450;
            int iFontHeight = 12;
            int iFontWidth = 9;
            int iFontAsciiStart = 32;
            int iFontAsciiEnd = 90;
            int iOutChar;

            for (int i = 0; i < sTextOut.Length; i++)
            {
                iOutChar = (int)sTextOut[i];
                if ((iOutChar >= iFontAsciiStart) & (iOutChar <= iFontAsciiEnd))
                {

                    m_SpriteBatch.Draw(t2dEditorImages,
                                     new Rectangle(x + (iFontWidth * i), y, iFontWidth, iFontHeight),
                                     new Rectangle(iFontX + ((iOutChar - iFontAsciiStart) * iFontWidth),
                                     iFontY, iFontWidth, iFontHeight),
                                     colorTint);
                }
            }
        }

        private void DrawEditorInterface()
        {
            Rectangle rectBase = rectEditorBaseButtonSRC;
            Rectangle rectTrans = rectEditorTransButtonSRC;
            Rectangle rectObject = rectEditorObjectButtonSRC;

            if (iEditorLayerMode == EditorLayerMode.Base) { rectBase.Offset(0, iEditorModeButtonPressedOffset); }
            if (iEditorLayerMode == EditorLayerMode.Trans) { rectTrans.Offset(0, iEditorModeButtonPressedOffset); }
            if (iEditorLayerMode == EditorLayerMode.Object) { rectObject.Offset(0, iEditorModeButtonPressedOffset); }

            m_SpriteBatch.Draw(t2dEditorImages, rectSelectTileDest, rectSelectTileSRC, Color.White);
            m_SpriteBatch.Draw(t2dEditorImages, rectEditorObjectButton, rectObject, Color.White);
            m_SpriteBatch.Draw(t2dEditorImages, rectEditorTransButton, rectTrans, Color.White);
            m_SpriteBatch.Draw(t2dEditorImages, rectEditorBaseButton, rectBase, Color.White);

            int iTileToDraw = iEditorCurrentTile;
            m_MapComponent.DrawTile(m_SpriteBatch, iTileToDraw, rectDrawTile);


            // if the mouse is currently in the "playfield", draw it's location
            if (rectPlayField.Contains(m_InputManager.msMouseState.X, m_InputManager.msMouseState.Y))
            {
                int iOverlayX, iOverlayY;
                iOverlayX = (((m_InputManager.msMouseState.X - m_MapComponent.Left) / m_MapComponent.TileWidth) * m_MapComponent.TileWidth) + m_MapComponent.Left;
                iOverlayY = (((m_InputManager.msMouseState.Y - m_MapComponent.Top) / m_MapComponent.TileHeight) * m_MapComponent.TileHeight) + m_MapComponent.Top;
                m_SpriteBatch.Draw(t2dEditorImages,
                                 new Rectangle(iOverlayX, iOverlayY, m_MapComponent.TileWidth, m_MapComponent.TileHeight),
                                 rectEditorBoxOverlay,
                                 Color.White);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            m_InputManager.BeginHandler();
            bool up = m_InputManager.IsKeyboardPress(Keys.Up);
            bool down = m_InputManager.IsKeyboardPress(Keys.Down);
            bool left = m_InputManager.IsKeyboardPress(Keys.Left);
            bool right = m_InputManager.IsKeyboardPress(Keys.Right);
            m_InputManager.EndHandler();

            
            switch (m_GameMode)
            {
                case PlayMode.Play:
                    spriteManager.Visible = true;
                    spriteManager.Enabled = true;

                    if (up)
                    {                        
                        m_MapComponent.ScrollByPixels(0, -10);
                    }
                    if (down)
                    {                     
                        m_MapComponent.ScrollByPixels(0, 10);
                    }
                    if (left)
                    {                     
                        m_MapComponent.ScrollByPixels(-10, 0);
                    }
                    if (right)
                    {                     
                        m_MapComponent.ScrollByPixels(10, 0);
                    }
                    break;

                case PlayMode.Edit:
                    spriteManager.Visible = false;
                    spriteManager.Enabled = false;

                    m_InputManager.ksKeyboardState = Keyboard.GetState();
                    m_InputManager.msMouseState = Mouse.GetState();

                    float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    fTotalElapsedTime += elapsed;                    
                    
                    if (fTotalElapsedTime >= fKeyPressCheckDelay)
                    {                        
                        if (CheckEditorKeys(m_InputManager.ksKeyboardState) == 1)
                        {
                            fTotalElapsedTime = 0;
                        }
                    }
                    CheckEditorModeMouseClicks(m_InputManager.msMouseState, m_InputManager.ksKeyboardState);
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

            //add sprite manager
            spriteManager = new SpriteManager(game);
            this.m_lstGameComponent.Add(spriteManager);

            //load game screen
            m_GameScreen = new ImageComponent(game, m_ResourceManager.imgGameScreen, ImageComponent.DrawMode.Stretch);
            this.m_lstGameComponent.Add(m_GameScreen);

            t2dEditorImages = m_ResourceManager.imgGameEditor;

            //config mini map
            int width = m_GameScreen.ImgRect.Width;
            int height = m_GameScreen.ImgRect.Height;

            int widthSRC = m_ResourceManager.imgGameScreen.Width;
            int heightSRC = m_ResourceManager.imgGameScreen.Height;

            double x = 1.0 * 18 * width / widthSRC;
            double y = 1.0 * 370 * height / heightSRC;
            Vector2 posMiniMap = new Vector2((float)x, (float)y);

            m_MapComponent.PositionMiniMap = posMiniMap;

            m_MapComponent.MiniMapCellWidth = (int)(1.0 * 120 * width / widthSRC) / m_MapComponent.MapWidth;
            m_MapComponent.MiniMapCellHeight = (int)(1.0 * 100 * height / heightSRC) / m_MapComponent.MapHeight;

            //draw edit mode
            int leftRectSelTile = m_MapComponent.MiniMapCellWidth * m_MapComponent.Width + 100;
            int top = game.Window.ClientBounds.Height - m_MapComponent.MiniMapCellHeight * m_MapComponent.Height + 10;

            rectSelectTileSRC = new Rectangle(540, 315, 75, 75);
            rectSelectTileDest = new Rectangle(leftRectSelTile, top - 60, 75, 75);
            rectDrawTile = new Rectangle(leftRectSelTile + 18, top - 52, 48, 48);

            int leftBtn = leftRectSelTile + rectSelectTileSRC.Width + 20;
            rectEditorBaseButtonSRC = new Rectangle(540, 225, 80, 30);
            rectEditorBaseButton = new Rectangle(leftBtn, top, 80, 30);
            
            rectEditorTransButtonSRC = new Rectangle(540, 45, 80, 30);
            rectEditorTransButton = new Rectangle(leftBtn, top - 30, 80, 30);

            rectEditorObjectButtonSRC = new Rectangle(540, 135, 80, 30);
            rectEditorObjectButton = new Rectangle(leftBtn, top - 60, 80, 30);

            rectEditorXOverlay = new Rectangle(405, 270, 48, 48);
            rectEditorBoxOverlay = new Rectangle(405, 360, 48, 48);

            rectPlayField = new Rectangle(m_MapComponent.Left, m_MapComponent.Top,m_GameScreen.ImgRect.Width,(int)(m_MapComponent.PositionMiniMap.Y - 58));
        }

        int CheckEditorKeys(KeyboardState ksKeyboardState)
        {
            if (ksKeyboardState.IsKeyDown(Keys.O))
            {                
                return 1;
            }
            if (ksKeyboardState.IsKeyDown(Keys.L))
            {             
                return 1;
            }
            
            if (ksKeyboardState.IsKeyDown(Keys.A))
            {
                switch (iEditorLayerMode)
                {
                    case EditorLayerMode.Base:
                        iEditorCurrentTile = m_MapComponent.GetPrevTile(MapComponent.Layer.Base);
                        break;
                    case EditorLayerMode.Trans:
                        iEditorCurrentTile = m_MapComponent.GetPrevTile(MapComponent.Layer.Trans);
                        break;
                    case EditorLayerMode.Object:
                        iEditorCurrentTile = m_MapComponent.GetPrevTile(MapComponent.Layer.Object);
                        break;
                    default:
                        break;
                }
                return 1;                
            }
            
            if (ksKeyboardState.IsKeyDown(Keys.D))
            {
                switch (iEditorLayerMode)
                {
                    case EditorLayerMode.Base:
                        iEditorCurrentTile = m_MapComponent.GetNextTile(MapComponent.Layer.Base);
                        break;
                    case EditorLayerMode.Trans:
                        iEditorCurrentTile = m_MapComponent.GetNextTile(MapComponent.Layer.Trans);
                        break;
                    case EditorLayerMode.Object:
                        iEditorCurrentTile = m_MapComponent.GetNextTile(MapComponent.Layer.Object);
                        break;
                    default:
                        break;
                }
                return 1;
            }
            return 0;
        }

        void CheckEditorModeMouseClicks(MouseState msMouseState, KeyboardState ksKeyboardState)
        {
             // Check for mouse clicks
            if (msMouseState.LeftButton == ButtonState.Pressed)
            {
                //check mouse click on button
                if ( rectEditorObjectButton.Contains(msMouseState.X,m_InputManager.msMouseState.Y))
                {
                    iEditorLayerMode = EditorLayerMode.Object;
                    iEditorCurrentTile = m_MapComponent.TileObjStartIndex;
                }
                if (rectEditorTransButton.Contains(msMouseState.X,msMouseState.Y))
                {
                    iEditorLayerMode = EditorLayerMode.Trans;
                    iEditorCurrentTile = m_MapComponent.TileTransStartIndex;
                }
                if (rectEditorBaseButton.Contains(msMouseState.X,msMouseState.Y))
                {
                    iEditorLayerMode = EditorLayerMode.Base;
                    iEditorCurrentTile = m_MapComponent.TileBaseStartIndex;
                }

                //check mouse click on play field
                if  (rectPlayField.Contains(msMouseState.X,msMouseState.Y))
                {
                    // Determine the X and Y tile location of where we clicked
                    int iClickedX = ((msMouseState.X - m_MapComponent.Left) / m_MapComponent.TileWidth) + m_MapComponent.MapX;
                    int iClickedY = ((msMouseState.Y - m_MapComponent.Top) / m_MapComponent.TileHeight) + m_MapComponent.MapY;
                    // If we are in "Base" mode:
                    if (iEditorLayerMode == EditorLayerMode.Base)
                    {
                        m_MapComponent.EditMapBase(iClickedX, iClickedY, iEditorCurrentTile);
                    }
                    // If we are in "Trans" mode:
                    if (iEditorLayerMode == EditorLayerMode.Trans)
                    {
                        m_MapComponent.EditMapTrans(iClickedX, iClickedY, iEditorCurrentTile);
                    }
                    // If we are in "Object" mode:
                    if (iEditorLayerMode == EditorLayerMode.Object)
                    {
                        m_MapComponent.EditMapObj(iClickedX, iClickedY, iEditorCurrentTile);
                    }
                }
            }

            // we will use the right mouse button to toggle walkable and non-walkable squares.
            if (msMouseState.RightButton == ButtonState.Pressed)
            {
                // If we right-clicked in the map area...
                if (rectPlayField.Contains(msMouseState.X,msMouseState.Y))
                {
                    // Determine the X and Y tile location of where we clicked
                    int iClickedX = ((msMouseState.X - m_MapComponent.Left) / m_MapComponent.TileWidth) + m_MapComponent.MapX;
                    int iClickedY = ((msMouseState.Y - m_MapComponent.Top) / m_MapComponent.TileHeight) + m_MapComponent.MapY;
                    if (ksKeyboardState.IsKeyDown(Keys.RightShift) || ksKeyboardState.IsKeyDown(Keys.LeftShift))
                    {
                        // Shift-Right Clicking clears the walkable flag
                        m_MapComponent.EditMapData(iClickedX, iClickedY, 0);
                    }
                    else
                    {
                        // Normal Right-Clicking sets the walkable flag, preventing walking
                        m_MapComponent.EditMapData(iClickedX, iClickedY, 1);
                    }
                }
            }
        }
        
        public enum PlayMode
        {
            Play,
            Edit
        }

        public enum EditorLayerMode
        {
            Base,
            Trans,
            Object
        }
    }
}

