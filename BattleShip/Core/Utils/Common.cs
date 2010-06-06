using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleShip.Core.Utils
{
    public class Common
    {
        public enum Direction
        {
            Left = 1,
            Right,
            Up,
            Down,
            None
        }

        public enum MapLayer
        {
            Base = 1,
            Trans,
            Object
            //Walkable
        }
    }
}
