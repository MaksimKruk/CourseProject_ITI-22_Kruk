using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Windows;
using SharpDX.DirectInput;
using System.Windows.Input;
using Key = SharpDX.DirectInput.Key;
using System.Windows.Forms;
using ModelLibrary;
using System.Windows.Controls;
using GameController;
using GameEngine;
using System.Threading;
using Timer = System.Windows.Forms.Timer;
using System.Diagnostics;

namespace WpfApp1
{
    class Game : IDisposable
    {
        //1
        /// <summary>
        /// Сделал класс спрайта и дх2д публичными.
        /// </summary>
        // Для расчета коэффициента масштабирования принимаем, что вся область окна по высоте вмещает 20 единиц длинны виртуального игрового пространства
        private static float _unitsPerHeight = 22.0f;
        public static float UnitsPerHeight { get => _unitsPerHeight; }

        // Окно программы
        private RenderControl _renderControl;
        public RenderControl RenderControl { get => _renderControl; }

        // Инфраструктурные объекты
        private DX2D _dx2d;
        public DX2D DX2D { get => _dx2d; }
        private DInput _dInput;
        private DInput _dIn;

        // Клиетская область порта отрисовки в устройство-независимых пикселях
        private RectangleF _clientRect;

        // Коэффициент масштабирования
        private float _scale;
        public float Scale { get => _scale; }

        // Помощник для работы со временем
        private TimeHelper _timeHelper;

        ///Спрайты
        private Sprite _firstGround, _secondGround, _finish, _firstPlayer, _secondPlayer, _background;

        List<Wrapper<Obstacle>> firstListOfObstacles = new List<Wrapper<Obstacle>>();
        List<Wrapper<Prize>> firstListOfPrizes = new List<Wrapper<Prize>>();

        List<Wrapper<Obstacle>> secondListOfObstacles = new List<Wrapper<Obstacle>>();
        List<Wrapper<Prize>> secondListOfPrizes = new List<Wrapper<Prize>>();

        private Obstacle finish;

        #region Example
        public Wrapper<RunnerComponent> firstPlayerWrapper, secondPlayerWrapper;
        public Wrapper<Obstacle> obstacleWrapper, firstObsOnScreen, finishWrapper, secondObsOnScreen;
        public Wrapper<Prize> prizeWrapper, firstPrzOnScreen, secondPrzOnScreen;

        public GameStop gameStop = new GameStop();
        public CheckCollision checkCollision = new CheckCollision();

        public RunnerComponent runnerComponent;
        public RunnerComponent secondRunnerComponent;

        public PrizeGenerate firstPrizeGenerate = new PrizeGenerate();
        public ObstacleGenerate firstObstacleGenerate = new ObstacleGenerate();

        public SecondPrizeGenerate secondPrizeGenerate = new SecondPrizeGenerate();
        public SecondObstacleGenerate secondObstacleGenerate = new SecondObstacleGenerate();

        public GenerateObjectsTimer firstObjectsTimer = new GenerateObjectsTimer();
        public GenerateObjectsTimer secondObjectsTimer = new GenerateObjectsTimer();

        public IncreaseJumpDecorator increaseJumpDecorator;
        public RemoveDecorations removeDecorations = new RemoveDecorations();
        public LoadMainWindowObjects windowObjects = new LoadMainWindowObjects();

        public Vector2 firstObsPos, posFirsPlayer, firstPrzPos, posSecondPlayer, secondPrzPos, secondObsPos;
        #endregion

        // Была ли нажата W в прошлом кадре
        private bool _wPressed;
        private bool _upPressed;

        private bool _isFirstColide = false;
        private bool _isFirstPrizeColide = false;

        private bool _isSecondColide = false;
        private bool _isSecondPrizeColide = false;

        private bool isFirstDecorated = false;
        private bool isSecondDecorated = false;

        private bool IsStarted = true;

        public int firstCathTime;
        public int secondCathTime;

        public int FirstPlayerScore, SecondPlayerScore;

