using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameEngine;
using ModelLibrary;
using SharpDX.Windows;

namespace GameController
{
    public class RemoveDecorations
    {
        public bool isDecorated;
        public Wrapper<RunnerComponent> playerWrapper;
        public IncreaseJumpDecorator increaseJumpDecorator;
        public IncreaseVelocityDecorator increaseVelocityDecorator;

        public RunnerComponent runnerComponent;
        public void DeleteJumpDecoration()
        {
            increaseJumpDecorator = new IncreaseJumpDecorator(runnerComponent);
            increaseVelocityDecorator = new IncreaseVelocityDecorator(runnerComponent);

            if (playerWrapper.field.GetType() == increaseJumpDecorator.GetType())
            {
                //MessageBox.Show("Origin Jump");
                playerWrapper.field = (playerWrapper.field as IncreaseJumpDecorator).GetOriginalObject();
                isDecorated = false;
            }
            else if (playerWrapper.field.GetType() == increaseVelocityDecorator.GetType())
            {
                //MessageBox.Show("Origin velosity");
                playerWrapper.field = (playerWrapper.field as IncreaseVelocityDecorator).GetOriginalObject();
                isDecorated = false;
            }
        }
    }
}
