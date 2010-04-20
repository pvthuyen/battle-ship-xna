using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using BattleShip;

namespace BattleShip.Core.GameStates
{
    public class BaseGameState:GameState
    {
        protected Game1 OurGame;
        protected ContentManager Content;
        public BaseGameState(Game game)
            : base(game)
        {
            Content = new ContentManager(game.Services);
            OurGame = game as Game1;
        }
    }
}
