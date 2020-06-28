using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappisPlanus
{
    public class LineHelper
    {
        Texture2D Texture;

        public LineHelper(Texture2D dotTexture)
        {
            Texture = dotTexture;
        }

        public void DrawOutline(SpriteBatch spriteBatch, Rectangle rect, int thickness, Color color)
        {
            spriteBatch.Draw(Texture, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            spriteBatch.Draw(Texture, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), color);
            spriteBatch.Draw(Texture, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            spriteBatch.Draw(Texture, new Rectangle(rect.Right, rect.Y, thickness, rect.Height), color);
        }

        public void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, int thickness, Color color)
        {
            Vector2 diff = end - start;

            float angle = (float)Math.Atan2(diff.Y, diff.X);

            spriteBatch.Draw(Texture, new Rectangle((int)start.X, (int)start.Y, (int)diff.Length(), thickness), null, color, angle, new Vector2(0, 0), SpriteEffects.None, 0f);
        }

        static public bool LineIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            float Ua = ((p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X));
            float Ub = ((p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X));

            float denominator = (p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y);

            if (Math.Abs(denominator) <= 0.00001f)
            {
                return false;
            }

            Ua /= denominator;
            Ub /= denominator;


            if ((Ua >= 0 && Ua <= 1) && (Ub >= 0 && Ub <= 1))
            {
                return true;
            }

            return false;
            
        }
    }
}
