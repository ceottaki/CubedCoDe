// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="DeploymentActionType.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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
    /// Defines the types of action that can be used for a deployment process.
    /// </summary>
    public enum DeploymentActionType
    {
        /// <summary>
        /// Indicates that no actions should be taken.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that an action should be taken for a file to be executed.
        /// </summary>
        ExecuteFile,

        /// <summary>
        /// Indicates that an action should be taken for a file to be copied.
        /// </summary>
        CopyFile,

        /// <summary>
        /// Indicates that an action should be taken for an email to be sent.
        /// </summary>
        SendEmail,

        /// <summary>
        /// Indicates that an action should be taken for unit tests to be checked.
        /// </summary>
        CheckUnitTests,

        /// <summary>
        /// Indicates that an action should be taken for a database script to be run.
        /// </summary>
        RunDatabaseScript,
    }
}
