// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeploymentManager.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CubedCoDe.Entities;

    /// <summary>
    /// Defines properties and methods for an implementation of a class that manages the deployment of software packages.
    /// </summary>
    public interface IDeploymentManager
    {
        /// <summary>
        /// Executes a deployment action.
        /// </summary>
        /// <param name="actionType">The type of the action.</param>
        /// <param name="actionParameters">The action parameters.</param>
        /// <returns><c>true</c> if action was executed successfully; <c>false</c> otherwise.</returns>
        bool ExecuteDeploymentAction(DeploymentActionType actionType, params string[] actionParameters);
    }
}
