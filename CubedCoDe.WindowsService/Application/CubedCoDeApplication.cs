// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="CubedCoDeApplication.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.WindowsService
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Timers;
    using CubedCoDe.Model;
    using CubedCoDe.Model.Interfaces;
    using CubedCoDe.WindowsService.Properties;

    /// <summary>
    /// Represents a CUBED CoDe application.
    /// </summary>
    public class CubedCoDeApplication : IDisposable
    {
        /// <summary>
        /// An object used for thread locking.
        /// </summary>
        private static object lockObject = new object();

        /// <summary>
        /// The deployment model used to run continuous deployment tasks.
        /// </summary>
        private IDeploymentModel _deploymentModel;

        /// <summary>
        /// A timer to keep running the continuous deployment tasks.
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Flag that indicates if the current instance has been disposed or not.
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// Starts this instance of the CUBED CoDe application.
        /// </summary>
        /// <returns><c>true</c> if application has started successfully; <c>false</c> otherwise.</returns>
        public bool Start()
        {
            bool result = true;

            Bootstrapper bootstrapper = new Bootstrapper();
            try
            {
                bootstrapper.InitialiseApplication();
            }
            catch (ConfigurationException)
            {
                WriteToLocalLog(Resources.ProblemInitialisingApplicationLogMessage);
                result = false;
            }

            if (result)
            {
                _deploymentModel = bootstrapper.DeploymentModel;
                _deploymentModel.ReadConfigurationFromFile();
                WriteToLocalLog(Resources.NextUpdateTimeLogMessage, _deploymentModel.NextUpdateTime);

                if (_deploymentModel.NextUpdateTime.CompareTo(DateTime.Now) <= 0)
                {
                    UpdateRepositories();
                }

                // Calculates the number of milliseconds until the next update is needed.
                double numberOfMillisecondsUntilNextUpdate = _deploymentModel.NextUpdateTime.Subtract(DateTime.Now).TotalMilliseconds;

                // Initialises the timer and set its interval to match the time for the next update. If the time for the next update is negative (meaning it should
                // have happened already) then set the interval to a default of 30 seconds.
                _timer = new Timer();

                if (numberOfMillisecondsUntilNextUpdate <= 0)
                {
                    numberOfMillisecondsUntilNextUpdate = 30000;
                }

                _timer.Interval = numberOfMillisecondsUntilNextUpdate;
                WriteToLocalLog(Resources.NextUpdateIntervalLogMessage, numberOfMillisecondsUntilNextUpdate / 1000);

                // Adds a handler to the timer elapsed event.
                _timer.Elapsed += Timer_Elapsed;

                // Starts the timer.
                _timer.Start();
            }

            return result;
        }

        /// <summary>
        /// Stops this instance of the CUBED CoDe application.
        /// </summary>
        public void Stop()
        {
            if ((_timer != null) && _timer.Enabled)
            {
                _timer.Stop();
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
                    _deploymentModel = null;
                    _timer.Dispose();
                }

                // Clean-up unmanaged resources.
                _disposed = true;
            }
        }

        /// <summary>
        /// Writes a message to the local log.
        /// </summary>
        /// <param name="message">The message.</param>
        private static void WriteToLocalLog(string message)
        {
            WriteToLocalLog(message, null);
        }

        /// <summary>
        /// Writes a message to the local log.
        /// </summary>
        /// <param name="message">The message in composite format string.</param>
        /// <param name="arguments">The objects with which to write using the format specified in <paramref name="message"/>.</param>
        private static void WriteToLocalLog(string message, params object[] arguments)
        {
            // TODO: Change this to a real implementation of logging, possibly with a new Logging model.
            System.Diagnostics.Debug.WriteLine(message, arguments);
        }

        /// <summary>
        /// Handles the Elapsed event of the Timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Prevents code from this method to execute in parallel threads.
            lock (lockObject)
            {
                // Only executes the updates if the timer had been running when this was called.
                if (_timer.Enabled)
                {
                    // Stops the timer to prevent this method from being called while it is still being processed.
                    _timer.Stop();

                    UpdateRepositories();

                    // Calculates the number of milliseconds until the next update is needed.
                    double numberOfMillisecondsUntilNextUpdate = _deploymentModel.NextUpdateTime.Subtract(DateTime.Now).TotalMilliseconds;

                    // Sets the timer interval to match the time for the next update. If the time for the next update is negative (meaning it should have happened already)
                    // then set the interval to a default of 30 seconds.
                    if (numberOfMillisecondsUntilNextUpdate <= 0)
                    {
                        numberOfMillisecondsUntilNextUpdate = 30000;
                    }

                    _timer.Interval = numberOfMillisecondsUntilNextUpdate;
                    WriteToLocalLog(Resources.NextUpdateIntervalLogMessage, numberOfMillisecondsUntilNextUpdate / 1000);

                    // Re-starts the timer.
                    _timer.Start();
                }
            }
        }

        /// <summary>
        /// Updates the repositories.
        /// </summary>
        private void UpdateRepositories()
        {
            WriteToLocalLog(Resources.CheckingRepositoriesForUpdatesLogMessage);
            _deploymentModel.CheckRepositoriesForUpdates();

            WriteToLocalLog(Resources.UpdatingRepositoriesLogMessage);
            _deploymentModel.UpdateRepositories();

            WriteToLocalLog(Resources.BuildingRepositoriesLogMessage);
            bool buildResult = _deploymentModel.BuildRepositories();

            if (buildResult)
            {
                WriteToLocalLog(Resources.RepositoriesBuiltSuccessfullyLogMessage);
            }
            else
            {
                WriteToLocalLog(Resources.ProblemBuildingRepositoriesLogMessage);
            }

            WriteToLocalLog(Resources.DeployingRepositoriesLogMessage);
            bool deployResult = _deploymentModel.DeployRepositories();

            if (deployResult)
            {
                WriteToLocalLog(Resources.RepositoriesDeployedSuccessfullyLogMessage);
            }
            else
            {
                WriteToLocalLog(Resources.ProblemDeployingRepositoriesLogMessage);
            }

            WriteToLocalLog(Resources.UpdateFinishedLogMessage);
        }
    }
}
