using System;

namespace OceanFSM
{
    /// <summary>
    /// The context of a state machine.
    /// </summary>
    public interface IStateMachineContext<T> where T : class
    {
        /// <summary>
        /// The Runner instance that is running this state machine.
        /// </summary>
        T Runner { get;  }
        
        /// <summary>
        /// The previous state of the state machine.
        /// </summary>
        State<T> PreviousState { get; }
        
        /// <summary>
        /// The current state of the state machine.
        /// </summary>
        State<T> CurrentState { get; }

        /// <summary>
        /// The ID of the initial state.
        /// </summary>
        string InitialStateID { get; }

        /// <summary>
        /// The event triggered when the state machine changes state.
        /// </summary>
        event Action<State<T>> StateChanged;
    }
}