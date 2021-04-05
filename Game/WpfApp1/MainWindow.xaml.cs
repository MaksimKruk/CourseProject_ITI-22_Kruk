using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Windows;
using GameController;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RenderControl _renderControl;
        Game game;

        public MainWindow()
        {
            InitializeComponent();
            _renderControl = new RenderControl();
            _windowsFormsHost.Child = _renderControl;
            game = new Game(_renderControl);

            /*
            Direct2DApp app = new Direct2DApp();
            _windowsFormsHost.KeyDown += app.KeyDown;
            _windowsFormsHost.Child = app.RenderControl;
            app.Run();
            app.Dispose();
            */
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            game.Run();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            game.Dispose();
        }
    }
}
