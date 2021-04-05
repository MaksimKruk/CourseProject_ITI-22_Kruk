using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.WIC;
using SharpDX.Windows;


namespace GameEngine
{
    public class DX2D : IDisposable
    {
        // Фабрика для создания 2D объектов
        private SharpDX.Direct2D1.Factory _factory;
        public SharpDX.Direct2D1.Factory Factory { get => _factory; }

        // Фабрика для работы с текстом
        private SharpDX.DirectWrite.Factory _writeFactory;
        public SharpDX.DirectWrite.Factory WriteFactory { get => _writeFactory; }

        // "Цель" отрисовки
        private WindowRenderTarget _renderTarget;
        public WindowRenderTarget RenderTarget { get => _renderTarget; }

        // Фабрика (уже третяя?) для работы с изображениями (WIC = Windows Imaging Component)
        private ImagingFactory _imagingFactory;
        public ImagingFactory ImagingFactory { get => _imagingFactory; }

        // Формат текста для вывода статистики в верхнем левом углу
        private TextFormat _textFormatStats;
        public TextFormat TextFormatStats { get => _textFormatStats; }
        // Формат текста для сообщения по центру окна (пока не используется)
        private TextFormat _textFormatMessage;
        public TextFormat TextFormatMessage { get => _textFormatStats; }
        // Две кисти для текста
        private Brush _redBrush;
        public Brush RedBrush { get => _redBrush; }
        private Brush _whiteBrush;
        public Brush WhiteBrush { get => _whiteBrush; }

        // Коллекция спрайтов
        // Есть конечно атлас спрайтов SpriteBatch
        // https://docs.microsoft.com/en-us/windows/win32/api/d2d1_3/nn-d2d1_3-id2d1spritebatch
        // но при его использовании не поддерживается антиалиасинг, можно использовать только AntialiasMode.Aliased
        // Источник: https://issue.life/questions/55093299
        // Таким образом он отлично подойдет, если спрайты вращать не надо
        private List<SharpDX.Direct2D1.Bitmap> _bitmaps;
        public List<SharpDX.Direct2D1.Bitmap> Bitmaps { get => _bitmaps; }

        // В конструкторе создаем все объекты
        public DX2D(RenderControl control)
        {
            // Создание фабрик для 2D объектов и текста
            _factory = new SharpDX.Direct2D1.Factory();
            _writeFactory = new SharpDX.DirectWrite.Factory();

            // Инициализация "прямой рисовалки":
            //   Свойства отрисовщика
            RenderTargetProperties renderProp = new RenderTargetProperties()
            {
                DpiX = 0,
                DpiY = 0,
                MinLevel = FeatureLevel.Level_10,
                PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
                Type = RenderTargetType.Hardware,
                Usage = RenderTargetUsage.None
            };
            //   Свойства отрисовщика, связанные с окном (дискриптор окна, размер в пикселях и способ представления результирующего изображения)
            HwndRenderTargetProperties winProp = new HwndRenderTargetProperties()
            {
                Hwnd = control.Handle,
                PixelSize = new Size2(control.ClientSize.Width, control.ClientSize.Height),
                PresentOptions = PresentOptions.None                                     // Immediately // None - vSync
            };
            //   Создание "цели" и задание свойств сглаживания
            _renderTarget = new WindowRenderTarget(_factory, renderProp, winProp);
            _renderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
            _renderTarget.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype;

            // Создание "картиночной" фабрики
            _imagingFactory = new ImagingFactory();

            // Задание форматов текста
            _textFormatStats = new TextFormat(_writeFactory, "Calibri", 12);
            _textFormatStats.ParagraphAlignment = ParagraphAlignment.Near;
            _textFormatStats.TextAlignment = TextAlignment.Leading;
            _textFormatMessage = new TextFormat(_writeFactory, "Calibri", 24);
            _textFormatMessage.ParagraphAlignment = ParagraphAlignment.Center;
            _textFormatMessage.TextAlignment = TextAlignment.Center;
            // Создание кистей для текста
            _redBrush = new SolidColorBrush(_renderTarget, Color.Red);
            _whiteBrush = new SolidColorBrush(_renderTarget, Color.White);
        }

        // Чтение изображения из bmp
        // Здесь Вам не тут!
        // System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap("image.bmp"); - для слабаков
        public int LoadBitmap(string imageFileName)
        {
            // Декодер формата
            BitmapDecoder decoder = new BitmapDecoder(_imagingFactory, imageFileName, DecodeOptions.CacheOnDemand);
            // Берем первый фрейм
            BitmapFrameDecode frame = decoder.GetFrame(0);
            // Также нужен конвертер формата 
            FormatConverter converter = new FormatConverter(_imagingFactory);
            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA, BitmapDitherType.Ordered4x4, null, 0.0, BitmapPaletteType.Custom);
            // Вот теперь можно и bitmap
            SharpDX.Direct2D1.Bitmap bitmap = SharpDX.Direct2D1.Bitmap.FromWicBitmap(_renderTarget, converter);

            // Освобождаем неуправляемые ресурсы
            Utilities.Dispose(ref converter);
            Utilities.Dispose(ref frame);
            Utilities.Dispose(ref decoder);

            // Добавляем изображение в коллекцию
            if (_bitmaps == null) _bitmaps = new List<SharpDX.Direct2D1.Bitmap>(4);
            _bitmaps.Add(bitmap);
            return _bitmaps.Count - 1;
        }

        // Освобождаем неуправляемые ресурсы
        public void Dispose()
        {
            for (int i = _bitmaps.Count - 1; i >= 0; i--) // foreach здесь не пойдет, поскольку итератор нельзя передавать как ref
            {
                SharpDX.Direct2D1.Bitmap bitmap = _bitmaps[i];
                _bitmaps.RemoveAt(i);
                Utilities.Dispose(ref bitmap);
            }
            Utilities.Dispose(ref _whiteBrush);
            Utilities.Dispose(ref _redBrush);
            Utilities.Dispose(ref _textFormatStats);
            Utilities.Dispose(ref _imagingFactory);
            Utilities.Dispose(ref _renderTarget);
            Utilities.Dispose(ref _factory);
        }
    }
}
