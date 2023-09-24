namespace Dungeonesque.StateMachine
{
    public interface IState
    {
        void Update();
        void OnEnter();
        void OnExit();
    }
}