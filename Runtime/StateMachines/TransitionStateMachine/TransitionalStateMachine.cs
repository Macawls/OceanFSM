using System.Collections.Generic;

namespace OceanFSM
{
    public class TransitionalStateMachine<T> : StateMachineBase<T>, 
        ITransitionalStateMachine<T> where T : class
    {
        /// <summary>
        /// The current list of transitions belonging to the current state.
        /// </summary>
        private List<StateTransition<T>> _mActiveTransitions = new();
        
        /// <summary>
        /// A dictionary of transitions where each state has a list of transitions.
        /// </summary>
        private readonly Dictionary<State<T>, List<StateTransition<T>>> _mTransitions;
        
        public override State<T> CurrentState
        {
            get => CurrentStateInternal;

            protected set
            {
                CurrentStateInternal?.OnExit();
                PreviousStateInternal = CurrentStateInternal;
                CurrentStateInternal = value;
                _mActiveTransitions = _mTransitions[CurrentStateInternal];
                
                CurrentStateInternal.OnEnter();
                InvokeOnStateChanged(CurrentStateInternal);
            }
        }
        
        public TransitionalStateMachine(T runner, State<T> startingState, 
            Dictionary<State<T>, List<StateTransition<T>>> transitions) : base(runner, startingState)
        {
            _mTransitions = transitions;
            
            foreach (var state in _mTransitions.Keys)
            {
                state.InitializeInternal(runner, this);
            }
        }
        /// <summary>
        /// Evaluates the current state's transitions and changes the state if a transition's condition is met.
        /// </summary>
        public void Evaluate()
        {
            _mActiveTransitions.ForEach(OnTransitionEvaluate);
        }
        
        private void OnTransitionEvaluate(StateTransition<T> stateTransition)
        {
            if (!stateTransition.Condition())
            {
                return;
            }
            
            CurrentState = stateTransition.To;
            stateTransition.OnTransition?.Invoke(Runner);
        }
    }
}