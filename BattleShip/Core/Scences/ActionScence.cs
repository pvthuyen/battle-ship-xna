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

        private Rectangle m_rectBaseBtnSRC;
        private Rectangle rectEditorBaseButton;

        private Rectangle m_rectTransBtnSRC;
        private Rectangle rectEditorTransButton;

        private Rectangle m_rectObjBtnSRC;
        private Rectangle rectEditorObjectButton;

        private Rectangle rectEditorBoxOverlay;
        private Rectangle rectEditorXOverlay;


        private int iEditorModeButtonPressedOffset = 45;
        private int iEditorCurrentTile;

        private MouseState msMouseState;
        private KeyboardState ksKeyboardState;

        private int iTileSetXCount = 12;
        private int iTileSetYCount = 10;

        private int iMapDisplayOffsetX;
        private int iMapDisplayOffsetY;

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

            spriteManager.Draw(gameTime);

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

            // Draw our Editor interface components.  The spriteBatch object should be in a Begin mode with alpha
            // blending BEFORE calling this routine.
            // Draw the square that holds the current tile
            Rectangle rectBase = m_rectBaseBtnSRC;
            Rectangle rectTrans = m_rectTransBtnSRC;
            Rectangle rectObject = m_rectObjBtnSRC;

            if (iEditorLayerMode == EditorLayerMode.Base) { rectBase.Offset(0, iEditorModeButtonPressedOffset); }
            if (iEditorLayerMode == EditorLayerMode.Trans) { rectTrans.Offset(0, iEditorModeButtonPressedOffset); }
            if (iEditorLayerMode == EditorLayerMode.Object) { rectObject.Offset(0, iEditorModeButtonPressedOffset); }

            m_SpriteBatch.Draw(t2dEditorImages, new Rectangle(10, 370, 75, 75), new Rectangle(540, 315, 75, 75), Color.White);
            m_SpriteBatch.Draw(t2dEditorImages, rectEditorObjectButton, rectObject, Color.White);
            m_SpriteBatch.Draw(t2dEditorImages, rectEditorTransButton, rectTrans, Color.White);
            m_SpriteBatch.Draw(t2dEditorImages, rectEditorBaseButton, rectBase, Color.White);

            /*
            int iTileToDraw = iEditorCurrentTile;
            if (iTileToDraw == iTileAnimationStartFrame)
            {
                iTileToDraw += iTileAnimationFrame;
            }

            //m_MapComponent.DrawTile(m_SpriteBatch, iTileToDraw);

            Rectangle recSource = new Rectangle((iTileToDraw % iTileSetXCount) * iTileWidth,
                                                (iTileToDraw / iTileSetXCount) * iTileHeight,
                                                iTileWidth, iTileHeight);
            m_SpriteBatch.Draw(t2dTileSet,
                             new Rectangle(iEditorCurrentTileX, iEditorCurrentTileY, iTileWidth, iTileHeight),
                             recSource,
                             Color.White);
            */

            Rectangle rectPlayField = new Rectangle(iMapDisplayOffsetX, iMapDisplayOffsetY,
                                                    (iMapDisplayWidth - 1) * 48, (iMapDisplayHeight - 1) * 48);
            // if the mouse is currently in the "playfield", draw it's location
            if ((msMouseState.X >= rectPlayField.Left) &
                (msMouseState.X <= rectPlayField.Right) &
                (msMouseState.Y >= rectPlayField.Top) &
                (msMouseState.Y <= rectPlayField.Bottom))
            {
                int iOverlayX, iOverlayY;
                iOverlayX = (((msMouseState.X - iMapDisplayOffsetX) / m_MapComponent.TileWidth) * m_MapComponent.TileWidth) + iMapDisplayOffsetX;
                iOverlayY = (((msMouseState.Y - iMapDisplayOffsetY) / m_MapComponent.TileHeight) * m_MapComponent.TileHeight) + iMapDisplayOffsetY;
                m_SpriteBatch.Draw(t2dEditorImages,
                                 new Rectangle(iOverlayX, iOverlayY, m_MapComponent.TileWidth, m_MapComponent.TileHeight),
                                 rectEditorBoxOverlay,
                                 Color.White);
            }

        }

        int iMapDisplayWidth = 14;           //kich thuot hien thi
        int iMapDisplayHeight = 8;

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //scroll map
            //m_MapComponent.ScrollByPixels(1, 1);

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
                    {
                        //m_Player.GoUp();
                        m_MapComponent.ScrollByPixels(0, -10);
                    }
                    if (down)
                    {
                        //m_Player.GoDown();
                        m_MapComponent.ScrollByPixels(0, 10);
                    }
                    if (left)
                    {
                        //m_Player.TurnLeft();
                        m_MapComponent.ScrollByPixels(-10, 0);
                    }
                    if (right)
                    {
                        //m_Player.TurnRight();
                        m_MapComponent.ScrollByPixels(10, 0);
                    }
                    break;

                case PlayMode.Edit:
                    
                    ksKeyboardState = Keyboard.GetState();
                    
                    msMouseState = Mouse.GetState();
                    /*
                    // If we AREN'T in the process of completing a smooth-scroll move...
                    if (iMoveCount <= 0)
                    {
                        if (fTotalElapsedTime >= fKeyPressCheckDelay)
                        {
                            if (CheckMapMovementKeys(ksKeyboardState) == 1)
                            {
                                fTotalElapsedTime = 0;
                            }
                            if (iProgramMode == ProgramMode.EditMode)
                            {
                                if (CheckEditorKeys(ksKeyboardState) == 1)
                                {
                                    fTotalElapsedTime = 0;
                                }
                            }
                        }
                        // Check for Editor Mode Mouse Clicks
                        if (iProgramMode == ProgramMode.EditMode)
                        {
                            CheckEditorModeMouseClicks(msMouseState, ksKeyboardState);
                        }
                    }
                    */
                    CheckEditorModeMouseClicks(msMouseState, ksKeyboardState);
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
        }

        int CheckEditorKeys(KeyboardState ksKeyboardState)
        {
            if (ksKeyboardState.IsKeyDown(Keys.O))
            {
                //WriteMapToFile("map000.txt");
                return 1;
            }
            if (ksKeyboardState.IsKeyDown(Keys.L))
            {
                //ReadMapFromFile("map000.txt");
                return 1;
            }
            if (ksKeyboardState.IsKeyDown(Keys.W))
            {
                iEditorCurrentTile -= 12;
                if (iEditorCurrentTile < 0) { iEditorCurrentTile += 12; }
                return 1;
            }
            if (ksKeyboardState.IsKeyDown(Keys.A))
            {
                iEditorCurrentTile--;
                if (iEditorCurrentTile < 0) { iEditorCurrentTile++; }
                return 1;
            }
            if (ksKeyboardState.IsKeyDown(Keys.S))
            {
                iEditorCurrentTile += 12;
                if (iEditorCurrentTile > 120) { iEditorCurrentTile -= 12; }
                return 1;
            }
            if (ksKeyboardState.IsKeyDown(Keys.D))
            {
                iEditorCurrentTile++;
                if (iEditorCurrentTile > 120) { iEditorCurrentTile--; }
                return 1;
            }
            return 0;
        }

        void CheckEditorModeMouseClicks(MouseState msMouseState, KeyboardState ksKeyboardState)
        {
            Rectangle rectPlayField = new Rectangle(iMapDisplayOffsetX, iMapDisplayOffsetY,
                                          (iMapDisplayWidth - 1) * m_MapComponent.TileWidth,
                                          (iMapDisplayHeight - 1) * m_MapComponent.TileHeight);
            // Check for mouse clicks
            if (msMouseState.LeftButton == ButtonState.Pressed)
            {
                // First, lets check to see if we click on one of the "mode" buttons.  If we do,
                // update the iEditorLayerMode variable as appropritate.
                if ((msMouseState.X >= rectEditorObjectButton.Left) &
                     (msMouseState.X <= rectEditorObjectButton.Right) &
                     (msMouseState.Y >= rectEditorObjectButton.Top) &
                     (msMouseState.Y <= rectEditorObjectButton.Bottom))
                {
                    iEditorLayerMode = EditorLayerMode.Object;
                }
                if ((msMouseState.X >= rectEditorTransButton.Left) &
                     (msMouseState.X <= rectEditorTransButton.Right) &
                     (msMouseState.Y >= rectEditorTransButton.Top) &
                     (msMouseState.Y <= rectEditorTransButton.Bottom))
                {
                    iEditorLayerMode = EditorLayerMode.Trans;
                }
                if ((msMouseState.X >= rectEditorBaseButton.Left) &
                     (msMouseState.X <= rectEditorBaseButton.Right) &
                     (msMouseState.Y >= rectEditorBaseButton.Top) &
                     (msMouseState.Y <= rectEditorBaseButton.Bottom))
                {
                    iEditorLayerMode = EditorLayerMode.Base;
                }
                // Finally, lets check to see if we are clicking inside the map area.  If so, we
                // will update the appropriate layer of the clicked tile with the currrently selected
                // drawing tile.
                if ((msMouseState.X >= rectPlayField.Left) &
                    (msMouseState.X <= rectPlayField.Right) &
                    (msMouseState.Y >= rectPlayField.Top) &
                    (msMouseState.Y <= rectPlayField.Bottom))
                {
                    // Determine the X and Y tile location of where we clicked
                    int iClickedX = ((msMouseState.X - iMapDisplayOffsetX) / m_MapComponent.TileWidth) + m_MapComponent.MapX;
                    int iClickedY = ((msMouseState.Y - iMapDisplayOffsetY) / m_MapComponent.TileHeight) + m_MapComponent.MapY;
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
                if ((msMouseState.X >= rectPlayField.Left) &
                    (msMouseState.X <= rectPlayField.Right) &
                    (msMouseState.Y >= rectPlayField.Top) &
                    (msMouseState.Y <= rectPlayField.Bottom))
                {
                    // Determine the X and Y tile location of where we clicked
                    int iClickedX = ((msMouseState.X - iMapDisplayOffsetX) / m_MapComponent.TileWidth) + m_MapComponent.MapX;
                    int iClickedY = ((msMouseState.Y - iMapDisplayOffsetY) / m_MapComponent.TileHeight) + m_MapComponent.MapY;
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

