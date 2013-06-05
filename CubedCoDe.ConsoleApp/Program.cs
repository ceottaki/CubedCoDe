// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Timers;
    using CubedCoDe.WindowsService;

    /// <summary>
    /// Represents a program that runs a continuous deployment application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The application used to run the continuous deployment.
        /// </summary>
        private static CubedCoDeApplication _cubedCoDeApplication;

        /// <summary>
        /// Main entry point for this program.
        /// </summary>
        private static void Main()
        {
            _cubedCoDeApplication = new CubedCoDeApplication();
            _cubedCoDeApplication.Start();

            Console.ReadKey();
        }
    }
}
