﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    /// <summary>
    /// Аn abstract class that defines an interface for inherited objects
    /// </summary>
    public abstract class RunnerComponent
    {
        /// <summary>
        /// Abstract speed property.
        /// </summary>
        public abstract float Velocity { get; set; }

        /// <summary>
        /// Abstract jump height property.
        /// </summary>
        public abstract float JumpHeight { get; set; }

        /// <summary>
        /// Abstract Character Health property.
        /// </summary>
        public abstract int HP { get; set; }
    }
}
