using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleShip.Core.GameComponents;
using Microsoft.Xna.Framework.Graphics;
using BattleShip.Core.Managers;
using Microsoft.Xna.Framework;
using BattleShip.Core.Sprites;
using Microsoft.Xna.Framework.Input;

namespace BattleShip.Core.Managers
{
    public class InputManager
    {
        protected KeyboardState m_oldKeyboardState;
        protected KeyboardState m_keyboardState;

        protected GamePadState m_oldGamePadState;
        protected GamePadState m_gamePadState;

        protected MouseState m_mouseState;

        public KeyboardState ksKeyboardState
        {
            get { return this.m_keyboardState; }
            set { this.m_keyboardState = value; }
        }

        public MouseState msMouseState
        {
            get { return this.m_mouseState; }
            set { this.m_mouseState = value; }
        }

        public InputManager()
        {
            m_oldGamePadState = GamePad.GetState(PlayerIndex.One);
            m_oldKeyboardState = Keyboard.GetState();                        
            m_mouseState = Mouse.GetState();
        }

        /// <summary>
        /// begin handler user's input
        /// </summary>
        public void BeginHandler()
        {
            m_keyboardState = Keyboard.GetState();
            m_gamePadState = GamePad.GetState(PlayerIndex.One);
        }

        /// <summary>
        /// end handler user's input
        /// </summary>
        public void EndHandler()
        {
            m_oldGamePadState = m_gamePadState;
            m_oldKeyboardState = m_keyboardState;
        }

        #region keyboard controls        
        public bool IsKeyboardPress(Keys key)
        {
            return (m_oldKeyboardState.IsKeyDown(key) && m_keyboardState.IsKeyUp(key)); 
        }

        public bool IsKeyDown(Keys key)
        {
            return m_keyboardState.IsKeyDown(key);
        }
        #endregion

        #region gamepad controls        
        public bool IsGamePadPressDown
        {
            get
            {
                bool down = (m_oldGamePadState.DPad.Down == ButtonState.Pressed) && (m_gamePadState.DPad.Down == ButtonState.Released);             
                return down;
            }
        }
        public bool IsGamePadPressUp
        {
            get
            {
                bool up = (m_oldGamePadState.DPad.Up == ButtonState.Pressed) && (m_gamePadState.DPad.Up == ButtonState.Released);             
                return up;
            }
        }
        public bool IsGamePadPressLeft
        {
            get
            {
                bool left = (m_oldGamePadState.DPad.Left == ButtonState.Pressed) && (m_gamePadState.DPad.Left == ButtonState.Released);             
                return left;
            }
        }
        public bool IsGamePadPressRight
        {
            get
            {
                bool right = (m_oldGamePadState.DPad.Right == ButtonState.Pressed) && (m_gamePadState.DPad.Right == ButtonState.Released);                
                return right;
            }
        }
        #endregion                
    }
}
