using System;
using System.Collections.Generic;

namespace OceanFSM
{
    /// <summary>
    /// Used to build a <i>Transition</i> State Machine.
    /// </summary>
    /// <typeparam name="T">
    /// A reference type used to associate/bind states and transitions with the state machine.
    /// </typeparam>
    public class TransitionalStateMachineBuilder<T> where T : class
    {
        private readonly T _mRunner;
        private State<T> _mStartingState;
        private readonly Dictionary<State<T>, List<StateTransition<T>>> _mTransitions = new();
        
        public TransitionalStateMachineBuilder(T stateRunner)
        {
            _mRunner = stateRunner;
        }
        /// <summary>
        /// Sets the starting state of the state machine.
        /// </summary>
        /// <param name="startingState">
        /// The starting/default state.
        /// </param>
        public TransitionalStateMachineBuilder<T> SetStartingState(State<T> startingState)
        {
            _mStartingState = startingState;
            return this;
        }
        
        /// <summary>
        ///  Adds a transition to the state machine.
        /// </summary>
        /// <param name="from">The state to transition from</param>
        /// <param name="to">The state to transition to.</param>
        /// <param name="condition">The condition or predicate to occur to change state.</param>
        /// <param name="onTransition"> An action to perform when the transition occurs.</param>
        public TransitionalStateMachineBuilder<T> AddTransition(State<T> from, State<T> to, Func<bool> condition, Action<T> onTransition = null)
        {
            if (!_mTransitions.ContainsKey(from))
            {
                _mTransitions.Add(from, new List<StateTransition<T>>());
            }

            var transition = onTransition == null ? 
                new StateTransition<T>(from, to, condition) : 
                new StateTransition<T>(from, to, condition, onTransition);
            
            
            _mTransitions[from].Add(transition);
            return this;
        }
        
        /// <inheritdoc cref="AddTransition(OceanFSM.State{T},OceanFSM.State{T},System.Func{bool},System.Action{T})"/>
        public TransitionalStateMachineBuilder<T> AddTransition(StateTransition<T> stateTransition)
        {
            if (!_mTransitions.ContainsKey(stateTransition.From))
            {
                _mTransitions.Add(stateTransition.From, new List<StateTransition<T>>());
            }
            
            _mTransitions[stateTransition.From].Add(stateTransition);
            return this;
        }

        /// <summary>
        /// Builds the state machine.
        /// </summary>
        /// <returns>
        /// A new <see cref="TransitionalStateMachine{T}"/> instance.
        /// </returns>
        public ITransitionalStateMachine<T> Build()
        {
            return new TransitionalStateMachine<T>(_mRunner, _mStartingState, _mTransitions);
        }
    }
}