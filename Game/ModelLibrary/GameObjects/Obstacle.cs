using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    /// <summary>
    /// Base class "obstacle".
    /// </summary>
    public abstract class Obstacle
    {
        /// <summary>
        /// Аbstract method of dealing damage to the player.
        /// </summary>
        /// <param name="player">Player Instance.</param>
        public abstract void DoDamage(RunnerComponent player);
    }
}
