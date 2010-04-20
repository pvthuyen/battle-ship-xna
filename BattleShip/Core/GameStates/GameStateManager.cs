using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleShip.Core.GameStates
{
    public class GameStateManager: GameComponent, IGameStateManager
    {
        private Stack<GameState> states = new Stack<GameState>();
        public event EventHandler OnStateChange;
        private int initialDrawOrder = 1000;
        private int drawOrder;

        public GameStateManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IGameStateManager), this);
            drawOrder = initialDrawOrder;
        }

        public void PopState()
        {
            RemoveState();
            drawOrder -= 100;

            if (OnStateChange != null)
                OnStateChange(this, null);
        }

        private void RemoveState()
        {
            GameState oldState = states.Peek() as GameState;
            //unregister event for this state
            OnStateChange -= oldState.StateChange;
            //remove the state from out game components
            Game.Components.Remove(oldState.Value);

            states.Pop();
        }

        public void PushState(GameState newState)
        {
            drawOrder += 100;
            newState.DrawOrder = drawOrder;

            AddState(newState);

            if (OnStateChange != null)
            {
                OnStateChange(this, null);
            }
        }

        private void AddState(GameState newState)
        {
            states.Push(newState);

            Game.Components.Add(newState);

            //register event
            OnStateChange += newState.StateChange;
        }

        public void ChangeState(GameState newState)
        {
            while (states.Count > 0)
            {
                RemoveState();
            }

            newState.DrawOrder = drawOrder = initialDrawOrder;
            AddState(newState);

            if (OnStateChange != null)
                OnStateChange(this, null);
        }

        public bool ContainsState(GameState state)
        {
            return (states.Contains(state));
        }

        public GameState State
        {
            get { return (states.Peek()); }
        }
    }
}
