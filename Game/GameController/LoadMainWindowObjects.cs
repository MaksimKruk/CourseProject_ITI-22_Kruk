using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using ModelLibrary;

namespace GameController
{

    public class LoadMainWindowObjects
    {
        public DX2D _dx2d;

        public Sprite _firstGround, _secondGround, _finish, _firstPlayer, _secondPlayer, _background;

        public void LoadMainObjects()
        {
            int backgroundIndex = _dx2d.LoadBitmap("C://Курсовая//Sprites//background.bmp");
            _background = new Sprite(_dx2d, backgroundIndex, 0.0f, 0.0f, 0.0f);

            int firstGroundIndex = _dx2d.LoadBitmap("C://Курсовая//Sprites//ground.bmp");
            _firstGround = new Sprite(_dx2d, firstGroundIndex, 0.0f, 2.0f, 0.0f);

            int secondGroundIndex = _dx2d.LoadBitmap("C://Курсовая//Sprites//secondGround.bmp");
            _secondGround = new Sprite(_dx2d, secondGroundIndex, 0.0f, 12.5f, 0.0f);

            int bitmapIndex = _dx2d.LoadBitmap("C://Курсовая//WpfApp1//WpfApp1//BasicSprites//dash.bmp");
            _firstPlayer = new Sprite(_dx2d, bitmapIndex, 10.0f, 0.0f, 0.0f);

            int finishIndex = _dx2d.LoadBitmap("C://Курсовая//Sprites//finish.bmp");
            _finish = new Sprite(_dx2d, finishIndex, 45.0f, 7.5f, 0.0f);

            int secondPlayerIndex = _dx2d.LoadBitmap("C://Курсовая//Sprites//secondPlayer.bmp");
            _secondPlayer = new Sprite(_dx2d, secondPlayerIndex, 10.0f, 0.0f, 0.0f);
        }

    }
}
