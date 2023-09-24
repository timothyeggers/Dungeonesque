using Dungeonesque.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Dungeonesque.StateMachine.States
{
    public class WanderState : IState
    {
        private readonly NavMeshAgent agent;
        private readonly float maxWanderRadius;
        private readonly float radius;
        private readonly Vector3 startPos;

        private readonly RangeFloat waitTime;
        private Vector3? queueDestination;
        private float waitFor;


        public WanderState(NavMeshAgent agent, RangeFloat waitTime, float radius, float maxDistanceFromStart = 0f)
        {
            this.agent = agent;
            this.waitTime = waitTime;
            this.radius = radius;
            maxWanderRadius = maxDistanceFromStart;
            startPos = agent.transform.position;
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }

        public void Update()
        {
            if (waitFor <= 0f && queueDestination != null)
            {
                agent.SetDestination((Vector3)queueDestination);
                queueDestination = null;
            }
            else
            {
                waitFor -= Time.deltaTime;
            }

            if (DestinationReached() && queueDestination == null)
            {
                waitFor = Random.Range(waitTime.x, waitTime.y);
                queueDestination = GetNextPosition();
            }
        }

        protected Vector3 GetNextPosition()
        {
            var point = Random.onUnitSphere * radius;
            point.y = 0;
            point += agent.transform.position;

            if (Vector3.Distance(startPos, point) > maxWanderRadius)
                return point.magnitude * 2 * (startPos - point).normalized;
            return point;
        }

        private bool DestinationReached()
        {
            return agent.remainingDistance < 1f;
        }
    }
}