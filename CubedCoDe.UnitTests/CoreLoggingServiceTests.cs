// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="CoreLoggingServiceTests.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CubedCoDe.Core.Interfaces;
    using CubedCoDe.Core.Services;
    using Moq;
    using NUnit.Framework;

    /// <summary>
    /// Represents a test fixture for the core logging service.
    /// </summary>
    public class CoreLoggingServiceTests
    {
        /// <summary>
        /// A mock for the logger.
        /// </summary>
        private Mock<ILogger> _loggerMock;
        
        /// <summary>
        /// The logging service to be tested.
        /// </summary>
        private ILoggingService _loggingService;

        /// <summary>
        /// Initialises this instance of the test fixture before running every test.
        /// </summary>
        [SetUp]
        public void Init()
        {
            _loggerMock = new Mock<ILogger>();
            _loggingService = new LoggingService(_loggerMock.Object);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        [TearDown]
        public void Dispose()
        {
            _loggingService = null;
            _loggerMock = null;
        }

        // TODO: Write tests for every log service method.

        /// <summary>
        /// Tests the write to log method with message parameter.
        /// </summary>
        [Test]
        public void TestWriteToLogWithMessage()
        {
            Assert.Fail("Test not implemented.");
        }

        /// <summary>
        /// Tests the write to log method with message and categories parameters.
        /// </summary>
        [Test]
        public void TestWriteToLogWithMessageAndCategories()
        {
            Assert.Fail("Test not implemented.");
        }

        /// <summary>
        /// Tests the write to log method with message and categories and priority parameters.
        /// </summary>
        [Test]
        public void TestWriteToLogWithMessageAndCategoriesAndPriority()
        {
            Assert.Fail("Test not implemented.");
        }

        /// <summary>
        /// Tests the write to log method with message and single category parameters.
        /// </summary>
        [Test]
        public void TestWriteToLogWithMessageAndSingleCategory()
        {
            Assert.Fail("Test not implemented.");
        }

        /// <summary>
        /// Tests the write to log method with message and single category and priority parameters.
        /// </summary>
        [Test]
        public void TestWriteToLogWithMessageAndSingleCategoryAndPriority()
        {
            Assert.Fail("Test not implemented.");
        }

        /// <summary>
        /// Tests the write method call to log method.
        /// </summary>
        [Test]
        public void TestWriteMethodCallToLog()
        {
            Assert.Fail("Test not implemented.");
        }
    }
}
