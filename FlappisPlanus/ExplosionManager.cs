using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappisPlanus
{
    public class Explosion
    {
        FlappisPlanusGame Game;
        public bool Done = true;
        List<Texture2D> Frames;
        public Vector2 Position = new Vector2(0, 0);
        int CurrentFrame;
        SoundEffect SoundEffect;

        public Explosion(FlappisPlanusGame game)
        {
            Frames = new List<Texture2D>(9);

            Game = game;

            SoundEffect = Game.Content.Load<SoundEffect>("boom");

            for (int i = 0; i < 9; i++)
            {
                string name = "regularExplosion0" + i.ToString();
                Frames.Add(Game.Content.Load<Texture2D>(name));
            }
        }

        public void Update(double deltaTime)
        {
            if (Done)
            {
                return;
            }

            CurrentFrame++;
            if (CurrentFrame >= Frames.Count)
            {
                Stop();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (!Done)
            {
                spriteBatch.Draw(Frames[CurrentFrame], Position, Color.White);
            }
            
            spriteBatch.End();
        }

        public void Start()
        {
            SoundEffect.CreateInstance().Play();
            CurrentFrame = 0;
            Done = false;
        }

        public void Stop()
        {
            Done = true;
        }
    }
}
