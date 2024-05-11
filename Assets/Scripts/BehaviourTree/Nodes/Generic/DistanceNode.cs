using Extensions;
using UnityEngine;

namespace BehaviourTree.Nodes.Generic
{
    /// <summary>
    /// Nodes that calculates the distance between self and the target
    /// </summary>
    public abstract class DistanceNode : Node 
    {
        private readonly Transform self;
        private readonly string target;

        public DistanceNode(Transform self, string target)
        {
            this.self = self;
            this.target = target;
        }

        protected abstract NodeState GetState(float distance);

        #region Node

        /// <inheritdoc/>
        public sealed override NodeState Evaluate() 
        {
            // If self is invalid, return failure
            if (this.self == null)
                return NodeState.FAILURE;

            // If target is invalid, return success
            var target = this.GetData<Transform>(this.target);

            if (target == null)
                return NodeState.SUCCESS;

            // Get distance
            var distance = this.self.Distance(target);

            // If close enough from target
            return this.GetState(distance);
        }

        #endregion
    }
}