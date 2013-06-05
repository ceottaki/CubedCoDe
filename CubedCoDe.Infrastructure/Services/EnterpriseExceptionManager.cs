// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="EnterpriseExceptionManager.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CubedCoDe.Core.Interfaces;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

    /// <summary>
    /// Represents an exception manager that uses the Microsoft Enterprise Library Exception Manager for handling exceptions.
    /// </summary>
    public class EnterpriseExceptionManager : IExceptionManager
    {
        /// <summary>
        /// The exception manager.
        /// </summary>
        private ExceptionManager _exceptionManager;

        /// <summary>
        /// Initialises a new instance of the <see cref="EnterpriseExceptionManager"/> class.
        /// </summary>
        public EnterpriseExceptionManager()
        {
            ExceptionHandlingSettings section = ConfigurationManager.GetSection(ExceptionHandlingSettings.SectionName) as ExceptionHandlingSettings;
            if (section != null)
            {
                _exceptionManager = section.BuildExceptionManager();
            }
        }

        /// <summary>
        /// Executes the supplied delegate <paramref name="action" /> and handles
        /// any thrown exception according to the rules configured for <paramref name="policyName" />.
        /// </summary>
        /// <param name="action">The delegate to execute.</param>
        /// <param name="policyName">The name of the policy to handle.</param>
        /// <example>
        /// The following code shows the usage of this method.
        /// <code>
        /// exceptionManager.Process(() =&gt; { DoWork(); }, "policy");
        /// </code>
        /// </example>
        public void Process(Action action, string policyName)
        {
            if (_exceptionManager != null)
            {
                _exceptionManager.Process(action, policyName);
            }
            else if (action != null)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Processes the specified action.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="defaultResult">The default result.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns>
        /// The result of the specified action.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">action is null.</exception>
        public TResult Process<TResult>(Func<TResult> action, TResult defaultResult, string policyName)
        {
            if (_exceptionManager != null)
            {
                return _exceptionManager.Process(action, defaultResult, policyName);
            }
            else if (action != null)
            {
                return action.Invoke();
            }
            else
            {
                throw new ArgumentNullException("action");
            }
        }

        /// <summary>
        /// Processes the specified action.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns>
        /// The result of the specified action.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">action is null.</exception>
        public TResult Process<TResult>(Func<TResult> action, string policyName)
        {
            if (_exceptionManager != null)
            {
                return _exceptionManager.Process(action, policyName);
            }
            else if (action != null)
            {
                return action.Invoke();
            }
            else
            {
                throw new ArgumentNullException("action");
            }
        }
    }
}
