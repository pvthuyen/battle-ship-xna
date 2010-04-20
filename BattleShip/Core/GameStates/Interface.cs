using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleShip.Core.GameStates
{
    public interface IGameState
    {
        GameState Value { get; }
    }

    public interface IGameStateManager
    {
        event EventHandler OnStateChange;
        GameState State { get; }
        void PopState();
        void PushState(GameState state);
        bool ContainsState(GameState state);
        void ChangeState(GameState newState);
    }
}
