using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BattleShip.Core.Sprites
{
    public abstract class Sprite
    {
        protected Texture2D m_TextureImage;
        protected Vector2 m_Position;
        protected Point m_FrameSize;
        protected int m_CollisionOffset;
        protected Point m_CurrentFrame;
        protected Point m_SheetSize;
        protected int m_iTimeSinceLastFrame = 0;
        protected int m_iMillisecondsPerFrame;
        protected Vector2 m_Speed;
        protected const int m_DefaultMillisecondsPerFrame = 16;
                
    }
}
