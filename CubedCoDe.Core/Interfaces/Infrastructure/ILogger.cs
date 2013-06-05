// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogger.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CubedCoDe.Entities;

    /// <summary>
    /// Represents a logger that provides functionality for writing log messages with categories, priority, severity, etc.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes a message to a log.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        void Write(string message);

        /// <summary>
        /// Writes a message to a log with the given list of categories.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        void Write(string message, IEnumerable<string> categories);

        /// <summary>
        /// Writes a message to a log with the given list of categories and priority.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        void Write(string message, IEnumerable<string> categories, LogPriority priority);

        ///// <summary>
        ///// Writes a message to a log with the given list of categories, priority, and event id.
        ///// </summary>
        ///// <remarks>
        ///// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        ///// </remarks>
        ///// <param name="message">The message to be written to the log.</param>
        ///// <param name="categories">A list of categories related to the message.</param>
        ///// <param name="priority">The priority of the message.</param>
        ///// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        ////void Write(string message, IEnumerable<string> categories, LogPriority priority, int eventId);

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, event id and severity.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        void Write(string message, IEnumerable<string> categories, LogPriority priority, int eventId, TraceEventType severity);

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, event id, severity and title.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="title">The title of the message.</param>
        void Write(string message, IEnumerable<string> categories, LogPriority priority, int eventId, TraceEventType severity, string title);

        /// <summary>
        /// Writes a message to a log with the given category, priority, event id, severity and title.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        void Write(string message, string category);

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, event id, severity and title.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        void Write(string message, string category, LogPriority priority);

        ///// <summary>
        ///// Writes a message to a log with the given list of categories, priority, event id, severity and title.
        ///// </summary>
        ///// <remarks>
        ///// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        ///// </remarks>
        ///// <param name="message">The message to be written to the log.</param>
        ///// <param name="category">The category related to the message.</param>
        ///// <param name="priority">The priority of the message.</param>
        ///// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        ////void Write(string message, string category, LogPriority priority, int eventId);

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, event id, severity and title.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        void Write(string message, string category, LogPriority priority, int eventId, TraceEventType severity);

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, event id, severity and title.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="title">The title of the message.</param>
        void Write(string message, string category, LogPriority priority, int eventId, TraceEventType severity, string title);

        /// <summary>
        /// Writes a message to a log.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        void Write(string message, int userId);

        /// <summary>
        /// Writes a message to a log with the given list of categories.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        void Write(string message, IEnumerable<string> categories, int userId);

        /// <summary>
        /// Writes a message to a log with the given list of categories and priority.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        void Write(string message, IEnumerable<string> categories, LogPriority priority, int userId);

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, and event id.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        void Write(string message, IEnumerable<string> categories, LogPriority priority, int eventId, int userId);

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, event id and severity.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        void Write(string message, IEnumerable<string> categories, LogPriority priority, int eventId, TraceEventType severity, int userId);

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, event id, severity and title.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="title">The title of the message.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        void Write(string message, IEnumerable<string> categories, LogPriority priority, int eventId, TraceEventType severity, string title, int userId);

        /// <summary>
        /// Writes a message to a log with the given category, priority, event id, severity and title.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        void Write(string message, string category, int userId);

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, event id, severity and title.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        void Write(string message, string category, LogPriority priority, int userId);

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, event id, severity and title.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        void Write(string message, string category, LogPriority priority, int eventId, int userId);

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, event id, severity and title.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        void Write(string message, string category, LogPriority priority, int eventId, TraceEventType severity, int userId);

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, event id, severity and title.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="title">The title of the message.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        void Write(string message, string category, LogPriority priority, int eventId, TraceEventType severity, string title, int userId);

        /// <summary>
        /// Checks if logging is enabled.
        /// </summary>
        /// <returns>True if logging is enabled; false otherwise.</returns>
        bool IsLoggingEnabled();
    }
}