        // В конструкторе создаем форму, инфраструктурные объекты, подгружаем спрайты, создаем помощник для работы со временем
        // В конце дергаем ресайзинг формы для вычисления масштаба и установки пределов по горизонтали и вертикали
        public Game(RenderControl renderControl)
        {
            _renderControl = renderControl;
            _dx2d = new DX2D(_renderControl);
            _dInput = new DInput(_renderControl);
            _dIn = new DInput(renderControl);

            firstPrizeGenerate._dx2d = _dx2d;
            firstObstacleGenerate._dx2d = _dx2d;

            secondPrizeGenerate._dx2d = _dx2d;
            secondObstacleGenerate._dx2d = _dx2d;

            windowObjects._dx2d = _dx2d;

            windowObjects.LoadMainObjects();

            _background = windowObjects._background;
            _firstGround = windowObjects._firstGround;
            _finish = windowObjects._finish;
            _secondGround = windowObjects._secondGround;
            _firstPlayer = windowObjects._firstPlayer;
            _secondPlayer = windowObjects._secondPlayer;

            runnerComponent = new Runner();
            secondRunnerComponent = new Runner();

            finish = new Pit();

            finishWrapper = new Wrapper<Obstacle>(_finish, finish);

            firstPlayerWrapper = new Wrapper<RunnerComponent>(_firstPlayer, runnerComponent);
            secondPlayerWrapper = new Wrapper<RunnerComponent>(_secondPlayer, secondRunnerComponent);


            firstObjectsTimer.ObjectsTimer(firstPrizeGenerate, firstObstacleGenerate);
            firstListOfPrizes = firstPrizeGenerate.ListOfPrizes;
            firstListOfObstacles = firstObstacleGenerate.ListOfObstacles;

            secondObjectsTimer.SecondObjectsTimer(secondPrizeGenerate, secondObstacleGenerate);
            secondListOfPrizes = secondPrizeGenerate.ListOfPrizes;
            secondListOfObstacles = secondObstacleGenerate.ListOfObstacles;

            _timeHelper = new TimeHelper();

            RenderForm_Resize(this, null);
        }

        /// <summary>
        /// Счетчик очков персонажа.
        /// </summary>
        public void UpdateScore()
        {
            FirstPlayerScore++;
            SecondPlayerScore++;
        }

