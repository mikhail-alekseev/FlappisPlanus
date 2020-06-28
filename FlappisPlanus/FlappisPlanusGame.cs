using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace FlappisPlanus
{
    public class FlappisPlanusGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Background bg;
        public Player player;
        ObstacleManager ObstacleManager;
        public int Level;
        public readonly int MaxLevel = 2;
        public SpriteFont Font;
        public bool GameOver = false;
        int Score;
        int HighScore;
        Explosion Explosion;
        double TimeSinceLastScoreUpdate;
        Song Music;

        public double TimeSpent;

        LineHelper LineHelper;

        bool debug = false;
        
        public FlappisPlanusGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Level = 0;
            TimeSpent = 0;

            Texture2D t = new Texture2D(graphics.GraphicsDevice, 1, 1);
            t.SetData(new Color[] { Color.White });
            LineHelper = new LineHelper(t);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);


            bg = new Background(this, Content.Load<Texture2D>("background"),
                new Rectangle(0, 0, 800, 480));

            player = new Player(new List<Texture2D>() { Content.Load<Texture2D>("planeRed1"), Content.Load<Texture2D>("planeRed2"), Content.Load<Texture2D>("planeRed3") },
                new Rectangle(50, 100, 88, 73));

            Explosion = new Explosion(this);

            Font = Content.Load<SpriteFont>("Fonty");
            Music = Content.Load<Song>("audio");
            MediaPlayer.Play(Music);
            MediaPlayer.IsRepeating = true;

            ObstacleManager = new ObstacleManager(this);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            double deltaTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            Explosion.Update(deltaTime);

            if (GameOver && Explosion.Done)
            {
                NewGame();
            }

            else if (!GameOver)
            {
                bg.Update(deltaTime);
                player.Update(deltaTime);
                TimeSinceLastScoreUpdate += deltaTime;
                if (TimeSinceLastScoreUpdate > 500)
                {
                    UpdateScore();
                }
                

                ObstacleManager.Update(deltaTime);

                TimeSpent += deltaTime;

                if (Level < MaxLevel && TimeSpent > (Level + 1) * 1000 * 5)
                {
                    ObstacleManager.StartLevelTransition();
                }
            }

            base.Update(gameTime);            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            bg.Draw(spriteBatch);
            if (!GameOver)
            {
                player.Draw(spriteBatch);
            }
            

            ObstacleManager.Draw(spriteBatch);
            Explosion.Draw(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.DrawString(Font, string.Format("Level. {0}", Level), new Vector2(10, 25), Color.White);
            spriteBatch.DrawString(Font, string.Format("Score. {0}", Score), new Vector2(10, 50), Color.White);
            spriteBatch.DrawString(Font, string.Format("HiScore. {0}", HighScore), new Vector2(10, 75), Color.White);

            if (debug)
            {
                LineHelper.DrawOutline(spriteBatch, player.Rectangle, 5, Color.White);

                LineHelper.DrawLine(spriteBatch, new Vector2(player.Rectangle.X, player.Rectangle.Y), new Vector2(player.Rectangle.Right, player.Rectangle.Bottom), 3, Color.White);
                LineHelper.DrawLine(spriteBatch, new Vector2(player.Rectangle.Right, player.Rectangle.Top), new Vector2(player.Rectangle.Left, player.Rectangle.Bottom), 3, Color.White);

                foreach (Obstacle obstacle in ObstacleManager.Obstacles)
                {
                    LineHelper.DrawOutline(spriteBatch, obstacle.Rectangle, 3, Color.Blue);
                    if (obstacle.FacingDown == 1)
                    {
                        LineHelper.DrawLine(spriteBatch, new Vector2(obstacle.Rectangle.Left, obstacle.Rectangle.Top), new Vector2(obstacle.Rectangle.Left + obstacle.Rectangle.Width * 0.66f, obstacle.Rectangle.Bottom), 3, Color.Blue);
                        LineHelper.DrawLine(spriteBatch, new Vector2(obstacle.Rectangle.Right, obstacle.Rectangle.Top), new Vector2(obstacle.Rectangle.Left + obstacle.Rectangle.Width * 0.66f, obstacle.Rectangle.Bottom), 3, Color.Blue);
                    }
                    else
                    {
                        LineHelper.DrawLine(spriteBatch, new Vector2(obstacle.Rectangle.Left, obstacle.Rectangle.Bottom), new Vector2(obstacle.Rectangle.Left + obstacle.Rectangle.Width * 0.66f, obstacle.Rectangle.Top), 3, Color.Blue);
                        LineHelper.DrawLine(spriteBatch, new Vector2(obstacle.Rectangle.Right, obstacle.Rectangle.Bottom), new Vector2(obstacle.Rectangle.Left + obstacle.Rectangle.Width * 0.66f, obstacle.Rectangle.Top), 3, Color.Blue);
                    }
                    LineHelper.DrawLine(spriteBatch, new Vector2(), new Vector2(), 3, Color.Blue);
                    LineHelper.DrawLine(spriteBatch, new Vector2(obstacle.Rectangle.X, obstacle.Rectangle.Y), new Vector2(obstacle.Rectangle.Right, obstacle.Rectangle.Bottom), 3, Color.Blue);
                    LineHelper.DrawLine(spriteBatch, new Vector2(obstacle.Rectangle.Right, obstacle.Rectangle.Top), new Vector2(obstacle.Rectangle.Left, obstacle.Rectangle.Bottom), 3, Color.Blue);
                }

                spriteBatch.DrawString(Font, string.Format("Obstacles. {0}", ObstacleManager.Obstacles.Count), new Vector2(400, 25), Color.White);
                spriteBatch.DrawString(Font, string.Format("Plane. {0}", player.State), new Vector2(400, 50), Color.White);
            }

            if (GameOver)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("textGameOver"), new Vector2(200, 300), Color.White);                
            }

            spriteBatch.End();
        }

        public void NewGame()
        {
            Score = 0;
            TimeSinceLastScoreUpdate = 0;
            Level = 0;
            TimeSpent = 0;
            player.Rectangle = new Rectangle(50, 100, 88, 73);
            GameOver = false;
            ObstacleManager = new ObstacleManager(this);
        }

        public void GoToNextLevel()
        {
            Level++;
        }

        public void StopGame()
        {
            GameOver = true;
            Explosion.Position = player.Rectangle.Center.ToVector2();
            Explosion.Start();
            HighScore = Math.Max(Score, HighScore);
        }

        public void UpdateScore()
        {
            TimeSinceLastScoreUpdate = 0;
            Score += (Level + 1) * 5;
        }
    }
}
