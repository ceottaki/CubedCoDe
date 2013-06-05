// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="BranchEntity.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a branch entity.
    /// </summary>
    public class BranchEntity
    {
        /// <summary>
        /// Gets or sets the name of the branch.
        /// </summary>
        /// <value>
        /// The name of the branch.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this branch is a remote branch.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this branch is a remote branch; if the branch is local, <c>false</c>.
        /// </value>
        public bool IsRemote
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this branch is the current head of the repository.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this branch is the current head of the repository; otherwise, <c>false</c>.
        /// </value>
        public bool IsCurrentHead
        {
            get;
            set;
        }
    }
}
