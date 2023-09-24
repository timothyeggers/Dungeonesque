using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Dungeonesque.StateMachine.States
{
    public class InvestigateState : IState
    {
        public delegate void InvestigationFinished();

        private readonly NavMeshAgent agent;
        private readonly InvestigationFinished onFinished;
        private readonly float timeToWait;

        private MonoBehaviour coroutine;
        private Vector3? queueDestination;

        private Vector3 startPos;

        public InvestigateState(NavMeshAgent agent, float timeToWait, InvestigationFinished onFinished)
        {
            this.agent = agent;
            this.timeToWait = timeToWait;
            startPos = agent.transform.position;
            this.onFinished = onFinished;
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
            if (queueDestination is Vector3 queue)
            {
                agent.SetDestination(queue);
                queueDestination = null;
            }

            if (agent.remainingDistance < 1f) coroutine.StartCoroutine(ScoutAreaRoutine());
        }

        public void SetTargetPosition(Vector3 position)
        {
            queueDestination = position;
        }

        private IEnumerator ScoutAreaRoutine()
        {
            yield return new WaitForSeconds(timeToWait);
            onFinished();
        }
    }
}