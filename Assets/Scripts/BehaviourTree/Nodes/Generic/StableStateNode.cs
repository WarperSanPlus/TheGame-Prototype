namespace BehaviourTree.Nodes.Generic
{
    public class StableStateNode : Node 
    {
        private readonly NodeState stableState;
        public StableStateNode(NodeState state) => this.stableState = state;

        /// <inheritdoc/>
        public override NodeState Evaluate() => this.stableState;
    }
}