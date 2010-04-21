using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BattleShip.Core.Sprites;

namespace BattleShip.Core.Managers
{
    class UnitManager
    {
        private Sprite[] m_SampleUnit;
        private int m_nSampleUnit;
        public UnitManager(string strXMLFile)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strXMLFile);

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
                if (this.m_SampleUnit[i].Name.Equals(strUnitName))
                {
                    return ProduceUnit(i);
                }
            }
            return null;
        }

        public Sprite ProduceUnit(int i)
        {
            if (i >= 0 && i < this.m_nSampleUnit)
            {
                return this.m_SampleUnit[i].Clone();
            }
            else
                return null;
        }
    }
}
