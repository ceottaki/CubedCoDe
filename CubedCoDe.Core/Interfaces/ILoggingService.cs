// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="ILoggingService.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CubedCoDe.Entities;

    /// <summary>
    /// Defines properties and methods for an implementation of a logging service.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        void WriteToLog(string message, int userId);

        /// <summary>
        /// Writes a message to a log with the given list of categories.
        /// </summary>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        void WriteToLog(string message, IEnumerable<string> categories, int userId);

        /// <summary>
        /// Writes a message to a log with the given list of categories and priority.
        /// </summary>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        void WriteToLog(string message, IEnumerable<string> categories, LogPriority priority, int userId);

        /// <summary>
        /// Writes a message to a log with the given list of categories.
        /// </summary>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        void WriteToLog(string message, string category, int userId);

        /// <summary>
        /// Writes a message to a log with the given list of categories and priority.
        /// </summary>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        void WriteToLog(string message, string category, LogPriority priority, int userId);

        /// <summary>
        /// Writes a log message related to a method call.
        /// </summary>
        /// <param name="methodName">Name of the method being called.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        /// <param name="parameters">The parameters with which the method was called. The key is the parameter name and the value is its value.</param>
        void WriteMethodCallToLog(string methodName, int userId, params KeyValuePair<string, object>[] parameters);

        /// <summary>
        /// Registers the start of a method, storing date and time it started.
        /// Nothing will be written to the log at this point.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">If <c>methodName</c> is null.</exception>
        /// <remarks>
        /// If this method is called more than once with the same method name the start time for the method will be overwritten.
        /// </remarks>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        /// <seealso cref="RegisterEndOfMethod"/>
        void RegisterStartOfMethod(string methodName, int userId);

        /// <summary>
        /// Registers the end of a method, writing to the log the amount of time the method took to run.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">If <c>methodName</c> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">If <see cref="RegisterStartOfMethod"/> hasn't been called previously with the same <c>methodName</c>.</exception>
        /// <remarks>
        /// This method relies on <see cref="RegisterStartOfMethod"/> having been called before with the same method name.
        /// If that hasn't happened, it will throw an exception.
        /// </remarks>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="userId">The user id responsible for the message.</param>
        /// <seealso cref="RegisterStartOfMethod"/>
        void RegisterEndOfMethod(string methodName, int userId);
    }
}
