using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    /// <summary>
    /// Base class of the factory method for generating Prizes.
    /// </summary>
    public abstract class PrizeCreator
    {
        /// <summary>
        /// Abstract factory method that returns an prize.
        /// </summary>
        /// <returns>Returns an object of the class "Prize".</returns>
        abstract public Prize Create();
    }
}
