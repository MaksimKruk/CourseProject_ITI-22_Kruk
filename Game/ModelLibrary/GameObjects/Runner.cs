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

        public float speed;

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
        private int hp;

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

        public override float Speed { get => speed; set => speed = value; }

        /// <summary>
        /// Сonstructor of the runner class.
        /// </summary>
        public Runner()
        {
            speed = 25.0f;
            HP = 100;
            maxHP = 100;
            maxJumpHeight = 15.0f;
            Score = 0;
            velocity = 2;
            Acceleration = 0.01f;
            IsJumping = false;
            jumpHeight = 5.0f;
        }
    }
}
