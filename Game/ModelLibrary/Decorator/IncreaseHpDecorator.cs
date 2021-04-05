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
    public class IncreaseHpDecorator : RunnerDecorator
    {
        private int _hp;
        /// <summary>
        /// Constructor of the decorator class.
        /// </summary>
        /// <param name="playerComp">Instance of the base class.</param>
        public IncreaseHpDecorator(RunnerComponent playerComp) : base(playerComp)
        {
            _hp = playerComp.HP + 10;
        }

        /// <summary>
        /// Redefined player health level property.
        /// </summary>
        public override int HP
        {
            get => _hp; set => _hp = value;
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
