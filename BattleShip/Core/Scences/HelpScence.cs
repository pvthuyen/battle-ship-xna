using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BattleShip.Core.GameComponents;

namespace BattleShip.Core.Scences
{
    public class HelpScence : GameScence
    {
        public HelpScence(Game game, Texture2D imgBackground, Texture2D imgForeground)
            : base(game)
        {            
            this.m_lstGameComponent.Add(new ImageComponent(game, imgForeground, ImageComponent.DrawMode.Stretch));
        }
    }
}
