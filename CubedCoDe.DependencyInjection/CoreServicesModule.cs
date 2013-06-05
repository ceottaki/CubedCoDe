// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="CoreServicesModule.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CubedCoDe.Core.Interfaces;
    using CubedCoDe.Core.Services;
    using Ninject.Modules;

    /// <summary>
    /// Represents a loadable unit that defines bindings for the core's service classes.
    /// </summary>
    public class CoreServicesModule : NinjectModule
    {
        /// <summary>
        /// The path to the configuration file.
        /// </summary>
        private string _pathToConfigurationFile;

        /// <summary>
        /// Initialises a new instance of the <see cref="CoreServicesModule"/> class.
        /// </summary>
        /// <param name="pathToConfigurationFile">The path to configuration file.</param>
        public CoreServicesModule(string pathToConfigurationFile)
        {
            _pathToConfigurationFile = pathToConfigurationFile;
        }

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IDeploymentService>().To<DeploymentService>().WithConstructorArgument("pathToConfigurationFile", _pathToConfigurationFile);
            Bind<ILoggingService>().To<LoggingService>();
        }
    }
}
