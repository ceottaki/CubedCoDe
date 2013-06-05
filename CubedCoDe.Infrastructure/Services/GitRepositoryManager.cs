// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="GitRepositoryManager.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using CubedCoDe.Core;
    using CubedCoDe.Core.Interfaces;
    using CubedCoDe.Entities;
    using CubedCoDe.Infrastructure.Properties;
    using LibGit2Sharp;

    /// <summary>
    /// Represents a Git repository manager.
    /// </summary>
    public class GitRepositoryManager : IDistributedRevControlRepositoryManager
    {
        /// <summary>
        /// Flag that indicates if the current instance has been disposed or not.
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// The current repository.
        /// </summary>
        private Repository _currentRepository = null;

        /// <summary>
        /// The current state of the repository manager.
        /// </summary>
        private RepositoryState _state = RepositoryState.Closed;

        /// <summary>
        /// A list of branches needing update.
        /// </summary>
        private IList<string> _branchesNeedingUpdate = new List<string>();

        /// <summary>
        /// The name of the remote.
        /// </summary>
        private string _remoteName = null;

        /// <summary>
        /// Finalises an instance of the <see cref="GitRepositoryManager"/> class.
        /// </summary>
        ~GitRepositoryManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the current state of the repository manager.
        /// </summary>
        /// <value>
        /// The current state of the repository manager.
        /// </value>
        public RepositoryState State
        {
            get
            {
                return _state;
            }
        }

        /// <summary>
        /// Gets the branches needing update.
        /// </summary>
        /// <value>
        /// The branches needing update.
        /// </value>
        /// <seealso cref="DownloadReferencesFromRemote" />
        /// <remarks>
        /// The value of this property is only updated through <see cref="DownloadReferencesFromRemote" />.
        /// </remarks>
        public IList<string> BranchesWithNewReferences
        {
            get
            {
                return _branchesNeedingUpdate;
            }
        }

        /// <summary>
        /// Gets all branches available.
        /// </summary>
        /// <value>
        /// A list of branches available.
        /// </value>
        /// <seealso cref="AllLocalBranches" />
        ///   <seealso cref="AllRemoteBranches" />
        ///   <seealso cref="DownloadReferencesFromRemote" />
        /// <remarks>
        /// This list contains all local branches and all remote branches if <see cref="DownloadReferencesFromRemote" /> has been called.
        /// </remarks>
        public IEnumerable<BranchEntity> AllBranches
        {
            get
            {
                IList<BranchEntity> result = new List<BranchEntity>();

                if ((_currentRepository != null) && (_state == RepositoryState.Opened))
                {
                    foreach (Branch branch in _currentRepository.Branches)
                    {
                        result.Add(new BranchEntity() { Name = branch.Name, IsRemote = branch.IsRemote, IsCurrentHead = branch.IsCurrentRepositoryHead });
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets all local branches.
        /// </summary>
        /// <value>
        /// A list of local branches available.
        /// </value>
        /// <seealso cref="AllRemoteBranches" />
        /// <seealso cref="AllBranches" />
        public IEnumerable<BranchEntity> AllLocalBranches
        {
            get
            {
                IList<BranchEntity> result = new List<BranchEntity>();

                if ((_currentRepository != null) && (_state == RepositoryState.Opened))
                {
                    foreach (Branch branch in _currentRepository.Branches.Where(b => !b.IsRemote))
                    {
                        result.Add(new BranchEntity() { Name = branch.Name, IsRemote = branch.IsRemote, IsCurrentHead = branch.IsCurrentRepositoryHead });
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets all remote branches.
        /// </summary>
        /// <value>
        /// A list of remote branches available.
        /// </value>
        /// <seealso cref="DownloadReferencesFromRemote" />
        ///   <seealso cref="AllLocalBranches" />
        ///   <seealso cref="AllBranches" />
        /// <remarks>
        /// This list is only updated from the remote repository when <see cref="DownloadReferencesFromRemote" /> is called.
        /// </remarks>
        public IEnumerable<BranchEntity> AllRemoteBranches
        {
            get
            {
                IList<BranchEntity> result = new List<BranchEntity>();

                if ((_currentRepository != null) && (_state == RepositoryState.Opened))
                {
                    foreach (Branch branch in _currentRepository.Branches.Where(b => b.IsRemote))
                    {
                        result.Add(new BranchEntity() { Name = branch.Name, IsRemote = branch.IsRemote, IsCurrentHead = branch.IsCurrentRepositoryHead });
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Opens a local repository present in the given path.
        /// </summary>
        /// <param name="localRepositoryPath">The local repository path.</param>
        /// <seealso cref="CloseRepository" />
        /// <seealso cref="State" />
        public void OpenRepository(string localRepositoryPath)
        {
            try
            {
                _currentRepository = new Repository(localRepositoryPath);
                if (_currentRepository != null)
                {
                    _state = RepositoryState.Opened;
                }
                else
                {
                    _state = RepositoryState.Unknown;
                }
            }
            catch
            {
                _state = RepositoryState.Faulted;
                throw;
            }
        }

        /// <summary>
        /// Closes the current repository.
        /// </summary>
        /// <seealso cref="OpenRepository" />
        /// <seealso cref="State" />
        /// <remarks>
        /// This operation will only have an effect if the current <see cref="State" /> is either <see cref="RepositoryState.Opened" /> or <see cref="RepositoryState.Faulted" />.
        /// An error will not be thrown if the current state is <see cref="RepositoryState.Closed" />.
        /// </remarks>
        public void CloseRepository()
        {
            if (_currentRepository != null)
            {
                _currentRepository.Dispose();
            }

            _state = RepositoryState.Closed;
        }

        /// <summary>
        /// Downloads the references from a remote repository.
        /// </summary>
        /// <param name="remoteRepositoryName">Name of the remote repository.</param>
        /// <seealso cref="BranchesWithNewReferences" />
        /// <remarks>
        /// The list of branches needing update will be updated on <see cref="BranchesWithNewReferences" /> when running this method.
        /// </remarks>
        public void DownloadReferencesFromRemote(string remoteRepositoryName)
        {
            if ((_currentRepository != null) && (_state == RepositoryState.Opened))
            {
                _branchesNeedingUpdate.Clear();
                _currentRepository.Fetch(remoteRepositoryName, onUpdateTips: RemoteUpdateTipsHandler);
                _remoteName = remoteRepositoryName;
            }
        }

        /// <summary>
        /// Gets a branch from its name.
        /// </summary>
        /// <param name="branchName">Name of the branch.</param>
        /// <remarks>
        /// If both local and remote branches exist with the same name, the local branch will be returned.
        /// </remarks>
        /// <returns>
        /// A branch with the name given if it exists; <c>null</c> otherwise.
        /// </returns>
        public BranchEntity GetBranchFromName(string branchName)
        {
            BranchEntity result = null;
            
            if ((_currentRepository != null) && (_state == RepositoryState.Opened))
            {
                Branch branchFromName = _currentRepository.Branches.FirstOrDefault(b => b.Name == branchName);
                if (branchFromName == null)
                {
                    // Could not find branch from the name given, will try with remote name.
                    string remoteBranchName = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", _remoteName, branchName);
                    branchFromName = _currentRepository.Branches.FirstOrDefault(b => b.Name == remoteBranchName);
                }

                if (branchFromName != null)
                {
                    result = new BranchEntity();
                    result.Name = branchFromName.Name;
                    result.IsRemote = branchFromName.IsRemote;
                    result.IsCurrentHead = branchFromName.IsCurrentRepositoryHead;
                }
            }
            
            return result;
        }

        /// <summary>
        /// Gets the position of a local branch in relation to its tracked remote branch.
        /// </summary>
        /// <param name="localBranchName">Name of the local branch.</param>
        /// <returns>
        /// The number of commits the local branch is ahead or behind the remote branch. Numbers will be positive if local branch is ahead or negative if it is behind.
        /// </returns>
        /// <remarks>
        /// If the given local branch does not have a remote tracked branch associated with it the result will be zero.
        /// This function will try to match the local branch with a remote branch of the same name if the local branch doesn't have any tracking information.
        /// </remarks>
        public int GetRelativePositionOfBranch(string localBranchName)
        {
            int result = 0;

            if ((_currentRepository != null) && (_currentRepository.Branches != null))
            {
                Branch localBranch = _currentRepository.Branches.FirstOrDefault(b => b.Name == localBranchName && !b.IsRemote);
                if (localBranch != null)
                {
                    if (localBranch.IsTracking)
                    {
                        if (localBranch.TrackingDetails.AheadBy.HasValue && (localBranch.TrackingDetails.AheadBy.Value != 0))
                        {
                            result = Math.Abs(localBranch.TrackingDetails.AheadBy.Value);
                        }
                        else if (localBranch.TrackingDetails.BehindBy.HasValue && (localBranch.TrackingDetails.BehindBy.Value != 0))
                        {
                            result = Math.Abs(localBranch.TrackingDetails.BehindBy.Value) * -1;
                        }
                    }
                    else
                    {
                        string remoteBranchName = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", _remoteName, localBranchName);
                        Branch remoteBranch = _currentRepository.Branches.FirstOrDefault(b => (b.Name == localBranchName || b.Name == remoteBranchName) && b.IsRemote);
                        if (remoteBranch != null)
                        {
                            // Sets up tracking information between local branch and remote branch.
                            _currentRepository.Config.Set<string>(string.Format(CultureInfo.InvariantCulture, "branch.{0}.remote", localBranchName), _remoteName);
                            _currentRepository.Config.Set<string>(string.Format(CultureInfo.InvariantCulture, "branch.{0}.merge", localBranchName), string.Format(CultureInfo.InvariantCulture, "refs/heads/{0}", localBranchName));

                            // Gets the result now that tracking has been properly set up.
                            result = GetRelativePositionOfBranch(localBranchName);

                            // Removes tracking configuration as it wasn't there before and it was set up just to facilitate comparing branches.
                            _currentRepository.Config.Unset(string.Format(CultureInfo.InvariantCulture, "branch.{0}.remote", localBranchName));
                            _currentRepository.Config.Unset(string.Format(CultureInfo.InvariantCulture, "branch.{0}.merge", localBranchName));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a local branch with the given name.
        /// </summary>
        /// <param name="branchName">Name of the branch to be created.</param>
        /// <returns>
        /// The branch created.
        /// </returns>
        /// <seealso cref="RemoveBranch" />
        /// <remarks>
        /// If there is a remote branch with the exact same name, tracking information will be set up so that the newly created local branch will be tracking the remote branch.
        /// If a local branch already exists with the given name no branch will be created. The existing branch will be fetched and returned in the same manner of <see cref="GetBranchFromName" />.
        /// Once the branch has been created the repository will be switched to point at that branch in the same manner of <see cref="SwitchBranch(string)"/>.
        /// </remarks>
        public BranchEntity CreateBranch(string branchName)
        {
            BranchEntity result = null;

            if ((_currentRepository != null) && (_state == RepositoryState.Opened))
            {
                // Checks if a local branch already exists with the given name and in that case simply gets that branch and returns it.
                if (_currentRepository.Branches.Any(b => b.Name == branchName && !b.IsRemote))
                {
                    result = GetBranchFromName(branchName);
                    SwitchBranch(result);
                }
                else
                {
                    // A local branch doesn't already exist. Gets the remote branch of same name if it exists.
                    BranchEntity remoteBranch = GetBranchFromName(branchName);
                    
                    // Creates the new local branch.
                    Branch newBranch;
                    if (remoteBranch != null)
                    {
                        newBranch = _currentRepository.CreateBranch(branchName, _currentRepository.Branches.First(b => b.Name == remoteBranch.Name && b.IsRemote).Commits.First());
                    }
                    else
                    {
                        newBranch = _currentRepository.CreateBranch(branchName);
                    }

                    if (newBranch != null)
                    {
                        // Sets up tracking information between local branch and remote branch if remote branch exists.
                        if ((remoteBranch != null) && remoteBranch.IsRemote)
                        {
                            _currentRepository.Config.Set<string>(string.Format(CultureInfo.InvariantCulture, "branch.{0}.remote", branchName), _remoteName);
                            _currentRepository.Config.Set<string>(string.Format(CultureInfo.InvariantCulture, "branch.{0}.merge", branchName), string.Format(CultureInfo.InvariantCulture, "refs/heads/{0}", branchName));
                        }

                        result = new BranchEntity();
                        result.Name = newBranch.Name;
                        result.IsRemote = newBranch.IsRemote;
                        result.IsCurrentHead = true;

                        // Switches to the newly created branch and populate result.
                        try
                        {
                            _currentRepository.Checkout(newBranch);
                        }
                        catch (MergeConflictException)
                        {
                            result.IsCurrentHead = false;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Switches the local repository to point to the head of the branch with the given name.
        /// </summary>
        /// <param name="branchName">Name of the branch.</param>
        public void SwitchBranch(string branchName)
        {
            SwitchBranch(branchName, false);
        }

        /// <summary>
        /// Switches the local repository to point to the head of the given branch.
        /// </summary>
        /// <param name="branch">The branch.</param>
        /// <exception cref="BranchDoesNotExistException">Branch does not exist in the currently opened repository.</exception>
        /// <remarks>
        /// If the branch given is a remote branch, a local branch that tracks the remote branch will be created using <see cref="CreateBranch" /> and switched to.
        /// </remarks>
        public void SwitchBranch(BranchEntity branch)
        {
            SwitchBranch(branch, false);
        }

        /// <summary>
        /// Switches the local repository to point to the head of the branch with the given name.
        /// </summary>
        /// <param name="branchName">Name of the branch.</param>
        /// <param name="force">if set to <c>true</c>, forces the switch ignoring any local changes.</param>
        public void SwitchBranch(string branchName, bool force)
        {
            SwitchBranch(GetBranchFromName(branchName), force);
        }

        /// <summary>
        /// Switches the local repository to point to the head of the given branch.
        /// </summary>
        /// <param name="branch">The branch.</param>
        /// <param name="force">if set to <c>true</c>, forces the switch ignoring any local changes.</param>
        /// <exception cref="BranchDoesNotExistException">Branch does not exist in the currently opened repository.</exception>
        /// <remarks>
        /// If the branch given is a remote branch, a local branch that tracks the remote branch will be created using <see cref="CreateBranch" /> and switched to.
        /// </remarks>
        public void SwitchBranch(BranchEntity branch, bool force)
        {
            if ((_currentRepository != null) && (_state == RepositoryState.Opened) && (branch != null))
            {
                if (!branch.IsRemote)
                {
                    Branch branchToSwitchTo = _currentRepository.Branches.FirstOrDefault(b => b.Name == branch.Name && !b.IsRemote);
                    if (branchToSwitchTo != null)
                    {
                        if (force)
                        {
                            _currentRepository.Checkout(branchToSwitchTo, CheckoutOptions.Force, null);
                        }
                        else
                        {
                            _currentRepository.Checkout(branchToSwitchTo);
                        }
                    }
                    else
                    {
                        throw new BranchDoesNotExistException(string.Format(CultureInfo.CurrentCulture, Resources.BranchDoesNotExistErrorMessage, branch.Name));
                    }
                }
                else
                {
                    string remoteBranchName = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", _remoteName, branch.Name);
                    Branch branchToSwitchTo = _currentRepository.Branches.FirstOrDefault(b => (b.Name == branch.Name || b.Name == remoteBranchName) && b.IsRemote);
                    if (branchToSwitchTo != null)
                    {
                        CreateBranch(branchToSwitchTo.Name);
                    }
                    else
                    {
                        throw new BranchDoesNotExistException(string.Format(CultureInfo.CurrentCulture, Resources.BranchDoesNotExistErrorMessage, branch.Name));
                    }
                }
            }
        }

        /// <summary>
        /// Locally removes a branch.
        /// </summary>
        /// <param name="branch">The branch to be removed.</param>
        /// <seealso cref="CreateBranch" />
        /// <exception cref="CubedCoDe.Core.BranchCannotBeRemovedException">
        /// The branch to be removed is a remote branch. Remote branches cannot be removed.
        /// or
        /// The branch to be removed is the current head of the repository and can't be removed.
        /// </exception>
        /// <exception cref="BranchDoesNotExistException">The branch to be removed does not exist in this repository.</exception>
        public void RemoveBranch(BranchEntity branch)
        {
            if ((_currentRepository != null) && (_state == RepositoryState.Opened) && (branch != null))
            {
                if (branch.IsRemote)
                {
                    throw new BranchCannotBeRemovedException("The branch to be removed is a remote branch. Remote branches cannot be removed.");
                }
                else if (branch.IsCurrentHead)
                {
                    throw new BranchCannotBeRemovedException("The branch to be removed is the current head of the repository and can't be removed.");
                }
                else
                {
                    Branch branchToRemove = _currentRepository.Branches.FirstOrDefault(b => b.Name == branch.Name && !b.IsRemote);
                    if (branchToRemove == null)
                    {
                        throw new BranchDoesNotExistException("The branch to be removed does not exist in this repository.");
                    }
                    else
                    {
                        if (branchToRemove.IsCurrentRepositoryHead)
                        {
                            throw new BranchCannotBeRemovedException("The branch to be removed is the current head of the repository and can't be removed.");
                        }
                        else
                        {
                            _currentRepository.Branches.Remove(branchToRemove);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reads the configuration from the given configuration file.
        /// </summary>
        /// <param name="pathToConfigurationFile">The path to configuration file.</param>
        /// <returns>
        /// A list of repository entities read from the given configuration file.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">pathToConfigurationFile; A valid path to the configuration file has not been provided.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The configuration file does not exist.</exception>
        public IList<RepositoryEntity> ReadConfigurationFromFile(string pathToConfigurationFile)
        {
            IList<RepositoryEntity> result = null;

            if (pathToConfigurationFile == null)
            {
                throw new ArgumentNullException("pathToConfigurationFile", "A valid path to the configuration file has not been provided.");
            }
            else
            {
                if (!File.Exists(pathToConfigurationFile))
                {
                    throw new FileNotFoundException("The configuration file does not exist.", pathToConfigurationFile);
                }
                else
                {
                    XmlSerializer xmlSerialiser = new XmlSerializer(typeof(List<RepositoryEntity>));
                    using (Stream fileStream = File.OpenRead(pathToConfigurationFile))
                    {
                        result = xmlSerialiser.Deserialize(fileStream) as List<RepositoryEntity>;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Saves the configuration of repositories to a configuration file with the given path.
        /// </summary>
        /// <param name="repositories">The repositories.</param>
        /// <param name="pathToConfigurationFile">The path to the configuration file.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <c>repositories</c>; A valid list of repositories has not been provided.
        /// or
        /// <c>pathToConfigurationFile</c>; A valid path to the configuration file has not been provided.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission or <c>pathToConfigurationFile</c> specified a read-only file or directory.</exception>
        /// <exception cref="System.ArgumentException"><c>pathToConfigurationFile</c> is a zero-length string, contains only white space, or contains one of more invalid characters as per <see cref="System.IO.Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="System.NotSupportedException"><c>pathToConfigurationFile</c></exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path and file name in <c>pathToConfigurationFile</c> exceeds the system-defined maximum length.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path within <c>pathToConfigurationFile</c> is invalid.</exception>
        /// <exception cref="System.InvalidOperationException">An error occurred during serialisation of the configuration contained in <c>repositories</c>.</exception>
        public void SaveConfigurationToFile(IList<RepositoryEntity> repositories, string pathToConfigurationFile)
        {
            if (repositories == null)
            {
                throw new ArgumentNullException("repositories", "A valid list of repositories has not been provided.");
            }
            else if (pathToConfigurationFile == null)
            {
                throw new ArgumentNullException("pathToConfigurationFile", "A valid path to the configuration file has not been provided.");
            }
            else
            {
                try
                {
                    XmlSerializer xmlSerialiser = new XmlSerializer(typeof(List<RepositoryEntity>));
                    using (Stream fileStream = File.OpenWrite(pathToConfigurationFile))
                    {
                        xmlSerialiser.Serialize(fileStream, repositories);
                    }
                }
                catch
                {
                    throw;
                }
            }
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
                    if (_currentRepository != null)
                    {
                        _currentRepository.Dispose();
                    }

                    _branchesNeedingUpdate = null;
                    _remoteName = null;
                }

                // Clean-up unmanaged resources.
                _disposed = true;
            }
        }

        /// <summary>
        /// Handles an update from the remote.
        /// </summary>
        /// <param name="referenceName">Name of the reference.</param>
        /// <param name="oldId">The old id.</param>
        /// <param name="newId">The new id.</param>
        /// <returns>A success code (zero).</returns>
        private int RemoteUpdateTipsHandler(string referenceName, ObjectId oldId, ObjectId newId)
        {
            _branchesNeedingUpdate.Add(referenceName);
            return 0;
        }
    }
}