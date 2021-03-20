using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    /// <summary>
    /// Base class of the factory method for generating obstacles.
    /// </summary>
    public abstract class ObstacleCreator
    {
        /// <summary>
        /// Abstract factory method that returns an obstacle.
        /// </summary>
        /// <returns>Returns an object of the class "Obstacle".</returns>
        abstract public Obstacle Create();
    }
}
