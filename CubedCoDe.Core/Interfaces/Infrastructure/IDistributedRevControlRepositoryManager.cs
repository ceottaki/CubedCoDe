// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="IDistributedRevControlRepositoryManager.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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
    using CubedCoDe.Entities;

    /// <summary>
    /// Defines properties and methods for an implementation of a class that manages distributed revision control repositories.
    /// </summary>
    public interface IDistributedRevControlRepositoryManager : IDisposable
    {
        /// <summary>
        /// Gets all branches available.
        /// </summary>
        /// <remarks>
        /// This list contains all local branches and all remote branches if <see cref="DownloadReferencesFromRemote"/> has been called.
        /// </remarks>
        /// <value>A list of branches available.</value>
        /// <seealso cref="AllLocalBranches"/>
        /// <seealso cref="AllRemoteBranches"/>
        /// <seealso cref="DownloadReferencesFromRemote"/>
        IEnumerable<BranchEntity> AllBranches
        {
            get;
        }

        /// <summary>
        /// Gets all local branches.
        /// </summary>
        /// <value>A list of local branches available.</value>
        /// <seealso cref="AllRemoteBranches"/>
        /// <seealso cref="AllBranches"/>
        IEnumerable<BranchEntity> AllLocalBranches
        {
            get;
        }

        /// <summary>
        /// Gets all remote branches.
        /// </summary>
        /// <remarks>
        /// This list is only updated from the remote repository when <see cref="DownloadReferencesFromRemote"/> is called.
        /// </remarks>
        /// <value>A list of remote branches available.</value>
        /// <seealso cref="DownloadReferencesFromRemote"/>
        /// <seealso cref="AllLocalBranches"/>
        /// <seealso cref="AllBranches"/>
        IEnumerable<BranchEntity> AllRemoteBranches
        {
            get;
        }

        /// <summary>
        /// Gets the current state of the repository manager.
        /// </summary>
        /// <value>
        /// The current state of the repository manager.
        /// </value>
        RepositoryState State
        {
            get;
        }

        /// <summary>
        /// Gets the names of branches for which new references have been downloaded.
        /// </summary>
        /// <remarks>
        /// The value of this property is only updated through <see cref="DownloadReferencesFromRemote"/>.
        /// </remarks>
        /// <value>
        /// The names of branches for which new references have been downloaded.
        /// </value>
        /// <seealso cref="DownloadReferencesFromRemote"/>
        IList<string> BranchesWithNewReferences
        {
            get;
        }

        /// <summary>
        /// Opens a local repository present in the given path.
        /// </summary>
        /// <param name="localRepositoryPath">The local repository path.</param>
        /// <seealso cref="CloseRepository"/>
        /// <seealso cref="State"/>
        void OpenRepository(string localRepositoryPath);

        /// <summary>
        /// Closes the current repository.
        /// </summary>
        /// <remarks>
        /// This operation will only have an effect if the current <see cref="State"/> is either <see cref="RepositoryState.Opened"/> or <see cref="RepositoryState.Faulted"/>.
        /// An error will not be thrown if the current state is <see cref="RepositoryState.Closed"/>.
        /// </remarks>
        /// <seealso cref="OpenRepository"/>
        /// <seealso cref="State"/>
        void CloseRepository();

        /// <summary>
        /// Downloads the references from a remote repository.
        /// </summary>
        /// <param name="remoteRepositoryName">Name of the remote repository.</param>
        /// <remarks>The list of branches needing update will be updated on <see cref="BranchesWithNewReferences"/> when running this method.</remarks>
        /// <seealso cref="BranchesWithNewReferences"/>
        void DownloadReferencesFromRemote(string remoteRepositoryName);

        /// <summary>
        /// Gets a branch from its name.
        /// </summary>
        /// <remarks>
        /// If both local and remote branches exist with the same name, the local branch will be returned.
        /// </remarks>
        /// <param name="branchName">Name of the branch.</param>
        /// <returns>A branch with the name given if it exists; <c>null</c> otherwise.</returns>
        BranchEntity GetBranchFromName(string branchName);

        /// <summary>
        /// Gets the position of a local branch in relation to its tracked remote branch.
        /// </summary>
        /// <remarks>
        /// If the given local branch does not have a remote tracked branch associated with it the result will be zero.
        /// This function will try to match the local branch with a remote branch of the same name if the local branch doesn't have any tracking information.
        /// </remarks>
        /// <param name="localBranchName">Name of the local branch.</param>
        /// <returns>The number of commits the local branch is ahead or behind the remote branch. Numbers will be positive if local branch is ahead or negative if it is behind.</returns>
        /// <exception cref="BranchDoesNotExistException">When trying to get the relative position of a branch that does not exist.</exception>
        int GetRelativePositionOfBranch(string localBranchName);

        /// <summary>
        /// Creates a local branch with the given name.
        /// </summary>
        /// <remarks>
        /// If there is a remote branch with the exact same name, tracking information will be set up so that the newly created local branch will be tracking the remote branch.
        /// If a local branch already exists with the given name no branch will be created. The existing branch will be fetched and returned in the same manner of <see cref="GetBranchFromName"/>.
        /// </remarks>
        /// <param name="branchName">Name of the branch to be created.</param>
        /// <returns>The branch created.</returns>
        /// <seealso cref="RemoveBranch"/>
        BranchEntity CreateBranch(string branchName);

        /// <summary>
        /// Switches the local repository to point to the head of the branch with the given name.
        /// </summary>
        /// <param name="branchName">Name of the branch.</param>
        /// <exception cref="BranchDoesNotExistException">When trying to switch to a branch that does not exist.</exception>
        void SwitchBranch(string branchName);

        /// <summary>
        /// Switches the local repository to point to the head of the given branch.
        /// </summary>
        /// <remarks>
        /// If the branch given is a remote branch, a local branch that tracks the remote branch will be created using <see cref="CreateBranch"/> and switched to.
        /// </remarks>
        /// <param name="branch">The branch.</param>
        /// <exception cref="BranchDoesNotExistException">When trying to switch to a branch that does not exist.</exception>
        void SwitchBranch(BranchEntity branch);

        /// <summary>
        /// Switches the local repository to point to the head of the branch with the given name.
        /// </summary>
        /// <param name="branchName">Name of the branch.</param>
        /// <param name="force">if set to <c>true</c>, forces the switch ignoring any local changes.</param>
        void SwitchBranch(string branchName, bool force);

        /// <summary>
        /// Switches the local repository to point to the head of the given branch.
        /// </summary>
        /// <param name="branch">The branch.</param>
        /// <param name="force">if set to <c>true</c>, forces the switch ignoring any local changes.</param>
        /// <exception cref="BranchDoesNotExistException">When trying to switch to a branch that does not exist.</exception>
        /// <remarks>
        /// If the branch given is a remote branch, a local branch that tracks the remote branch will be created using <see cref="CreateBranch" /> and switched to.
        /// </remarks>
        void SwitchBranch(BranchEntity branch, bool force);

        /// <summary>
        /// Locally removes a branch.
        /// </summary>
        /// <param name="branch">The branch to be removed.</param>
        /// <seealso cref="CreateBranch"/>
        /// <exception cref="CubedCoDe.Core.BranchCannotBeRemovedException">
        /// The branch to be removed is a remote branch. Remote branches cannot be removed.
        /// or
        /// The branch to be removed is the current head of the repository and can't be removed.
        /// </exception>
        /// <exception cref="BranchDoesNotExistException">The branch to be removed does not exist in this repository.</exception>
        void RemoveBranch(BranchEntity branch);

        /// <summary>
        /// Reads the configuration from the given configuration file.
        /// </summary>
        /// <param name="pathToConfigurationFile">The path to configuration file.</param>
        /// <returns>A list of repository entities read from the given configuration file.</returns>
        /// <exception cref="System.IO.FileNotFoundException">When the configuration file does not exist.</exception>
        /// <exception cref="System.ArgumentNullException">When a path to the configuration file hasn't been provided.</exception>
        IList<RepositoryEntity> ReadConfigurationFromFile(string pathToConfigurationFile);

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
        void SaveConfigurationToFile(IList<RepositoryEntity> repositories, string pathToConfigurationFile);
    }
}