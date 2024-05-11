namespace BehaviourTree
{
    /// <summary>
    /// Node that succeed when any nodes succeed (OR)
    /// </summary>
    public class Selector : Node
    {
        #region Constructor

        public Selector(params Node[] children) : base(children) { }

        #endregion

        /// <inheritdoc/>
        public override NodeState Evaluate()
        {
            this.state = NodeState.FAILURE;

            foreach (var child in this.children)
            {
                var childState = child.Evaluate();

                // If child succeed or is running
                if (childState == NodeState.SUCCESS || childState == NodeState.RUNNING)
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