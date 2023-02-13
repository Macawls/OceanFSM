namespace OceanFSM
{
    /// <summary>
    /// The base class for all the states in the FSM.
    /// </summary>
    /// <typeparam name="T">
    /// A reference type used to associate states and machines with a specific instance of a class.
    /// </typeparam>
    public abstract class State<T> where T : class
    { 
        /// <summary>
        /// The instance used to run the state machine.
        /// </summary>
        protected T Runner { get; private set; }

        /// <summary>
        /// The machine associated with this state. Will be null if the state machine does not implement <see cref="IAutonomousMachine{T}"/>.
        /// </summary>
        protected IAutonomousMachine<T> Machine { get; private set; }
        
        internal void InitializeInternal(T runner, IStateMachine<T> machine)
        {
            Runner = runner;
            Machine = machine as IAutonomousMachine<T>;
            OnInitialize();
        }

        /// <summary>
        /// Called when the state machine is built.
        /// </summary>
        public virtual void OnInitialize() { }
        
        /// <summary>
        /// Called when the state machine enters this state.
        /// </summary>
        public virtual void OnEnter() { }
        
        /// <summary>
        /// Called when the state machine exits this state.
        /// </summary>
        public virtual void OnExit() { }
        
        
        /// <summary>
        /// Called every tick of the state machine.
        /// </summary>
        /// <param name="deltaTime">
        /// The time in seconds since the last frame.
        /// </param>
        public virtual void OnUpdate(float deltaTime) { }
    }
}