using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameEngine;
using ModelLibrary;


namespace GameController
{
    public class GenerateObjectsTimer
    {
        public void ObjectsTimer(PrizeGenerate prizeGenerate, ObstacleGenerate obstacleGenerate)
        {
            Timer timer1 = new Timer();
            timer1.Interval = 1000;
            timer1.Tick += prizeGenerate.setPrizeData;
            timer1.Enabled = true;
            prizeGenerate.setPrizeData(timer1, EventArgs.Empty);

            Timer timer2 = new Timer();
            timer2.Interval = 1000;
            timer2.Tick += obstacleGenerate.setObsData;
            timer2.Enabled = true;
            obstacleGenerate.setObsData(timer2, EventArgs.Empty);
        }

        public void SecondObjectsTimer(SecondPrizeGenerate prizeGenerate, SecondObstacleGenerate obstacleGenerate)
        {
            Timer timer1 = new Timer();
            timer1.Interval = 1000;
            timer1.Tick += prizeGenerate.setPrizeData;
            timer1.Enabled = true;
            prizeGenerate.setPrizeData(timer1, EventArgs.Empty);

            Timer timer2 = new Timer();
            timer2.Interval = 1000;
            timer2.Tick += obstacleGenerate.setObsData;
            timer2.Enabled = true;
            obstacleGenerate.setObsData(timer2, EventArgs.Empty);
        }
    }
}
