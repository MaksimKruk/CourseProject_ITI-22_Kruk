using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;

namespace GameController
{
    public class CheckCollision
    {
        public bool IsCollision(Sprite sprite1, Sprite sprite2)
        {
            bool collisionX = sprite1.PositionOfCenter.X + sprite1._width/20 >= (sprite2.PositionOfCenter.X - sprite2._width/20) &&
            sprite2.PositionOfCenter.X + sprite2._width/20 >= (sprite1.PositionOfCenter.X - sprite1._width/20);
            bool collisionY = (sprite1.PositionOfCenter.Y + sprite1._height/20) >= (sprite2.PositionOfCenter.Y - sprite2._height/20) &&
            sprite2.PositionOfCenter.Y + sprite2._height/20 >= (sprite1.PositionOfCenter.Y - sprite2._height / 20);
            return collisionX && collisionY;
        }
    }
}
