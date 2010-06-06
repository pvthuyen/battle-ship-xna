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
        private bool bIsReturnMainMenu = false;
        private MapComponent m_MapComponent;

        private ImageComponent m_GameScreen;
        private SpriteBatch m_SpriteBatch;

        private InputManager m_InputManager;
        private AnimateEffect m_PlayerStatus;

        private Sprite m_Player;
        private Mission m_CurrMission;
        private int m_iCurrMission = 1;
        private int m_iTotalMission;
        private MessageComponent m_Message;

        private Rectangle rectPlayerLivesSRC;
        private Rectangle rectPlayerLivesDES;

        private GameState m_GameMode;

        public bool IsReturnMainMenu
        {
            get { return bIsReturnMainMenu; }
        }

        public GameState GameMode
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

        Rectangle rectSaveMapSRC;
        Rectangle rectSaveMapDest;

        Rectangle rectClearMapSRC;
        Rectangle rectClearMapDest;

        Rectangle rectNextBtnSRC;
        Rectangle rectNextBtnDest;

        Rectangle rectPrevBtnSRC;
        Rectangle rectPrevBtnDest;

        private int iEditorModeButtonPressedOffset = 45;
        private int iEditorCurrentTile;

        //thoi gian xu ly input
        float fKeyPressCheckDelay = 0.25f;
        float fMouseCheckDelay = 0.1f;

        float fTotalElapsedTimeMouse = 0;
        float fTotalElapsedTimeKeyPress = 0;

        private BattleShip.Core.Utils.Common.MapLayer iEditorLayerMode = BattleShip.Core.Utils.Common.MapLayer.Base;
        #endregion

        public ActionScence(Game game)
            : base(game)
        {
            m_SpriteBatch = game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

            m_InputManager = new InputManager();
            //load game screen
            m_GameScreen = new ImageComponent(game, ResourceManager.imgGameScreen, ImageComponent.DrawMode.Stretch);

            //load editor texture
            t2dEditorImages = ResourceManager.imgGameEditor;

            //load map
            m_MapComponent = new MapComponent(game, Utils.Utility.strMapPath);

            MapComponent.Left = 0;
            MapComponent.Top = 0;
            MapComponent.ScreenWidth = 18;
            MapComponent.ScreenHeight = 14;
            MapComponent.TileHeightWorld = game.Window.ClientBounds.Height / MapComponent.ScreenHeight;
            MapComponent.TileWidthWorld = game.Window.ClientBounds.Width / MapComponent.ScreenWidth;
            m_MapComponent.DrawFirstRowTilesOnTransLayers = false;

            m_MapComponent.Enabled = true;
            m_MapComponent.Visible = true;

            m_iTotalMission = MapComponent.TotalMaps;

            //config mini map
            int width = m_GameScreen.ImgRect.Width;
            int height = m_GameScreen.ImgRect.Height;

            int widthSRC = ResourceManager.imgGameScreen.Width;
            int heightSRC = ResourceManager.imgGameScreen.Height;

            double x = 1.0 * 18 * width / widthSRC;
            double y = 1.0 * 370 * height / heightSRC;
            Vector2 posMiniMap = new Vector2((float)x, (float)y);

            m_MapComponent.PositionMiniMap = posMiniMap;

            m_MapComponent.MiniMapCellWidth = (int)(1.0 * 120 * width / widthSRC) / MapComponent.MapWidth;
            m_MapComponent.MiniMapCellHeight = (int)(1.0 * 100 * height / heightSRC) / MapComponent.MapHeight;

            InitButtonLocation();

            ///////////////////////////////////////////////////////

            //load Player Status
            m_PlayerStatus = new AnimateEffect(ResourceManager.imgPlayerStatus);
            for (int i = 0; i < 11; i++)
            {
                m_PlayerStatus.MySprite.AddAnimation(i.ToString(), i * 71, 0, 71, 30, 1, 0.1f, true);
            }
            m_PlayerStatus.Position = new Vector2(50, 15);
            m_PlayerStatus.IsActive = true;
            m_PlayerStatus.IsVisible = true;
            m_PlayerStatus.IsLoop = true;

            //player lives
            rectPlayerLivesSRC = new Rectangle(475, 370, 45, 33);
            rectPlayerLivesDES = new Rectangle(50 + m_PlayerStatus.MySprite.Width + 50, 15, 45, 33);

            m_CurrMission = new Mission(game, m_MapComponent, rectPlayField);
            m_Message = new MessageComponent(game, ResourceManager.imgGameEditor, new Rectangle(407, 92, 130, 40), new Rectangle(0, 0, 400, 400), rectPlayField);

            //init player
            m_Player = ResourceManager.Units.ProduceUnit(UnitManager.UnitType.Player);
            m_Player.SpriteDirection = BattleShip.Core.Utils.Common.Direction.Up;
            (m_Player as UserControlledSprite).AddBullet(ResourceManager.Units.ProduceBullet(2));

            m_CurrMission.Play(m_Player, 1);

            this.m_lstGameComponent.Add(m_CurrMission);
            this.m_lstGameComponent.Add(m_GameScreen);
            this.m_lstGameComponent.Add(m_Message);
        }

        private void InitButtonLocation()
        {
            //draw edit mode
            /*
            int leftRectSelTile = m_MapComponent.MiniMapCellWidth * MapComponent.Width + 100;
            int top = Game.Window.ClientBounds.Height - m_MapComponent.MiniMapCellHeight * MapComponent.Height + 10;
            */

            int leftRectSelTile = m_MapComponent.MiniMapCellWidth * MapComponent.MapWidth + 100;
            int top = Game.Window.ClientBounds.Height - m_MapComponent.MiniMapCellHeight * MapComponent.MapHeight + 50;

            //init button location
            rectSelectTileSRC = new Rectangle(540, 315, 75, 75);
            rectSelectTileDest = new Rectangle(leftRectSelTile, top - 60, 75, 75);
            rectDrawTile = new Rectangle(leftRectSelTile + 18, top - 52, 48, 48);

            rectNextBtnSRC = new Rectangle(600, 393, 10, 14);
            rectNextBtnDest = new Rectangle(leftRectSelTile + rectSelectTileDest.Width * 2 / 3, top - 60 + rectSelectTileDest.Height + 5, 10, 12);

            rectPrevBtnSRC = new Rectangle(587, 393, 10, 14);
            rectPrevBtnDest = new Rectangle(leftRectSelTile + rectSelectTileDest.Width / 3, top - 60 + rectSelectTileDest.Height + 5, 10, 12);

            int leftBtn = leftRectSelTile + rectSelectTileSRC.Width + 20;
            rectEditorBaseButtonSRC = new Rectangle(540, 225, 80, 30);
            rectEditorBaseButton = new Rectangle(leftBtn, top, 80, 30);

            rectEditorTransButtonSRC = new Rectangle(540, 45, 80, 30);
            rectEditorTransButton = new Rectangle(leftBtn, top - 30, 80, 30);

            rectEditorObjectButtonSRC = new Rectangle(540, 135, 80, 30);
            rectEditorObjectButton = new Rectangle(leftBtn, top - 60, 80, 30);

            rectEditorXOverlay = new Rectangle(405, 270, 48, 48);
            rectEditorBoxOverlay = new Rectangle(405, 360, 48, 48);

            rectPlayField = new Rectangle(MapComponent.Left, MapComponent.Top, m_GameScreen.ImgRect.Width, (int)(m_MapComponent.PositionMiniMap.Y - 58));

            leftBtn = leftBtn + rectEditorBaseButton.Width + 100;
            rectSaveMapSRC = new Rectangle(405, 0, 134, 43);
            rectSaveMapDest = new Rectangle(leftBtn, top, 134, 43);

            rectClearMapSRC = new Rectangle(405, 45, 134, 43);
            rectClearMapDest = new Rectangle(leftBtn, top - rectSaveMapSRC.Height - 10, 134, 43);
        }

        void WriteText(string sTextOut, int x, int y, Color colorTint)
        {
            int iFontX = 0;
            int iFontY = 450;
            int iFontHeight = 12;
            int iFontWidth = 9;
            int iFontAsciiStart = 32;
            int iFontAsciiEnd = 90;
            int iOutChar;

            int iFontWidthDES = iFontWidth;
            int iFontHeightHES = iFontHeight;

            for (int i = 0; i < sTextOut.Length; i++)
            {
                iOutChar = (int)sTextOut[i];
                if ((iOutChar >= iFontAsciiStart) & (iOutChar <= iFontAsciiEnd))
                {

                    m_SpriteBatch.Draw(t2dEditorImages,
                                     new Rectangle(x + (iFontWidthDES * i), y, iFontWidthDES, iFontHeightHES),
                                     new Rectangle(iFontX + ((iOutChar - iFontAsciiStart) * iFontWidth),
                                     iFontY, iFontWidth, iFontHeight),
                                     colorTint);
                }
            }
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

                    m_SpriteBatch.Draw(t2dEditorImages,
                                     new Rectangle(x + (iFontWidthDES * i), y, iFontWidthDES, iFontHeightHES),
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

            if (iEditorLayerMode == BattleShip.Core.Utils.Common.MapLayer.Base) { rectBase.Offset(0, iEditorModeButtonPressedOffset); }
            if (iEditorLayerMode == BattleShip.Core.Utils.Common.MapLayer.Trans) { rectTrans.Offset(0, iEditorModeButtonPressedOffset); }
            if (iEditorLayerMode == BattleShip.Core.Utils.Common.MapLayer.Object) { rectObject.Offset(0, iEditorModeButtonPressedOffset); }

            m_SpriteBatch.Draw(t2dEditorImages, rectSelectTileDest, rectSelectTileSRC, Color.White);
            m_SpriteBatch.Draw(t2dEditorImages, rectEditorObjectButton, rectObject, Color.White);
            m_SpriteBatch.Draw(t2dEditorImages, rectEditorTransButton, rectTrans, Color.White);
            m_SpriteBatch.Draw(t2dEditorImages, rectEditorBaseButton, rectBase, Color.White);
            m_SpriteBatch.Draw(t2dEditorImages, rectSaveMapDest, rectSaveMapSRC, Color.White);
            m_SpriteBatch.Draw(t2dEditorImages, rectClearMapDest, rectClearMapSRC, Color.White);
            m_SpriteBatch.Draw(t2dEditorImages, rectNextBtnDest, rectNextBtnSRC, Color.White);
            m_SpriteBatch.Draw(t2dEditorImages, rectPrevBtnDest, rectPrevBtnSRC, Color.White);

            int iTileToDraw = iEditorCurrentTile;
            m_MapComponent.DrawTile(m_SpriteBatch, iTileToDraw, rectDrawTile);

            // if the mouse is currently in the "playfield", draw it's location
            if (rectPlayField.Contains(m_InputManager.msMouseState.X, m_InputManager.msMouseState.Y))
            {
                int iOverlayX, iOverlayY;

                iOverlayX = (((m_InputManager.msMouseState.X - MapComponent.Left) / MapComponent.TileWidthWorld) * MapComponent.TileWidthWorld) + MapComponent.Left;
                iOverlayY = (((m_InputManager.msMouseState.Y - MapComponent.Top) / MapComponent.TileHeightWorld) * MapComponent.TileHeightWorld) + MapComponent.Top;

                m_SpriteBatch.Draw(t2dEditorImages,
                                 new Rectangle(iOverlayX, iOverlayY, MapComponent.TileWidthWorld, MapComponent.TileHeightWorld),
                                 rectEditorBoxOverlay,
                                 Color.White);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            switch (m_GameMode)
            {
                case GameState.PlayGame:

                    base.Draw(gameTime);
                    m_PlayerStatus.Draw(m_SpriteBatch);
                    m_SpriteBatch.Draw(t2dEditorImages, rectPlayerLivesDES, rectPlayerLivesSRC, Color.White);
                    WriteText(" X " + (m_Player as UserControlledSprite).TotalLives.ToString(), rectPlayerLivesDES.X + rectPlayerLivesDES.Width + 5, rectPlayerLivesDES.Y + rectPlayerLivesDES.Height / 3, Color.Red, 2 * rectPlayerLivesDES.Height / 3, rectPlayerLivesDES.Height / 3);

                    //draw current bullet using
                    SpriteAnimation sa = (m_Player as UserControlledSprite).BulletUsing.MySprite;
                    m_SpriteBatch.Draw(sa.Texture, new Rectangle(rectPlayField.Width - 300, 15, 16, sa.Height), new Rectangle(0, 0, sa.Width, sa.Height), Color.White);
                    WriteText(" : " + (m_Player as UserControlledSprite).BulletUsing.NumBullet.ToString(), rectPlayField.Width - 300 + 20, 15, Color.Red, 15, 12);

                    WriteText("PLAY MODE", 3 * rectPlayField.Width / 4, rectPlayField.Height + 50, Color.White);
                    WriteText("YOUR SCORE: " + m_Player.Point.ToString(), rectPlayField.Width - 150, 15, Color.Red);

                    break;
                case GameState.EditGame:

                    m_MapComponent.Draw(gameTime);
                    m_MapComponent.DrawMiniMap(null, null);

                    DrawEditorInterface();

                    WriteText("EDIT MODE", 3 * rectPlayField.Width / 4, rectPlayField.Height + 50, Color.White);
                    break;
                case GameState.GameCompleted:
                    base.Draw(gameTime);
                    break;
                case GameState.MissionCompleted:
                    base.Draw(gameTime);
                    break;
                case GameState.PauseGame:
                    base.Draw(gameTime);
                    break;
                case GameState.GameOver:
                    base.Draw(gameTime);
                    break;
                default:
                    break;
            }

        }

        private void UpdatePlayerStatus(Sprite sprite)
        {
            if (sprite.CurrHP > 0)
            {
                int scale = sprite.CurrHP * 10 / sprite.MaxHP;
                m_PlayerStatus.MySprite.CurrentAnimation = (10 - scale).ToString();
            }
            else
            {
                m_PlayerStatus.MySprite.CurrentAnimation = "10";
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            fTotalElapsedTimeMouse += elapsed;
            fTotalElapsedTimeKeyPress += elapsed;

            m_InputManager.ksKeyboardState = Keyboard.GetState();
            m_InputManager.msMouseState = Mouse.GetState();

            switch (m_GameMode)
            {
                case GameState.PlayGame:
                    m_CurrMission.Enabled = true;
                    m_CurrMission.Visible = true;

                    switch (m_CurrMission.CurrMissionState)
                    {
                        case Mission.MissionState.Playing:
                            break;
                        case Mission.MissionState.Completed:
                            if (m_iCurrMission == m_iTotalMission)
                            {
                                m_GameMode = GameState.GameCompleted;
                            }
                            else
                            {
                                m_GameMode = GameState.MissionCompleted;
                            }
                            break;
                        case Mission.MissionState.Fail:
                            m_GameMode = GameState.GameOver;
                            break;
                        default:
                            break;
                    }

                    UpdatePlayerStatus(m_Player);
                    m_PlayerStatus.Update(gameTime);
                    break;

                case GameState.EditGame:
                    m_CurrMission.Enabled = false;
                    m_CurrMission.Visible = false;

                    m_MapComponent.Update(gameTime);


                    if (fTotalElapsedTimeKeyPress >= fKeyPressCheckDelay)
                    {
                        CheckEditorKeys(m_InputManager.ksKeyboardState);
                        fTotalElapsedTimeKeyPress = 0;
                    }

                    if (fTotalElapsedTimeMouse >= fMouseCheckDelay)
                    {
                        CheckEditorMouseClicks(m_InputManager.msMouseState, m_InputManager.ksKeyboardState);
                        fTotalElapsedTimeMouse = 0;
                    }

                    break;
                case GameState.GameCompleted:
                    bIsReturnMainMenu = false;
                    //show message "Congragulation"
                    m_Message.ShowMessage("CONGRAGULATION!", null, null);
                    //handler mouse input. if click on OK -> return main menu
                    if (fTotalElapsedTimeMouse >= fMouseCheckDelay)
                    {
                        if (m_Message.SetMouseListener(m_InputManager.msMouseState) == MessageComponent.MessageResult.OK)
                        {
                            bIsReturnMainMenu = true;
                        }
                        fTotalElapsedTimeMouse = 0;
                    }
                    break;
                case GameState.MissionCompleted:
                    //show message "Mission Completed"
                    m_Message.ShowMessage("MISSION COMPLETED", null, null);
                    //handler mouse input. if click on OK -> next mission
                    if (fTotalElapsedTimeMouse >= fMouseCheckDelay)
                    {
                        if (m_Message.SetMouseListener(m_InputManager.msMouseState) == MessageComponent.MessageResult.OK)
                        {
                            m_GameMode = GameState.PlayGame;
                            m_CurrMission.Play(m_Player, ++m_iCurrMission);
                        }
                        fTotalElapsedTimeMouse = 0;
                    }
                    break;
                case GameState.PauseGame:
                    //show message "Pause Game"
                    m_Message.ShowMessage("PAUSE GAME", null, null);
                    //handler mouse input. if click on OK -> continue playing game
                    if (fTotalElapsedTimeMouse >= fMouseCheckDelay)
                    {
                        if (m_Message.SetMouseListener(m_InputManager.msMouseState) == MessageComponent.MessageResult.OK)
                        {
                            m_GameMode = GameState.PlayGame;
                        }
                        fTotalElapsedTimeMouse = 0;
                    }
                    break;
                case GameState.GameOver:
                    bIsReturnMainMenu = false;
                    //show message "Game Over"
                    m_Message.ShowMessage("GAME OVER", null, null);
                    //handler mouse input. if click on OK -> return main menu
                    if (fTotalElapsedTimeMouse >= fMouseCheckDelay)
                    {
                        if (m_Message.SetMouseListener(m_InputManager.msMouseState) == MessageComponent.MessageResult.OK)
                        {
                            bIsReturnMainMenu = true;
                        }
                        fTotalElapsedTimeMouse = 0;
                    }
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

        int CheckEditorKeys(KeyboardState ksKeyboardState)
        {
            if (ksKeyboardState.IsKeyDown(Keys.Left))
            {
                MapComponent.ScrollByPixels(-MapComponent.TileWidthWorld, 0);
                return 1;
            }
            if (ksKeyboardState.IsKeyDown(Keys.Right))
            {
                MapComponent.ScrollByPixels(MapComponent.TileWidthWorld, 0);
                return 1;
            }
            if (ksKeyboardState.IsKeyDown(Keys.Up))
            {
                MapComponent.ScrollByPixels(0, -MapComponent.TileHeightWorld);
                return 1;
            }
            if (ksKeyboardState.IsKeyDown(Keys.Down))
            {
                MapComponent.ScrollByPixels(0, MapComponent.TileHeightWorld);
                return 1;
            }
            return 0;
        }

        void CheckEditorMouseClicks(MouseState msMouseState, KeyboardState ksKeyboardState)
        {
            // Check for mouse clicks
            if (msMouseState.LeftButton == ButtonState.Pressed)
            {
                //click on obj button
                if (rectEditorObjectButton.Contains(msMouseState.X, m_InputManager.msMouseState.Y))
                {
                    iEditorLayerMode = BattleShip.Core.Utils.Common.MapLayer.Object;
                    iEditorCurrentTile = m_MapComponent.TileObjStartIndex;
                }
                //click on trans button
                if (rectEditorTransButton.Contains(msMouseState.X, msMouseState.Y))
                {
                    iEditorLayerMode = BattleShip.Core.Utils.Common.MapLayer.Trans;
                    iEditorCurrentTile = m_MapComponent.TileTransStartIndex;
                }
                //click on base button
                if (rectEditorBaseButton.Contains(msMouseState.X, msMouseState.Y))
                {
                    iEditorLayerMode = BattleShip.Core.Utils.Common.MapLayer.Base;
                    iEditorCurrentTile = m_MapComponent.TileBaseStartIndex;
                }
                //click on prev button
                if (rectPrevBtnDest.Contains(msMouseState.X, msMouseState.Y))
                {
                    switch (iEditorLayerMode)
                    {
                        case BattleShip.Core.Utils.Common.MapLayer.Base:
                            iEditorCurrentTile = m_MapComponent.GetPrevTile(BattleShip.Core.Utils.Common.MapLayer.Base);
                            break;
                        case BattleShip.Core.Utils.Common.MapLayer.Trans:
                            iEditorCurrentTile = m_MapComponent.GetPrevTile(BattleShip.Core.Utils.Common.MapLayer.Trans);
                            break;
                        case BattleShip.Core.Utils.Common.MapLayer.Object:
                            iEditorCurrentTile = m_MapComponent.GetPrevTile(BattleShip.Core.Utils.Common.MapLayer.Object);
                            break;
                        default:
                            break;
                    }
                }
                //click on next button
                if (rectNextBtnDest.Contains(msMouseState.X, msMouseState.Y))
                {
                    switch (iEditorLayerMode)
                    {
                        case BattleShip.Core.Utils.Common.MapLayer.Base:
                            iEditorCurrentTile = m_MapComponent.GetNextTile(BattleShip.Core.Utils.Common.MapLayer.Base);
                            break;
                        case BattleShip.Core.Utils.Common.MapLayer.Trans:
                            iEditorCurrentTile = m_MapComponent.GetNextTile(BattleShip.Core.Utils.Common.MapLayer.Trans);
                            break;
                        case BattleShip.Core.Utils.Common.MapLayer.Object:
                            iEditorCurrentTile = m_MapComponent.GetNextTile(BattleShip.Core.Utils.Common.MapLayer.Object);
                            break;
                        default:
                            break;
                    }
                }
                //click on save map button
                if (rectSaveMapDest.Contains(msMouseState.X, msMouseState.Y))
                {
                    m_MapComponent.SaveMap(Utils.Utility.strMapPath, MapComponent.TotalMaps);
                }
                //click on clear map button
                if (rectClearMapDest.Contains(msMouseState.X, msMouseState.Y))
                {
                    m_MapComponent.ClearMap();
                }

                //mouse on play field
                if (rectPlayField.Contains(msMouseState.X, msMouseState.Y))
                {
                    // Determine the X and Y tile location of where we clicked
                    int iClickedX = MapComponent.X_World_To_X_Map(msMouseState.X);
                    int iClickedY = MapComponent.Y_World_To_Y_Map(msMouseState.Y);

                    switch (iEditorLayerMode)
                    {
                        case BattleShip.Core.Utils.Common.MapLayer.Base:
                            MapComponent.EditMap(iClickedX, iClickedY, iEditorCurrentTile, BattleShip.Core.Utils.Common.MapLayer.Base);
                            break;
                        case BattleShip.Core.Utils.Common.MapLayer.Trans:
                            MapComponent.EditMap(iClickedX, iClickedY, iEditorCurrentTile, BattleShip.Core.Utils.Common.MapLayer.Trans);
                            break;
                        case BattleShip.Core.Utils.Common.MapLayer.Object:
                            MapComponent.EditMap(iClickedX, iClickedY, iEditorCurrentTile, BattleShip.Core.Utils.Common.MapLayer.Object);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public enum GameState
        {
            PlayGame,
            EditGame,
            PauseGame,
            GameOver,
            GameCompleted,
            MissionCompleted
        }

        public void Restart()
        {
            (m_Player as UserControlledSprite).Restart();
            m_CurrMission.Play(m_Player, 1);
            m_iCurrMission = 1;
            bIsReturnMainMenu = false;
        }
    }
}
