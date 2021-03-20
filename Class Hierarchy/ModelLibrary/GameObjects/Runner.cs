using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    /// <summary>
    /// A class representing a game character.
    /// </summary>
    public class Runner : RunnerComponent
    {
        /// <summary>
        /// The number of points scored by the player.
        /// </summary>
        public int score;

        /// <summary>
        /// Character movement speed.
        /// </summary>
        public float velocity; 

        /// <summary>
        /// Acceleration.
        /// </summary>
        public float acceleration; 

        /// <summary>
        /// The variable stores information about whether the character is in the jump.
        /// </summary>
        public bool isJumping; 

        /// <summary>
        /// Character's jump height.
        /// </summary>
        public float jumpHeight; 

        /// <summary>
        /// Maximum jump height of the character.
        /// </summary>
        public float maxJumpHeight;

        /// <summary>
        /// Character's Health level.
        /// </summary>
        public int hp;

        /// <summary>
        /// Maximum character health level.
        /// </summary>
        public int maxHP;

        /// <summary>
        /// Character Movement Speed property.
        /// </summary>
        public override float Velocity { get => velocity; set => velocity = value; }

        /// <summary>
        /// The character's jump height property.
        /// </summary>
        public override float JumpHeight { get => jumpHeight; set => jumpHeight = value; }

        /// <summary>
        /// Character Health property.
        /// </summary>
        public override int HP { get => hp; set => hp = value; }

        /// <summary>
        /// Сonstructor of the runner class.
        /// </summary>
        public Runner()
        {
            hp = 100;
            maxHP = 100;
            maxJumpHeight = 10.0f;
            score = 0;
            velocity = 2;
            acceleration = 0.01f;
            isJumping = false;
            jumpHeight = 7.0f;
        }
    }
}
