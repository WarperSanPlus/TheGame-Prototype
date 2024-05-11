using System.Collections.Generic;

namespace BehaviourTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    /// <summary>
    /// Class that represents a single element in the tree
    /// </summary>
    public class Node
    {
        public Node parent = null;
        protected List<Node> children = new();

        /// <summary>
        /// Sets this node as the parent of the given node
        /// </summary>
        private void Attach(Node node)
        {
            node.parent = this;
            this.children.Add(node);
        }

        #region Constructor

        public Node(params Node[] children)
        {
            foreach (var item in children)
                this.Attach(item);
        }

        #endregion

        #region Data

        private readonly Dictionary<string, object> dataContext = new();

        /// <summary>
        /// Stores the given value at the given key
        /// </summary>
        public void SetData(string key, object value, bool inRoot = false) 
        {
            if (inRoot && this.parent != null)
            {
                this.parent.SetData(key, value, true);
            }
            else
            {
                this.dataContext[key] = value;
            }
        }

        /// <summary>
        /// Fetches the value of the given key
        /// </summary>
        /// <returns>Value or null</returns>
        public T GetData<T>(string key)
        {
            // If key in self, return
            if (this.dataContext.TryGetValue(key, out var value) && value is T t)
                return t;

            // Search in parent
            var node = this.parent;
            while (node != null)
            {
                value = node.GetData<T>(key);
                if (value is T v)
                    return v;

                node = node.parent;
            }

            return default(T);
        }

        /// <summary>
        /// Removes the data associated with the given key
        /// </summary>
        /// <returns>The key was found</returns>
        public bool ClearData(string key)
        {
            // If has key, remove
            if (this.dataContext.ContainsKey(key))
            {
                _ = this.dataContext.Remove(key);
                return true;
            }

            // Search in parent
            var node = this.parent;
            while (node != null)
            {
                if (node.ClearData(key))
                    return true;

                node = node.parent;
            }

            return false;
        }

        #endregion
    
        #region State

        /// <summary>
        /// Node of this state
        /// </summary>
        protected NodeState state;

        /// <returns>State of this node</returns>
        public virtual NodeState Evaluate() => NodeState.FAILURE;

        #endregion
    }
}