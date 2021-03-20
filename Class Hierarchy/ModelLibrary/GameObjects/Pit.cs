using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    /// <summary>
    /// An obstacle of the "pit" type".
    /// </summary>
    public class Pit : Obstacle
    {
        /// <summary>
        /// Obstacle damage level.
        /// </summary>
        public float hitLevel;

        /// <summary>
        /// Dealing damage to the player.
        /// </summary>
        /// <param name="player">Player Instance.</param>
        public override void DoDamage(Runner player)
        {
            hitLevel = 100;
            player.HP -= (int)hitLevel;
        }
    }
}
