// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="DeploymentService.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using CubedCoDe.Core.Interfaces;
    using CubedCoDe.Core.Properties;
    using CubedCoDe.Entities;

    /// <summary>
    /// Represents a deployment service.
    /// </summary>
    public class DeploymentService : IDeploymentService
    {
        /// <summary>
        /// Pattern to be used to detect replacement tokens with regular expressions.
        /// </summary>
        private const string TokenPattern = @"\$\{.+\}";

        /// <summary>
        /// Flag that indicates if the current instance has been disposed or not.
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// The path to the configuration file.
        /// </summary>
        private string _pathToConfigurationFile = null;

        /// <summary>
        /// The list of repositories in the configuration file.
        /// </summary>
        private IList<RepositoryEntity> _repositories = new List<RepositoryEntity>();

        /// <summary>
        /// The repository manager.
        /// </summary>
        private IDistributedRevControlRepositoryManager _repositoryManager;

        /// <summary>
        /// The project builder.
        /// </summary>
        private IProjectBuilder _projectBuilder;

        /// <summary>
        /// The deployment manager.
        /// </summary>
        private IDeploymentManager _deploymentManager;

        /// <summary>
        /// The exception manager.
        /// </summary>
        private IExceptionManager _exceptionManager;

        /// <summary>
        /// Initialises a new instance of the <see cref="DeploymentService" /> class.
        /// </summary>
        /// <param name="pathToConfigurationFile">The path to configuration file.</param>
        /// <param name="repositoryManager">The repository manager.</param>
        /// <param name="projectBuilder">The project builder.</param>
        /// <param name="deploymentManager">The deployment manager.</param>
        /// <param name="exceptionManager">The exception manager.</param>
        /// <exception cref="System.ArgumentNullException">pathToConfigurationFile; A valid path to the configuration file has not been provided.</exception>
        public DeploymentService(string pathToConfigurationFile, IDistributedRevControlRepositoryManager repositoryManager, IProjectBuilder projectBuilder, IDeploymentManager deploymentManager, IExceptionManager exceptionManager)
            : this(pathToConfigurationFile, true, repositoryManager, projectBuilder, deploymentManager, exceptionManager)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DeploymentService" /> class.
        /// </summary>
        /// <param name="pathToConfigurationFile">The path to configuration file.</param>
        /// <param name="readConfigurationAndCheckForUpdates">if set to <c>true</c> [read configuration and check for updates].</param>
        /// <param name="repositoryManager">The repository manager.</param>
        /// <param name="projectBuilder">The project builder.</param>
        /// <param name="deploymentManager">The deployment manager.</param>
        /// <param name="exceptionManager">The exception manager.</param>
        /// <exception cref="System.ArgumentNullException">pathToConfigurationFile; A valid path to the configuration file has not been provided.</exception>
        public DeploymentService(string pathToConfigurationFile, bool readConfigurationAndCheckForUpdates, IDistributedRevControlRepositoryManager repositoryManager, IProjectBuilder projectBuilder, IDeploymentManager deploymentManager, IExceptionManager exceptionManager)
        {
            _repositoryManager = repositoryManager;
            _projectBuilder = projectBuilder;
            _deploymentManager = deploymentManager;
            _exceptionManager = exceptionManager;

            if (pathToConfigurationFile == null)
            {
                throw new ArgumentNullException("pathToConfigurationFile", "A valid path to the configuration file has not been provided.");
            }
            else
            {
                _pathToConfigurationFile = pathToConfigurationFile;
                if (readConfigurationAndCheckForUpdates)
                {
                    ReadConfigurationFromFile();
                    CheckRepositoriesForUpdates();
                }
            }
        }

        /// <summary>
        /// Finalises an instance of the <see cref="DeploymentService"/> class.
        /// </summary>
        ~DeploymentService()
        {
            Dispose(false);
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

                if (_repositories != null)
                {
                    result = _repositories;
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

                if (_repositories != null)
                {
                    _repositories.Where(r => r.IsNeedingUpdate).ToList().ForEach(r => result.Add(r));
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

                if (_repositories != null)
                {
                    _repositories.Where(r => r.IsNeedingBuilding).ToList().ForEach(r => result.Add(r));
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

                if (_repositories != null)
                {
                    _repositories.Where(r => r.IsNeedingDeployment).ToList().ForEach(r => result.Add(r));
                }

                return result;
            }
        }

        /// <summary>
        /// Reads the configuration from the configuration file and updates the values in the model to use the new configuration.
        /// </summary>
        public void ReadConfigurationFromFile()
        {
            if (_repositoryManager != null)
            {
                ////try
                ////{
                    if (_pathToConfigurationFile != null)
                    {
                        _repositories = _exceptionManager.Process(() => _repositoryManager.ReadConfigurationFromFile(_pathToConfigurationFile), Resources.DefaultExceptionHandlingPolicyName);
                        ////_repositories = _repositoryManager.ReadConfigurationFromFile(_pathToConfigurationFile);
                    }
                ////}
                ////catch
                ////{
                ////    throw;
                ////}
            }
        }

        /// <summary>
        /// Saves the configuration to the configuration file with updated values.
        /// </summary>
        public void SaveConfigurationToFile()
        {
            if (_repositoryManager != null)
            {
                ////try
                ////{
                    if (_pathToConfigurationFile != null)
                    {
                        _exceptionManager.Process(() => _repositoryManager.SaveConfigurationToFile(_repositories, _pathToConfigurationFile), Resources.DefaultExceptionHandlingPolicyName);
                        ////_repositoryManager.SaveConfigurationToFile(_repositories, _pathToConfigurationFile);
                    }
                ////}
                ////catch
                ////{
                ////    throw;
                ////}
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

            if ((_repositoryManager != null) && (_repositories != null))
            {
                // TODO: 
                // Goes through all repositories available, opens them, downloads references and check if deployment branches need updating.
                foreach (RepositoryEntity repo in _repositories)
                {
                    // Checks if repository needs checking.
                    if (repo.LastUpdateTime.Add(repo.CheckForUpdatesTimeSpan).CompareTo(DateTime.Now) <= 0)
                    {
                        // Opens current repository.
                        _exceptionManager.Process(() => _repositoryManager.OpenRepository(repo.LocationPath), Resources.DefaultExceptionHandlingPolicyName);

                        // Checks if repository was successfully opened and download references.
                        if (_repositoryManager.State == RepositoryState.Opened)
                        {
                            _exceptionManager.Process(
                                () =>
                                {
                                    _repositoryManager.DownloadReferencesFromRemote(repo.RemoteName);
                                    repo.IsNeedingUpdate = _repositoryManager.BranchesWithNewReferences.Contains(repo.DeploymentBranchName)
                                                            || (_repositoryManager.GetRelativePositionOfBranch(repo.DeploymentBranchName) < 0)
                                                            || ((!_repositoryManager.AllLocalBranches.Any(b => b.Name == repo.DeploymentBranchName)) && (_repositoryManager.AllRemoteBranches.Any(b => b.Name == repo.DeploymentBranchName) || _repositoryManager.AllRemoteBranches.Any(b => b.Name == string.Format(CultureInfo.InvariantCulture, "{0}/{1}", repo.RemoteName, repo.DeploymentBranchName))));
                                    repo.LastUpdateTime = DateTime.Now;
                                },
                                Resources.DefaultExceptionHandlingPolicyName);
                        }

                        _repositoryManager.CloseRepository();
                    }
                }

                result = _repositories.Any(r => r.IsNeedingUpdate);

                // Saves current configuration to file so that changes to the IsNeedingUpdate flag are not lost if the application exits.
                SaveConfigurationToFile();
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
        /// Please take into careful consideration that this method will discard any local changes made to the repository.
        /// </remarks>
        public void UpdateRepositories()
        {
            if (_repositoryManager != null && _repositories != null && _repositories.Any(r => r.IsNeedingUpdate))
            {
                foreach (RepositoryEntity repository in _repositories.Where(r => r.IsNeedingUpdate))
                {
                    _exceptionManager.Process(() => _repositoryManager.OpenRepository(repository.LocationPath), Resources.DefaultExceptionHandlingPolicyName);
                    BranchEntity deploymentBranch = _repositoryManager.AllLocalBranches.FirstOrDefault(b => b.Name == repository.DeploymentBranchName);
                    BranchEntity tempBranch = null;
                    if (deploymentBranch != null)
                    {
                        // A local branch exists. Checks if it is the current head of the repository.
                        if (deploymentBranch.IsCurrentHead)
                        {
                            // Deployment branch is the current head of the repository. Can't remove current head, so create a temporary branch and switch to it.
                            string tempBranchName = GetTemporaryBranchName();

                            tempBranch = _exceptionManager.Process(() => _repositoryManager.CreateBranch(tempBranchName), Resources.DefaultExceptionHandlingPolicyName);
                            _exceptionManager.Process(() => _repositoryManager.SwitchBranch(tempBranch, true), Resources.DefaultExceptionHandlingPolicyName);
                        }

                        // Removes local deployment branch.
                        _exceptionManager.Process(() => _repositoryManager.RemoveBranch(deploymentBranch), Resources.DefaultExceptionHandlingPolicyName);
                    }

                    // Creates the local deployment branch again, which will download new information from the remote branch, then switch to it.
                    deploymentBranch = _exceptionManager.Process(() => _repositoryManager.CreateBranch(repository.DeploymentBranchName), Resources.DefaultExceptionHandlingPolicyName);
                    _exceptionManager.Process(() => _repositoryManager.SwitchBranch(deploymentBranch, true), Resources.DefaultExceptionHandlingPolicyName);

                    // Removes the temporary branch if one has been created.
                    _exceptionManager.Process(() => _repositoryManager.RemoveBranch(tempBranch), Resources.DefaultExceptionHandlingPolicyName);

                    // Updates repository's flags.
                    repository.IsNeedingUpdate = false;
                    repository.IsNeedingBuilding = true;

                    // Saves current configuration to file so that changes to the IsNeedingUpdate and IsNeedingBuilding flags are not lost if the application exits.
                    SaveConfigurationToFile();

                    _repositoryManager.CloseRepository();
                }
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

            if (_projectBuilder != null && _repositories != null && _repositories.Any(r => r.IsNeedingBuilding))
            {
                result = true;
                foreach (RepositoryEntity repository in _repositories.Where(r => r.IsNeedingBuilding))
                {
                    bool hasSolutionBeenSuccessfullyLoaded = _exceptionManager.Process(() => _projectBuilder.LoadSolutionFile(repository.SolutionFileToBuild), false, Resources.DefaultExceptionHandlingPolicyName);
                    if (hasSolutionBeenSuccessfullyLoaded)
                    {
                        bool hasSolutionBeenSuccessfullyBuilt = _exceptionManager.Process(() => _projectBuilder.BuildSolution(repository.SolutionConfigurationToBuild), false, Resources.DefaultExceptionHandlingPolicyName);
                        if (hasSolutionBeenSuccessfullyBuilt)
                        {
                            repository.IsNeedingBuilding = false;
                            repository.IsNeedingDeployment = true;

                            // Saves current configuration to file so that changes to the IsNeedingBuilding and IsNeedingDeployment flags are not lost if the application exits.
                            SaveConfigurationToFile();
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
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
        /// <seealso cref="RepositoriesToDeploy" />
        /// <remarks>
        /// This method will not build the updated repositories. To do so, call <see cref="BuildRepositories" />.
        /// The list of repositories that will be deployed by this method is available through <see cref="RepositoriesToDeploy" />.
        /// </remarks>
        public bool DeployRepositories()
        {
            bool result = true;
            if (_deploymentManager != null && _repositories != null && _repositories.Any(r => r.IsNeedingDeployment))
            {
                foreach (RepositoryEntity repository in _repositories.Where(r => r.IsNeedingDeployment))
                {
                    IDictionary<string, string> replacementsTable = BuildReplacementsTable(repository);
                    foreach (DeploymentActionEntity deploymentAction in repository.DeploymentActions)
                    {
                        // Replaces tokens in action parameters.
                        for (int i = 0; i < deploymentAction.ActionParameters.Count; i++)
                        {
                            foreach (KeyValuePair<string, string> replacement in replacementsTable)
                            {
                                deploymentAction.ActionParameters[i] = deploymentAction.ActionParameters[i].Replace(replacement.Key, replacement.Value);
                            }
                        }

                        // Executes deployment action.
                        bool currentActionResult = _exceptionManager.Process(() => _deploymentManager.ExecuteDeploymentAction(deploymentAction.ActionType, deploymentAction.ActionParameters.ToArray()), false, Resources.DefaultExceptionHandlingPolicyName);
                        result = result && currentActionResult;
                    }

                    repository.IsNeedingDeployment = !result;

                    // Saves current configuration to file so that changes to the IsNeedingDeployment flag are not lost if the application exits.
                    SaveConfigurationToFile();
                }
            }

            return result;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    _pathToConfigurationFile = null;
                    _repositories = null;
                    _repositoryManager = null;
                    _projectBuilder = null;
                }

                // Clean-up unmanaged resources.
                _disposed = true;
            }
        }

        /// <summary>
        /// Builds the replacements table for tokens and values based on the given repository.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <returns>A dictionary of tokens and values to replace the tokens.</returns>
        private static IDictionary<string, string> BuildReplacementsTable(RepositoryEntity repository)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            result.Add("${REPO_FOLDER}", repository.LocationPath);
            result.Add("${WIN_FOLDER}", Environment.SystemDirectory);

            return result;
        }

        /// <summary>
        /// Gets the name of the temporary branch.
        /// </summary>
        /// <returns>A valid and currently inexistent temporary branch name for the currently opened repository.</returns>
        private string GetTemporaryBranchName()
        {
            if (_repositoryManager == null)
            {
                return null;
            }
            else
            {
                int nameIndex = 1;
                string tempBranchName = string.Format(CultureInfo.CurrentCulture, "TempBranch{0}", nameIndex);

                while (_repositoryManager.AllBranches.Any(b => b.Name == tempBranchName))
                {
                    nameIndex++;
                    tempBranchName = string.Format(CultureInfo.CurrentCulture, "TempBranch{0}", nameIndex);
                }

                return tempBranchName;
            }
        }
    }
}
