using System;

namespace OceanFSM
{
    public interface IStateMachineContext<T> where T : class
    {
        T Runner { get;  }
        State<T> PreviousState { get; }
        State<T> CurrentState { get; }
        State<T> StartingState { get; }
        
        event Action<State<T>> OnStateChanged;
    }
}