using System;
using System.Collections.Generic;

namespace OceanFSM
{
    /// <summary>
    /// Used to build a <i>Polling</i> State Machine.
    /// </summary>
    /// <typeparam name="T">
    /// A reference type used to associate states and machines with a specific instance of a class.
    /// </typeparam>
    public class PollingBuilder<T> where T : class
    {
        private readonly T _mRunner;
        private string _mInitialStateID;
        private readonly Dictionary<State<T>, List<Transition<T>>> _mTransitions = new();
        
        public PollingBuilder(T runner)
        {
            _mRunner = runner;
        }
        
        /// <summary>
        /// Sets the initial state of the state machine.
        /// </summary>
        public PollingBuilder<T> SetInitialState(string stateID)
        {
            _mInitialStateID = stateID;
            return this;
        }
        
        /// <summary>
        ///  Adds a transition to the state machine.
        /// </summary>
        /// <param name="transition"> The transition to add. </param>
        public PollingBuilder<T> AddTransition(Transition<T> transition)
        {
            if (!_mTransitions.ContainsKey(transition.From))
            {
                _mTransitions.Add(transition.From, new List<Transition<T>>());
            }
            
            _mTransitions[transition.From].Add(transition);
            return this;
        }
        
        /// <inheritdoc cref="AddTransition(OceanFSM.Transition{T})"/>
        public PollingBuilder<T> AddTransition(State<T> from, State<T> to, Func<bool> condition, Action onTransition = null)
        {
            return AddTransition(new Transition<T>(from, to, condition, onTransition));
        }
        
        /// <summary>
        /// Add multiple transitions to the state machine.
        /// </summary>
        public PollingBuilder<T> AddTransitions(IEnumerable<Transition<T>> transitions)
        {
            foreach (var transition in transitions)
            {
                AddTransition(transition);
            }
            
            return this;
        }

        /// <summary>
        /// Builds the state machine.
        /// </summary>
        /// <returns>
        /// A new <see cref="PollingMachine{T}"/> instance.
        /// </returns>
        public IPollingMachine<T> Build()
        {
            return new PollingMachine<T>(_mRunner, _mInitialStateID, _mTransitions);
        }
    }
}