// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeploymentModel.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.Model.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CubedCoDe.Entities;

    /// <summary>
    /// Defines properties and methods for an implementation of a deployment model.
    /// </summary>
    public interface IDeploymentModel
    {
        /// <summary>
        /// Gets all repositories available.
        /// </summary>
        /// <value>
        /// All repositories available.
        /// </value>
        /// <remarks>
        /// This property is updated through <see cref="ReadConfigurationFromFile"/>.
        /// </remarks>
        /// <seealso cref="ReadConfigurationFromFile"/>
        IEnumerable<RepositoryEntity> AllRepositories
        {
            get;
        }

        /// <summary>
        /// Gets the repositories needing to be updated.
        /// </summary>
        /// <value>
        /// The repositories needing to be updated.
        /// </value>
        /// <remarks>
        /// This property is updated through <see cref="CheckRepositoriesForUpdates"/>.
        /// </remarks>
        /// <seealso cref="CheckRepositoriesForUpdates"/>
        IEnumerable<RepositoryEntity> RepositoriesNeedingUpdate
        {
            get;
        }

        /// <summary>
        /// Gets the repositories available to build.
        /// </summary>
        /// <value>
        /// The repositories available to build.
        /// </value>
        IEnumerable<RepositoryEntity> RepositoriesToBuild
        {
            get;
        }

        /// <summary>
        /// Gets the repositories to deploy.
        /// </summary>
        /// <value>
        /// The repositories to deploy.
        /// </value>
        IEnumerable<RepositoryEntity> RepositoriesToDeploy
        {
            get;
        }

        /// <summary>
        /// Gets the next update time.
        /// </summary>
        /// <value>
        /// The next update time.
        /// </value>
        DateTime NextUpdateTime
        {
            get;
        }

        /// <summary>
        /// Reads the configuration from the configuration file and updates the values in the model to use the new configuration.
        /// </summary>
        void ReadConfigurationFromFile();

        /// <summary>
        /// Checks the repositories for updates.
        /// </summary>
        /// <remarks>
        /// The list of repositories for which there are updates is made available through <see cref="RepositoriesNeedingUpdate"/>.
        /// </remarks>
        /// <returns><c>true</c> if any repositories have been updated; <c>false</c> otherwise.</returns>
        /// <seealso cref="RepositoriesNeedingUpdate"/>
        bool CheckRepositoriesForUpdates();

        /// <summary>
        /// Updates the local repositories with most up to date data from the remote repositories.
        /// </summary>
        /// <remarks>
        /// This method will not check the remote repositories for updates, so if <see cref="CheckRepositoriesForUpdates"/> hasn't been run
        /// it will not update the repositories. The list of repositories that will be updated by this method is available through <see cref="RepositoriesNeedingUpdate"/>.
        /// Once updated the repositories will be made available to be built. See the list of repositories available for building on <see cref="RepositoriesToBuild"/>.
        /// </remarks>
        /// <seealso cref="CheckRepositoriesForUpdates"/>
        /// <seealso cref="RepositoriesNeedingUpdate"/>
        /// <seealso cref="RepositoriesToBuild"/>
        void UpdateRepositories();

        /// <summary>
        /// Builds the repositories that have been recently updated.
        /// </summary>
        /// <remarks>
        /// This method will not update the local repositories from their remote origins. To do so, call <see cref="UpdateRepositories"/>.
        /// The list of repositories that will be built by this method is available through <see cref="RepositoriesToBuild"/>.
        /// Once built the repositories will be made available for deployment. See the list of repositories available for deployment on <see cref="RepositoriesToDeploy"/>.
        /// </remarks>
        /// <returns><c>true</c> if all repositories were built successfully; <c>false</c> if at least one repository failed to build.</returns>
        /// <seealso cref="UpdateRepositories"/>
        /// <seealso cref="RepositoriesToBuild"/>
        /// <seealso cref="RepositoriesToDeploy"/>
        bool BuildRepositories();

        /// <summary>
        /// Deploys the repositories that have been recently updated and built.
        /// </summary>
        /// <remarks>
        /// This method will not build the updated repositories. To do so, call <see cref="BuildRepositories"/>.
        /// The list of repositories that will be deployed by this method is available through <see cref="RepositoriesToDeploy"/>.
        /// </remarks>
        /// <returns><c>true</c> if all repositories were deployed successfully; <c>false</c> if at least one repository failed to deploy.</returns>
        /// <seealso cref="BuildRepositories"/>
        /// <seealso cref="RepositoriesToDeploy"/>
        bool DeployRepositories();
    }
}
