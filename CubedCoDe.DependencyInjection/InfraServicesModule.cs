// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="InfraServicesModule.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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
    using CubedCoDe.Infrastructure.Services;
    using Ninject.Modules;

    /// <summary>
    /// Represents a loadable unit that defines bindings for the infrastructure's service classes.
    /// </summary>
    public class InfraServicesModule : NinjectModule
    {
        /// <summary>
        /// The number of milliseconds to wait for a process to finish when executing an ExecuteFile action.
        /// </summary>
        private int _processWaitMilliseconds;

        /// <summary>
        /// Initialises a new instance of the <see cref="InfraServicesModule"/> class.
        /// </summary>
        /// <param name="processWaitMilliseconds">The number of milliseconds to wait for a process to finish when executing an ExecuteFile action.</param>
        public InfraServicesModule(int processWaitMilliseconds)
        {
            _processWaitMilliseconds = processWaitMilliseconds;
        }

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IDistributedRevControlRepositoryManager>().To<GitRepositoryManager>();
            Bind<IProjectBuilder>().To<DotNetBuilder>();
            Bind<IDeploymentManager>().To<WindowsDeploymentManager>().WithConstructorArgument("processWaitMilliseconds", _processWaitMilliseconds);
            Bind<ILogger>().To<EnterpriseLogger>();
            Bind<IExceptionManager>().To<EnterpriseExceptionManager>();
        }
    }
}
