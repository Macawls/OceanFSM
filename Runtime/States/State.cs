namespace OceanFSM
{
    /// <summary>
    /// The base class for all the states in the FSM.
    /// </summary>
    /// <typeparam name="T">
    /// A reference type used to associate/bind states and transitions with the state machine.
    /// </typeparam>
    public abstract class State<T> where T : class
    { 
        /// <summary>
        /// The instance used to run the state machine.
        /// </summary>
        public T Runner { get; private set; }

        /// <summary>
        /// The machine associated with this state. Will be null if the state machine does not implement <see cref="IAutonomousStateMachine{T}"/>.
        /// </summary>
        public IAutonomousStateMachine<T> Machine { get; private set; }
        
        internal void InitializeInternal(T runner, IStateMachine<T> machine)
        {
            Runner = runner;
            Machine = machine as IAutonomousStateMachine<T>;
            OnInitialize();
        }

        public virtual void OnInitialize() { }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnUpdate(float deltaTime) { }
        public virtual void OnFixedUpdate(float fixedDeltaTime) { }
    }
}