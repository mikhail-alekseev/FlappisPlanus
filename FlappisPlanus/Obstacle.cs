using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappisPlanus
{
    public class Obstacle
    {
        private Texture2D Texture;
        public Rectangle Rectangle;
        public int FacingDown;
        private int Speed;

        public Obstacle(Texture2D texture, Point position, int speed, int facingDown)
        {
            Texture = texture;
            Rectangle = new Rectangle(position.X, position.Y, texture.Width, texture.Height);
            Speed = speed;
            FacingDown = facingDown;
        }

        public void Update(double deltaTime)
        {
            Rectangle.X += Speed;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Texture, Rectangle, Color.White);
            sb.End();
        }

        public bool Collision(Rectangle rect)
        {
/*            if (!Rectangle.Intersects(rect))
            {
                return false;
            }*/

            Vector2 a1, a2, b1, b2, c1, c2;

            if (FacingDown == 1)
            {
                a1 = new Vector2(Rectangle.Left, Rectangle.Top);
                a2 = new Vector2(Rectangle.Right, Rectangle.Top);

                b1 = new Vector2(Rectangle.Left, Rectangle.Top);
                b2 = new Vector2(Rectangle.Left + Rectangle.Width * 0.66f, Rectangle.Bottom);

                c1 = new Vector2(Rectangle.Right, Rectangle.Top);
                c2 = new Vector2(Rectangle.Left + Rectangle.Width * 0.66f, Rectangle.Bottom);
            }

            else
            {
                a1 = new Vector2(Rectangle.Left, Rectangle.Bottom);
                a2 = new Vector2(Rectangle.Right, Rectangle.Bottom);

                b1 = new Vector2(Rectangle.Left, Rectangle.Bottom);
                b2 = new Vector2(Rectangle.Left + Rectangle.Width * 0.66f, Rectangle.Top);

                c1 = new Vector2(Rectangle.Right, Rectangle.Bottom);
                c2 = new Vector2(Rectangle.Left + Rectangle.Width * 0.66f, Rectangle.Top);
            }

            List<Vector2[]> lines = new List<Vector2[]>() { new Vector2[] { a1, a2 }, new Vector2[] { b1, b2 }, new Vector2[] { c1, c2 } };

            Vector2 d1 = new Vector2(rect.Left, rect.Top);
            Vector2 d2 = new Vector2(rect.Left, rect.Bottom);
            Vector2 d3 = new Vector2(rect.Right, rect.Bottom);
            Vector2 d4 = new Vector2(rect.Right, rect.Top);

            foreach (Vector2[] vector2s in lines)
            {
                Vector2 first = vector2s[0];
                Vector2 second = vector2s[1];

                if (LineHelper.LineIntersect(first, second, d1, d2))
                {
                    return true;
                }
                else if (LineHelper.LineIntersect(first, second, d2, d3))
                {
                    return true;
                }
                else if (LineHelper.LineIntersect(first, second, d3, d4))
                {
                    return true;
                }
                else if (LineHelper.LineIntersect(first, second, d1, d4))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
