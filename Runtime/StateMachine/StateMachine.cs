using System;
using System.Collections.Generic;

namespace OceanFSM
{
    /// <summary>
    /// A state machine with a finite number of states.
    /// </summary>
    /// <typeparam name="T">
    /// A reference type used to associate/bind states and transitions with the state machine.
    /// </typeparam>
    public class StateMachine<T> where T : class
    {
        private State<T> _mCurrentState;
        private State<T> _mPreviousState;
        
        /// <summary>
        /// The current list of transitions belonging to the current state.
        /// </summary>
        private List<StateTransition<T>> _mActiveTransitions = new();
        
        /// <summary>
        /// A dictionary of transitions belonging to each state.
        /// </summary>
        private readonly Dictionary<State<T>, List<StateTransition<T>>> _mTransitions;
        
        /// <summary>
        /// The runner instance of the state machine.
        /// </summary>
        public T Runner { get;  }
        
        /// <summary>
        /// The initial state of the state machine.
        /// </summary>
        public State<T> StartingState { get; }
        
        /// <summary>
        /// The current state of the state machine.
        /// </summary>
        public State<T> CurrentState
        {
            get => _mCurrentState;
            
            private set
            {
                _mCurrentState?.OnExit();
                _mPreviousState = _mCurrentState;
                _mCurrentState = value;
                _mActiveTransitions = _mTransitions[CurrentState];
                
                _mCurrentState.OnEnter();
                OnStateChanged?.Invoke(_mCurrentState);
            }
        }
        /// <summary>
        /// The event invoked when the state machine changes state.
        /// </summary>
        public event Action<State<T>> OnStateChanged;
        
        /// <summary>
        /// The previous state of the state machine.
        /// </summary>
        public State<T> PreviousState => _mPreviousState;

        public StateMachine(T runner, State<T> startingState, Dictionary<State<T>, List<StateTransition<T>>> transitions)
        {
            Runner = runner;
            StartingState = startingState;
            _mTransitions = transitions;
        }

        /// <summary>
        /// Starts the state machine and transitions to the initial state by default.
        /// Use the overload to start in a different state, however the starting state will remain the same.
        /// </summary>
        public void Start()
        {
            CurrentState = StartingState;
        }

        /// <inheritdoc cref="Start()"/>
        public void Start(State<T> state)
        {
            CurrentState = state;
        }
        
        /// <summary>
        /// Stops the state machine.
        /// </summary>
        public void Stop()
        {
            CurrentState.OnExit();
            _mCurrentState = null;
        }
        
        /// <summary>
        /// Updates the current state of the state machine.
        /// </summary>
        public void Update(float deltaTime)
        {
            CurrentState.OnUpdate(deltaTime);
        }
        
        /// <summary>
        /// Updates the current state of the state machine.
        /// </summary>
        public void FixedUpdate(float fixedDeltaTime)
        {
            CurrentState.OnFixedUpdate(fixedDeltaTime);
        }

        /// <summary>
        /// Evaluates the current state of the state machine and checks for any possible transitions.
        /// If a condition is met, the state machine will transition to the state defined in the transition. 
        /// </summary>
        public void Evaluate()
        {
            _mActiveTransitions.ForEach(OnTransitionEvaluate);
        }
        
        private void OnTransitionEvaluate(StateTransition<T> stateTransition)
        {
            if (stateTransition.Condition() == false)
            {
                return;
            }
            
            CurrentState = stateTransition.To;
            stateTransition.OnTransition?.Invoke(Runner);
        }
    } 
}


