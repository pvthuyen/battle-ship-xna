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
using BattleShip.Core.Managers;


namespace BattleShip.Core.GameComponents
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MapComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        
        private class TileSet
        {
            public Texture2D m_t2dTexture;
            public string m_strFileName;
            public int m_iVerticalSpacing;
            public int m_iHorizontalSpacing;
            public int m_iTileWidth;
            public int m_iTileHeight;
            public int m_iTilesPerRow;
        }

        private class TileAnimation
        {
            public int m_iStartFrame;
            public int m_iFrameCount;
            public int m_iCurrentFrame;
            public int m_iFrameRate;
        }

        private List<TileSet> m_TileSets = new List<TileSet>();
        private List<TileAnimation> m_taAnimations = new List<TileAnimation>();

        private int m_iTileWidth;
        private int m_iTileHeight;

        private int m_iTileSetCount;

        private int m_iMapWidth;
        private int m_iMapHeight;

        private int[,] m_iMap;
        private int[,] m_iMapTrans;
        private int[,] m_iMapObj;
        private int[,] m_iMapData;

        private int m_iMapX;
        private int m_iMapY;
        private int m_iMapSubX;
        private int m_iMapSubY;

        private int m_iScreenHeight;
        private int m_iScreenWidth;
        private int m_iScreenX;
        private int m_iScreenY;

        private bool m_bDrawBase = true;
        private bool m_bDrawTrans = true;
        private bool m_bDrawObj = true;
        private bool m_bDrawFirstRowTiles = true;

        private SpriteBatch m_spriteBatch;
        private ResourceManager m_resourceManager;

        public int Left
        {
            // The Left property determines the left pixel position of the
            // tile engine drawing area on the display.
            get { return this.m_iScreenX; }
            set { this.m_iScreenX = value; }
        }

        public int Top
        {
            // The Top property determines the top pixel position of the
            // tile engine drawing area on the display.
            get { return this.m_iScreenY; }
            set { this.m_iScreenY = value; }
        }

        public int Width
        {
            // The Width property determines how many tiles wide will be
            // drawn by the tile engine.  Note that this property is in TILES
            // and not in PIXELS
            get { return this.m_iScreenWidth; }
            set { this.m_iScreenWidth = value; }
        }

        public int Height
        {
            // the Height property determines how many tiles high will be
            // drawn by the tile engine.  Note that this property is in TILES
            // and not in PIXELS
            get { return this.m_iScreenHeight; }
            set { this.m_iScreenHeight = value; }
        }

        public int TileWidth
        {
            // Determines the width of an individual tile in pixels.
            get { return this.m_iTileWidth; }
            set { this.m_iTileWidth = value; }
        }

        public int TileHeight
        {
            // Determines the height of an individual tile in pixels.
            get { return this.m_iTileHeight; }
            set { this.m_iTileHeight = value; }
        }

        public int MapX
        {
            // Determines the X map coordinate.  X=0 is the left-most tile on
            // the map.  The X coordinate represents the X value of the left-most
            // displayed map tile.
            get { return this.m_iMapX; }
            set { this.m_iMapX = value; }
        }

        public int MapY
        {
            // Determines the Y map coordinate.  Y=0 is the top-most tile on
            // the map.  The Y coordinate represents the Y value of the left-most
            // displayed map tile.
            get { return this.m_iMapY; }
            set { this.m_iMapY = value; }
        }

        public int MapWidth
        {
            // The MapWidth property is read-only since it is determined
            // by the map that is loaded.
            get { return this.m_iMapWidth; }
        }

        public int MapHeight
        {
            // The MapHeight property is read-only since it is determined
            // by the map that is loaded.
            get { return this.m_iMapHeight; }
        }

        public bool BaseVisible
        {
            get { return this.m_bDrawBase; }
            set { this.m_bDrawBase = value; }
        }

        public bool TransVisible
        {
            get { return this.m_bDrawTrans; }
            set { this.m_bDrawTrans = value; }
        }

        public bool ObjVisible
        {
            get { return this.m_bDrawObj; }
            set { this.m_bDrawObj = value; }
        }

        public bool DrawFirstRowTilesOnTransLayers
        {
            get { return this.m_bDrawFirstRowTiles; }
            set { this.m_bDrawFirstRowTiles = value; }
        }

        public void LoadMap(string strXmlFile, int iLevel)
        {

        }

        public void SaveMap()
        {

        }

        public void AddTileset(string sFileName, int iTileWidth, int iTileHeight, int iHorizontalSpacing, int iVerticalSpacing)
        {
            // Load a new tileset from a file and add it to the tileset list.
            // Each tileset *must* be the same size, and all tiles must be the
            // same size.  
            /*
            TileSet NewTileSet = new TileSet();
            try
            {
                NewTileSet.t2dTexture = this.m_resourceManager.Load(sFileName);

                NewTileSet.m_strFileName = sFileName;
                NewTileSet.m_iTileWidth = iTileWidth;
                NewTileSet.m_iTileHeight = iTileHeight;
                NewTileSet.m_iHorizontalSpacing = iHorizontalSpacing;
                NewTileSet.m_iVerticalSpacing = iVerticalSpacing;
                NewTileSet.m_iTilesPerRow = NewTileSet.m_t2dTexture.Width /
                                         (NewTileSet.m_iTileWidth + NewTileSet.m_iHorizontalSpacing);
                
                m_TileSets.Add(NewTileSet);
                m_iTileSetCount++;
            }
            catch
            {
                throw;
            }*/
        }

        public void SetMapLocation(int x, int y, int subx, int suby)
        {
            // Sets the current map location, providing the X,Y coordintate
            // as well as the Sub-Tile X and Y positions.
            m_iMapX = x;
            m_iMapY = y;
            m_iMapSubX = subx;
            m_iMapSubY = suby;
        }

        public void ScrollByPixels(int x, int y)
        {
            // Move the map by X pixels horizontally and Y pixels vertically.
            // Accounts for moving off of a tile and onto another as well as
            // moving off of the end of the map by looping around to the other
            // side.
            m_iMapSubX += x;
            m_iMapSubY += y;
            while (m_iMapSubX >= m_iTileWidth)
            {
                m_iMapSubX -= m_iTileWidth;
                m_iMapX++;
            }

            while (m_iMapSubY >= m_iTileHeight)
            {
                m_iMapSubY -= m_iTileHeight;
                m_iMapY++;
            }

            while (m_iMapSubX < 0)
            {
                m_iMapSubX += m_iTileWidth;
                m_iMapX--;
            }

            while (m_iMapSubY < 0)
            {
                m_iMapSubY += m_iTileHeight;
                m_iMapY--;
            }

            if (m_iMapX >= m_iMapWidth)
            {
                m_iMapX -= m_iMapWidth;
            }

            if (m_iMapX < 0)
            {
                m_iMapX += m_iMapWidth;
            }

            if (m_iMapY >= m_iMapHeight)
            {
                m_iMapY -= m_iMapHeight;
            }

            if (m_iMapY < 0)
            {
                m_iMapY += m_iMapHeight;
            }
        }

        public void EditMap(int x, int y, int iBase, int iTrans, int iObj, int iData)
        {
            m_iMap[y, x] = iBase;
            m_iMapTrans[y, x] = iTrans;
            m_iMapObj[y, x] = iObj;
            m_iMapData[y, x] = iData;
        }

        public int GetMapBase(int x, int y)
        {
            return m_iMap[y, x];
        }

        public int GetMapTrans(int x, int y)
        {
            return m_iMapTrans[y, x];
        }

        public int GetMapObj(int x, int y)
        {
            return m_iMapObj[y, x];
        }

        public int GetMapData(int x, int y)
        {
            return m_iMapData[y, x];
        }

        public void AddTileAnimation(int iStartFrame, int iFrameCount, int iFrameRate)
        {
            // Define a new tileset animation.  Tiles in an animation must
            // be consecutive and must all reside on the same tileset.
            TileAnimation thisAnimation = new TileAnimation();

            thisAnimation.m_iStartFrame = iStartFrame;
            thisAnimation.m_iFrameCount = iFrameCount;
            thisAnimation.m_iFrameRate = iFrameRate;
            thisAnimation.m_iCurrentFrame = 0;

            m_taAnimations.Add(thisAnimation);
        }

        private bool IsAnimatedTile(int iTile)
        {
            foreach (TileAnimation thisAnimation in m_taAnimations)
            {
                if (thisAnimation.m_iStartFrame == iTile)
                {
                    return true;
                }
            }
            return false;
        }

        private int GetAnimatedFrame(int iTile)
        {
            foreach (TileAnimation thisAnimation in m_taAnimations)
            {
                if (thisAnimation.m_iStartFrame == iTile)
                {
                    return thisAnimation.m_iStartFrame + (thisAnimation.m_iCurrentFrame / thisAnimation.m_iFrameRate);
                }
            }
            return 0;
        }

        private void UpdateAnimationFrames()
        {
            for (int x = 0; x < m_taAnimations.Count; x++)
            {
                m_taAnimations[x].m_iCurrentFrame++;
                if (m_taAnimations[x].m_iCurrentFrame > ((m_taAnimations[x].m_iFrameCount * m_taAnimations[x].m_iFrameRate)
                                                  + (m_taAnimations[x].m_iFrameRate - 1)))
                {
                    m_taAnimations[x].m_iCurrentFrame = 0;
                }
            }
        }

        private Rectangle GetTileRectangle(int iTileNumber)
        {
            // Returns a rectangle representing the location on the tileset that
            // contains the requested tile.
            // The 10000's digit determines the tileset number.  The remainder 
            // determines the tile within the set, so seperate out the remainder 
            // to determine where to pull the rectangle from.
            int iTile = iTileNumber % 10000;
            int iTileSet = iTileNumber / 10000;

            // Return a rectangle representing a location of a tile on the tileset
            return new Rectangle(
              (iTile % m_TileSets[iTileSet].m_iTilesPerRow) * m_TileSets[iTileSet].m_iTileWidth,
              (iTile / m_TileSets[iTileSet].m_iTilesPerRow) * m_TileSets[iTileSet].m_iTileHeight,
              m_TileSets[iTileSet].m_iTileWidth,
              m_TileSets[iTileSet].m_iTileHeight
            );
        }

        public MapComponent(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            this.Enabled = false;
            this.Visible = false;

            // Set the tileset count to 0
            m_iTileSetCount = 0;

            this.m_resourceManager = new ResourceManager(Game.Content);
            this.m_spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
                                                
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
         
    }
}