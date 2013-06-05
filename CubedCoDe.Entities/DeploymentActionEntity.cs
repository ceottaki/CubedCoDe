// ----------------------------------------------------------------------------------------------------------------------------
// <copyright file="DeploymentActionEntity.cs" company="Felipe Ceotto">Copyright (c) Felipe Ceotto. All rights reserved.</copyright>
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

namespace CubedCoDe.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a deployment action entity.
    /// </summary>
    [Serializable]
    public class DeploymentActionEntity : IEquatable<DeploymentActionEntity>
    {
        /// <summary>
        /// The action type.
        /// </summary>
        private DeploymentActionType _actionType = DeploymentActionType.None;

        /// <summary>
        /// The action parameters.
        /// </summary>
        private Collection<string> _actionParameters = new Collection<string>();

        /// <summary>
        /// The list of rollback actions to take if this action fails.
        /// </summary>
        private Collection<DeploymentActionEntity> _rollbackActions = new Collection<DeploymentActionEntity>();

        /// <summary>
        /// A flag indicating if this action is key to deployment or not.
        /// </summary>
        private bool _isKeyToDeployment = true;

        /// <summary>
        /// Initialises a new instance of the <see cref="DeploymentActionEntity"/> class.
        /// </summary>
        public DeploymentActionEntity()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DeploymentActionEntity"/> class.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <param name="actionParameters">The action parameters.</param>
        public DeploymentActionEntity(DeploymentActionType actionType, Collection<string> actionParameters)
        {
            _actionType = actionType;
            _actionParameters = actionParameters;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DeploymentActionEntity"/> class.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <param name="actionParameters">The action parameters.</param>
        public DeploymentActionEntity(DeploymentActionType actionType, params string[] actionParameters)
        {
            _actionType = actionType;
            _actionParameters = new Collection<string>(actionParameters);
        }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        public DeploymentActionType ActionType
        {
            get
            {
                return _actionType;
            }

            set
            {
                _actionType = value;
            }
        }

        /// <summary>
        /// Gets the action parameters.
        /// </summary>
        /// <value>
        /// The action parameters.
        /// </value>
        public Collection<string> ActionParameters
        {
            get
            {
                return _actionParameters;
            }
        }

        /// <summary>
        /// Gets the rollback action to be executed if the current action fails.
        /// </summary>
        /// <remarks>
        /// If empty then the default rollback action is assumed for the given action.
        /// To set no rollback action add an action with type <see cref="DeploymentActionType.None"/>.
        /// </remarks>
        /// <value>
        /// The rollback action to be executed if the current action fails.
        /// </value>
        public Collection<DeploymentActionEntity> RollbackActions
        {
            get
            {
                return _rollbackActions;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this action is key to deployment.
        /// </summary>
        /// <remarks>
        /// If an action that is key to the deployment fails, the whole deployment is deemed failed and all actions taken before this action are rolled back.
        /// If an action is not key to the deployment its failure will be ignored and other actions taken if there are any.
        /// </remarks>
        /// <value>
        ///   <c>true</c> if this action is key to deployment; otherwise, <c>false</c>. Default is <c>true</c>.
        /// </value>
        public bool IsKeyToDeployment
        {
            get
            {
                return _isKeyToDeployment;
            }

            set
            {
                _isKeyToDeployment = value;
            }
        }

        /// <summary>
        /// The equality operator returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.
        /// </summary>
        /// <param name="deploymentAction1">The deployment action 1.</param>
        /// <param name="deploymentAction2">The deployment action 2.</param>
        /// <returns><c>true</c> if the values of its operands are equal, <c>false</c> otherwise</returns>
        public static bool operator ==(DeploymentActionEntity deploymentAction1, DeploymentActionEntity deploymentAction2)
        {
            if (deploymentAction1 == null)
            {
                return deploymentAction2 == null;
            }
            else
            {
                return deploymentAction1.Equals(deploymentAction2);
            }
        }

        /// <summary>
        /// The inequality operator returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.
        /// </summary>
        /// <param name="deploymentAction1">The deployment action 1.</param>
        /// <param name="deploymentAction2">The deployment action 2.</param>
        /// <returns><c>true</c> if the values of its operands are not equal, <c>false</c> otherwise</returns>
        public static bool operator !=(DeploymentActionEntity deploymentAction1, DeploymentActionEntity deploymentAction2)
        {
            if (deploymentAction1 == null)
            {
                return deploymentAction2 != null;
            }
            else
            {
                return !deploymentAction1.Equals(deploymentAction2);
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(DeploymentActionEntity other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                return (_actionType == other.ActionType) && (_actionParameters == other.ActionParameters);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            DeploymentActionEntity other = obj as DeploymentActionEntity;
            if (other == null)
            {
                return false;
            }
            else
            {
                return Equals(other);
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _actionType.GetHashCode() ^ _actionParameters.GetHashCode();
        }
    }
}