        // Делегат, вызываемый для формирования каждого кадра
        private void RenderCallback()
        {
            if (IsStarted)
            {
                RenderForm_Resize(this, null);
            }
            UpdateScore();

            firstPrzOnScreen = firstListOfPrizes[0];
            firstObsOnScreen = firstListOfObstacles[0];

            secondPrzOnScreen = secondListOfPrizes[0];
            secondObsOnScreen = secondListOfObstacles[0];

            firstObsPos = firstObsOnScreen.sprite.PositionOfCenter;
            firstPrzPos = firstPrzOnScreen.sprite.PositionOfCenter;

            secondObsPos = secondObsOnScreen.sprite.PositionOfCenter;
            secondPrzPos = secondPrzOnScreen.sprite.PositionOfCenter;

            posFirsPlayer = firstPlayerWrapper.sprite.PositionOfCenter;
            posSecondPlayer = secondPlayerWrapper.sprite.PositionOfCenter;

            // Дергаем обновление состояния "временного" помощника и объектов ввода
            _timeHelper.Update();
            _dInput.UpdateKeyboardState();
            _dIn.UpdateKeyboardState();


            // Обновляем время и счетчик кадров
            //int fps = _timeHelper.FPS;
            float time = _timeHelper.Time;
            float dT = _timeHelper.dT;

            // Область просмотра в "прямом иксе 2 измерения" считается в "попугаях", т.е. в устройство-независимых пикселях несмотря на "dpiAware" в манифесте приложения
            // Поэтому для расчетов масштаба берем не клиентскую область формы, которая в честных пикселях, а RenderTarget-а
            WindowRenderTarget target = _dx2d.RenderTarget;
            Size2F targetSize = target.Size;
            _clientRect.Width = targetSize.Width;
            _clientRect.Height = targetSize.Height;

            // Начинаем вывод графики
            target.BeginDraw();
            // Перво-наперво - очистить область отображения
            target.Clear(SharpDX.Color.Black);

            // Перемещаем и выводим спрайты
            // Перемещение с расчетом коллизий и внутриигровой физики по-хорошему надо вынести в отдельный поток

            firstObsPos.X -= dT * firstPlayerWrapper.field.Speed;
            secondObsPos.X -= dT * secondPlayerWrapper.field.Speed;

            if (isFirstDecorated == false) { firstPrzPos.X -= dT * 7.0f; }
            if (isSecondDecorated == false) { secondPrzPos.X -= dT * 7.0f; }

            //przPos.X -= dT * 7.0f;

            // FPS-ов до небес, поэтому нужно триггерить события ввода
            if (_dInput.KeyboardUpdated) { Move();}

            Vector2 backgroundPos = new Vector2(0, 0);
            _background.PositionOfCenter = backgroundPos;
            _background.DrawBackground(1.0f, _scale, _unitsPerHeight / 1080.0f, _clientRect.Height);
            
            // Если выскочили за пределы окна по гориозонтали, начинаем сначала
            if ((posFirsPlayer.X - 1) * _scale > _clientRect.Width) posFirsPlayer.X = 0;

            UdpateObjects();

            //конец игры -> запуск финишного спрайта.
            if (FirstPlayerScore >= 10000 && firstObsPos.X * _scale < 0) { firstListOfObstacles[0] = finishWrapper;}

            _firstPlayer.PositionOfCenter = posFirsPlayer;
            firstObsOnScreen.sprite.PositionOfCenter = firstObsPos;
            firstPrzOnScreen.sprite.PositionOfCenter = firstPrzPos;

            _secondPlayer.PositionOfCenter = posSecondPlayer;
            secondObsOnScreen.sprite.PositionOfCenter = secondObsPos;
            secondPrzOnScreen.sprite.PositionOfCenter = secondPrzPos;
            


            // Наконец-то отрисовка
            _firstPlayer.Draw(1.0f, _scale, _clientRect.Height);
            _firstGround.Draw(1.0f, _scale, _clientRect.Height);

            _secondPlayer.Draw(1.0f, _scale, _clientRect.Height);
            _secondGround.Draw(1.0f, _scale, _clientRect.Height);

            firstListOfObstacles[0].sprite.Draw(1.0f, _scale, _clientRect.Height);
            firstListOfPrizes[0].sprite.Draw(1.0f, _scale, _clientRect.Height);

            secondListOfObstacles[0].sprite.Draw(1.0f, _scale, _clientRect.Height);
            secondListOfPrizes[0].sprite.Draw(1.0f, _scale, _clientRect.Height);

            gameStop.StopGame(firstPlayerWrapper.field, secondPlayerWrapper.field);

            // В левом верхнем углу выводми статистику
            target.Transform = Matrix3x2.Identity;
            target.DrawText($"Playe 1 Health: {firstPlayerWrapper.field.HP:f2}, Player 1 Score: {FirstPlayerScore:f2}", _dx2d.TextFormatStats, _clientRect, _dx2d.WhiteBrush);
            target.DrawText($"\nPlayer 2 Health: {secondPlayerWrapper.field.HP:f2}, Player 2 Score: {SecondPlayerScore:f2}", _dx2d.TextFormatStats, _clientRect, _dx2d.WhiteBrush);
            target.EndDraw();
        }

        //ресайз окна
        private void RenderForm_Resize(object sender, EventArgs e)
        {
            int width = _renderControl.ClientSize.Width;
            int height = _renderControl.ClientSize.Height;
            _dx2d.RenderTarget.Resize(new Size2(width, height));

            _clientRect.Width = _dx2d.RenderTarget.Size.Width;
            _clientRect.Height = _dx2d.RenderTarget.Size.Height;

            _clientRect.Width = _dx2d.RenderTarget.Size.Width;
            _clientRect.Height = _dx2d.RenderTarget.Size.Height;
            _scale = _clientRect.Height / _unitsPerHeight;
        }

        /// <summary>
        /// Обновление припятсвтия на экране
        /// </summary>
        public void UdpateObjects()
        {
            if ((firstObsPos.X) * _scale < -10)
            {
                firstObsPos.X = 45;
                for (int i = 0; i < firstListOfObstacles.Count; i++)
                {
                    firstListOfObstacles.Remove(firstListOfObstacles[0]);
                }
            }

            if ((firstPrzPos.X) * _scale < -10)
            {
                firstPrzPos.X = 45;
                for (int i = 0; i < firstListOfPrizes.Count; i++)
                {
                    firstListOfPrizes.Remove(firstListOfPrizes[0]);
                }
            }

            if ((secondObsPos.X) * _scale < -10)
            {
                secondObsPos.X = 45;
                for (int i = 0; i < secondListOfObstacles.Count; i++)
                {
                    secondListOfObstacles.Remove(secondListOfObstacles[0]);
                }
            }

            if ((secondPrzPos.X) * _scale < -10)
            {
                secondPrzPos.X = 45;
                for (int i = 0; i < secondListOfPrizes.Count; i++)
                {
                    secondListOfPrizes.Remove(secondListOfPrizes[0]);
                }
            }
        }

