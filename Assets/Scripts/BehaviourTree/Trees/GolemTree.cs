using BehaviourTree.Nodes.Generic;
using UnityEngine;

namespace BehaviourTree.Trees
{
    public class GolemTree : Tree
    {
        public enum GolemAttackFlags
        {
            None = 0,
            Throw = 1,
            Stomp = 2,
        }

        public Transform target1;
        public float throwMinRange;
        public float throwMaxRange;
        public float walkMinRange;
        public float walkMaxRange;
        public float stompMinRange;

        protected override Node SetUpTree()
        {
            var target = "currentTarget";

            var root = new Selector(
                //this.Throw(target, this.throwMinRange, this.throwMaxRange),
                //new CallbackNode(n => SetAttack(this.animator, GolemAttackFlags.None), NodeState.FAILURE),
                this.WalkToTarget(target, this.walkMinRange, this.walkMaxRange),
                new CallbackNode(n => this.SetWalking(false), NodeState.FAILURE)
                //this.Stomp(target, this.stompMinRange),
                //new CallbackNode(n => SetAttack(this.animator, GolemAttackFlags.None), NodeState.FAILURE)
            );

            root.SetData(MoveToTarget.SPEED, 0.01f);
            root.SetData(target, this.target1);

            return root;
        }

        private Node WalkToTarget(string target, float minDistance, float maxDistance)
        {
            var minLimit = new DistanceGreater(this.transform, target, minDistance);
            var maxLimit = new DistanceSmaller(this.transform, target, maxDistance);
            var action = new Parallel(
                new RotateToTarget(this.transform, target),
                new MoveToTarget(this.transform, target, this.SetWalking)
            );

            return new Sequence(
                // Has to be more than minDistance
                minLimit,

                // Has to be less than maxDistance
                maxLimit,

                // If far enough, do action
                action
            );
        }

        private Node Stomp(string target, float minDistance)
        {
            var distanceLimit = new DistanceSmaller(this.transform, target, minDistance);
            var action = new AnimationNode(this.animator, 1f, 1.667f, a => SetAttack(a, GolemAttackFlags.Stomp));

            return new Sequence(
                // Needs to be less than minDistance
                distanceLimit,

                // If near enough, do action
                action
            );
        }

        private Node Throw(string target, float minDistance, float maxDistance)
        {
            var minLimit = new DistanceGreater(this.transform, target, minDistance);
            var maxLimit = new DistanceSmaller(this.transform, target, maxDistance);
            var action = new AnimationNode(this.animator, 2f, 2.917f, a => SetAttack(a, GolemAttackFlags.Throw));

            return new Sequence(
                // Has to be more than minDistance
                minLimit,

                // Has to be less than maxDistance
                maxLimit,

                // If far enough, do action
                action
            );
        }

        #region Animation

        [Header("Animation")]
        [SerializeField]
        private Animator animator;

        private void SetWalking(bool IsWalking) => this.animator.SetBool("IsWalking", IsWalking);

        private static void SetAttack(Animator animator, GolemAttackFlags attack)
        {
            animator.SetInteger("Attack", (int)attack);
            animator.SetBool("IsAttacking", attack != GolemAttackFlags.None);
        }

        #endregion

        #region Gizmos
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(this.transform.position, this.transform.up, this.stompMinRange);

            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(this.transform.position, this.transform.up, this.throwMinRange);
            UnityEditor.Handles.DrawWireDisc(this.transform.position, this.transform.up, this.throwMaxRange);

            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawWireDisc(this.transform.position, this.transform.up, this.walkMinRange);
            UnityEditor.Handles.DrawWireDisc(this.transform.position, this.transform.up, this.walkMaxRange);
        }
#endif
        #endregion
    }
}