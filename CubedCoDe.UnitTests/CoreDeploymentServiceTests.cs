// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="CoreDeploymentServiceTests.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CubedCoDe.Core.Interfaces;
    using CubedCoDe.Core.Services;
    using CubedCoDe.Entities;
    using Moq;
    using NUnit.Framework;

    /// <summary>
    /// Represents a test fixture for the core deployment service.
    /// </summary>
    [TestFixture]
    public class CoreDeploymentServiceTests : IDisposable
    {
        /// <summary>
        /// The deployment branch name to use for testing.
        /// </summary>
        private const string DeploymentBranchName = "Live";

        /// <summary>
        /// The name of the configuration file to use for testing.
        /// </summary>
        private const string ConfigurationFileName = @"C:\FakeConfigFile.xml";

        /// <summary>
        /// Flag that indicates if the current instance has been disposed or not.
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// The deployment service to be tested.
        /// </summary>
        private IDeploymentService _deploymentService;

        /// <summary>
        /// A mock of the repository manager.
        /// </summary>
        private Mock<IDistributedRevControlRepositoryManager> _repositoryManagerMock;

        /// <summary>
        /// A mock of the project builder.
        /// </summary>
        private Mock<IProjectBuilder> _projectBuilderMock;

        /// <summary>
        /// A mock of the deployment manager.
        /// </summary>
        private Mock<IDeploymentManager> _deploymentManagerMock;

        /// <summary>
        /// A mock of the exception manager.
        /// </summary>
        private Mock<IExceptionManager> _exceptionManagerMock;

        /// <summary>
        /// Initialises this instance of the test fixture before running every test.
        /// </summary>
        [SetUp]
        public void Init()
        {
            _disposed = false;

            _repositoryManagerMock = new Mock<IDistributedRevControlRepositoryManager>();
            _repositoryManagerMock.Setup(rm => rm.State).Returns(RepositoryState.Closed);
            _repositoryManagerMock.Setup(rm => rm.AllBranches).Returns(new List<BranchEntity>() { new BranchEntity() { Name = "Live", IsRemote = false, IsCurrentHead = true }, new BranchEntity() { Name = "Live", IsRemote = true, IsCurrentHead = false } });
            _repositoryManagerMock.Setup(rm => rm.AllLocalBranches).Returns(_repositoryManagerMock.Object.AllBranches.ToList().Where(b => !b.IsRemote));
            _repositoryManagerMock.Setup(rm => rm.AllRemoteBranches).Returns(_repositoryManagerMock.Object.AllBranches.ToList().Where(b => b.IsRemote));
            _repositoryManagerMock.Setup(rm => rm.ReadConfigurationFromFile(It.IsAny<string>())).Returns(GetMockConfiguration());
            _repositoryManagerMock.Setup(rm => rm.OpenRepository(It.IsAny<string>())).Callback(() => _repositoryManagerMock.Setup(rm => rm.State).Returns(RepositoryState.Opened));
            _repositoryManagerMock.Setup(rm => rm.CloseRepository()).Callback(() => _repositoryManagerMock.Setup(rm => rm.State).Returns(RepositoryState.Closed));
            _repositoryManagerMock.Setup(rm => rm.DownloadReferencesFromRemote(It.IsAny<string>())).Callback(() => _repositoryManagerMock.Setup(rm => rm.BranchesWithNewReferences).Returns(new List<string>() { DeploymentBranchName }));

            _projectBuilderMock = new Mock<IProjectBuilder>();
            _projectBuilderMock.Setup(pb => pb.LoadSolutionFile(It.IsAny<string>())).Returns(true).Callback((string s) => _projectBuilderMock.Setup(pb => pb.CurrentlyLoadedSolutionFile).Returns(s));
            _projectBuilderMock.Setup(pb => pb.BuildSolution(It.IsAny<string>())).Returns(true);

            _deploymentManagerMock = new Mock<IDeploymentManager>();
            _deploymentManagerMock.Setup(dm => dm.ExecuteDeploymentAction(It.IsAny<DeploymentActionType>(), It.IsAny<string[]>())).Returns(true);

            // TODO: setup exception manager mock.
            _exceptionManagerMock = new Mock<IExceptionManager>();

            _deploymentService = new DeploymentService(ConfigurationFileName, false, _repositoryManagerMock.Object, _projectBuilderMock.Object, _deploymentManagerMock.Object, _exceptionManagerMock.Object);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [TearDown]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Tests the read configuration from file method by checking that no repositories are available before reading the configuration file
        /// and that at least one repository is available after reading the configuration file.
        /// </summary>
        [Test]
        public void TestReadConfigurationFromFile()
        {
            Assert.AreEqual(_deploymentService.AllRepositories.Count(), 0, "There are repositories available before reading the configuration file, which should never happen.");
            _deploymentService.ReadConfigurationFromFile();
            Assert.Greater(_deploymentService.AllRepositories.Count(), 0, "No repositories have been reported as available after configuration file has been read, when there should've been at least one.");
        }

        /// <summary>
        /// Tests the check repositories for updates method by checking that no repositories are needing update before running the method
        /// and that at least one repository is needing update after checking them for updates.
        /// </summary>
        [Test]
        public void TestCheckRepositoriesForUpdates()
        {
            // Reads the configuration file to initialise the model correctly.
            _deploymentService.ReadConfigurationFromFile();

            Assert.AreEqual(_deploymentService.RepositoriesNeedingUpdate.Count(), 0, "There are repositories needing update before checking for updates, which should never happen.");
            bool result = _deploymentService.CheckRepositoriesForUpdates();
            Assert.IsTrue(result, "Function that checks repositories for updates returned false when true was expected.");
            Assert.Greater(_deploymentService.RepositoriesNeedingUpdate.Count(), 0, "No repositories have been reported as needing updates after checking for updates, when there should've been at least one.");

            // TODO: Test if no local branch is present but a remote branch is, that the repository is added as needing update.
            // TODO: Test if the local branch is behind remote branch that the repository is added as needing update.
        }

        /// <summary>
        /// Tests the update repositories method by checking that no repositories are ready to be built before running the method
        /// and that at least one repository is ready to be built after updating them.
        /// </summary>
        [Test]
        public void TestUpdateRepositories()
        {
            _deploymentService.ReadConfigurationFromFile();
            _deploymentService.CheckRepositoriesForUpdates();

            Assert.AreEqual(_deploymentService.RepositoriesToBuild.Count(), 0, "There are repositories needing to be built before updating repositories, which should never happen.");
            _deploymentService.UpdateRepositories();
            Assert.Greater(_deploymentService.RepositoriesToBuild.Count(), 0, "No repositories have been reported as needing to be built after updating repositories, when there should've been at least one.");
        }

        /// <summary>
        /// Tests the update repositories method by checking that no repositories are ready to be deployed before running the method
        /// and that at least one repository is ready to be deployed after updating them.
        /// </summary>
        [Test]
        public void TestBuildRepositories()
        {
            _deploymentService.ReadConfigurationFromFile();
            _deploymentService.CheckRepositoriesForUpdates();
            _deploymentService.UpdateRepositories();

            Assert.AreEqual(_deploymentService.RepositoriesToDeploy.Count(), 0, "There are repositories needing to be deployed before building repositories, which should never happen.");
            bool result = _deploymentService.BuildRepositories();
            Assert.IsTrue(result, "Function that builds repositories returned false when true was expected.");
            Assert.Greater(_deploymentService.RepositoriesToDeploy.Count(), 0, "No repositories have been reported as needing to be deployed after building repositories, when there should've been at least one.");
        }

        /// <summary>
        /// Tests the deploy repositories method by checking that the function that deploy repositories does not fail and zeroes the number of repositories to be deployed.
        /// </summary>
        [Test]
        public void TestDeployRepositories()
        {
            _deploymentService.ReadConfigurationFromFile();
            _deploymentService.CheckRepositoriesForUpdates();
            _deploymentService.UpdateRepositories();
            _deploymentService.BuildRepositories();
            
            bool result = _deploymentService.DeployRepositories();
            Assert.IsTrue(result, "Function that deploys repositories returned false when true was expected.");
            Assert.AreEqual(_deploymentService.RepositoriesToDeploy.Count(), 0, "There are repositories still needing to be deployed after deploying repositories, which should never happen.");
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
                    _deploymentService.Dispose();
                    _repositoryManagerMock = null;
                }

                // Clean-up unmanaged resources.
                _disposed = true;
            }
        }

        #region Test Setup Helpers

        /// <summary>
        /// Gets a mock configuration.
        /// </summary>
        /// <returns>A list of repository entities representing a mock configuration.</returns>
        private IList<RepositoryEntity> GetMockConfiguration()
        {
            IList<RepositoryEntity> result = new List<RepositoryEntity>();

            for (int i = 1; i < 5; i++)
            {
                result.Add(new RepositoryEntity()
                {
                    Name = string.Format("Mock Repo {0}", i),
                    LocationPath = string.Format(@"C:\Temp\MockRepo{0}", i),
                    RemoteName = "origin",
                    CheckForUpdatesTimeSpan = new TimeSpan(3, 0, 0),
                    DeploymentBranchName = DeploymentBranchName,
                    LastUpdateTime = DateTime.Now.Subtract(new TimeSpan(3, 1, 0)),
                    SolutionFileToBuild = string.Format(@"C:\Temp\MockRepo{0}\MockRepo{0}.sln", i),
                    SolutionConfigurationToBuild = "Release"
                });
            }

            return result;
        }

        #endregion
    }
}
