using System.Collections.Generic;

namespace OceanFSM
{
    /// <summary>
    /// Represents a state machine whereby the machine evaluates the current state's transitions.
    /// </summary>
    /// <typeparam name="T">
    /// A reference type used to associate states and machines with a specific instance of a class.
    /// </typeparam>
    public interface IPollingMachine<T> : IStateMachine<T> where T : class
    {
        /// <summary>
        /// Evaluates the current state's transitions and changes the state if a transition's condition is met.
        /// </summary>
        void Evaluate();
        
        /// <summary>
        /// A dictionary of all the transitions in the machine. Each state has a list of transitions.
        /// </summary>
        IReadOnlyDictionary<State<T>, List<Transition<T>>> Transitions { get; }
    }
}