using System;

namespace OceanFSM
{
    /// <summary>
    /// Represents a transition between two states.
    /// </summary>
    /// <typeparam name="T">
    /// A reference type used to associate states and machines with a specific instance of a class.
    /// </typeparam>
    public class Transition<T> where T : class
    {
        /// <summary>
        /// The state to transition from.
        /// </summary>
        public State<T> From { get; }
        
        /// <summary>
        /// The state to transition to.
        /// </summary>
        public State<T> To { get; }
        
        /// <summary>
        /// The condition that must be met to trigger the transition.
        /// </summary>
        public Func<bool> Condition { get; }
        
        /// <summary>
        /// Optional. An action that is called when the transition is triggered.
        /// </summary>
        public Action OnTransition { get; }
        
        public Transition(State<T> from, State<T> to, Func<bool> condition, Action onTransition = null)
        {
            From = from;
            To = to;
            Condition = condition;
            OnTransition = onTransition;
        }
    }
}