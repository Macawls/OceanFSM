using System;

namespace OceanFSM
{
    public class StateTransition<T> where T : class
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
        public Action<T> OnTransition { get; }
        
        public StateTransition(State<T> from, State<T> to, Func<bool> condition, Action<T> onTransition = null)
        {
            From = from;
            To = to;
            Condition = condition;
            OnTransition = onTransition;
        }
    }
}