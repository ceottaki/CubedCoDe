// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="DeploymentModel.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "It is a design choice to use underscore to mark local class fields for simplicity rather than always using 'this'.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:PrefixLocalCallsWithThis", Justification = "It is a design choice to use underscore to mark local class fields for simplicity rather than always using 'this'.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "It is a design choice to use underscore to mark local class fields for simplicity rather than always using 'this'.")]

namespace CubedCoDe.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using CubedCoDe.Core.Interfaces;
    using CubedCoDe.Entities;
    using CubedCoDe.Model.Interfaces;

    /// <summary>
    /// Represents a deployment model that exposes operations to provide continuous deployment of .NET applications stored in version control repositories.
    /// </summary>
    public class DeploymentModel : IDeploymentModel
    {
        /// <summary>
        /// The repository service.
        /// </summary>
        private IDeploymentService _repositoryService;

        /// <summary>
        /// The logging service.
        /// </summary>
        private ILoggingService _loggingService;

        /// <summary>
        /// Initialises a new instance of the <see cref="DeploymentModel" /> class.
        /// </summary>
        /// <param name="repositoryService">The repository service.</param>
        /// <param name="loggingService">The logging service.</param>
        public DeploymentModel(IDeploymentService repositoryService, ILoggingService loggingService)
        {
            _repositoryService = repositoryService;
            _loggingService = loggingService;
        }

        /// <summary>
        /// Gets all repositories available.
        /// </summary>
        /// <value>
        /// All repositories available.
        /// </value>
        /// <seealso cref="ReadConfigurationFromFile" />
        /// <remarks>
        /// This property is updated through <see cref="ReadConfigurationFromFile" />.
        /// </remarks>
        public IEnumerable<RepositoryEntity> AllRepositories
        {
            get
            {
                IList<RepositoryEntity> result = new List<RepositoryEntity>();

                if (_repositoryService != null)
                {
                    result = _repositoryService.AllRepositories.ToList();
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the repositories needing to be updated.
        /// </summary>
        /// <value>
        /// The repositories needing to be updated.
        /// </value>
        /// <seealso cref="CheckRepositoriesForUpdates" />
        /// <remarks>
        /// This property is updated through <see cref="CheckRepositoriesForUpdates" />.
        /// </remarks>
        public IEnumerable<RepositoryEntity> RepositoriesNeedingUpdate
        {
            get
            {
                IList<RepositoryEntity> result = new List<RepositoryEntity>();

                if (_repositoryService != null)
                {
                    result = _repositoryService.RepositoriesNeedingUpdate.ToList();
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the repositories available to build.
        /// </summary>
        /// <value>
        /// The repositories available to build.
        /// </value>
        public IEnumerable<RepositoryEntity> RepositoriesToBuild
        {
            get
            {
                IList<RepositoryEntity> result = new List<RepositoryEntity>();

                if (_repositoryService != null)
                {
                    result = _repositoryService.RepositoriesToBuild.ToList();
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the repositories to deploy.
        /// </summary>
        /// <value>
        /// The repositories to deploy.
        /// </value>
        public IEnumerable<RepositoryEntity> RepositoriesToDeploy
        {
            get
            {
                IList<RepositoryEntity> result = new List<RepositoryEntity>();

                if (_repositoryService != null)
                {
                    result = _repositoryService.RepositoriesToDeploy.ToList();
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the next update time.
        /// </summary>
        /// <value>
        /// The next update time.
        /// </value>
        public DateTime NextUpdateTime
        {
            get
            {
                DateTime result = DateTime.MaxValue;
                
                if (_repositoryService != null)
                {
                    if (_repositoryService.AllRepositories.Count() > 0)
                    {
                        result = _repositoryService.AllRepositories.Min(r => r.LastUpdateTime.Add(r.CheckForUpdatesTimeSpan));
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Reads the configuration from the configuration file and updates the values in the model to use the new configuration.
        /// </summary>
        public void ReadConfigurationFromFile()
        {
            if (_repositoryService != null)
            {
                try
                {
                    _repositoryService.ReadConfigurationFromFile();
                }
                catch (Exception ex)
                {
                    // TODO: Log exception.
                }
            }
            else
            {
                // TODO: Log that repository service is not present.
            }
        }

        /// <summary>
        /// Checks the repositories for updates.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if any repositories have been updated; <c>false</c> otherwise.
        /// </returns>
        /// <seealso cref="RepositoriesNeedingUpdate" />
        /// <remarks>
        /// The list of repositories for which there are updates is made available through <see cref="RepositoriesNeedingUpdate" />.
        /// </remarks>
        public bool CheckRepositoriesForUpdates()
        {
            bool result = false;

            if (_repositoryService != null)
            {
                result = _repositoryService.CheckRepositoriesForUpdates();
            }
            else
            {
                // TODO: Log that repository service is not present.
            }

            return result;
        }

        /// <summary>
        /// Updates the local repositories with most up to date data from the remote repositories.
        /// </summary>
        /// <seealso cref="CheckRepositoriesForUpdates" />
        ///   <seealso cref="RepositoriesNeedingUpdate" />
        ///   <seealso cref="RepositoriesToBuild" />
        /// <remarks>
        /// This method will not check the remote repositories for updates, so if <see cref="CheckRepositoriesForUpdates" /> hasn't been run
        /// it will not update the repositories. The list of repositories that will be updated by this method is available through <see cref="RepositoriesNeedingUpdate" />.
        /// Once updated the repositories will be made available to be built. See the list of repositories available for building on <see cref="RepositoriesToBuild" />.
        /// </remarks>
        public void UpdateRepositories()
        {
            if (_repositoryService != null)
            {
                _repositoryService.UpdateRepositories();
            }
            else
            {
                // TODO: Log that repository service is not present.
            }
        }

        /// <summary>
        /// Builds the repositories that have been recently updated.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if all repositories were built successfully; <c>false</c> if at least one repository failed to build.
        /// </returns>
        /// <seealso cref="UpdateRepositories" />
        ///   <seealso cref="RepositoriesToBuild" />
        ///   <seealso cref="RepositoriesToDeploy" />
        /// <remarks>
        /// This method will not update the local repositories from their remote origins. To do so, call <see cref="UpdateRepositories" />.
        /// The list of repositories that will be built by this method is available through <see cref="RepositoriesToBuild" />.
        /// Once built the repositories will be made available for deployment. See the list of repositories available for deployment on <see cref="RepositoriesToDeploy" />.
        /// </remarks>
        public bool BuildRepositories()
        {
            bool result = false;

            if (_repositoryService != null)
            {
                result = _repositoryService.BuildRepositories();
            }
            else
            {
                // TODO: Log that repository service is not present.
            }

            return result;
        }

        /// <summary>
        /// Deploys the repositories that have been recently updated and built.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if all repositories were deployed successfully; <c>false</c> if at least one repository failed to deploy.
        /// </returns>
        /// <seealso cref="BuildRepositories" />
        ///   <seealso cref="RepositoriesToDeploy" />
        /// <remarks>
        /// This method will not build the updated repositories. To do so, call <see cref="BuildRepositories" />.
        /// The list of repositories that will be deployed by this method is available through <see cref="RepositoriesToDeploy" />.
        /// </remarks>
        public bool DeployRepositories()
        {
            bool result = false;

            if (_repositoryService != null)
            {
                result = _repositoryService.DeployRepositories();
            }
            else
            {
                // TODO: Log that repository service is not present.
            }

            return result;
        }
    }
}
