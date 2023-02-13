using System;

namespace OceanFSM
{
    /// <summary>
    /// Represents a state machine whereby states are responsible for their own transitions.
    /// </summary>
    /// <typeparam name="T">
    /// A reference type used to associate states and machines with a specific instance of a class.
    /// </typeparam>
    public interface IAutonomousMachine<T> : 
        IStateMachine<T>, 
        ITransitionCommandHandler<T> where T : class
    {
        /// <summary>
        /// Changes the current state to the specified state.
        /// </summary>
        /// <param name="onTransition">
        /// Optional callback that will be invoked when a transition occurs.
        /// </param>
        /// <typeparam name="TState">
        /// The type of the state.
        /// </typeparam>
        /// <returns>
        /// The state that was transitioned to. Null if the transition failed.
        /// </returns>
        TState ChangeState<TState>(Action onTransition = null) where TState : State<T>;
        
        
        /// <summary>
        /// Changes the current state to the specified state.
        /// </summary>
        /// <param name="stateID">
        /// The name of the state to transition to.
        /// </param>
        /// <param name="onTransition">
        /// Optional callback that will be invoked when a transition occurs.
        /// </param>
        /// <returns></returns>
        State<T> ChangeState(string stateID, Action onTransition = null);
    }
}

