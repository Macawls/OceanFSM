using System.Collections.Generic;

namespace OceanFSM
{
    /// <summary>
    /// Used to build an <i>Autonomous</i> State Machine.
    /// </summary>
    /// <typeparam name="T">
    /// A reference type used to associate states and machines with a specific instance of a class.
    /// </typeparam>
    public class AutonomousBuilder<T> where T : class
    {
        private readonly T _mRunner;
        private string _mInitialStateID;
        private readonly HashSet<State<T>> _mStates = new();

        public AutonomousBuilder(T runner)
        {
            _mRunner = runner;
        }
        
        /// <summary>
        /// Sets the initial state of the state machine.
        /// </summary>
        public AutonomousBuilder<T> SetInitialState(string stateID)
        {
            _mInitialStateID = stateID;
            return this;
        }

        /// <summary>
        /// Adds a state to the state machine.
        /// </summary>
        /// <param name="state">
        /// The state to add.
        /// </param>
        public AutonomousBuilder<T> AddState(State<T> state)
        {
            _mStates.Add(state);
            return this;
        }
        
        /// <inheritdoc cref="AddState"/>
        public AutonomousBuilder<T> AddStates(params State<T>[] states)
        {
            _mStates.UnionWith(states);
            return this;
        }
        
        /// <inheritdoc cref="AddState"/>
        public AutonomousBuilder<T> AddStates(IEnumerable<State<T>> states)
        {
            _mStates.UnionWith(states);
            return this;
        }

        /// <summary>
        /// Builds the state machine.
        /// </summary>
        /// <returns>
        /// A new <see cref="IAutonomousMachine{T}"/> instance.
        /// </returns>
        public IAutonomousMachine<T> Build()
        {
            return new AutonomousMachine<T>(_mRunner, _mInitialStateID, _mStates);
        }
    }
}