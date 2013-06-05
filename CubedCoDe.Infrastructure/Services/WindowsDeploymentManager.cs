// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowsDeploymentManager.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using CubedCoDe.Core.Interfaces;
    using CubedCoDe.Entities;

    /// <summary>
    /// Represents a Windows deployment manager.
    /// </summary>
    public class WindowsDeploymentManager : IDeploymentManager
    {
        /// <summary>
        /// The number of milliseconds to wait for a process to finish when executing an ExecuteFile action.
        /// </summary>
        private int _processWaitMilliseconds;

        /// <summary>
        /// Initialises a new instance of the <see cref="WindowsDeploymentManager"/> class.
        /// </summary>
        /// <param name="processWaitMilliseconds">The number of milliseconds to wait for a process to finish when executing an ExecuteFile action.</param>
        public WindowsDeploymentManager(int processWaitMilliseconds)
        {
            _processWaitMilliseconds = processWaitMilliseconds;
        }

        // TODO: This is an overly simplified manager. It should handle rollbacks and provide logging information about the actions.

        /// <summary>
        /// Executes a deployment action.
        /// </summary>
        /// <remarks>
        /// If <paramref name="actionType"/> is <see cref="DeploymentActionType.CopyFile"/> then two parameters are expected: the path to the source file to be copied as the first parameter and the path where to copy that file to as the second parameter.<br/>
        /// If <paramref name="actionType"/> is <see cref="DeploymentActionType.ExecuteFile"/> then at least two parameters are expected but up to five can be provided, in this order:<br/>
        /// 1- File to be executed<br/>
        /// 2- Arguments to be passed in to the execution of the file<br/>
        /// 3- Normal or Elevated, indicating if execution should be done without or with elevated rights<br/>
        /// 4- The user name of the user with which to execute the file<br/>
        /// 5- The password of the user with which to execute the file.
        /// </remarks>
        /// <param name="actionType">The type of the action.</param>
        /// <param name="actionParameters">The action parameters.</param>
        /// <returns>
        ///   <c>true</c> if action was executed successfully; <c>false</c> otherwise.
        /// </returns>
        public bool ExecuteDeploymentAction(DeploymentActionType actionType, params string[] actionParameters)
        {
            bool result = true;

            switch (actionType)
            {
                case DeploymentActionType.CheckUnitTests:
                    result = CheckUnitTests(actionParameters);
                    break;

                case DeploymentActionType.CopyFile:
                    result = CopyFile(actionParameters);
                    break;

                case DeploymentActionType.ExecuteFile:
                    result = ExecuteFile(_processWaitMilliseconds, actionParameters);
                    break;

                case DeploymentActionType.RunDatabaseScript:
                    result = RunDatabaseScript(actionParameters);
                    break;

                case DeploymentActionType.SendEmail:
                    result = SendEmail(actionParameters);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Checks if all given unit tests pass.
        /// </summary>
        /// <param name="actionParameters">
        /// The action parameters.
        /// At least one parameter is expected: the path to the file to be unit tested.
        /// </param>
        /// <returns><c>true</c> if action was executed successfully and all unit tests passed; <c>false</c> otherwise.</returns>
        private static bool CheckUnitTests(params string[] actionParameters)
        {
            // TODO: Implement deployment action 'check unit tests'.
            return false;
        }

        /// <summary>
        /// Copies a file to a given destination.
        /// </summary>
        /// <param name="actionParameters">
        /// The action parameters.
        /// Two parameters are expected: the path to the source file to be copied as the first parameter and the path where to copy that file to as the second parameter.
        /// </param>
        /// <returns><c>true</c> if action was executed successfully; <c>false</c> otherwise.</returns>
        private static bool CopyFile(params string[] actionParameters)
        {
            bool result = true;

            if ((actionParameters == null) || (actionParameters.Length < 2) || string.IsNullOrWhiteSpace(actionParameters[0]) || string.IsNullOrWhiteSpace(actionParameters[1]))
            {
                result = false;
            }
            else
            {
                try
                {
                    result = false;
                    File.Copy(actionParameters[0], actionParameters[1], true);
                }
                finally
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Executes a file.
        /// </summary>
        /// <param name="processWaitMilliseconds">The number of milliseconds to wait for a process to finish when executing the action.</param>
        /// <param name="actionParameters">The action parameters. At least two parameters are expected but up to five can be provided, in this order:<br />
        /// 1- File to be executed<br />
        /// 2- Arguments to be passed in to the execution of the file<br />
        /// 3- Normal or Elevated, indicating if execution should be done without or with elevated rights<br />
        /// 4- The user name of the user with which to execute the file<br />
        /// 5- The password of the user with which to execute the file.</param>
        /// <returns>
        ///   <c>true</c> if action was executed successfully; <c>false</c> otherwise.
        /// </returns>
        private static bool ExecuteFile(int processWaitMilliseconds, params string[] actionParameters)
        {
            bool result = true;

            if ((actionParameters == null) || (actionParameters.Length < 1) || string.IsNullOrWhiteSpace(actionParameters[0]))
            {
                result = false;
            }
            else
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(actionParameters[0], actionParameters.Length > 1 ? actionParameters[1] : null);
                startInfo.CreateNoWindow = true;

                // Processes optional parameters.
                if ((actionParameters.Length > 2) && (actionParameters[2].ToUpperInvariant() == "ELEVATED"))
                {
                    startInfo.Verb = "runas";
                }

                if ((actionParameters.Length > 3) && (!string.IsNullOrWhiteSpace(actionParameters[3])))
                {
                    startInfo.UserName = actionParameters[3];
                }

                if ((actionParameters.Length > 4) && (!string.IsNullOrWhiteSpace(actionParameters[4])))
                {
                    using (SecureString securePassword = new SecureString())
                    {
                        foreach (char character in actionParameters[4])
                        {
                            securePassword.AppendChar(character);
                        }

                        startInfo.Password = securePassword;
                    }
                }

                // Starts the process and waits for it to finish.
                Process myProcess = Process.Start(startInfo);
                result = myProcess.WaitForExit(processWaitMilliseconds);

                if (result)
                {
                    result = myProcess.ExitCode == 0;
                }
            }

            return result;
        }

        /// <summary>
        /// Runs the given database scripts.
        /// </summary>
        /// <param name="actionParameters">
        /// The action parameters.
        /// </param>
        /// <returns><c>true</c> if action was executed successfully; <c>false</c> otherwise.</returns>
        private static bool RunDatabaseScript(params string[] actionParameters)
        {
            // TODO: Implement deployment action 'run database script'.
            return false;
        }

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="actionParameters">
        /// The action parameters.
        /// </param>
        /// <returns><c>true</c> if action was executed successfully; <c>false</c> otherwise.</returns>
        private static bool SendEmail(params string[] actionParameters)
        {
            // TODO: Implement deployment action 'send email'.
            return false;
        }
    }
}
