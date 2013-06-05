// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseLogger.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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
    using CubedCoDe.Entities;

    /// <summary>
    /// Provides a base for an implementation of a logger class. This is an abstract class.
    /// </summary>
    public abstract class BaseLogger
    {
        /// <summary>
        /// The default category for a log.
        /// </summary>
        private const string DefaultCategory = "General";

        /// <summary>
        /// The default priority for a lot.
        /// </summary>
        private const LogPriority DefaultPriority = LogPriority.Debug;

        /// <summary>
        /// The default event id for a log.
        /// </summary>
        private const int DefaultEventId = 1;

        /// <summary>
        /// The default severity for a log.
        /// </summary>
        private const TraceEventType DefaultSeverity = TraceEventType.Information;

        /// <summary>
        /// The default title for a log.
        /// </summary>
        private const string DefaultTitle = "";

        /// <summary>
        /// The priority used as default for log messages.
        /// </summary>
        private LogPriority _priority;

        /// <summary>
        /// The list of categories used as default for log messages.
        /// </summary>
        private IEnumerable<string> _categories;

        /// <summary>
        /// The event id used as default for log messages.
        /// </summary>
        private int _eventId;

        /// <summary>
        /// The severity used as default for log messages.
        /// </summary>
        private TraceEventType _severity;

        /// <summary>
        /// The title used as default for log messages.
        /// </summary>
        private string _title;

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseLogger"/> class.
        /// </summary>
        protected BaseLogger()
            : this(new List<string>() { DefaultCategory }, DefaultPriority, DefaultEventId, DefaultSeverity, DefaultTitle)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseLogger"/> class with the given category.
        /// </summary>
        /// <param name="category">The category related to the message.</param>
        protected BaseLogger(string category)
            : this(new List<string>() { category }, DefaultPriority, DefaultEventId, DefaultSeverity, DefaultTitle)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseLogger"/> class with the given category and priority.
        /// </summary>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        protected BaseLogger(string category, LogPriority priority)
            : this(new List<string>() { category }, priority, DefaultEventId, DefaultSeverity, DefaultTitle)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseLogger"/> class with the given category, priority and event id.
        /// </summary>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        protected BaseLogger(string category, LogPriority priority, int eventId)
            : this(new List<string>() { category }, priority, eventId, DefaultSeverity, DefaultTitle)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseLogger"/> class with the given category, priority, event id and severity.
        /// </summary>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        protected BaseLogger(string category, LogPriority priority, int eventId, TraceEventType severity)
            : this(new List<string>() { category }, priority, eventId, severity, DefaultTitle)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseLogger"/> class with the given category, priority, event id, severity and title.
        /// </summary>
        /// <param name="category">The category related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="title">The title of the message.</param>
        protected BaseLogger(string category, LogPriority priority, int eventId, TraceEventType severity, string title)
            : this(new List<string>() { category }, priority, eventId, severity, title)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseLogger"/> class with the given list of categories.
        /// </summary>
        /// <param name="categories">The list of categories related to the message.</param>
        protected BaseLogger(IEnumerable<string> categories)
            : this(categories, DefaultPriority, DefaultEventId, DefaultSeverity, DefaultTitle)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseLogger"/> class with the given list of categories and priority.
        /// </summary>
        /// <param name="categories">The list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        protected BaseLogger(IEnumerable<string> categories, LogPriority priority)
            : this(categories, priority, DefaultEventId, DefaultSeverity, DefaultTitle)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseLogger"/> class with the given list of categories, priority and event id.
        /// </summary>
        /// <param name="categories">The list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        protected BaseLogger(IEnumerable<string> categories, LogPriority priority, int eventId)
            : this(categories, priority, eventId, DefaultSeverity, DefaultTitle)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseLogger"/> class with the given list of categories, priority, event id and severity.
        /// </summary>
        /// <param name="categories">The list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        protected BaseLogger(IEnumerable<string> categories, LogPriority priority, int eventId, TraceEventType severity)
            : this(categories, priority, eventId, severity, DefaultTitle)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseLogger"/> class with the given list of categories, priority, event id, severity and title.
        /// </summary>
        /// <param name="categories">The list of categories related to the message.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="eventId">The event id of the message. Specially applicable for messages being logged to the Windows Event Log.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="title">The title of the message.</param>
        protected BaseLogger(IEnumerable<string> categories, LogPriority priority, int eventId, TraceEventType severity, string title)
        {
            _categories = categories;
            _priority = priority;
            _eventId = eventId;
            _severity = severity;
            _title = title;
        }

        /// <summary>
        /// Gets or sets the list of categories used as default for log messages.
        /// </summary>
        /// <value>
        /// The list of categories used as default for log messages.
        /// </value>
        public IEnumerable<string> Categories
        {
            get
            {
                return _categories;
            }

            set
            {
                _categories = value;
            }
        }

        /// <summary>
        /// Gets or sets the priority used as default for log messages.
        /// </summary>
        /// <value>
        /// The priority used as default for log messages.
        /// </value>
        public LogPriority Priority
        {
            get
            {
                return _priority;
            }

            set
            {
                _priority = value;
            }
        }

        /// <summary>
        /// Gets or sets the event id used as default for log messages.
        /// </summary>
        /// <value>
        /// The event id used as default for log messages.
        /// </value>
        public int EventId
        {
            get
            {
                return _eventId;
            }

            set
            {
                _eventId = value;
            }
        }

        /// <summary>
        /// Gets or sets the severity used as default for log messages.
        /// </summary>
        /// <value>
        /// The severity used as default for log messages.
        /// </value>
        public TraceEventType Severity
        {
            get
            {
                return _severity;
            }

            set
            {
                _severity = value;
            }
        }

        /// <summary>
        /// Gets or sets the title used as default for log messages.
        /// </summary>
        /// <value>
        /// The title used as default for log messages.
        /// </value>
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }
    }
}
