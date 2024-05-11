namespace BehaviourTree
{
    /// <summary>
    /// Node that succeed when all nodes succeed (AND)
    /// </summary>
    public class Sequence : Node
    {
        #region Constructor

        public Sequence(params Node[] children) : base(children) { }

        #endregion

        /// <inheritdoc/>
        public override NodeState Evaluate() 
        {
            this.state = NodeState.SUCCESS;

            foreach (var child in this.children)
            {
                var childState = child.Evaluate();

                // If child failed or is running
                if (childState == NodeState.FAILURE || childState == NodeState.RUNNING)
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