using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    /// <summary>
    /// A class of a specific factory that overrides the method for creating a specific object.
    /// </summary>
    public class AnimalCreator : ObstacleCreator
    {
        /// <summary>
        /// Redefined method for creating an object.
        /// </summary>
        /// <returns>A specific object of the "animal" type.</returns>
        public override Obstacle Create()
        {
            return new Animal();
        }
    }
}
