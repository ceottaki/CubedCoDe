// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="EnterpriseLogger.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CubedCoDe.Core.Interfaces;
    using CubedCoDe.Entities;
    using Microsoft.Practices.EnterpriseLibrary.Common;
    using Microsoft.Practices.EnterpriseLibrary.Logging;

    /// <summary>
    /// Represents a logger that uses the Microsoft Enterprise Library Logger for writing log messages with categories, priority, severity, etc.
    /// </summary>
    public class EnterpriseLogger : BaseLogger, ILogger
    {
        /// <summary>
        /// The log writer factory.
        /// </summary>
        private LogWriterFactory _logWriterFactory = new LogWriterFactory();

        /// <summary>
        /// The log writer.
        /// </summary>
        private LogWriter _writer;

        /// <summary>
        /// Initialises a new instance of the <see cref="EnterpriseLogger"/> class.
        /// </summary>
        public EnterpriseLogger()
            : base()
        {
            _writer = _logWriterFactory.Create();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="EnterpriseLogger"/> class with the given category.
        /// </summary>
        /// <param name="category">The category related to the message.</param>
        public EnterpriseLogger(string category)
            : base(category)
        {
            _writer = _logWriterFactory.Create();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="EnterpriseLogger"/> class with the given category and priority.
        /// </summary>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        public EnterpriseLogger(string category, LogPriority priority)
            : base(category, priority)
        {
            _writer = _logWriterFactory.Create();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="EnterpriseLogger"/> class with the given category, priority and event id.
        /// </summary>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        public EnterpriseLogger(string category, LogPriority priority, int eventId)
            : base(category, priority, eventId)
        {
            _writer = _logWriterFactory.Create();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="EnterpriseLogger"/> class with the given category, priority, event id and severity.
        /// </summary>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        public EnterpriseLogger(string category, LogPriority priority, int eventId, TraceEventType severity)
            : base(category, priority, eventId, severity)
        {
            _writer = _logWriterFactory.Create();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="EnterpriseLogger"/> class with the given category, priority, event id, severity and title.
        /// </summary>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="title">The title of the message.</param>
        public EnterpriseLogger(string category, LogPriority priority, int eventId, TraceEventType severity, string title)
            : base(category, priority, eventId, severity, title)
        {
            _writer = _logWriterFactory.Create();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="EnterpriseLogger"/> class with the given list of categories.
        /// </summary>
        /// <param name="categories">The list of categories related to the message.</param>
        public EnterpriseLogger(IEnumerable<string> categories)
            : base(categories)
        {
            _writer = _logWriterFactory.Create();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="EnterpriseLogger"/> class with the given list of categories and priority.
        /// </summary>
        /// <param name="categories">The list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        public EnterpriseLogger(IEnumerable<string> categories, LogPriority priority)
            : base(categories, priority)
        {
            _writer = _logWriterFactory.Create();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="EnterpriseLogger"/> class with the given list of categories, priority and event id.
        /// </summary>
        /// <param name="categories">The list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        public EnterpriseLogger(IEnumerable<string> categories, LogPriority priority, int eventId)
            : base(categories, priority, eventId)
        {
            _writer = _logWriterFactory.Create();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="EnterpriseLogger"/> class with the given list of categories, priority, event id and severity.
        /// </summary>
        /// <param name="categories">The list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        public EnterpriseLogger(IEnumerable<string> categories, LogPriority priority, int eventId, TraceEventType severity)
            : base(categories, priority, eventId, severity)
        {
            _writer = _logWriterFactory.Create();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="EnterpriseLogger"/> class with the given list of categories, priority, event id, severity and title.
        /// </summary>
        /// <param name="categories">The list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="title">The title of the message.</param>
        public EnterpriseLogger(IEnumerable<string> categories, LogPriority priority, int eventId, TraceEventType severity, string title)
            : base(categories, priority, eventId, severity, title)
        {
            _writer = _logWriterFactory.Create();
        }

        /// <summary>
        /// Writes a message to a log.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        public void Write(string message)
        {
            Write(message, this.Categories, this.Priority, this.EventId, this.Severity, this.Title);
        }

        /// <summary>
        /// Writes a message to a log with the given list of categories.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">The category related to the message.</param>
        public void Write(string message, IEnumerable<string> categories)
        {
            Write(message, categories, this.Priority, this.EventId, this.Severity, this.Title);
        }

        /// <summary>
        /// Writes a message to a log with the given list of categories and priority.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        public void Write(string message, IEnumerable<string> categories, LogPriority priority)
        {
            Write(message, categories, priority, this.EventId, this.Severity, this.Title);
        }

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, event id and severity.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        public void Write(string message, IEnumerable<string> categories, LogPriority priority, int eventId, System.Diagnostics.TraceEventType severity)
        {
            Write(message, categories, priority, eventId, severity, this.Title);
        }

        /// <summary>
        /// Writes a message to a log with the given list of categories, priority, event id, severity and title.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="title">The title of the message.</param>
        public void Write(string message, IEnumerable<string> categories, LogPriority priority, int eventId, System.Diagnostics.TraceEventType severity, string title)
        {
            if (IsLoggingEnabled())
            {
                _writer.Write(message, categories, (int)priority, eventId, severity, title);
            }
        }

        /// <summary>
        /// Writes a message to a log with the given category and priority.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        public void Write(string message, string category)
        {
            Write(message, new List<string>() { category }, this.Priority, this.EventId, this.Severity, this.Title);
        }

        /// <summary>
        /// Writes a message to a log with the given category and priority.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        public void Write(string message, string category, LogPriority priority)
        {
            Write(message, new List<string>() { category }, priority, this.EventId, this.Severity, this.Title);
        }

        /// <summary>
        /// Writes a message to a log with the given category, priority, event id and severity.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        public void Write(string message, string category, LogPriority priority, int eventId, TraceEventType severity)
        {
            Write(message, new List<string>() { category }, priority, eventId, severity, this.Title);
        }

        /// <summary>
        /// Writes a message to a log with the given category, priority, event id, severity and title.
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
        public void Write(string message, string category, LogPriority priority, int eventId, TraceEventType severity, string title)
        {
            Write(message, new List<string>() { category }, priority, eventId, severity, title);
        }

        /// <summary>
        /// Writes a message to a log.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        public void Write(string message, int userId)
        {
            Write(message, this.Categories, this.Priority, this.EventId, this.Severity, this.Title, userId);
        }

        /// <summary>
        /// Writes a message to a log with the given list of categories.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="categories">A list of categories related to the message.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        public void Write(string message, IEnumerable<string> categories, int userId)
        {
            Write(message, categories, this.Priority, this.EventId, this.Severity, this.Title, userId);
        }

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
        public void Write(string message, IEnumerable<string> categories, LogPriority priority, int eventId, int userId)
        {
            Write(message, categories, priority, eventId, this.Severity, this.Title, userId);
        }

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
        public void Write(string message, IEnumerable<string> categories, LogPriority priority, int eventId, TraceEventType severity, int userId)
        {
            Write(message, categories, priority, eventId, severity, this.Title, userId);
        }

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
        public void Write(string message, IEnumerable<string> categories, LogPriority priority, int eventId, TraceEventType severity, string title, int userId)
        {
            if (IsLoggingEnabled())
            {
                _writer.Write(new LogEntry(message, categories.ToList(), (int)priority, eventId, severity, title, null));
            }
        }

        /// <summary>
        /// Writes a message to a log with the given category, priority, event id, severity and title.
        /// </summary>
        /// <remarks>
        /// Only writes to the log if logging is enabled, which can be verified through <see cref="IsLoggingEnabled"/>.
        /// </remarks>
        /// <param name="message">The message to be written to the log.</param>
        /// <param name="category">The category related to the message.</param>
        /// <param name="userId">The user id of the user responsible for the message.</param>
        public void Write(string message, string category, int userId)
        {
            Write(message, new List<string>() { category }, this.Priority, this.EventId, this.Severity, this.Title, userId);
        }

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
        public void Write(string message, string category, LogPriority priority, int eventId, int userId)
        {
            Write(message, new List<string>() { category }, priority, eventId, this.Severity, this.Title, userId);
        }

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
        public void Write(string message, string category, LogPriority priority, int eventId, TraceEventType severity, int userId)
        {
            Write(message, new List<string>() { category }, priority, eventId, severity, this.Title, userId);
        }

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
        public void Write(string message, string category, LogPriority priority, int eventId, TraceEventType severity, string title, int userId)
        {
            Write(message, new List<string>() { category }, priority, eventId, severity, title, userId);
        }

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
        public void Write(string message, IEnumerable<string> categories, LogPriority priority, int userId)
        {
            Write(message, categories, priority, this.EventId, this.Severity, this.Title, userId);
        }

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
        public void Write(string message, string category, LogPriority priority, int userId)
        {
            Write(message, new List<string> { category }, priority, this.EventId, this.Severity, this.Title, userId);
        }

        /// <summary>
        /// Checks if logging is enabled.
        /// </summary>
        /// <returns>True if logging is enabled; false otherwise.</returns>
        public bool IsLoggingEnabled()
        {
            return _writer.IsLoggingEnabled();
        }
    }
}
