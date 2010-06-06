using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BattleShip.Core.Sprites;
using Microsoft.Xna.Framework.Content;

namespace BattleShip.Core.Managers
{
    public class UnitManager
    {
        static private Sprite[] m_SampleUnit;
        static private Bullet[] m_SampleBullet;
        
        private int m_nSampleUnit;
        private int m_nSampleBullet;

        public int TotalSampleUnits
        {
            get { return m_nSampleUnit; }
        }

        public int TotalSampleBullets
        {
            get { return m_nSampleBullet; }
        }

        public UnitManager(string strXMLFileSprites, string strXMLFileBullets, ContentManager contentManager)
        {
            try
            {                
                XmlDocument doc = new XmlDocument();
                
                //load all sprites
                doc.Load(strXMLFileSprites);
                XmlNodeList nodeList = doc.GetElementsByTagName("Unit");
                m_nSampleUnit = nodeList.Count;
                
                m_SampleUnit = new Sprite[m_nSampleUnit];

                for (int i = 0; i < m_nSampleUnit; i++)
                {
                    if (nodeList[i].Attributes.Count > 0 && nodeList[i].Attributes["name"].Value.CompareTo("Player") == 0)
                    {
                        m_SampleUnit[i] = new UserControlledSprite();                                              
                    }                        
                    else
                    {
                        m_SampleUnit[i] = new AutomatedSprite();
                    }
                    m_SampleUnit[i].LoadFromXml(nodeList[i], contentManager);  
                }

                //load all bullets
                doc.Load(strXMLFileBullets);
                nodeList = doc.GetElementsByTagName("Bullet");
                m_nSampleBullet = nodeList.Count;
                m_SampleBullet = new Bullet[m_nSampleBullet];

                for (int i = 0; i < m_nSampleBullet; i++)
                {
                    m_SampleBullet[i] = new Bullet();
                    m_SampleBullet[i].LoadFromXml(nodeList[i], contentManager);
                }                                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Sprite ProduceUnit(string strUnitName)
        {
            for(int i=0;i<this.m_nSampleUnit;i++)
            {
                if (m_SampleUnit[i].Name.Equals(strUnitName))
                {
                    return ProduceUnit((UnitType)i);
                }
            }
            return null;
        }

        public Sprite ProduceUnit(UnitType unitType)
        {
            int index = (int)unitType;
            if (index >= 0 && index < this.m_nSampleUnit)
            {
                return m_SampleUnit[index].Clone();
            }
            else
                return null;
        }

        public Sprite ProduceUnit(int index)
        {            
            if (index >= 0 && index < this.m_nSampleUnit)
            {
                return m_SampleUnit[index].Clone();
            }
            else
                return null;
        }
                
        public Bullet ProduceBullet(int index)
        {            
            if (index >= 0 && index < this.m_nSampleBullet)
            {
                return m_SampleBullet[index].Clone();
            }
            else
                return null;
        }

        public enum UnitType
        {
            Player = 0,
            Computer
        }
    }
}
