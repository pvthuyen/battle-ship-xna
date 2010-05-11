using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace BattleShip.Core.Managers
{
    public class SoundManager
    {
        private SoundEffect m_explosion;

        public SoundEffect Explosion
        {
            get { return m_explosion; }            
        }
        private SoundEffect m_newMeteor;

        public SoundEffect NewMeteor
        {
            get { return m_newMeteor; }            
        }
        private SoundEffect m_menuBack;

        public SoundEffect MenuBack
        {
            get { return m_menuBack; }            
        }
        private SoundEffect m_menuSelect;

        public SoundEffect MenuSelect
        {
            get { return m_menuSelect; }            
        }
        private SoundEffect m_menuScroll;

        public SoundEffect MenuScroll
        {
            get { return m_menuScroll; }            
        }
        private SoundEffect m_powerGet;

        public SoundEffect PowerGet
        {
            get { return m_powerGet; }            
        }
        private SoundEffect m_powerShow;

        public SoundEffect PowerShow
        {
            get { return m_powerShow; }            
        }
        private Song m_backMusic;

        public Song BackMusic
        {
            get { return m_backMusic; }            
        }
        private Song m_startMusic;

        public Song StartMusic
        {
            get { return m_startMusic; }            
        }

        public void LoadContent(ContentManager content)
        {
            /*
            m_explosion = content.Load<SoundEffect>("explosion");
             */
        }
    }
}
