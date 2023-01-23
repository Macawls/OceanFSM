using System;
using System.Linq;
using System.Collections.Generic;

namespace OceanFSM
{
    /// <summary>
    ///  Represents a state machine whereby states 
    /// </summary>
    /// <typeparam name="T">
    /// A reference type used to associate states and machines with a specific instance of a class.
    /// </typeparam>
    public class AutonomousStateMachine<T> : StateMachineBase<T>, 
        IAutonomousStateMachine<T> where T : class
    {
        private readonly Dictionary<Type, State<T>> _mStateMap = new();
        private readonly Action<string> _mDebugHandler;

        
        /// <summary>
        /// Initializes a new instance of the <see cref="AutonomousStateMachine{T}" /> class.
        /// </summary>
        /// <param name="runner">
        /// The runner.
        /// </param>
        /// <param name="startingState">
        /// The starting state.
        /// </param>
        /// <param name="states">
        /// A list of states.
        /// </param>
        /// <param name="debugHandler">
        /// The debug handler.
        /// </param>
        public AutonomousStateMachine(T runner, State<T> startingState, List<State<T>> states, Action<string> debugHandler) : 
            base(runner, startingState)
        {
            _mDebugHandler = debugHandler;
            
            // Object initializer syntax
            // - Create a new list including the starting state
            new List<State<T>>(states)
            {
                startingState
            }.ForEach(state => 
            {
                AddState(state);
                state.InitializeInternal(runner, this);
            });
        }
        
        private void AddState(State<T> state)
        {
            var type = state.GetType();
            
            if (_mStateMap.ContainsKey(type))
            {
                _mDebugHandler.Invoke($"State: [{type.Name}] already exists in the machine, will not add again.");
                return;
            }
            
            _mStateMap.Add(type, state);
        }
        
        public TState ChangeState<TState>(Action onTransition = null) where TState : State<T>
        {
            var type = typeof(TState);
            
            if (!_mStateMap.ContainsKey(type))
            {
                _mDebugHandler.Invoke($"State: [{type.Name}] does not exist in the machine, cannot change to it.");
                return null;
            }
            
            CurrentState = _mStateMap[type];
            onTransition?.Invoke();
            return CurrentState as TState;
        }

        public State<T> ChangeState(string stateName, Action onTransition = null)
        {
            var type = _mStateMap.Values.FirstOrDefault(s => s.GetType().Name == stateName)?.GetType();
            
            if (type == null)
            {
                _mDebugHandler.Invoke($"State: [{stateName}] does not exist in the machine, cannot change to it.");
                return null;
            }
            
            if (!_mStateMap.ContainsKey(type))
            {
                _mDebugHandler.Invoke($"State: [{type.Name}] does not exist in the machine, cannot change to it.");
                return null;
            }
            
            CurrentState = _mStateMap[type];
            onTransition?.Invoke();
            return CurrentState;
        }
    }
}