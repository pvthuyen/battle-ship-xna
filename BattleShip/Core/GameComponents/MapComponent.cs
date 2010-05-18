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
using System.Xml;
using BattleShip.Core.Sprites;


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

        private List<TileSet> m_TileSets = new List<TileSet>();                 //list cac tile set
        private List<TileAnimation> m_taAnimations = new List<TileAnimation>(); //list cac tile animation trong tile set

        //kich thuoc cua 1 tile
        private int m_iTileWidth;   
        private int m_iTileHeight;

        //so luong tile trong tile set
        //private int m_iTileSetCount;

        //kich thuoc cua map hien thi
        private int m_iMapWidth;
        private int m_iMapHeight;

        //du lieu cua map
        private int[,] m_iMap;          //du lieu map o tang background
        private int[,] m_iMapTrans;     //du lieu map o tang transition
        private int[,] m_iMapObj;       //du lieu map o tang obj
        private int[,] m_iMapData;      //khu vuc di chuyen cua map

        //vi tri goc trai tren cung duoc hien thi cua map
        private int m_iMapX;
        private int m_iMapY;
        private int m_iMapSubX;
        private int m_iMapSubY;

        //kich thuoc hien thi man hinh
        private int m_iScreenHeight;    //so luong tile theo chieu rong
        private int m_iScreenWidth;     //so luong tile theo chieu ngang
        private int m_iScreenX;         //vi tri x o goc trai tren
        private int m_iScreenY;         //vi tri y o goc trai tren

        private bool m_bDrawBase = true;
        private bool m_bDrawTrans = true;
        private bool m_bDrawObj = true;
        private bool m_bDrawFirstRowTiles = true;

        private int m_iXLoc;
        private int m_iYLoc;
        private int m_iTileToDraw;

        //info of type of tile: start index, end index, current index (base, trans, obj)
        private int m_iTileBaseCurrIndex;
        private int m_iTileBaseStart;
        private int m_iTileBaseEnd;

        private int m_iTileTransCurrIndex;
        private int m_iTileTransStart;
        private int m_iTileTransEnd;

        private int m_iTileObjCurrIndex;
        private int m_iTileObjStart;
        private int m_iTileObjEnd;

        //information of mini map
        private Vector2 m_PosMiniMap;
        private int m_iMiniMapCellWidth;
        private int m_iMiniMapCellHeight;

        private SpriteBatch m_spriteBatch;
        private ResourceManager m_resourceManager;

        public int TileBaseStartIndex
        {
            get { return this.m_iTileBaseStart; }
        }

        public int TileTransStartIndex
        {
            get { return this.m_iTileTransStart; }
        }

        public int TileObjStartIndex
        {
            get { return this.m_iTileObjStart; }
        }

        public Vector2 PositionMiniMap
        {
            set { this.m_PosMiniMap = value; }
            get { return this.m_PosMiniMap; }
        }

        public int MiniMapCellWidth
        {
            set { this.m_iMiniMapCellWidth = value; }
            get { return this.m_iMiniMapCellWidth; }
        }

        public int MiniMapCellHeight
        {
            set { this.m_iMiniMapCellHeight = value; }
            get { return this.m_iMiniMapCellHeight; }
        }

        public int Left
        {
            get { return this.m_iScreenX; }
            set { this.m_iScreenX = value; }
        }

        public int Top
        {
            get { return this.m_iScreenY; }
            set { this.m_iScreenY = value; }
        }

        /// <summary>
        /// so luong tile theo chieu rong cua map
        /// </summary>
        public int Width
        {            
            get { return this.m_iScreenWidth; }
            set { this.m_iScreenWidth = value; }
        }

        /// <summary>
        /// so luong tile theo chieu cao cua map
        /// </summary>
        public int Height
        {
            get { return this.m_iScreenHeight; }
            set { this.m_iScreenHeight = value; }
        }

        /// <summary>
        /// kich thuoc chieu rong cua 1 tile tinh bang pixel
        /// </summary>
        public int TileWidth
        {            
            get { return this.m_iTileWidth; }
            set { this.m_iTileWidth = value; }
        }

        /// <summary>
        /// kich thuoc chieu cao cua 1 tile tinh bang pixel
        /// </summary>
        public int TileHeight
        {            
            get { return this.m_iTileHeight; }
            set { this.m_iTileHeight = value; }
        }

        /// <summary>
        /// vi tri hien thi cua map theo x
        /// </summary>
        public int MapX
        {
            get { return this.m_iMapX; }
            set { this.m_iMapX = value; }
        }

        /// <summary>
        /// vi tri hien thi cua map theo y
        /// </summary>
        public int MapY
        {
            get { return this.m_iMapY; }
            set { this.m_iMapY = value; }
        }

        /// <summary>
        /// kich thuot hien thi theo chieu ngang
        /// </summary>
        public int MapWidth
        {
            get { return this.m_iMapWidth; }
        }

        /// <summary>
        /// kich thuoc hien thi theo chieu cao
        /// </summary>
        public int MapHeight
        {
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

        /// <summary>
        /// load du lieu map tu file xml
        /// </summary>
        /// <param name="strXmlFile">file du lieu</param>
        /// <param name="iLevel">vong choi</param>
        public void LoadMap(string strXmlFile, int iLevel)
        {
            XmlDocument mapXml = new XmlDocument();
            mapXml.Load(strXmlFile);

            //read data
            XmlNodeList nodeList = mapXml.GetElementsByTagName("Data");
            string data = string.Empty;
            foreach (XmlNode node in nodeList)
            {
                this.m_iMapWidth = int.Parse(node.Attributes["MapWidth"].Value);
                this.m_iMapHeight = int.Parse(node.Attributes["MapHeight"].Value);
                data = node.InnerText;
            }

            m_iMapData = new int[this.m_iMapWidth, this.m_iMapHeight];
            m_iMapObj = new int[this.m_iMapWidth, this.m_iMapHeight];
            m_iMapTrans = new int[this.m_iMapWidth, this.m_iMapHeight];
            m_iMap = new int[this.m_iMapWidth, this.m_iMapHeight];

            int i, j;
            string[] arrValue = data.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int index = 0;
            for (i = 0; i < this.m_iMapHeight; i++)
            {
                for (j = 0; j < this.m_iMapWidth; j++)
                {
                    m_iMap[i, j] = int.Parse(arrValue[index++]);                    
                    m_iMapTrans[i, j] = int.Parse(arrValue[index++]);
                    m_iMapObj[i, j] = int.Parse(arrValue[index++]);
                    m_iMapData[i, j] = int.Parse(arrValue[index++]);
                }
            }

            //read tile set
            nodeList = mapXml.GetElementsByTagName("TileSet");
            foreach (XmlNode node in nodeList)
            {
                TileSet tileSet = new TileSet();
                tileSet.m_strFileName = node.Attributes["FileName"].Value;
                tileSet.m_t2dTexture = m_resourceManager.LoadTexture(@"Resource/Background/" + tileSet.m_strFileName);
                tileSet.m_iTileWidth = int.Parse(node.Attributes["TileWidth"].Value);
                tileSet.m_iTileHeight = int.Parse(node.Attributes["TileHeight"].Value);
                tileSet.m_iVerticalSpacing = int.Parse(node.Attributes["VerticalSpacing"].Value);
                tileSet.m_iHorizontalSpacing = int.Parse(node.Attributes["HorizontalSpacing"].Value);
                tileSet.m_iTilesPerRow = tileSet.m_t2dTexture.Width /
                                         (tileSet.m_iTileWidth + tileSet.m_iHorizontalSpacing);

                this.m_iTileHeight = tileSet.m_iTileHeight;
                this.m_iTileWidth = tileSet.m_iTileWidth;
   
                this.m_TileSets.Add(tileSet);

                m_iTileBaseStart = int.Parse(node.Attributes["TileBaseStart"].Value);
                m_iTileBaseEnd = int.Parse(node.Attributes["TileBaseEnd"].Value);
                m_iTileBaseCurrIndex = m_iTileBaseStart;

                m_iTileTransStart = int.Parse(node.Attributes["TileTransStart"].Value);
                m_iTileTransEnd = int.Parse(node.Attributes["TileTransEnd"].Value);
                m_iTileTransCurrIndex = m_iTileTransStart;

                m_iTileObjStart = int.Parse(node.Attributes["TileObjStart"].Value);
                m_iTileObjEnd = int.Parse(node.Attributes["TileObjEnd"].Value);
                m_iTileObjCurrIndex = m_iTileObjStart;
            }
                        
            nodeList = mapXml.GetElementsByTagName("TileAnimation");
            foreach (XmlNode node in nodeList)
            {
                TileAnimation ta = new TileAnimation();
                ta.m_iCurrentFrame = 0;
                ta.m_iStartFrame = int.Parse(node.Attributes["StartFrame"].Value);
                ta.m_iFrameCount = int.Parse(node.Attributes["FrameCount"].Value);
                ta.m_iFrameRate = int.Parse(node.Attributes["FrameRate"].Value);

                this.m_taAnimations.Add(ta);
            }                        
        }

        /// <summary>
        /// luu map vao file xml
        /// </summary>
        public void SaveMap(String strXmlFile)
        {
            XmlWriter writer;
            
            XmlDocument mapXml = new XmlDocument();
            mapXml.Load(strXmlFile);

            XmlElement elem = mapXml.CreateElement("Data");
            
            /*
            mapXml.a

            //read data
            XmlNodeList nodeList = mapXml.GetElementsByTagName("Data");
            string data = string.Empty;
            foreach (XmlNode node in nodeList)
            {
                this.m_iMapWidth = int.Parse(node.Attributes["MapWidth"].Value);
                this.m_iMapHeight = int.Parse(node.Attributes["MapHeight"].Value);
                data = node.InnerText;
            }

            m_iMapData = new int[this.m_iMapWidth, this.m_iMapHeight];
            m_iMapObj = new int[this.m_iMapWidth, this.m_iMapHeight];
            m_iMapTrans = new int[this.m_iMapWidth, this.m_iMapHeight];
            m_iMap = new int[this.m_iMapWidth, this.m_iMapHeight];

            int i, j;
            string[] arrValue = data.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int index = 0;
            for (i = 0; i < this.m_iMapHeight; i++)
            {
                for (j = 0; j < this.m_iMapWidth; j++)
                {
                    m_iMap[i, j] = int.Parse(arrValue[index++]);
                    m_iMapTrans[i, j] = int.Parse(arrValue[index++]);
                    m_iMapObj[i, j] = int.Parse(arrValue[index++]);
                    m_iMapData[i, j] = int.Parse(arrValue[index++]);
                }
            }

            //read tile set
            nodeList = mapXml.GetElementsByTagName("TileSet");
            foreach (XmlNode node in nodeList)
            {
                TileSet tileSet = new TileSet();
                tileSet.m_strFileName = node.Attributes["FileName"].Value;
                tileSet.m_t2dTexture = m_resourceManager.LoadTexture(@"Resource/Background/" + tileSet.m_strFileName);
                tileSet.m_iTileWidth = int.Parse(node.Attributes["TileWidth"].Value);
                tileSet.m_iTileHeight = int.Parse(node.Attributes["TileHeight"].Value);
                tileSet.m_iVerticalSpacing = int.Parse(node.Attributes["VerticalSpacing"].Value);
                tileSet.m_iHorizontalSpacing = int.Parse(node.Attributes["HorizontalSpacing"].Value);
                tileSet.m_iTilesPerRow = tileSet.m_t2dTexture.Width /
                                         (tileSet.m_iTileWidth + tileSet.m_iHorizontalSpacing);

                this.m_iTileHeight = tileSet.m_iTileHeight;
                this.m_iTileWidth = tileSet.m_iTileWidth;

                this.m_TileSets.Add(tileSet);

                m_iTileBaseStart = int.Parse(node.Attributes["TileBaseStart"].Value);
                m_iTileBaseEnd = int.Parse(node.Attributes["TileBaseEnd"].Value);
                m_iTileBaseCurrIndex = m_iTileBaseStart;

                m_iTileTransStart = int.Parse(node.Attributes["TileTransStart"].Value);
                m_iTileTransEnd = int.Parse(node.Attributes["TileTransEnd"].Value);
                m_iTileTransCurrIndex = m_iTileTransStart;

                m_iTileObjStart = int.Parse(node.Attributes["TileObjStart"].Value);
                m_iTileObjEnd = int.Parse(node.Attributes["TileObjEnd"].Value);
                m_iTileObjCurrIndex = m_iTileObjStart;
            }

            nodeList = mapXml.GetElementsByTagName("TileAnimation");
            foreach (XmlNode node in nodeList)
            {
                TileAnimation ta = new TileAnimation();
                ta.m_iCurrentFrame = 0;
                ta.m_iStartFrame = int.Parse(node.Attributes["StartFrame"].Value);
                ta.m_iFrameCount = int.Parse(node.Attributes["FrameCount"].Value);
                ta.m_iFrameRate = int.Parse(node.Attributes["FrameRate"].Value);

                this.m_taAnimations.Add(ta);
            }                 
             */
        }
               
        /// <summary>
        /// set vi tri ban dau cua map
        /// </summary>
        /// <param name="x">x bat dau hien thi</param>
        /// <param name="y">y bat dau hien thi</param>
        /// <param name="subx">vi tri ben trong cua 1 o</param>
        /// <param name="suby">vi tri ben trong cua 1 o</param>
        public void SetMapLocation(int x, int y, int subx, int suby)
        {
            m_iMapX = x;
            m_iMapY = y;
            m_iMapSubX = subx;
            m_iMapSubY = suby;
        }

        /// <summary>
        /// scroll map
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ScrollByPixels(int x, int y)
        {
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

        public void EditMapBase(int x, int y, int iBase)
        {
            m_iMap[y, x] = iBase;
        }

        public void EditMapTrans(int x, int y, int iTrans)
        {
            m_iMapTrans[y, x] = iTrans;
        }

        public void EditMapObj(int x, int y, int iObj)
        {
            m_iMapObj[y, x] = iObj;
        }

        public void EditMapData(int x, int y, int iData)
        {
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

        public int GetNextTile(Layer layer)
        {            
            switch (layer)
            {
                case Layer.Base:                    
                    m_iTileBaseCurrIndex++;
                    if (m_iTileBaseCurrIndex > m_iTileBaseEnd)
                    {
                        m_iTileBaseCurrIndex = m_iTileBaseStart;
                    }
                    return m_iTileBaseCurrIndex;
                    
                case Layer.Trans:                    
                    m_iTileTransCurrIndex++;
                    if (m_iTileTransCurrIndex > m_iTileTransEnd)
                    {
                        m_iTileTransCurrIndex = m_iTileTransStart;
                    }
                    return m_iTileTransCurrIndex;                    

                case Layer.Object:             
                    m_iTileObjCurrIndex++;
                    if (m_iTileObjCurrIndex > m_iTileObjEnd)
                    {
                        m_iTileObjCurrIndex = m_iTileObjStart;
                    }
                    return m_iTileObjCurrIndex;

                default:
                    return -1;
            }            
        }

        public int GetPrevTile(Layer layer)
        {
            switch (layer)
            {
                case Layer.Base:
                    m_iTileBaseCurrIndex--;
                    if (m_iTileBaseCurrIndex < m_iTileBaseStart)
                    {
                        m_iTileBaseCurrIndex = m_iTileBaseEnd;
                    }
                    return m_iTileBaseCurrIndex;

                case Layer.Trans:
                    m_iTileTransCurrIndex--;
                    if (m_iTileTransCurrIndex < m_iTileTransStart)
                    {
                        m_iTileTransCurrIndex = m_iTileTransEnd;
                    }
                    return m_iTileTransCurrIndex;

                case Layer.Object:
                    m_iTileObjCurrIndex--;
                    if (m_iTileObjCurrIndex < m_iTileObjStart)
                    {
                        m_iTileObjCurrIndex = m_iTileObjEnd;
                    }
                    return m_iTileObjCurrIndex;

                default:
                    return -1;
            }
        }


        /// <summary>
        /// kiem tra 1 tile co phai la trong day animation hay khong
        /// </summary>
        /// <param name="iTile"></param>
        /// <returns></returns>
        private bool IsAnimatedTile(int iTile)
        {
            foreach (TileAnimation thisAnimation in m_taAnimations)
            {
                if (thisAnimation.m_iStartFrame <= iTile && thisAnimation.m_iStartFrame + thisAnimation.m_iFrameCount >= iTile)
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

        /// <summary>
        /// hien thi animation cho tile
        /// </summary>
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
            m_resourceManager = game.Services.GetService(typeof(ResourceManager)) as ResourceManager;
            m_spriteBatch = game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            
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
            //m_iTileSetCount = 0;

            this.m_resourceManager = new ResourceManager(Game.Content);
            this.m_spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
                                                
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            m_spriteBatch.End();
            m_spriteBatch.Begin(SpriteBlendMode.None);
            
            // Draw the base layer of the map
            if (m_bDrawBase)
            {
                for (int y = 0; y < m_iScreenHeight; y++)
                {
                    for (int x = 0; x < m_iScreenWidth; x++)
                    {                        
                        m_iXLoc = x + m_iMapX;
                        m_iYLoc = y + m_iMapY;

                        // Account for map wrap-arounds
                        if (m_iXLoc >= m_iMapHeight)
                        {
                            m_iXLoc -= m_iMapWidth;
                        }

                        if (m_iXLoc < 0)
                        {
                            m_iXLoc += m_iMapWidth;
                        }

                        if (m_iYLoc >= m_iMapHeight)
                        {
                            m_iYLoc -= m_iMapHeight;
                        }

                        if (m_iYLoc < 0)
                        {
                            m_iYLoc += m_iMapHeight;
                        }

                        m_iTileToDraw = m_iMap[m_iYLoc, m_iXLoc];

                        if (IsAnimatedTile(m_iTileToDraw))
                        {
                            m_iTileToDraw = GetAnimatedFrame(m_iTileToDraw);
                        }

                        if (m_iTileToDraw >= 0)
                        // Only draw tiles > 0 since TileStudio uses -1
                        // to indicate an empty tile.
                        {
                            // Draw the tile.  We divide the tile number by 10000 to
                            // determine what tileset the tile is on (0-9999=tileset 0,
                            // 10000-19999=Tileset 2, etc)
                            m_spriteBatch.Draw(m_TileSets[(m_iTileToDraw / 10000)].m_t2dTexture,
                                             new Rectangle(((x * m_iTileWidth) + m_iScreenX) - m_iMapSubX,
                                             ((y * m_iTileHeight) + m_iScreenY) - m_iMapSubY,
                                             m_TileSets[(m_iTileToDraw / 10000)].m_iTileWidth,
                                             m_TileSets[(m_iTileToDraw / 10000)].m_iTileHeight),
                                             GetTileRectangle(m_iTileToDraw),
                                             Color.White);
                        }
                    }
                }
            }
            m_spriteBatch.End();

            m_spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            // Draw transitions and Objects layers of the map
            for (int y = 0; y < m_iScreenHeight; y++)
            {
                for (int x = 0; x < m_iScreenWidth; x++)
                {
                    Rectangle recDest = new Rectangle(((x * m_iTileWidth) + m_iScreenX) - m_iMapSubX,
                                         ((y * m_iTileHeight) + m_iScreenY) - m_iMapSubY,
                                         m_TileSets[m_iTileToDraw / 10000].m_iTileWidth,
                                         m_TileSets[m_iTileToDraw / 10000].m_iTileHeight);

                    m_iXLoc = x + m_iMapX;
                    m_iYLoc = y + m_iMapY;
                    if (m_iXLoc >= m_iMapHeight)
                    {
                        m_iXLoc -= m_iMapWidth;
                    }

                    if (m_iXLoc < 0)
                    {
                        m_iXLoc += m_iMapWidth;
                    }

                    if (m_iYLoc >= m_iMapHeight)
                    {
                        m_iYLoc -= m_iMapHeight;
                    }

                    if (m_iYLoc < 0)
                    {
                        m_iYLoc += m_iMapHeight;
                    }

                    if (m_bDrawTrans)
                    {
                        m_iTileToDraw = m_iMapTrans[m_iYLoc, m_iXLoc];

                        if (IsAnimatedTile(m_iTileToDraw))
                        {
                            m_iTileToDraw = GetAnimatedFrame(m_iTileToDraw);
                        }

                        if (m_iTileToDraw >= 0 && ((m_iTileToDraw >
                            m_TileSets[m_iTileToDraw / 10000].m_iTilesPerRow) || m_bDrawFirstRowTiles))
                        {
                            m_spriteBatch.Draw(m_TileSets[m_iTileToDraw / 10000].m_t2dTexture,
                                               recDest, GetTileRectangle(m_iTileToDraw),
                                               Color.White);
                        }
                    }

                    if (m_bDrawObj)
                    {
                        m_iTileToDraw = m_iMapObj[m_iYLoc, m_iXLoc];

                        if (IsAnimatedTile(m_iTileToDraw))
                        {
                            m_iTileToDraw = GetAnimatedFrame(m_iTileToDraw);
                        }

                        if (m_iTileToDraw >= 0 && ((m_iTileToDraw >
                            m_TileSets[m_iTileToDraw / 1000].m_iTilesPerRow) || m_bDrawFirstRowTiles))
                        {
                            m_spriteBatch.Draw(m_TileSets[m_iTileToDraw / 10000].m_t2dTexture,
                                recDest, GetTileRectangle(m_iTileToDraw),
                                Color.White);
                        }
                    }
                }
            }
            
            //m_spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            UpdateAnimationFrames();
            base.Update(gameTime);
        }

        /// <summary>
        /// draw the mini map at location (iX, iY) on the screen
        /// </summary>
        /// <param name="iX">left of mini map</param>
        /// <param name="iY">top of mini map</param>
        /// <param name="arrSprite">array of sprite in action map</param>
        public void DrawMiniMap(Sprite[] arrSprite)
        {
            int i;
            int j;                       

            for (i = 0; i < m_iMapHeight; i++)
            {
                for (j = 0; j < m_iMapWidth; j++)
                {
                    int iTile = m_iMap[i, j];

                    Rectangle rect = new Rectangle();

                    rect.Width = this.m_iMiniMapCellWidth;
                    rect.Height = this.m_iMiniMapCellHeight;
                    
                    rect.X = (int)(this.m_PosMiniMap.X + j * rect.Width);
                    rect.Y = (int)(this.m_PosMiniMap.Y + i * rect.Height);

                    //draw base
                    m_spriteBatch.Draw(m_TileSets[(iTile / 10000)].m_t2dTexture,
                                             rect,
                                             GetTileRectangle(iTile),
                                             Color.White);                                        
                }
            }

            //draw pos of array sprite

            //draw client window

        }

        public void DrawTile(SpriteBatch spriteBatch, int iTileToDraw, Rectangle rectDestination)
        {            
            Rectangle recSource = new Rectangle((iTileToDraw % m_TileSets[0].m_iTilesPerRow ) * m_iTileWidth,
                                                (iTileToDraw / m_TileSets[0].m_iTilesPerRow) * m_iTileHeight,
                                                m_iTileWidth, m_iTileHeight );
            spriteBatch.Draw(m_TileSets[0].m_t2dTexture, rectDestination, recSource, Color.White);
        }

        public enum Layer
        {
            Base,
            Trans,
            Object
        }

        public void ClearMap()
        {
            
        }
    }
}