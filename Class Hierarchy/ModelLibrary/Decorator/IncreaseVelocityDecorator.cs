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
    public class IncreaseVelocityDecorator : RunnerDecorator
    {
        /// <summary>
        /// Constructor of the decorator class.
        /// </summary>
        /// <param name="playerComp">Instance of the base class.</param>
        public IncreaseVelocityDecorator(RunnerComponent playerComp) : base(playerComp)
        {}

        /// <summary>
        /// Оverrides the speed property.
        /// </summary>
        public override float Velocity 
        { 
            get => base.Velocity + 3.0f; set => base.Velocity = value; 
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
