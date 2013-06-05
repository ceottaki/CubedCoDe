// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="IExceptionManager.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

    /// <summary>
    /// Defines properties and methods for an implementation of an exception manager.
    /// </summary>
    public interface IExceptionManager
    {
        /// <summary>
        /// Executes the supplied delegate <paramref name="action"/> and handles 
        /// any thrown exception according to the rules configured for <paramref name="policyName"/>.
        /// </summary>
        /// <param name="action">The delegate to execute.</param>
        /// <param name="policyName">The name of the policy to handle.</param>        
        /// <example>
        /// The following code shows the usage of this method.
        /// <code>
        /// exceptionManager.Process(() => { DoWork(); }, "policy");
        /// </code>
        /// </example>
        void Process(Action action, string policyName);

        /// <summary>
        /// Executes the supplied delegate <paramref name="action"/>, and handles
        /// any thrown exception according to the rules configured for <paramref name="policyName"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of return value from <paramref name="action"/>.</typeparam>
        /// <param name="action">The delegate to execute.</param>
        /// <param name="defaultResult">The value to return if an exception is thrown and the
        /// exception policy swallows it instead of re-throwing.</param>
        /// <param name="policyName">The name of the policy to handle.</param>
        /// <returns>If no exception occurs, returns the result from executing <paramref name="action"/>. If
        /// an exception occurs and the policy does not re-throw, returns <paramref name="defaultResult"/>.</returns>
        TResult Process<TResult>(Func<TResult> action, TResult defaultResult, string policyName);

        /// <summary>
        /// Executes the supplied delegate <paramref name="action"/>, and handles
        /// any thrown exception according to the rules configured for <paramref name="policyName"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of return value from <paramref name="action"/>.</typeparam>
        /// <param name="action">The delegate to execute.</param>
        /// <param name="policyName">The name of the policy to handle.</param>
        /// <returns>If no exception occurs, returns the result from executing <paramref name="action"/>. If
        /// an exception occurs and the policy does not re-throw, returns the default value for <typeparamref name="TResult"/>.</returns>
        TResult Process<TResult>(Func<TResult> action, string policyName);
    }
}
