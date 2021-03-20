using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    /// <summary>
    /// Character's Prize Class.
    /// </summary>
    class HealthPrize : Prize
    {

        /// <summary>
        /// The method that imposes a prize on the character.
        /// </summary>
        /// <param name="runner">An instance of an abstract character.</param>
        public void SetPrize(RunnerComponent runner)
        {
            runner = new Runner();
            runner = new IncreaseHpDecorator(runner);
        }
    }
}
