// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="BranchDoesNotExistException.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an error that happens when an operation is attempted on a branch that does not exist.
    /// </summary>
    [Serializable]
    public class BranchDoesNotExistException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="BranchDoesNotExistException"/> class.
        /// </summary>
        public BranchDoesNotExistException()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BranchDoesNotExistException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BranchDoesNotExistException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BranchDoesNotExistException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public BranchDoesNotExistException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BranchDoesNotExistException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected BranchDoesNotExistException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
