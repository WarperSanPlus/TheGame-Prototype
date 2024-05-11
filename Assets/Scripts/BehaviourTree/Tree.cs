using UnityEngine;

namespace BehaviourTree
{
    // Notion from here: https://www.youtube.com/watch?v=aR6wt5BlE-E
    public abstract class Tree : MonoBehaviour
    {
        private Node root = null;

        private void Start() {
            this.root = this.SetUpTree();
        }

        private void Update() 
        {
            _ = (this.root?.Evaluate());
        }

        protected abstract Node SetUpTree();
    }
}