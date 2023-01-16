using System;

namespace OceanFSM
{
    public class StateTransition<T> where T : class, IStateMachineRunner<T>
    {
        /// <summary>
        /// The state to transition from.
        /// </summary>
        public State<T> From { get; private set; }
        
        /// <summary>
        /// The state to transition to.
        /// </summary>
        public State<T> To { get; private set; }
        
        /// <summary>
        /// The condition that must be met to trigger the transition.
        /// </summary>
        public Func<bool> Condition { get; private set; }
        
        /// <summary>
        /// Optional. An action that is called when the transition is triggered.
        /// </summary>
        public Action<T> OnTransition { get; private set; }
        
        public StateTransition(State<T> from, State<T> to, Func<bool> condition, Action<T> onTransition = null)
        {
            From = from;
            To = to;
            Condition = condition;
            OnTransition = onTransition;
        }
    }
}