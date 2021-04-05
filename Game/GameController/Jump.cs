using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using ModelLibrary;
using SharpDX.DirectInput;
using SharpDX;

namespace GameController
{
    public class Jump
    {
        public TimeHelper _timeHelper;
        public DInput _dInput;

        public Vector2 posPlayer;

        public Wrapper<RunnerComponent> wrapper;

        private bool _wPressed;
        private bool _aPressed;
        private bool _dPressed;
        public int cathTime;

        public void Jumping()
        {
            float dT = _timeHelper.dT;
            bool wPressed = _dInput.KeyboardState.IsPressed(Key.W);
            bool sPressed = _dInput.KeyboardState.IsPressed(Key.S);
            bool aPressed = _dInput.KeyboardState.IsPressed(Key.A);
            bool dPressed = _dInput.KeyboardState.IsPressed(Key.D);

            if (wPressed && !_wPressed)
            {
                do
                {
                    posPlayer.Y += wrapper.field.Velocity * dT + 0.5f * wrapper.field.Acceleration * (dT * dT);
                    wrapper.field.Velocity += wrapper.field.Acceleration * dT;
                }
                while (posPlayer.Y < wrapper.field.JumpHeight);
                wrapper.field.IsJumping = true;
            }
            if (wrapper.field.IsJumping == true)         
                {
                    wrapper.field.Velocity = wrapper.field.Velocity - wrapper.field.Acceleration;
                    posPlayer.Y = posPlayer.Y + wrapper.field.Velocity;
                }
            if (posPlayer.Y <= 1.4f)          
                {
                    wrapper.field.Velocity = 0;
                    posPlayer.Y = 1.3f;
                    wrapper.field.IsJumping = false;
                }
            if (aPressed && !_aPressed) posPlayer.X = (float)(posPlayer.X - 0.3);
            if (dPressed && !_dPressed) posPlayer.X = (float)(posPlayer.X + 0.3);

            _wPressed = wPressed;
            _aPressed = aPressed;
            _dPressed = dPressed;
        }

        /*
        public void JumpingFirst(bool state)
        {
            float time = _timeHelper.Time;
            float dT = _timeHelper.dT;
            bool wPressed = _dInput.KeyboardState.IsPressed(Key.W);
            bool isColade = checkCollision.IsCollision(firstPlayerWrapper.sprite, ListOfObstacles[0].sprite);
            bool isPrizeColade = checkCollision.IsCollision(firstPlayerWrapper.sprite, ListOfPrizes[0].sprite);

            if (state && !_wPressed)
            {
                do
                {
                    posFirsPlayer.Y += firstPlayerWrapper.field.Velocity * dT + 0.5f * firstPlayerWrapper.field.Acceleration * (dT * dT);
                    firstPlayerWrapper.field.Velocity += firstPlayerWrapper.field.Acceleration * dT;
                }
                while (posFirsPlayer.Y < firstPlayerWrapper.field.JumpHeight);
                firstPlayerWrapper.field.IsJumping = true;
            }
            if (firstPlayerWrapper.field.IsJumping == true)          
            {
                firstPlayerWrapper.field.Velocity = firstPlayerWrapper.field.Velocity - firstPlayerWrapper.field.Acceleration;
                posFirsPlayer.Y = posFirsPlayer.Y + firstPlayerWrapper.field.Velocity;
            }
            if (posFirsPlayer.Y <= 1.4f)          
            {
                firstPlayerWrapper.field.Velocity = 0;
                posFirsPlayer.Y = 1.3f;
                firstPlayerWrapper.field.IsJumping = false;
            }

            //минус очки если игрок попал на препятствие
            if (isColade && !_isColide) { ListOfObstacles[0].field.DoDamage(firstPlayerWrapper.field); FirstPlayerScore -= 200; }

            //навешивание декоратора и запоминание времени этого события
            if (isPrizeColade && !_isPrizeColide) { firstPlayerWrapper.field = ListOfPrizes[0].field.SetPrize(firstPlayerWrapper.field); isDecorated = true; cathTime = (int)time; przPos.X = -10; }

            //снятие декоратора спустя время
            if (isDecorated && (time - cathTime) > 15) { removeDecorations.playerWrapper = firstPlayerWrapper; removeDecorations.DeleteJumpDecoration(); isDecorated = false; }

            _isColide = isColade;
            _wPressed = wPressed;
            _isPrizeColide = isPrizeColade;
        }  */
    }
}
