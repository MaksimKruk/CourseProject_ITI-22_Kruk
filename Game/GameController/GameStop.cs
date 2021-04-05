using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModelLibrary;

namespace GameController
{
    public class GameStop
    {
        public void StopGame(RunnerComponent runner1, RunnerComponent runner2)
        {
            if(runner1.HP <= 0)
            {
                MessageBox.Show("Игрок 1 проиграл.");
                runner1.HP = 100;
                runner2.HP = 100;
            }
            else if (runner2.HP <= 0)
            {
                MessageBox.Show("Игрок 2 проиграл.");
                runner1.HP = 100;
                runner2.HP = 100;
            }
        }
    }
}
