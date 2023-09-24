using UnityEngine;
using UnityEngine.AI;

namespace Dungeonesque.StateMachine.States
{
    public class ChaseState : IState
    {
        public delegate void ChaseFinished();

        private readonly NavMeshAgent agent;
        private readonly float maxDistanceFromTarget;
        private readonly ChaseFinished onFinished;

        private MonoBehaviour coroutine;
        private Vector3? queueDestination;

        private Vector3 startPos;
        private GameObject target;

        public ChaseState(NavMeshAgent agent, float maxDistanceFromTarget, ChaseFinished onFinished)
        {
            this.agent = agent;
            startPos = agent.transform.position;
            this.onFinished = onFinished;
            this.maxDistanceFromTarget = maxDistanceFromTarget;
/*        this.enemyLayer = 1 << LayerMask.NameToLayer("Default");*/
        }

        public void OnEnter()
        {
            coroutine = agent.gameObject.GetComponent<MonoBehaviour>();
        }

        public void OnExit()
        {
            coroutine = null;
        }

        public void Update()
        {
            agent.SetDestination(target.transform.position);

            if (agent.remainingDistance > maxDistanceFromTarget) onFinished();
        }

        public void SetTarget(GameObject target)
        {
            this.target = target;
        }
    }
}