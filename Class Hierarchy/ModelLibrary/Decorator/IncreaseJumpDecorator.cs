using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    /// <summary>
    /// A class that represents additional functionality. A specific decorator.
    /// </summary>
    public class IncreaseJumpDecorator : RunnerDecorator
    {
        /// <summary>
        /// Constructor of the decorator class.
        /// </summary>
        /// <param name="playerComp">Instance of the base class.</param>
        public IncreaseJumpDecorator(RunnerComponent playerComp) : base(playerComp)
        { }

        /// <summary>
        /// Оverrides the jump height property
        /// </summary>
        public override float JumpHeight
        {
            get => base.JumpHeight + 3.0f; set => base.JumpHeight = value;
        }

        /// <summary>
        /// Method for returning an instance of a class before changes.
        /// </summary>
        /// <returns>Instance of the class before changes.</returns>
        public RunnerComponent GetOriginalObject()
        {
            return playerComp;
        }
    }
}
