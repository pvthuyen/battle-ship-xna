using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace BattleShip.Core.GameStates
{
    public interface ITitleIntroState : IGameState { }
    public interface IStartMenuState : IGameState { }
    public interface IOptionsMenuState : IGameState { }
    public interface IPlayingState : IGameState { }
    public interface IPausedState : IGameState { }
    public interface ILostGameState : IGameState { }
    public interface IWonGameState : IGameState { }
    public interface IStartLevelState : IGameState { }
    public interface IYesNoDialogState : IGameState { }

    public interface IFadingState : IGameState
    {
        Color Color { get; set; }
    }
}
