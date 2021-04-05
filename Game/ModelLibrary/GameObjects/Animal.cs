using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    /// <summary>
    /// An obstacle of the "animal" type".
    /// </summary>
    public class Animal : Obstacle
    {
        /// <summary>
        /// Obstacle damage level.
        /// </summary>
        public float hitLevel;

        /// <summary>
        /// Dealing damage to the player.
        /// </summary>
        /// <param name="player">Player Instance.</param>
        public override void DoDamage(RunnerComponent player)
        {
            hitLevel = 50;
            player.HP -= (int)hitLevel;
        }
    }
}
