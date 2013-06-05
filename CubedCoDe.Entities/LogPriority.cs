// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="LogPriority.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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
    /// Specifies the priority of a log message.
    /// </summary>
    /// <remarks>
    /// Priorities are actually stored as numbers for configuration and filtering purposes. The lower the number the more granular messages will be.
    /// </remarks>
    public enum LogPriority
    {
        /// <summary>
        /// Log message is useful for debugging problems.
        /// </summary>
        Debug = 0,

        /// <summary>
        /// Log message is useful as general information.
        /// </summary>
        Information = 1,

        /// <summary>
        /// Log message is useful as a warning for a recoverable error.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Log message is useful as an unrecoverable error.
        /// </summary>
        Error = 3,
    }
}