        public void SecondPlayerEvent (bool up, bool stateObs, bool statePrz)
        {
            float dT = _timeHelper.dT;
            float time = _timeHelper.Time;

            if (up && !_upPressed)
            {
                do
                {
                    posSecondPlayer.Y += secondPlayerWrapper.field.Velocity * dT + 0.5f * secondPlayerWrapper.field.Acceleration * (dT * dT);
                    secondPlayerWrapper.field.Velocity += secondPlayerWrapper.field.Acceleration * dT;
                }
                while (posSecondPlayer.Y < secondPlayerWrapper.field.JumpHeight + 12.5f);
                secondPlayerWrapper.field.IsJumping = true;
            }
            if (secondPlayerWrapper.field.IsJumping == true)
            {
                secondPlayerWrapper.field.Velocity = secondPlayerWrapper.field.Velocity - secondPlayerWrapper.field.Acceleration;
                posSecondPlayer.Y = posSecondPlayer.Y + secondPlayerWrapper.field.Velocity;
            }
            if (posSecondPlayer.Y <= 14.0f)
            {
                secondPlayerWrapper.field.Velocity = 0;
                posSecondPlayer.Y = 13.7f;
                secondPlayerWrapper.field.IsJumping = false;
            }

            //минус очки если игрок попал на препятствие
            if (stateObs && !_isSecondColide) { secondListOfObstacles[0].field.DoDamage(secondPlayerWrapper.field); SecondPlayerScore -= 200; }

            //навешивание декоратора и запоминание времени этого события
            if (statePrz && !_isSecondPrizeColide) { secondPlayerWrapper.field = secondListOfPrizes[0].field.SetPrize(secondPlayerWrapper.field); isSecondDecorated = true; secondCathTime = (int)time; secondPrzPos.X = -10; }

            //снятие декоратора спустя время
            if (isSecondDecorated && (time - secondCathTime) > 15) { removeDecorations.playerWrapper = secondPlayerWrapper; removeDecorations.DeleteJumpDecoration(); isSecondDecorated = false; }

        }

        public void FirstPlayerEvent(bool w, bool stateObs, bool statePrz)
        {
            float time = _timeHelper.Time;
            float dT = _timeHelper.dT;
            if (w && !_wPressed)
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
            if (stateObs && !_isFirstColide) { firstListOfObstacles[0].field.DoDamage(firstPlayerWrapper.field); FirstPlayerScore -= 200; }

            //навешивание декоратора и запоминание времени этого события
            if (statePrz && !_isFirstPrizeColide) { firstPlayerWrapper.field = firstListOfPrizes[0].field.SetPrize(firstPlayerWrapper.field); isFirstDecorated = true; firstCathTime = (int)time; firstPrzPos.X = -10; }

            //снятие декоратора спустя время
            if (isFirstDecorated && (time - firstCathTime) > 15) { removeDecorations.playerWrapper = firstPlayerWrapper; removeDecorations.DeleteJumpDecoration(); isFirstDecorated = false; }
        }

        public void JumpFirstPlayer(bool state)
        {

            float time = _timeHelper.Time;
            float dT = _timeHelper.dT;

            bool isFirstColade = checkCollision.IsCollision(firstPlayerWrapper.sprite, firstListOfObstacles[0].sprite);
            bool isFirstPrizeColade = checkCollision.IsCollision(firstPlayerWrapper.sprite, firstListOfPrizes[0].sprite);

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

            _wPressed = state;
            
            //минус очки если игрок попал на препятствие
            if (isFirstColade && !_isFirstColide) { firstListOfObstacles[0].field.DoDamage(firstPlayerWrapper.field); FirstPlayerScore -= 200; }

            //навешивание декоратора и запоминание времени этого события
            if (isFirstPrizeColade && !_isFirstPrizeColide) { firstPlayerWrapper.field = firstListOfPrizes[0].field.SetPrize(firstPlayerWrapper.field); isFirstDecorated = true; firstCathTime = (int)time; firstPrzPos.X = -10; }

            //снятие декоратора спустя время
            if (isFirstDecorated && (time - firstCathTime) > 15) { removeDecorations.playerWrapper = firstPlayerWrapper; removeDecorations.DeleteJumpDecoration(); isFirstDecorated = false; }

            _isFirstColide = isFirstColade;
            _wPressed = state;
            _isFirstPrizeColide = isFirstPrizeColade;
            
        }

