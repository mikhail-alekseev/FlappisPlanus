using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappisPlanus
{
    public class Player
    {
        public enum PlaneState
        {
            Flying,
            Jumping,
            Dead
        }

        public Rectangle Rectangle;
        private List<Texture2D> Frames;
        private int CurrentFrame = 0;

        int YVelocity = 5;

        public PlaneState State;

        public Player(List<Texture2D> frames, Rectangle rect)
        {
            Rectangle = rect;

            Frames = new List<Texture2D>(frames.Count);

            foreach (Texture2D texture in frames)
            {
                Frames.Add(texture);
            }

            State = PlaneState.Flying;
        }

        public void Jump()
        {
            State = PlaneState.Jumping;
            YVelocity = -15;
        }

        public void Kill()
        {
            State = PlaneState.Dead;
        }

        public bool IsAlive()
        {
            return State != PlaneState.Dead;
        }

        public void Update(double deltaTime)
        {
            if (State != PlaneState.Dead)
            {
                CurrentFrame = (CurrentFrame + 1) % Frames.Count;

                if (Keyboard.GetState().IsKeyDown(Keys.J))
                {
                    Jump();                    
                }
            }

            if (State == PlaneState.Jumping)
            {
                YVelocity += 2;

                if (YVelocity >= 5)
                {
                    State = PlaneState.Flying;
                }
            }

            Rectangle.Y += YVelocity;

            if (Rectangle.Y < 0)
            {
                Rectangle.Y = 0;
            }

            else if (Rectangle.Bottom > 480)
            {
                Rectangle.Y = 480 - Rectangle.Height;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Frames[CurrentFrame], Rectangle, Color.White);
            sb.End();
        }
    }
}
