using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
namespace Tetris
{
    class kbd
    {
        KeyboardState State;
        double timer;
        Keys lastTriggeredKey;
        const double waitbeforespam = 300;
        const double timeBetweenSpams = 50;
        public void updateState()
        {
            State = Keyboard.GetState();
        }

        public bool isKeyHeldDown(Keys k, GameTime gameTime)
        {
            if (lastTriggeredKey == k)
            {
                if (Keyboard.GetState().IsKeyDown(k))
                {
                    timer += gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                else
                {
                    timer = 0;
                }
                if (timer > waitbeforespam)
                {
                    timer = waitbeforespam-timeBetweenSpams;
                    return true;
                }
            }            
            return false;
        }

        public bool KeyTriggered(Keys k)
        {            
            if (!State.IsKeyDown(k) && Keyboard.GetState().IsKeyDown(k))
            {
                lastTriggeredKey = k;
                return true;               
            }            
            return false;
        }
    }
}
