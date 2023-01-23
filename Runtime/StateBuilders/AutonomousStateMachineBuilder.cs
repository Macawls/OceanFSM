using System.Collections.Generic;
using UnityEngine;

namespace OceanFSM
{
    public class AutonomousStateMachineBuilder<T> where T : class
    {
        private readonly T _mRunner;
        private State<T> _mStartingState;
        private readonly List<State<T>> _mStates = new();
        
        public AutonomousStateMachineBuilder(T runner)
        {
            _mRunner = runner;
        }
        
        public AutonomousStateMachineBuilder<T> SetStartingState(State<T> state)
        {
            _mStartingState = state;
            return this;
        }
        
        public AutonomousStateMachineBuilder<T> AddState(State<T> state)
        {
            _mStates.Add(state);
            return this;
        }

        /// <summary>
        /// Builds the state machine.
        /// </summary>
        /// <returns>
        /// A new <see cref="IAutonomousStateMachine{T}"/> instance.
        /// </returns>
        public IAutonomousStateMachine<T> Build()
        {
            return new AutonomousStateMachine<T>(_mRunner, _mStartingState, _mStates, Debug.Log);
        }
    }
}