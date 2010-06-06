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
using System.IO;
using System.Text;


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

        static private List<TileSet> m_TileSets = new List<TileSet>();                 //list cac tile set
        static private List<TileAnimation> m_taAnimations = new List<TileAnimation>(); //list cac tile animation trong tile set

        //kich thuoc cua 1 tile
        static private int m_iTileWidthMap;
        static private int m_iTileHeightMap;

        static private int m_iTileWidhtWorld;
        static private int m_iTileHeightWorld;

        //kich thuoc cua map hien thi
        static private int m_iMapWidth;
        static private int m_iMapHeight;

        //du lieu cua map
        static private int[,] m_iMap;          //du lieu map o tang background
        static private int[,] m_iMapTrans;     //du lieu map o tang transition
        static private int[,] m_iMapObj;       //du lieu map o tang obj
        //static private int[,] m_iMapData;      //khu vuc di chuyen cua map

        //vi tri goc trai tren cung duoc hien thi cua map
        static private int m_iMapX;
        static private int m_iMapY;
        static private int m_iMapSubX;
        static private int m_iMapSubY;

        //kich thuoc hien thi man hinh
        static private int m_iScreenHeight;    //so luong tile theo chieu rong
        static private int m_iScreenWidth;     //so luong tile theo chieu ngang
        static private int m_iScreenX;         //vi tri x o goc trai tren
        static private int m_iScreenY;         //vi tri y o goc trai tren
                
        private bool m_bDrawBase = true;
        private bool m_bDrawTrans = true;
        private bool m_bDrawObj = true;
        private bool m_bDrawFirstRowTiles = true;

        private int m_iXLoc;
        private int m_iYLoc;
        private int m_iTileToDraw;

        static private int m_iTileCanWalk;
        //info of type of tile: start index, end index, current index (base, trans, obj)
        static private int m_iTileBaseCurrIndex;
        static private int m_iTileBaseStart;
        static private int m_iTileBaseEnd;

        static private int m_iTileTransCurrIndex;
        static private int m_iTileTransStart;
        static private int m_iTileTransEnd;

        static private int m_iTileObjCurrIndex;
        static private int m_iTileObjStart;
        static private int m_iTileObjEnd;

        static private int m_iTotalMaps;

        //information of mini map
        private Vector2 m_PosMiniMap;
        private int m_iMiniMapCellWidth;
        private int m_iMiniMapCellHeight;

        private SpriteBatch m_spriteBatch;
        private ResourceManager m_resourceManager;

        static public int TotalMaps
        {
            get { return m_iTotalMaps; }
        }

        static public int TileWidthWorld
        {
            set { m_iTileWidhtWorld = value; }
            get { return m_iTileWidhtWorld; }
        }

        static public int TileHeightWorld
        {
            set { m_iTileHeightWorld = value; }
            get { return m_iTileHeightWorld; }
        }

        public int TileBaseStartIndex
        {
            get { return m_iTileBaseStart; }
        }

        public int TileTransStartIndex
        {
            get { return m_iTileTransStart; }
        }

        public int TileObjStartIndex
        {
            get { return m_iTileObjStart; }
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

        static public int Left
        {
            get { return m_iScreenX; }
            set { m_iScreenX = value; }
        }

        static public int Top
        {
            get { return m_iScreenY; }
            set { m_iScreenY = value; }
        }

        /// <summary>
        /// so luong tile theo chieu rong cua map
        /// </summary>
        static public int ScreenWidth
        {            
            get { return m_iScreenWidth; }
            set { m_iScreenWidth = value; }
        }

        /// <summary>
        /// so luong tile theo chieu cao cua map
        /// </summary>
        static public int ScreenHeight
        {
            get { return m_iScreenHeight; }
            set { m_iScreenHeight = value; }
        }

        /// <summary>
        /// kich thuoc chieu rong cua 1 tile tinh bang pixel
        /// </summary>
        static public int TileWidthMap
        {            
            get { return m_iTileWidthMap; }
            set { m_iTileWidthMap = value; }
        }

        /// <summary>
        /// kich thuoc chieu cao cua 1 tile tinh bang pixel
        /// </summary>
        static public int TileHeightMap
        {            
            get { return m_iTileHeightMap; }
            set { m_iTileHeightMap = value; }
        }

        /// <summary>
        /// vi tri hien thi cua map theo x
        /// </summary>
        static public int MapX
        {
            get { return m_iMapX; }
            set { m_iMapX = value; }
        }

        /// <summary>
        /// vi tri hien thi cua map theo y
        /// </summary>
        static public int MapY
        {
            get { return m_iMapY; }
            set { m_iMapY = value; }
        }

        /// <summary>
        /// kich thuot hien thi theo chieu ngang
        /// </summary>
        static public int MapWidth
        {
            get { return m_iMapWidth; }
        }

        /// <summary>
        /// kich thuoc hien thi theo chieu cao
        /// </summary>
        static public int MapHeight
        {
            get { return m_iMapHeight; }
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

        /*
        static public bool CheckTileCanWalk(int iTile)
        {
            if (iTile >= m_taAnimations[0].m_iStartFrame && (iTile <= m_taAnimations[0].m_iStartFrame + m_taAnimations[0].m_iFrameCount))
                return true;
            if (iTile >= m_iTileTransStart && iTile <= m_iTileTransEnd)
                return true;
            if (iTile >= m_iTileObjStart && iTile <= m_iTileObjEnd)
                return true;
            return false;
        }
         */

        /// <summary>
        /// check map can walk???
        /// </summary>
        /// <param name="xWorld"></param>
        /// <param name="yWorld"></param>
        /// <returns></returns>
        static public bool CheckCanWalk(int xWorld, int yWorld)
        {
            int xMap = X_World_To_X_Map(xWorld);
            int yMap = Y_World_To_Y_Map(yWorld);
            int dataBase = GetMap(xMap, yMap, BattleShip.Core.Utils.Common.MapLayer.Base);
            int dataObj = GetMap(xMap, yMap, BattleShip.Core.Utils.Common.MapLayer.Object);
            int dataTrans = GetMap(xMap, yMap, BattleShip.Core.Utils.Common.MapLayer.Trans);

            if (dataBase >= m_taAnimations[0].m_iStartFrame && (dataBase <= m_taAnimations[0].m_iStartFrame + m_taAnimations[0].m_iFrameCount)
                && (dataObj < m_iTileObjStart || dataObj > m_iTileObjEnd)
                && (dataTrans < m_iTileTransStart || dataTrans > m_iTileTransEnd))
                return true;
            return false;
        }

        static public bool CheckTileObject(int iTile)
        {
            if (m_iTileObjStart <= iTile && m_iTileObjEnd >= iTile)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void LoadMapConfig(string strXmlFile)
        {
            XmlDocument mapXml = new XmlDocument();
            mapXml.Load(strXmlFile);

            //read tile set
            XmlNodeList nodeList = mapXml.GetElementsByTagName("TileSet");
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

                m_iTileHeightMap = tileSet.m_iTileHeight;
                m_iTileWidthMap = tileSet.m_iTileWidth;

                m_iTileCanWalk = int.Parse(node.Attributes["TileCanWalk"].Value);

                m_TileSets.Add(tileSet);

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

                m_taAnimations.Add(ta);
            }                        

            //read total maps
            nodeList = mapXml.GetElementsByTagName("Data");
            m_iTotalMaps = nodeList.Count;
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
                if (node.Attributes["Level"].Value.CompareTo(iLevel.ToString()) == 0)
                {
                    m_iMapWidth = int.Parse(node.Attributes["MapWidth"].Value);
                    m_iMapHeight = int.Parse(node.Attributes["MapHeight"].Value);
                    data = node.InnerText;
                    break;
                }
            }
                        
            m_iMapObj = new int[m_iMapWidth, m_iMapHeight];
            m_iMapTrans = new int[m_iMapWidth, m_iMapHeight];
            m_iMap = new int[m_iMapWidth, m_iMapHeight];

            int i, j;
            string[] arrValue = data.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int index = 0;
            for (i = 0; i < m_iMapHeight; i++)
            {
                for (j = 0; j < m_iMapWidth; j++)
                {
                    m_iMap[i, j] = int.Parse(arrValue[index++]);                    
                    m_iMapTrans[i, j] = int.Parse(arrValue[index++]);
                    m_iMapObj[i, j] = int.Parse(arrValue[index++]);
                    //m_iMapData[i, j] = int.Parse(arrValue[index++]);
                }
            }            
        }

        /// <summary>
        /// luu map vao file xml
        /// </summary>
        public void SaveMap(string strXmlFile, int iLevel)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(strXmlFile);
            XmlElement elm = doc.CreateElement("Data");
            elm.SetAttribute("Level", iLevel.ToString());
            elm.SetAttribute("MapHeight", m_iMapHeight.ToString());
            elm.SetAttribute("MapWidth", m_iMapWidth.ToString());
            StringBuilder sb = new StringBuilder();
            sb.Append("\n");
            for (int i = 0; i < m_iMapHeight; i++)
            {
                for (int j = 0; j < m_iMapWidth; j++)
                {
                    sb.Append(m_iMap[i, j]);
                    sb.Append("\n");
                    sb.Append(m_iMapTrans[i, j]);
                    sb.Append("\n");
                    sb.Append(m_iMapObj[i, j]);
                    sb.Append("\n");
                    //sb.Append(m_iMapData[i, j]);
                    //sb.Append("\n");
                }
            }
            elm.InnerText = sb.ToString();
            doc.DocumentElement.AppendChild(elm);
            doc.Save(strXmlFile);                        
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
        static public bool ScrollByPixels(int x, int y)
        {
            bool isScroll = true;

            m_iMapSubX += x;
            m_iMapSubY += y;

            while (m_iMapSubX >= m_iTileWidhtWorld)
            {
                m_iMapSubX -= m_iTileWidhtWorld;
                m_iMapX++;
            }

            while (m_iMapSubY >= m_iTileHeightWorld)
            {
                m_iMapSubY -= m_iTileHeightWorld;
                m_iMapY++;
            }

            while (m_iMapSubX < 0)
            {
                m_iMapSubX += m_iTileWidhtWorld;
                m_iMapX--;
            }

            while (m_iMapSubY < 0)
            {
                m_iMapSubY += m_iTileHeightWorld;
                m_iMapY--;
            }

            while (m_iMapX >= m_iMapWidth)
            {
                m_iMapX--;
                isScroll = false;
            }
            while (m_iMapX < 0)
            {
                m_iMapX++;
                isScroll = false;
            }
            while (m_iMapY >= m_iMapHeight)
            {
                m_iMapY--;
                isScroll = false;
            }
            while (m_iMapY < 0)
            {
                m_iMapY++;
                isScroll = false;
            }

            return isScroll;
        }
                
        static public void EditMap(int x, int y, int iBase, int iTrans, int iObj, int iData)
        {
            m_iMap[y, x] = iBase;
            m_iMapTrans[y, x] = iTrans;
            m_iMapObj[y, x] = iObj;            
        }
                
        public static void EditMap(int x, int y, int iData, BattleShip.Core.Utils.Common.MapLayer layer)
        {
            switch (layer)
            {
                case BattleShip.Core.Utils.Common.MapLayer.Base:
                    if (iData == 2)
                        iData = 108;
                    m_iMap[y, x] = iData;
                    break;
                case BattleShip.Core.Utils.Common.MapLayer.Trans:
                    m_iMapTrans[y, x] = iData;
                    break;
                case BattleShip.Core.Utils.Common.MapLayer.Object:
                    m_iMapObj[y, x] = iData;
                    break;                
                default:
                    break;
            }
        }

        static public int X_World_To_X_Map(int iX_World)
        {
            return (int)Math.Ceiling((double)( ((iX_World - m_iScreenX) / m_iTileWidhtWorld) + m_iMapX));
        }

        static public int Y_World_To_Y_Map(int iY_World)
        {
            return (int)Math.Ceiling((double) (((iY_World - m_iScreenY) / m_iTileHeightWorld) + m_iMapY));
        }

        static public int GetMap(int x, int y, BattleShip.Core.Utils.Common.MapLayer layer)
        {
            if (x >= 0 && x < m_iMapWidth && y >= 0 && y < m_iMapHeight)
            {
                switch (layer)
                {
                    case BattleShip.Core.Utils.Common.MapLayer.Base:
                        return m_iMap[y, x];
                    case BattleShip.Core.Utils.Common.MapLayer.Trans:
                        return m_iMapTrans[y, x];
                    case BattleShip.Core.Utils.Common.MapLayer.Object:
                        return m_iMapObj[y, x];                    
                    default:
                        return -1;
                }
            }
            else
                return -1;
        }

        public int GetNextTile(BattleShip.Core.Utils.Common.MapLayer layer)
        {            
            switch (layer)
            {
                case BattleShip.Core.Utils.Common.MapLayer.Base:                    
                    m_iTileBaseCurrIndex++;
                    if (m_iTileBaseCurrIndex > m_iTileBaseEnd)
                    {
                        m_iTileBaseCurrIndex = m_iTileBaseStart;
                    }
                    return m_iTileBaseCurrIndex;

                case BattleShip.Core.Utils.Common.MapLayer.Trans:                    
                    m_iTileTransCurrIndex++;
                    if (m_iTileTransCurrIndex > m_iTileTransEnd)
                    {
                        m_iTileTransCurrIndex = m_iTileTransStart;
                    }
                    return m_iTileTransCurrIndex;

                case BattleShip.Core.Utils.Common.MapLayer.Object:             
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

        public int GetPrevTile(BattleShip.Core.Utils.Common.MapLayer layer)
        {
            switch (layer)
            {
                case BattleShip.Core.Utils.Common.MapLayer.Base:
                    m_iTileBaseCurrIndex--;
                    if (m_iTileBaseCurrIndex < m_iTileBaseStart)
                    {
                        m_iTileBaseCurrIndex = m_iTileBaseEnd;
                    }
                    return m_iTileBaseCurrIndex;

                case BattleShip.Core.Utils.Common.MapLayer.Trans:
                    m_iTileTransCurrIndex--;
                    if (m_iTileTransCurrIndex < m_iTileTransStart)
                    {
                        m_iTileTransCurrIndex = m_iTileTransEnd;
                    }
                    return m_iTileTransCurrIndex;

                case BattleShip.Core.Utils.Common.MapLayer.Object:
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
            return new Rectangle(
              (iTileNumber % m_TileSets[0].m_iTilesPerRow) * m_TileSets[0].m_iTileWidth,
              (iTileNumber / m_TileSets[0].m_iTilesPerRow) * m_TileSets[0].m_iTileHeight,
              m_TileSets[0].m_iTileWidth,
              m_TileSets[0].m_iTileHeight
            );
        }

        public MapComponent(Game game, string XmlMapFile)
            : base(game)
        {
            // TODO: Construct any child components here
            m_resourceManager = game.Services.GetService(typeof(ResourceManager)) as ResourceManager;
            m_spriteBatch = game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            
            LoadMapConfig(XmlMapFile);
            LoadMap(XmlMapFile, 1);            
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
                        if (m_iXLoc >= m_iMapWidth)
                        {
                            //m_iXLoc -= m_iMapWidth;
                            continue;
                        }

                        if (m_iXLoc < 0)
                        {
                            //m_iXLoc += m_iMapWidth;
                            continue;
                        }

                        if (m_iYLoc >= m_iMapHeight)
                        {
                            //m_iYLoc -= m_iMapHeight;
                            continue;
                        }

                        if (m_iYLoc < 0)
                        {
                            //m_iYLoc += m_iMapHeight;
                            continue;
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
                            
                            m_spriteBatch.Draw(m_TileSets[0].m_t2dTexture,
                                             new Rectangle(((x * m_iTileWidhtWorld) + m_iScreenX) - m_iMapSubX,
                                             ((y * m_iTileHeightWorld) + m_iScreenY) - m_iMapSubY,
                                             m_iTileWidhtWorld,
                                             m_iTileHeightWorld),
                                             GetTileRectangle(m_iTileToDraw),
                                             Color.White);
                             
                            /*
                            m_spriteBatch.Draw(m_TileSets[0].m_t2dTexture,
                                             new Rectangle(x * m_iTileWidhtWorld, y * m_iTileHeightWorld, m_iTileWidhtWorld, m_iTileHeightWorld),
                                             GetTileRectangle(m_iTileToDraw),
                                             Color.White);
                             */
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
                    
                    Rectangle recDest = new Rectangle(((x * m_iTileWidhtWorld) + m_iScreenX) - m_iMapSubX,
                                         ((y * m_iTileHeightWorld) + m_iScreenY) - m_iMapSubY,
                                         m_iTileWidhtWorld,
                                         m_iTileHeightWorld);
                    

                    //Rectangle recDest = new Rectangle(x * m_iTileWidhtWorld, y * m_iTileHeightWorld, m_iTileWidhtWorld, m_iTileHeightWorld);

                    m_iXLoc = x + m_iMapX;
                    m_iYLoc = y + m_iMapY;
                    if (m_iXLoc >= m_iMapWidth)
                    {
                        //m_iXLoc -= m_iMapWidth;
                        continue;
                    }

                    if (m_iXLoc < 0)
                    {
                        //m_iXLoc += m_iMapWidth;
                        continue;
                    }

                    if (m_iYLoc >= m_iMapHeight)
                    {
                        //m_iYLoc -= m_iMapHeight;
                        continue;
                    }

                    if (m_iYLoc < 0)
                    {
                        //m_iYLoc += m_iMapHeight;
                        continue;
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
                            m_spriteBatch.Draw(m_TileSets[0].m_t2dTexture,
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
                            m_spriteBatch.Draw(m_TileSets[0].m_t2dTexture,
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
        public void DrawMiniMap(Sprite player, Sprite[] arrSprite)
        {
            int i;
            int j;                       
            Rectangle rect = new Rectangle();

            for (i = 0; i < m_iMapHeight; i++)
            {
                for (j = 0; j < m_iMapWidth; j++)
                {
                    int iTile = m_iMap[i, j];
                                        
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

            int x;
            int y;
            
            //draw player
            if (player!=null && player.IsVisible)
            {
                /*
                x = ((((int)player.Sprite.Position.X - Left) / TileWidth) + MapX) % m_iMapWidth;
                y = ((((int)player.Sprite.Position.Y - Top) / TileHeight) + MapY) % m_iMapHeight;
                */

                x = MapComponent.X_World_To_X_Map((int)player.MySprite.CenterPosition.X);
                y = MapComponent.Y_World_To_Y_Map((int)player.MySprite.CenterPosition.Y);
                
                //draw pos of array sprite
                rect.X = (int)(this.m_PosMiniMap.X + x * rect.Width);
                rect.Y = (int)(this.m_PosMiniMap.Y + y * rect.Height);

                m_spriteBatch.Draw(ResourceManager.imgPoint, rect, player.ColorOnMap);
            }

            if (arrSprite != null)
            {
                //draw pos of array sprite
                foreach (Sprite sprite in arrSprite)
                {
                    if (sprite.IsVisible)
                    {
                        /*
                        x = ((((int)sprite.Sprite.Position.X - Left) / TileWidth) + MapX) % m_iMapWidth;
                        y = ((((int)sprite.Sprite.Position.Y - Top) / TileHeight) + MapY) % m_iMapHeight;
                        */

                        x = MapComponent.X_World_To_X_Map((int)sprite.MySprite.CenterPosition.X);
                        y = MapComponent.Y_World_To_Y_Map((int)sprite.MySprite.CenterPosition.Y);
                        
                        //draw pos of array sprite
                        rect.X = (int)(this.m_PosMiniMap.X + x * rect.Width);
                        rect.Y = (int)(this.m_PosMiniMap.Y + y * rect.Height);

                        m_spriteBatch.Draw(ResourceManager.imgPoint, rect, sprite.ColorOnMap);
                    }
                }
            }
            //draw client window
            
        }

        public void DrawTile(SpriteBatch spriteBatch, int iTileToDraw, Rectangle rectDestination)
        {            
            Rectangle recSource = new Rectangle((iTileToDraw % m_TileSets[0].m_iTilesPerRow ) * m_iTileWidthMap,
                                                (iTileToDraw / m_TileSets[0].m_iTilesPerRow) * m_iTileHeightMap,
                                                m_iTileWidthMap, m_iTileHeightMap );
            spriteBatch.Draw(m_TileSets[0].m_t2dTexture, rectDestination, recSource, Color.White);
        }
                
        public void ClearMap()
        {        
            for(int i=0;i<m_iMapHeight;i++)
                for (int j = 0; j < m_iMapWidth; j++)
                {

                    m_iMap[i, j] = m_taAnimations[0].m_iStartFrame;
                    m_iMapTrans[i, j] = 0;
                    m_iMapObj[i, j] = 0;

                }    
        }
    }
}