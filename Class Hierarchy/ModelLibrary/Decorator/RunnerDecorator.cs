using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    /// <summary>
    /// The decorator is implemented as an abstract class and has the same base class as the objects being decorated.
    /// </summary>
    public abstract class RunnerDecorator : RunnerComponent
    {
        /// <summary>
        /// Instance of the base class.
        /// </summary>
        protected RunnerComponent playerComp;

        /// <summary>
        /// Redefined character speed property.
        /// </summary>
        public override float Velocity { get => playerComp.Velocity; set => playerComp.Velocity = value; }

        /// <summary>
        /// Redefined jump speed property.
        /// </summary>
        public override float JumpHeight { get => playerComp.JumpHeight; set => playerComp.JumpHeight = value; }

        /// <summary>
        /// Redefined health level property.
        /// </summary>
        public override int HP { get => playerComp.HP; set => playerComp.HP = value; }

        /// <summary>
        /// Class Constructor.
        /// </summary>
        /// <param name="playerComp">Instance of the base class.</param>
        public RunnerDecorator(RunnerComponent playerComp)
        {
            this.playerComp = playerComp;
        }
    }
}
