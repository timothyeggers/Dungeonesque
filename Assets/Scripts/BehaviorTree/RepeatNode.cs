namespace Dungeonesque.BehaviorTree
{
    public class RepeatNode : DecoraterNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            child.Update();
            return State.Running;
        }
    }
}