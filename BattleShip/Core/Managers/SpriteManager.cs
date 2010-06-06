using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using BattleShip.Core.Sprites;
using BattleShip.Core.GameComponents;
using System.Xml;


namespace BattleShip.Core.Managers
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Rectangle m_PlayField;
        private Random m_Random;

        //player
        private Sprite m_Player;

        //bullets of player
        private Bullet[] m_arrBulletsPlayer;
        private int m_iTotalBullets = 10;

        //bullets of enemies
        private Bullet[] m_arrBulletsEnemies;

        //enemies
        private Sprite[] m_Enemies;
        private int m_iTotalEnemies;
        private int m_iTotalEnemiesAppear;
        private int m_iCurrentGenerateEnemies = 0;

        //bullets bonus and animate effect bonus        
        private AnimateEffect[] m_arrAnimateEffect;

        //explosions
        private AnimateEffect[] m_Explosions;
                        
        //fishs
        private AnimateEffect[] m_Fishs;
        private int m_iTotalFishs = 10;
        private float m_fTimerFish = 0;
        private float m_fTimerFishAppear = 0.0f;
        private bool bAllEnemiesDied = false;
        public Sprite[] Enemies
        {
            get { return m_Enemies; }
        }

        public Sprite Player
        {
            get { return m_Player; }
            set
            {
                m_Player = value;
                while (true)
                {
                    int x = m_Random.Next(0, m_PlayField.Width - m_Player.MySprite.Width);
                    int y = m_PlayField.Height - m_Player.MySprite.Height;
                    if (MapComponent.CheckCanWalk(x, y))
                    {
                        m_Player.Position = new Vector2(x, y);
                        break;
                    }
                }
            }
        }

        public AnimateEffect[] Fishes
        {
            get { return m_Fishs; }
        }

        public bool AllEnemiesDied
        {
            get
            {
                return bAllEnemiesDied;
            }
        }

        private bool CheckAllEnemiesDied()
        {
            foreach (Sprite sprite in m_Enemies)
            {
                if (sprite.IsActive)
                    return false;
            }
            return true;
        }

        public SpriteManager(Game game, Rectangle playField)
            : base(game)
        {
            // TODO: Construct any child components here
            spriteBatch = game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

            m_Random = new Random();
            m_PlayField = playField;
                                    
            //init enemies
            InitEnemies(1);

            //init bullets of player and enemies
            InitBullets();
                        
            //init explosions
            InitExplosions();

            //init fish            
            InitFishs();

            //init bonus
            InitBonus();
        }

        private void GenerateEnemies()
        {
            m_Enemies = new AutomatedSprite[m_iTotalEnemies];
            for (int i = 0; i < m_iTotalEnemies; i++)
            {
                m_Enemies[i] = ResourceManager.Units.ProduceUnit(m_Random.Next(1, ResourceManager.Units.TotalSampleUnits));
                m_Enemies[i].Position = GeneratePosition();
            }
            bAllEnemiesDied = false;
        }

        public void InitEnemies(int iLevel)
        {
            //read config for xml file
            XmlDocument doc = new XmlDocument();
            doc.Load(Utils.Utility.strMissionPath);

            XmlNodeList nodeList = doc.GetElementsByTagName("Mission");
            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes["Level"].Value.Equals(iLevel.ToString()))
                {
                    m_iTotalEnemiesAppear = int.Parse(node.Attributes["TotalEnemies"].Value);
                    m_iTotalEnemies = int.Parse(node.Attributes["TotalEnemiesAppearPerTimes"].Value);
                    break;
                }
            }

            m_Enemies = new AutomatedSprite[m_iTotalEnemies];
            for (int i = 0; i < m_iTotalEnemies; i++)
            {
                m_Enemies[i] = ResourceManager.Units.ProduceUnit(m_Random.Next(1, ResourceManager.Units.TotalSampleUnits));
                m_Enemies[i].Position = GeneratePosition();
            }

            m_iCurrentGenerateEnemies = 0;
            bAllEnemiesDied = false;
        }

        private void InitBullets()
        {
            m_arrBulletsPlayer = new Bullet[m_iTotalBullets];
            m_arrBulletsEnemies = new Bullet[m_iTotalBullets];

            for (int i = 0; i < m_iTotalBullets; i++)
            {
                m_arrBulletsPlayer[i] = ResourceManager.Units.ProduceBullet(2);
                m_arrBulletsEnemies[i] = ResourceManager.Units.ProduceBullet(1);
            }
        }

        public void InitExplosions()
        {
            m_Explosions = new AnimateEffect[m_iTotalEnemies + 1];
            for (int i = 0; i < m_iTotalEnemies + 1; i++)
            {
                m_Explosions[i] = new AnimateEffect(ResourceManager.imgExplosion);
                for (int j = 0; j < 8; j++)
                {
                    m_Explosions[i].MySprite.AddAnimation(j.ToString(), 0, j * 64, 64, 64, 16, 0.1f, false);
                }
            }
        }

        public void InitBonus()
        {            
            m_arrAnimateEffect = new AnimateEffect[m_iTotalEnemies];
            for (int i = 0; i < m_iTotalEnemies; i++)
            {         
                m_arrAnimateEffect[i] = new AnimateEffect();
            }
        }

        private void InitFishs()
        {
            int tileWidth = 0;
            int tileHeight = 0;
            int frameCount = 0;
            float frameLength = 0;
            m_Fishs = new AnimateEffect[m_iTotalFishs];
            for (int i = 0; i < m_iTotalFishs; i++)
            {
                if (i % 2 == 1)
                {
                    m_Fishs[i] = new AnimateEffect(ResourceManager.imgFish);
                    tileHeight = 30;
                    tileWidth = 30;
                    frameCount = 10;
                    frameLength = 0.4f;
                }
                else
                {
                    m_Fishs[i] = new AnimateEffect(ResourceManager.imgJumpingFish);
                    tileHeight = 32;
                    tileWidth = 32;
                    frameCount = 12;
                    frameLength = 0.2f;
                }
                for (int j = 0; j < 8; j++)
                {
                    m_Fishs[i].MySprite.AddAnimation(j.ToString(), 0, j * tileHeight, tileWidth, tileHeight, frameCount, frameLength, false);
                }
            }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            DrawFishs();

            //draw player
            m_Player.Draw(spriteBatch);

            DrawEnemies();

            DrawBullets();

            DrawExplosions();

            DrawBonus();                        
        }

        private void DrawBonus()
        {
            //draw bonus
            for (int i = 0; i < m_iTotalEnemies; i++)
            {
                if (m_arrAnimateEffect[i].IsActive)
                {
                    m_arrAnimateEffect[i].Draw(spriteBatch);
                }
            }
        }

        private void DrawBullets()
        {
            //draw bullets
            int i;
            for (i = 0; i < m_iTotalBullets; i++)
            {
                if (m_arrBulletsEnemies[i].IsActive)
                    m_arrBulletsEnemies[i].Draw(spriteBatch);
                if (m_arrBulletsPlayer[i].IsActive)
                    m_arrBulletsPlayer[i].Draw(spriteBatch);
            }
        }

        private void DrawEnemies()
        {
            //draw enemies
            foreach (AutomatedSprite enemy in m_Enemies)
            {
                if (enemy.IsActive)
                {
                    enemy.Draw(spriteBatch);
                }
            }

        }

        private void DrawFishs()
        {
            //draw fish
            foreach (AnimateEffect fish in m_Fishs)
            {
                if (fish.IsActive)
                    fish.Draw(spriteBatch);
            }
        }

        private void DrawExplosions()
        {
            //draw explosions
            for (int i = 0; i < m_iTotalEnemies + 1; i++)
            {
                if (m_Explosions[i].IsActive)
                    m_Explosions[i].Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //update player
            UserControlledSprite player = m_Player as UserControlledSprite;
            m_Player.Update(gameTime, m_PlayField);
            if (m_Player.IsAttack)
            {                
                if (player.BulletUsing.NumBullet > 0)
                {
                    FireBullet(m_Player, m_Player.MySprite.X, m_Player.MySprite.Y, m_Player.SpriteDirection);
                    player.BulletUsing.NumBullet --;
                }
            }            
            if (player.IsSwitchBullet)
            {
                player.InitNextBullet(m_arrBulletsPlayer);
            }

            switch (m_Player.ScrollMap)
            {
                case BattleShip.Core.Utils.Common.Direction.Left:
                    if (MapComponent.ScrollByPixels(-MapComponent.TileWidthWorld, 0))
                        UpdatePositionAllSprites(MapComponent.TileWidthWorld, 0);
                    break;
                case BattleShip.Core.Utils.Common.Direction.Right:
                    if (MapComponent.ScrollByPixels(MapComponent.TileWidthWorld, 0))
                        UpdatePositionAllSprites(-MapComponent.TileWidthWorld, 0);
                    break;
                case BattleShip.Core.Utils.Common.Direction.Up:
                    if (MapComponent.ScrollByPixels(0, -MapComponent.TileHeightWorld))
                        UpdatePositionAllSprites(0, MapComponent.TileHeightMap);
                    break;
                case BattleShip.Core.Utils.Common.Direction.Down:
                    if (MapComponent.ScrollByPixels(0, MapComponent.TileHeightWorld))
                        UpdatePositionAllSprites(0, -MapComponent.TileHeightMap);
                    break;
                case BattleShip.Core.Utils.Common.Direction.None:
                    break;
                default:
                    break;
            }
                                    
            //update enemies
            int i;
            int j;
            for (i = 0; i < m_Enemies.Length - 1; i++)
            {
                for (j = i + 1; j < m_Enemies.Length; j++)
                {
                    if (m_Enemies[i].IsVisible && m_Enemies[j].IsVisible && m_Enemies[i].BoundingBox.Intersects(m_Enemies[j].BoundingBox))
                    {
                        AutomatedSprite tmp1 = (AutomatedSprite)m_Enemies[i];
                        tmp1.ChangeWay(m_PlayField);

                        AutomatedSprite tmp2 = (AutomatedSprite)m_Enemies[j];
                        tmp2.ChangeWay(m_PlayField);
                    }
                }
            }

            foreach (AutomatedSprite enemy in m_Enemies)
            {
                if (enemy.IsActive)
                {
                    if(m_Player.IsActive && enemy.BoundingBox.Intersects(m_Player.BoundingBox))
                    {
                        AutomatedSprite tmp2 = (AutomatedSprite)enemy;
                        tmp2.ChangeWay(m_PlayField);
                    }
                    enemy.Update(gameTime, m_PlayField);
                    if (enemy.IsAttack)
                    {
                        FireBullet(enemy, enemy.MySprite.X, enemy.MySprite.Y, enemy.SpriteDirection);
                    }
                }
            }

            //update explosions
            UpdateExplosions(gameTime);

            //update bullets            
            UpdateBullets(gameTime);

            CheckBulletsHits();

            float ellapse = (float)gameTime.ElapsedGameTime.TotalSeconds;
            m_fTimerFish += ellapse;
            if (m_fTimerFish > m_fTimerFishAppear)
            {
                m_fTimerFish = 0;
                GenerateFishs();
            }

            UpdateFishs(gameTime);

            //UpdateBulletsBonus(gameTime);

            UpdateBonus(gameTime);

            CheckPlayerGetBonus();

            //check if player is died
            if (!player.IsVisible && player.TotalLives >= 0)
            {
                player.NewLive();
                if (player.TotalLives != -1)
                {                    
                    while (true)
                    {
                        int x = m_Random.Next(0, m_PlayField.Width - player.MySprite.Width);
                        int y = m_PlayField.Height - player.MySprite.Height;
                        if (MapComponent.CheckCanWalk(x, y))
                        {
                            player.Position = new Vector2(x, y);
                            break;
                        }
                    }
                }
                else
                    player.IsActive = player.IsVisible = false;
            }
                        
            //generate enemies if they died
            if (m_iCurrentGenerateEnemies < m_iTotalEnemiesAppear && CheckAllEnemiesDied())
            {
                GenerateEnemies();
                m_iCurrentGenerateEnemies++;
            }
            if (m_iCurrentGenerateEnemies >= m_iTotalEnemiesAppear && CheckAllEnemiesDied())
            {
                bAllEnemiesDied = true;
            }
        }

        private void UpdateBonus(GameTime gameTime)
        {
            //update bullets bonus
            for (int i = 0; i < m_iTotalEnemies; i++)
            {
                if (m_arrAnimateEffect[i].IsActive)
                {
                    m_arrAnimateEffect[i].Update(gameTime);
                }
            }
        }

        private void CheckPlayerGetBonus()
        {            
            foreach (AnimateEffect effect in m_arrAnimateEffect)
            {
                if (effect.IsActive && Intersects(effect.CollisionBox, m_Player.CollisionBox))
                {
                    if (effect.Name.Equals("star"))
                    {
                        (m_Player as UserControlledSprite).TotalStars += effect.Point;
                    }
                    else
                    {
                        (m_Player as UserControlledSprite).AddBullet(effect as Bullet);                                             
                    }

                    effect.IsActive = false;
                    effect.IsVisible = false;   
                    //play sound get bonus
                    //.....
                }
            }
        }

        private void UpdateBullets(GameTime gameTime)
        {
            for (int i = 0; i < m_iTotalBullets; i++)
            {
                if (m_arrBulletsEnemies[i].IsActive)
                    m_arrBulletsEnemies[i].Update(gameTime);
                if (m_arrBulletsPlayer[i].IsActive)
                    m_arrBulletsPlayer[i].Update(gameTime);
            }
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            foreach (AnimateEffect explosion in m_Explosions)
            {
                if (explosion.IsActive)
                    explosion.Update(gameTime);
            }
        }
               
        private void UpdateFishs(GameTime gameTime)
        {
            //update fishs
            foreach (AnimateEffect fish in m_Fishs)
            {
                if (fish.IsActive)
                    fish.Update(gameTime);
            }
        }

        private void UpdatePositionAllSprites(int x, int y)
        {
            foreach (Sprite enemy in m_Enemies)
            {
                enemy.Position += new Vector2(x, y);
            }

            foreach (AnimateEffect fish in m_Fishs)
            {
                fish.Position += new Vector2(x, y);
            }
        }

        private void GenerateFishs()
        {
            int numFishActive = m_Random.Next(m_iTotalFishs / 2, m_iTotalFishs);
            int count = 0;
                      
            int xWorld;
            int yWorld;

            foreach (AnimateEffect fish in m_Fishs)
            {
                if (!fish.IsActive)
                {
                    fish.MySprite.CurrentAnimation = m_Random.Next(0, 7).ToString();

                    xWorld = GenerateX();
                    yWorld = GenerateY();

                    if (MapComponent.CheckCanWalk(xWorld,yWorld))
                        /*
                        && MapComponent.CheckCanWalk(MapComponent.GetMap(xMap + 1, yMap, BattleShip.Core.Utils.Common.MapLayer.Base))
                        && MapComponent.CheckCanWalk(MapComponent.GetMap(xMap - 1, yMap, BattleShip.Core.Utils.Common.MapLayer.Base))
                        && MapComponent.CheckCanWalk(MapComponent.GetMap(xMap, yMap + 1, BattleShip.Core.Utils.Common.MapLayer.Base))
                        && MapComponent.CheckCanWalk(MapComponent.GetMap(xMap, yMap - 1, BattleShip.Core.Utils.Common.MapLayer.Base))
                        && MapComponent.CheckCanWalk(MapComponent.GetMap(xMap + 1, yMap + 1, BattleShip.Core.Utils.Common.MapLayer.Base))
                        && MapComponent.CheckCanWalk(MapComponent.GetMap(xMap - 1, yMap - 1, BattleShip.Core.Utils.Common.MapLayer.Base))
                        && MapComponent.CheckCanWalk(MapComponent.GetMap(xMap + 1, yMap - 1, BattleShip.Core.Utils.Common.MapLayer.Base))
                        && MapComponent.CheckCanWalk(MapComponent.GetMap(xMap - 1, yMap + 1, BattleShip.Core.Utils.Common.MapLayer.Base))
                        )*/
                    {
                        fish.Activate(xWorld, yWorld);
                    }
                    count++;
                }
                if (count >= numFishActive)
                    return;

            }
        }

        private void CheckBulletsHits()
        {
            //check bullet hits player
            int i, j;
            for (i = 0; i < m_iTotalBullets; i++)
            {
                if (m_arrBulletsEnemies[i].IsActive)
                {
                    if (Intersects(m_arrBulletsEnemies[i].CollisionBox, m_Player.CollisionBox))
                    {
                        //play sound
                        SoundManager.m_explosion.Play();

                        //draw explosion
                        m_Explosions[m_iTotalEnemies].Activate(m_Player.MySprite.X, m_Player.MySprite.Y);
                        m_Explosions[m_iTotalEnemies].MySprite.CurrentAnimation = m_Random.Next(0, 7).ToString();

                        m_Player.CurrHP -= m_arrBulletsEnemies[i].Attack;
                        m_Player.IsCollision = true;

                        m_arrBulletsEnemies[i].IsActive = false;
                        m_arrBulletsEnemies[i].IsVisible = false;
                    }
                }
            }


            for (j = 0; j < m_iTotalEnemies; j++)
            {
                if (m_Enemies[j].IsActive)
                {
                    for (i = 0; i < m_iTotalBullets; i++)
                    {
                        if (m_arrBulletsPlayer[i].IsActive)
                        {
                            if (Intersects(m_arrBulletsPlayer[i].CollisionBox, m_Enemies[j].CollisionBox))
                            {
                                //draw explosion
                                m_Explosions[j].Activate(m_Enemies[j].MySprite.X, m_Enemies[j].MySprite.Y);
                                m_Explosions[j].MySprite.CurrentAnimation = m_Random.Next(0, 7).ToString();

                                m_Enemies[j].CurrHP -= m_arrBulletsEnemies[i].Attack;
                                m_Player.Point += m_Enemies[j].Point;
                                m_Enemies[j].IsCollision = true;

                                m_arrBulletsPlayer[i].IsActive = false;
                                m_arrBulletsPlayer[i].IsVisible = false;

                                //generate bonus if enemy is died
                                if (m_Enemies[j].CurrHP < 0)
                                {                                    
                                    GenerateAnimateEffectBonus(m_Enemies[j].Position);
                                    SoundManager.m_shipExplosion.Play();
                                }
                                //play sound
                                SoundManager.m_explosion.Play();
                            }
                        }
                    }
                }
            }
        }           
    
        private void GenerateAnimateEffectBonus(Vector2 position)
        {
            for (int i = 0; i < m_iTotalEnemies; i++)
            {
                if (!m_arrAnimateEffect[i].IsVisible)
                {                                  
                    switch (m_Random.Next(0, 2))
                    {
                        case 0:                            
                            m_arrAnimateEffect[i] = new AnimateEffect(ResourceManager.imgStar);
                            m_arrAnimateEffect[i].MySprite.AddAnimation("star_appear", 0, 0, 25, 19, 9, 0.2f, true);
                            m_arrAnimateEffect[i].Name = "star";
                            m_arrAnimateEffect[i].IsLoop = true;
                            m_arrAnimateEffect[i].Point = 1;
                            break;
                        case 1:
                            m_arrAnimateEffect[i] = ResourceManager.Units.ProduceBullet(m_Random.Next(0, ResourceManager.Units.TotalSampleBullets - 1));
                            break;
                        default:
                            return;                            
                    }

                    m_arrAnimateEffect[i].Position = new Vector2(position.X, position.Y);
                    m_arrAnimateEffect[i].TimeToLive = 4.5f;
                    m_arrAnimateEffect[i].IsVisible = true;
                    m_arrAnimateEffect[i].IsActive = true;                    
                    return;
                }
            }
        }
               
        protected bool Intersects(Rectangle rectA, Rectangle rectB)
        {
            // Returns True if rectA and rectB contain any overlapping points
            return (rectA.Right > rectB.Left && rectA.Left < rectB.Right &&
                    rectA.Bottom > rectB.Top && rectA.Top < rectB.Bottom);
        }

        private void FireBullet(Sprite sprite, int x, int y, BattleShip.Core.Utils.Common.Direction direction)
        {
            if (sprite is UserControlledSprite)
            {
                foreach (Bullet bullet in m_arrBulletsPlayer)
                {
                    if (!bullet.IsActive)
                    {
                        bullet.Fire(x, y, direction, m_PlayField);
                        return;
                    }
                }
            }
            else
            {
                foreach (Bullet bullet in m_arrBulletsEnemies)
                {
                    if (!bullet.IsActive)
                    {
                        bullet.Fire(x, y, direction, m_PlayField);
                        return;
                    }
                }
            }
        }               

        private int GenerateX()
        {
            return m_Random.Next(m_PlayField.Width);
        }

        private int GenerateY()
        {
            return m_Random.Next(m_PlayField.Height);
        }

        private Vector2 GeneratePosition()
        {
            Vector2 v = new Vector2();

            v.X = m_Random.Next(m_PlayField.Width);
            v.Y = -100;

            return v;
        }
    }
}
