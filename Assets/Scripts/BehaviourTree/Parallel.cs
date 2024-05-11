namespace BehaviourTree
{
    /// <summary>
    /// Node that fails when at least one node fails
    /// </summary>
    public class Parallel : Node
    {
        #region Constructor

        public Parallel(params Node[] children) : base(children) { }

        #endregion

        /// <inheritdoc/>
        public override NodeState Evaluate() 
        {
            this.state = NodeState.RUNNING;

            foreach (var child in this.children)
            {
                var childState = child.Evaluate();

                // If child failed 
                if (childState == NodeState.FAILURE)
                {
                    // Copy and exit
                    this.state = childState;
                    break;
                }
            }

            return this.state;
        }
    }
}