using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappisPlanus
{
    class Background
    {
        FlappisPlanusGame Game;
        Texture2D SkyTexture;
        Texture2D GrassTexture;

        Rectangle Rectangle;

        int[] Speeds;

        public Background(FlappisPlanusGame game, Texture2D sky, Rectangle skyRect)
        {
            Game = game;
            SkyTexture = sky;
            Rectangle = skyRect;
            
            Speeds = new int[] { -15, -17, -20 };
        }

        public void Update(double deltaTime)
        {
            Rectangle.X += Speeds[Game.Level];

            if (Rectangle.Right <= 0)
            {
                Rectangle.X = 0;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(SkyTexture, Rectangle, Color.White);
            sb.Draw(SkyTexture, new Vector2(Rectangle.X + Rectangle.Width, 0), Color.White);
            sb.End();
        }
    }
}
