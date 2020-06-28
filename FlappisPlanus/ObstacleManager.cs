using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappisPlanus
{
    public class ObstacleManager
    {
        FlappisPlanusGame Game;
        public List<Obstacle> Obstacles;
        Texture2D[,] Textures;
        int[] Speeds;
        double TimeSinceLastSpawn;
        bool LevelChanging = false;

        Random Random;

        public ObstacleManager(FlappisPlanusGame game)
        {
            Game = game;
            Random = new Random();
            Obstacles = new List<Obstacle>();

            Textures = new Texture2D[2,3]
            {
                {
                    game.Content.Load<Texture2D>("rockGrass"),
                    game.Content.Load<Texture2D>("rockSnow"),
                    game.Content.Load<Texture2D>("rockIce")
                },

                {
                    game.Content.Load<Texture2D>("rockGrassDown"),
                    game.Content.Load<Texture2D>("rockSnowDown"),
                    game.Content.Load<Texture2D>("rockIceDown")
                }                
            };

            Obstacles.Add(new Obstacle(Game.Content.Load<Texture2D>("rockGrassDown"), new Point(1000, 0), -5, 1));
            Obstacles.Add(new Obstacle(Game.Content.Load<Texture2D>("rockGrass"), new Point(700, 242), -5, 0));

            Speeds = new int[] { -5, -7, -10 };
        }

        public void Update(double deltaTime)
        {

            foreach (Obstacle obstacle in Obstacles.ToList())
            {
                obstacle.Update(deltaTime);

                if (obstacle.Collision(Game.player.Rectangle))
                {
                    Game.StopGame();
                }

                if (obstacle.Rectangle.Right < 0)
                {
                    Obstacles.Remove(obstacle);
                }
            }

            if (!LevelChanging && Obstacles.Count <= Game.Level + 1 && TimeSinceLastSpawn > 1000)
            {
                int facingDown = Random.Next(2);

                Obstacles.Add(GetObstacle(1000, facingDown));
                TimeSinceLastSpawn = 0;
            }
            else
            {
                TimeSinceLastSpawn += deltaTime;
            }

            if (Obstacles.Count == 0)
            {
                LevelChanging = false;
                Game.GoToNextLevel();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Obstacle obstacle in Obstacles)
            {
                obstacle.Draw(spriteBatch);
            }
        }

        Obstacle GetObstacle(int XCoordinate, int facingDown)
        {
            int YCoordinate = 0;

            if (facingDown == 0)
            {
                YCoordinate = 242;
            }

            return new Obstacle(Textures[facingDown, Game.Level], new Point(XCoordinate, YCoordinate), Speeds[Game.Level], facingDown);
        }

        public void StartLevelTransition()
        {
            LevelChanging = true;
        }
    }
}
