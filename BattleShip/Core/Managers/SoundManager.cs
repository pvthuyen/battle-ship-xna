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
        static public Song m_backMusic;
        static public Song m_startMusic;

        static public SoundEffect m_explosion;
        static public SoundEffect m_shipExplosion;

        static public SoundEffect m_doubleMachineGunFire;
        static public SoundEffect m_peashooterFire;

        static public SoundEffect m_SelectMenu;
        static public SoundEffect m_Thunder;
        
        static public SoundEffect m_powerGet;
        static public SoundEffect m_powerShow;
        static public SoundEffect m_newMeteor;
        static public SoundEffect m_menuBack;
        static public SoundEffect m_menuSelect;
        static public SoundEffect m_menuScroll;

        static public SoundEffect ShipExlosion
        {
            get { return m_shipExplosion; }
        }

        static public SoundEffect DoubleMachineGunFire 
        {
            get { return m_doubleMachineGunFire; }
        }

        static public SoundEffect PeashooterFire
        {
            get { return m_peashooterFire; }
        }

        static public SoundEffect Explosion
        {
            get { return m_explosion; }            
        }

        static public SoundEffect NewMeteor
        {
            get { return m_newMeteor; }            
        }

        static public SoundEffect MenuBack
        {
            get { return m_menuBack; }            
        }

        static public SoundEffect MenuSelect
        {
            get { return m_menuSelect; }            
        }

        static public SoundEffect MenuScroll
        {
            get { return m_menuScroll; }            
        }

        static public SoundEffect PowerGet
        {
            get { return m_powerGet; }            
        }

        static public SoundEffect PowerShow
        {
            get { return m_powerShow; }            
        }

        static public Song BackMusic
        {
            get { return m_backMusic; }            
        }

        static public Song StartMusic
        {
            get { return m_startMusic; }            
        }

        static public void LoadContent(ContentManager content)
        {            
            m_explosion = content.Load<SoundEffect>(@"Resource/Sound/RocketExplode");
            m_shipExplosion = content.Load<SoundEffect>(@"Resource/Sound/ExplodeShip");
            m_doubleMachineGunFire=content.Load<SoundEffect>(@"Resource/Sound/DoubleMachineGunFire");
            m_peashooterFire = content.Load<SoundEffect>(@"Resource/Sound/PeashooterFire");
            //m_startMusic = content.Load<Song>(@"Resource/Sound/Hawaiian_Sting");
            //m_SelectMenu = content.Load<SoundEffect>(@"Resource/Sound/challenge_menu_mouseover");
        }                
    }
}
