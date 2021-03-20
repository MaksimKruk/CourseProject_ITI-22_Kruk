using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Game
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        double x = 0;
        double y = 0;

        public MainWindow()
        {
            InitializeComponent();
            //this.DataContext = new ApplicationViewModel();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(MovePlayer);
            timer.Start();
        }


        private void MovePlayer(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Down))
            {
                y += .05;
                Canvas.SetTop(img, y);
            }
            if (Keyboard.IsKeyDown(Key.Up))
            {
                y -= .05;
                Canvas.SetTop(img, y);
            }
            if (Keyboard.IsKeyDown(Key.Left))
            {
                x -= .05;
                Canvas.SetLeft(img, x);
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
                x += .05;
                Canvas.SetLeft(img, x);
            }
        }
    }
}