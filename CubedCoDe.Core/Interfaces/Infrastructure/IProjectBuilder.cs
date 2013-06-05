// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="IProjectBuilder.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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
    /// Defines properties and methods for an implementation of a class that allows source-code projects to be built.
    /// </summary>
    public interface IProjectBuilder
    {
        /// <summary>
        /// Gets the currently loaded solution file.
        /// </summary>
        /// <value>
        /// The currently loaded solution file.
        /// </value>
        string CurrentlyLoadedSolutionFile
        {
            get;
        }

        /// <summary>
        /// Loads the solution file containing instructions on what to be built.
        /// </summary>
        /// <remarks>
        /// The name of the file of the solution is made available in property <see cref="CurrentlyLoadedSolutionFile"/>.
        /// </remarks>
        /// <param name="solutionFileName">Name of the solution file.</param>
        /// <returns><c>true</c> if solution file has been loaded successfully; <c>false</c> otherwise.</returns>
        /// <seealso cref="CurrentlyLoadedSolutionFile"/>
        bool LoadSolutionFile(string solutionFileName);

        /// <summary>
        /// Builds the solution file.
        /// </summary>
        /// <remarks>
        /// The solution file to be built is available in <see cref="CurrentlyLoadedSolutionFile"/>. If no solution file has been loaded this method will return <c>false</c>.
        /// </remarks>
        /// <param name="configurationName">Name of the configuration to build.</param>
        /// <returns><c>true</c> if solution has been built successfully; <c>false</c> otherwise;</returns>
        /// <seealso cref="CurrentlyLoadedSolutionFile"/>
        /// <seealso cref="LoadSolutionFile"/>
        bool BuildSolution(string configurationName);
    }
}
