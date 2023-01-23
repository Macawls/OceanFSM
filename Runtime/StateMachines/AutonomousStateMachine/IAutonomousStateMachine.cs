using System;

namespace OceanFSM
{
    public interface IAutonomousStateMachine<T> : 
        IStateMachine<T> where T : class
    {
        TState ChangeState<TState>(Action onTransition = null) where TState : State<T>;
        State<T> ChangeState(string stateName, Action onTransition = null);
    }
}

