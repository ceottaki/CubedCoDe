// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="DotNetBuilder.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.Infrastructure.Services
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using CubedCoDe.Core.Interfaces;
    using Microsoft.Build.Evaluation;

    /// <summary>
    /// Represents a .NET builder.
    /// </summary>
    public class DotNetBuilder : IProjectBuilder
    {
        /// <summary>
        /// The name of the file of the currently loaded solution.
        /// </summary>
        private string _currentlyLoadedSolutionFile = null;

        /// <summary>
        /// Gets the currently loaded solution file.
        /// </summary>
        /// <value>
        /// The currently loaded solution file.
        /// </value>
        public string CurrentlyLoadedSolutionFile
        {
            get
            {
                return _currentlyLoadedSolutionFile;
            }
        }

        /// <summary>
        /// Loads the solution file containing instructions on what to be built.
        /// </summary>
        /// <param name="solutionFileName">Name of the solution file.</param>
        /// <returns>
        ///   <c>true</c> if solution file has been loaded successfully; <c>false</c> otherwise.
        /// </returns>
        /// <seealso cref="CurrentlyLoadedSolutionFile" />
        /// <remarks>
        /// The name of the file of the solution is made available in property <see cref="CurrentlyLoadedSolutionFile" />.
        /// </remarks>
        public bool LoadSolutionFile(string solutionFileName)
        {
            bool result = false;

            // Checks that file exists.
            if (File.Exists(solutionFileName))
            {
                // Checks that file can be opened for reading.
                using (FileStream solutionFileStream = File.OpenRead(solutionFileName))
                {
                    _currentlyLoadedSolutionFile = solutionFileName;
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Builds the solution file.
        /// </summary>
        /// <param name="configurationName">Name of the configuration to build.</param>
        /// <returns>
        ///   <c>true</c> if solution has been built successfully; <c>false</c> otherwise;
        /// </returns>
        /// <seealso cref="CurrentlyLoadedSolutionFile" />
        ///   <seealso cref="LoadSolutionFile" />
        /// <remarks>
        /// The solution file to be built is available in <see cref="CurrentlyLoadedSolutionFile" />. If no solution file has been loaded this method will return <c>false</c>.
        /// </remarks>
        public bool BuildSolution(string configurationName)
        {
            if (_currentlyLoadedSolutionFile == null)
            {
                return false;
            }
            else
            {
                bool result = false;

                string msBuildProjectFile = BuildMsBuildProjectFile(_currentlyLoadedSolutionFile, configurationName);
                using (ProjectCollection msBuildProjectCollection = new ProjectCollection())
                {
                    msBuildProjectCollection.LoadProject(msBuildProjectFile);

                    if (msBuildProjectCollection.IsBuildEnabled)
                    {
                        result = true;
                        msBuildProjectCollection.LoadedProjects.ToList().ForEach(p => result = result && p.Build("Build"));
                    }
                }

                if (result)
                {
                    File.Delete(msBuildProjectFile);
                }

                return result;
            }
        }

        /// <summary>
        /// Builds the MS build project file that can be used to build the solution.
        /// </summary>
        /// <param name="solutionFileName">Name of the solution file.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <returns>The file name with full path of the MS build project file.</returns>
        private static string BuildMsBuildProjectFile(string solutionFileName, string configurationName)
        {
            string msBuildProjectFileName = null;

            string tempPath = Path.GetTempPath();
            int fileNumber = 0;

            do
            {
                msBuildProjectFileName = Path.Combine(tempPath, string.Format(CultureInfo.InvariantCulture, "{0}{1}.proj", solutionFileName, fileNumber));
                fileNumber++;
            }
            while (File.Exists(msBuildProjectFileName));

            using (XmlWriter xmlWriter = new XmlTextWriter(msBuildProjectFileName, null))
            {
                xmlWriter.WriteStartElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003");
                xmlWriter.WriteStartElement("ItemGroup");
                xmlWriter.WriteStartElement("Solution");
                xmlWriter.WriteAttributeString("Include", solutionFileName);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Target");
                xmlWriter.WriteAttributeString("Name", "Build");
                xmlWriter.WriteStartElement("MSBuild");
                xmlWriter.WriteAttributeString("Projects", "@(Solution)");
                xmlWriter.WriteAttributeString("Targets", "Build");
                xmlWriter.WriteAttributeString("Properties", string.Format(CultureInfo.InvariantCulture, "Configuration={0}", configurationName));
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }

            return msBuildProjectFileName;
        }
    }
}
