// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryState.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
//
// This file is part of CUBED CoDe, a continuous deployment solution.
//
// CUBED CoDe is free software: you can redistribute it and/or modify it under the terms of the GNU General
// Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// CUBED CoDe is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the
// implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
//
// You should have received a copy of the GNU General Public License along with CUBED CoDe. If not, see
// http://www.gnu.org/licenses/.
// ----------------------------------------------------------------------------------------------------------------------------

namespace CubedCoDe.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the states in which a distributed revision control repository manager can exist.
    /// </summary>
    public enum RepositoryState
    {
        /// <summary>
        /// Indicates that the repository manager has an unknown state and probably has not been initialised.
        /// </summary>
        Unknown,

        /// <summary>
        /// Indicates that the repository manager has now opened a valid repository and is ready to be used.
        /// </summary>
        Opened,

        /// <summary>
        /// Indicates that the repository manager is closed and is not usable. This is the initial state of a recently created repository manager.
        /// </summary>
        Closed,

        /// <summary>
        /// Indicates that the repository manager has encountered an error or fault that renders is unusable.
        /// </summary>
        Faulted,
    }
}
