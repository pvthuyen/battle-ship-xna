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
            set { m_explosion = value; }
        }
        private SoundEffect m_newMeteor;

        public SoundEffect NewMeteor
        {
            get { return m_newMeteor; }
            set { m_newMeteor = value; }
        }
        private SoundEffect m_menuBack;

        public SoundEffect MenuBack
        {
            get { return m_menuBack; }
            set { m_menuBack = value; }
        }
        private SoundEffect m_menuSelect;

        public SoundEffect MenuSelect
        {
            get { return m_menuSelect; }
            set { m_menuSelect = value; }
        }
        private SoundEffect m_menuScroll;

        public SoundEffect MenuScroll
        {
            get { return m_menuScroll; }
            set { m_menuScroll = value; }
        }
        private SoundEffect m_powerGet;

        public SoundEffect PowerGet
        {
            get { return m_powerGet; }
            set { m_powerGet = value; }
        }
        private SoundEffect m_powerShow;

        public SoundEffect PowerShow
        {
            get { return m_powerShow; }
            set { m_powerShow = value; }
        }
        private Song m_backMusic;

        public Song BackMusic
        {
            get { return m_backMusic; }
            set { m_backMusic = value; }
        }
        private Song m_startMusic;

        public Song StartMusic
        {
            get { return m_startMusic; }
            set { m_startMusic = value; }
        }

        public void LoadContent(ContentManager content)
        {
            /*
            m_explosion = content.Load<SoundEffect>("explosion");
             */
        }
    }
}
