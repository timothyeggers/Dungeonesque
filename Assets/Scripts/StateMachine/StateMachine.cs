using System;
using System.Collections.Generic;

//* Credit to Jason Weimann for creating a super simple yet effective StateMachine! *//

namespace Dungeonesque.StateMachine
{
    public class StateMachine
    {
        private static readonly List<Transition> EmptyTransitions = new(0);
        private readonly List<Transition> _anyTransitions = new();

        private readonly Dictionary<Type, List<Transition>> _transitions = new();
        private IState _currentState;
        private List<Transition> _currentTransitions = new();


        public void At(IState from, IState to, Func<bool> predicate)
        {
            AddTransition(from, to, predicate);
        }

        public void Any(IState to, Func<bool> predicate)
        {
            AddAnyTransition(to, predicate);
        }

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
                SetState(transition.To);

            _currentState?.Update();
        }

        public void SetState(IState state)
        {
            if (state == _currentState)
                return;

            _currentState?.OnExit();
            _currentState = state;

            _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
            if (_currentTransitions == null)
                _currentTransitions = EmptyTransitions;

            _currentState.OnEnter();
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            _anyTransitions.Add(new Transition(state, predicate));
        }

        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions)
                if (transition.Condition())
                    return transition;

            foreach (var transition in _currentTransitions)
                if (transition.Condition())
                    return transition;

            return null;
        }

        private class Transition
        {
            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }

            public Func<bool> Condition { get; }
            public IState To { get; }
        }
    }
}