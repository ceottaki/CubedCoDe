// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.Model
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CubedCoDe.DependencyInjection;
    using CubedCoDe.Model.Interfaces;
    using CubedCoDe.Model.Properties;
    using Ninject;
    using Ninject.Modules;

    /// <summary>
    /// Represents a bootstrapper that allows the application to be initialised, reading needed configuration and loading all necessary modules.
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        /// The deployment model that will be made available to the application using this library after initialisation.
        /// </summary>
        private IDeploymentModel _deploymentModel;

        /// <summary>
        /// Gets the deployment model to be used by the interfaced application.
        /// </summary>
        /// <value>
        /// The deployment model.
        /// </value>
        /// <remarks>
        /// This model is only available after the application has been initialised using <see cref="InitialiseApplication"/>.
        /// </remarks>
        /// <seealso cref="InitialiseApplication"/>
        public IDeploymentModel DeploymentModel
        {
            get
            {
                return _deploymentModel;
            }
        }

        /// <summary>
        /// Initialises the application reading configuration files, loading dependency injection modules and making model available.
        /// </summary>
        /// <exception cref="System.Configuration.ConfigurationException">When configuration key RepositoriesConfigurationFilePath is not present.</exception>
        /// <remarks>
        /// The interface application should make the following values available through their configuration files:
        ///     RepositoriesConfigurationFilePath - text with the full path of the configuration file used for the repositories.
        /// </remarks>
        public void InitialiseApplication()
        {
            // Reads the configurations from the app.config file.
            string repositoriesConfigurationFilePath = null;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("RepositoriesConfigurationFilePath"))
            {
                repositoriesConfigurationFilePath = ConfigurationManager.AppSettings["RepositoriesConfigurationFilePath"];
            }
            else
            {
                throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, Resources.ConfigurationKeyNotPresentError, "RepositoriesConfigurationFilePath"));
            }

            int processWaitMilliseconds = 0;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("ProcessWaitMilliseconds"))
            {
                if (!int.TryParse(ConfigurationManager.AppSettings["ProcessWaitMilliseconds"], out processWaitMilliseconds))
                {
                    throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, Resources.ConfigurationKeyInvalidValueError, "ProcessWaitMilliseconds"));
                }
            }
            else
            {
                throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, Resources.ConfigurationKeyNotPresentError, "ProcessWaitMilliseconds"));
            }

            // Loads the injection modules.
            using (NinjectModule infraServicesModule = new InfraServicesModule(processWaitMilliseconds), coreServicesModule = new CoreServicesModule(repositoriesConfigurationFilePath), modelModule = new ModelModule())
            {
                using (IKernel kernel = new StandardKernel(infraServicesModule, coreServicesModule, modelModule))
                {
                    _deploymentModel = kernel.Get<IDeploymentModel>();
                }
            }
        }
    }
}
