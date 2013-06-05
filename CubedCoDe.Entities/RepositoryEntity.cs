// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryEntity.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a repository entity.
    /// </summary>
    public class RepositoryEntity
    {
        /// <summary>
        /// The list of deployment actions.
        /// </summary>
        private Collection<DeploymentActionEntity> _deploymentActions = new Collection<DeploymentActionEntity>();

        /// <summary>
        /// Gets or sets the name of the repository.
        /// </summary>
        /// <value>
        /// The name of the repository.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the location path of the repository.
        /// </summary>
        /// <value>
        /// The location path of the repository.
        /// </value>
        public string LocationPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the remote to be used.
        /// </summary>
        /// <value>
        /// The name of the remote.
        /// </value>
        public string RemoteName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time span used to check for updates of this repository.
        /// </summary>
        /// <value>
        /// The time span used to check for updates of this repository.
        /// </value>
        [XmlIgnore]
        public TimeSpan CheckForUpdatesTimeSpan
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time span used to check for updates of this repository as a string.
        /// </summary>
        /// <value>
        /// The time span used to check for updates of this repository as a string.
        /// </value>
        /// <remarks>
        /// This property is available for XML serialisation purposes only. Please use <see cref="CheckForUpdatesTimeSpan"/> instead for getting and setting this value.
        /// </remarks>
        /// <seealso cref="CheckForUpdatesTimeSpan"/>
        [Browsable(false)]
        [XmlElement(ElementName = "CheckForUpdatesTimeSpan", DataType = "duration")]
        public string CheckForUpdatesTimeSpanAsString
        {
            get
            {
                return XmlConvert.ToString(CheckForUpdatesTimeSpan);
            }

            set
            {
                CheckForUpdatesTimeSpan = string.IsNullOrEmpty(value) ? TimeSpan.Zero : XmlConvert.ToTimeSpan(value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the deployment branch.
        /// </summary>
        /// <value>
        /// The name of the deployment branch.
        /// </value>
        public string DeploymentBranchName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the path to the solution file to build.
        /// </summary>
        /// <value>
        /// The path to the solution file to build.
        /// </value>
        public string SolutionFileToBuild
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the solution configuration to build.
        /// </summary>
        /// <value>
        /// The solution configuration to build.
        /// </value>
        public string SolutionConfigurationToBuild
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time of the last update.
        /// </summary>
        /// <value>
        /// The time of the last update.
        /// </value>
        public DateTime LastUpdateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this repository needs updating.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this repository needs updating; otherwise, <c>false</c>.
        /// </value>
        public bool IsNeedingUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this repository needs building.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this repository needs building; otherwise, <c>false</c>.
        /// </value>
        public bool IsNeedingBuilding
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this repository needs to be deployed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this repository needs to be deployed; otherwise, <c>false</c>.
        /// </value>
        public bool IsNeedingDeployment
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the deployment actions.
        /// </summary>
        /// <value>
        /// The deployment actions.
        /// </value>
        public Collection<DeploymentActionEntity> DeploymentActions
        {
            get
            {
                return _deploymentActions;
            }
        }
    }
}
