// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="CubedCoDeWindowsService.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.WindowsService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading.Tasks;
    using CubedCoDe.Model;
    using CubedCoDe.Model.Interfaces;

    /// <summary>
    /// Represents a windows service application that provides a continuous deployment environment.
    /// </summary>
    public partial class CubedCoDeWindowsService : ServiceBase
    {
        /// <summary>
        /// The actual application.
        /// </summary>
        private CubedCoDeApplication _cubedCoDeApplication;

        /// <summary>
        /// Initialises a new instance of the <see cref="CubedCoDeWindowsService"/> class.
        /// </summary>
        public CubedCoDeWindowsService()
        {
            InitializeComponent();

            // Initialises the application.
            _cubedCoDeApplication = new CubedCoDeApplication();
        }

        /// <summary>
        /// Executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically).
        /// Initialises the application, reads any configurations and starts processes.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            StartupService();
        }

        /// <summary>
        /// Executes when a Stop command is sent to the service by the Service Control Manager (SCM).
        /// </summary>
        protected override void OnStop()
        {
            _cubedCoDeApplication.Stop();
        }

        /// <summary>
        /// Executes when the system is shutting down.
        /// </summary>
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        /// <summary>
        /// Executes when a Pause command is sent to the service by the Service Control Manager (SCM).
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
        }

        /// <summary>
        /// <see cref="M:System.ServiceProcess.ServiceBase.OnContinue" /> runs when a Continue command is sent to the service by the Service Control Manager (SCM).
        /// </summary>
        protected override void OnContinue()
        {
            base.OnContinue();
        }

        /// <summary>
        /// Starts the service up.
        /// </summary>
        /// <returns><c>true</c> if the service has started correctly; <c>false</c> otherwise.</returns>
        private bool StartupService()
        {
            bool result = _cubedCoDeApplication.Start();

            if (!result)
            {
                Stop();
            }

            return result;
        }
    }
}
