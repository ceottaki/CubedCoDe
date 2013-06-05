// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingService.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CubedCoDe.Core.Interfaces;
    using CubedCoDe.Core.Properties;
    using CubedCoDe.Entities;

    /// <summary>
    /// Represents a logging service.
    /// </summary>
    public class LoggingService : ILoggingService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// A list of the start times of methods that will be used by <see cref="RegisterEndOfMethod"/> to calculate time elapsed.
        /// </summary>
        private IDictionary<string, DateTime> methodStartTimes = new Dictionary<string, DateTime>();

        /// <summary>
        /// Initialises a new instance of the <see cref="LoggingService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LoggingService(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        public void WriteToLog(string message, int userId)
        {
            if (_logger != null)
            {
                _logger.Write(message, userId);
            }
        }

        /// <summary>
        /// Writes a message to a log with the given list of categories.
        /// </summary>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        public void WriteToLog(string message, IEnumerable<string> categories, int userId)
        {
            if (_logger != null)
            {
                _logger.Write(message, categories, userId);
            }
        }

        /// <summary>
        /// Writes a message to a log with the given list of categories.
        /// </summary>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        public void WriteToLog(string message, string category, int userId)
        {
            if (_logger != null)
            {
                _logger.Write(message, category, userId);
            }
        }

        /// <summary>
        /// Writes a message to a log with the given list of categories and priority.
        /// </summary>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        public void WriteToLog(string message, IEnumerable<string> categories, LogPriority priority, int userId)
        {
            if (_logger != null)
            {
                _logger.Write(message, categories, priority, userId);
            }
        }

        /// <summary>
        /// Writes a message to a log with the given list of categories and priority.
        /// </summary>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        public void WriteToLog(string message, string category, LogPriority priority, int userId)
        {
            if (_logger != null)
            {
                _logger.Write(message, category, priority, userId);
            }
        }

        /// <summary>
        /// Writes a log message related to a method call. The category of the log will be 'Method Call' and the priority will be Debug (0).
        /// </summary>
        /// <param name="methodName">Name of the method being called.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        /// <param name="parameters">The parameters with which the method was called. The key is the parameter name and the value is its value.</param>
        public void WriteMethodCallToLog(string methodName, int userId, params KeyValuePair<string, object>[] parameters)
        {
            string message = string.Format(CultureInfo.CurrentCulture, Resources.MethodCallLogMessage, methodName);
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    message += string.Format(CultureInfo.CurrentCulture, Resources.MethodCallLogParameter, parameter.Key, parameter.Value);
                }
            }

            _logger.Write(message, Resources.MethodCallLogCategoryName, LogPriority.Debug, userId);
        }

        /// <summary>
        /// Registers the start of a method, storing date and time it started.
        /// Nothing will be written to the log at this point.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        /// <seealso cref="RegisterEndOfMethod" />
        /// <exception cref="System.ArgumentNullException">If <c>methodName</c> is null.</exception>
        /// <remarks>
        /// If this method is called more than once with the same method name and user id the start time for the method will be overwritten.
        /// </remarks>
        public void RegisterStartOfMethod(string methodName, int userId)
        {
            if (methodName == null)
            {
                throw new ArgumentNullException("methodName");
            }
            else
            {
                string startTimeKey = string.Format(CultureInfo.CurrentCulture, "{0}|{1}", methodName, userId);
                if (methodStartTimes.ContainsKey(startTimeKey))
                {
                    methodStartTimes[startTimeKey] = DateTime.UtcNow;
                }
                else
                {
                    methodStartTimes.Add(startTimeKey, DateTime.UtcNow);
                }
            }
        }

        /// <summary>
        /// Registers the end of a method, writing to the log the amount of time the method took to run.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        /// <seealso cref="RegisterStartOfMethod" />
        /// <exception cref="System.ArgumentNullException">If <c>methodName</c> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">If <see cref="RegisterStartOfMethod" /> hasn't been called previously with the same <c>methodName</c> and <c>userId</c>.</exception>
        /// <remarks>
        /// This method relies on <see cref="RegisterStartOfMethod" /> having been called before with the same method name and user id.
        /// If that hasn't happened, it will throw an exception.
        /// </remarks>
        public void RegisterEndOfMethod(string methodName, int userId)
        {
            if (methodName == null)
            {
                throw new ArgumentNullException("methodName");
            }
            else
            {
                string startTimeKey = string.Format(CultureInfo.CurrentCulture, "{0}|{1}", methodName, userId);
                if (!methodStartTimes.ContainsKey(startTimeKey))
                {
                    throw new ArgumentOutOfRangeException("methodName", "The method name provided has not had its start registered for this user.");
                }
                else
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Resources.MethodTimeRunningLogMessage, methodName, DateTime.UtcNow.Subtract(methodStartTimes[startTimeKey]).TotalSeconds);
                    _logger.Write(message, Resources.PerformanceCategoryName, LogPriority.Debug, userId);
                }
            }
        }
    }
}