        public void Move()
        {
            bool upPressed = _dInput.KeyboardState.IsPressed(Key.Up);
            bool wPressed = _dInput.KeyboardState.IsPressed(Key.W);

            bool isFirstColade = checkCollision.IsCollision(firstPlayerWrapper.sprite, firstListOfObstacles[0].sprite);
            bool isFirstPrizeColade = checkCollision.IsCollision(firstPlayerWrapper.sprite, firstListOfPrizes[0].sprite);

            bool isSecondColade = checkCollision.IsCollision(secondPlayerWrapper.sprite, secondListOfObstacles[0].sprite);
            bool isSecondPrizeColade = checkCollision.IsCollision(secondPlayerWrapper.sprite, secondListOfPrizes[0].sprite);

            FirstPlayerEvent(wPressed,isFirstColade,isFirstPrizeColade);
            SecondPlayerEvent(upPressed, isSecondColade, isSecondPrizeColade);

            _isFirstColide = isFirstColade;
            _isFirstPrizeColide = isFirstPrizeColade;

            _isSecondColide = isSecondColade;
            _isSecondPrizeColide = isSecondPrizeColade;

            _upPressed = upPressed;
            _wPressed = wPressed;
        }

        public void JumpSecondPlayer(bool state)
        {
            bool isSecondColade = checkCollision.IsCollision(secondPlayerWrapper.sprite, secondListOfObstacles[0].sprite);
            bool isSecondPrizeColade = checkCollision.IsCollision(secondPlayerWrapper.sprite, secondListOfPrizes[0].sprite);

            float dT = _timeHelper.dT;
            float time = _timeHelper.Time;

            if (state && !_upPressed)
            {
                do
                {
                    posSecondPlayer.Y += secondPlayerWrapper.field.Velocity * dT + 0.5f * secondPlayerWrapper.field.Acceleration * (dT * dT);
                    secondPlayerWrapper.field.Velocity += secondPlayerWrapper.field.Acceleration * dT;
                }
                while (posSecondPlayer.Y < secondPlayerWrapper.field.JumpHeight + 10.0f);
                secondPlayerWrapper.field.IsJumping = true;
            }
            if (secondPlayerWrapper.field.IsJumping == true)          
                {
                    secondPlayerWrapper.field.Velocity = secondPlayerWrapper.field.Velocity - secondPlayerWrapper.field.Acceleration;
                    posSecondPlayer.Y = posSecondPlayer.Y + secondPlayerWrapper.field.Velocity;
                }
            if (posSecondPlayer.Y <= 14.0f)        
                {
                    secondPlayerWrapper.field.Velocity = 0;
                    posSecondPlayer.Y = 13.7f;
                    secondPlayerWrapper.field.IsJumping = false;
                }

            //минус очки если игрок попал на препятствие
            if (isSecondColade && !_isSecondColide) { secondListOfObstacles[0].field.DoDamage(secondPlayerWrapper.field); SecondPlayerScore -= 200; }

            //навешивание декоратора и запоминание времени этого события
            if (isSecondPrizeColade && !_isSecondPrizeColide) { secondPlayerWrapper.field = secondListOfPrizes[0].field.SetPrize(secondPlayerWrapper.field); isSecondDecorated = true; secondCathTime = (int)time; secondPrzPos.X = -10; }

            //снятие декоратора спустя время
            if (isSecondDecorated && (time - secondCathTime) > 15) { removeDecorations.playerWrapper = secondPlayerWrapper; removeDecorations.DeleteJumpDecoration(); isSecondDecorated = false; }

            _isSecondColide = isSecondColade;
            _upPressed = state;
            _isSecondPrizeColide = isSecondPrizeColade;
        }

        /// <summary>
        /// запуск игры
        /// </summary>
        public void Run()
        {
            _renderControl.Resize += RenderForm_Resize;
            RenderLoop.Run(_renderControl, RenderCallback);
        }

       /// <summary>
       /// удаление "мусора"
       /// </summary>
        public void Dispose()
        {
            _dInput.Dispose();
            _dx2d.Dispose();
            _renderControl.Dispose();
            firstListOfObstacles.Clear();
            firstListOfPrizes.Clear();
        }
    }
}
