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
        protected T Runner { get; private set; }
        internal void SetRunner(T runner)
        {
            Runner = runner;
        }
        public virtual void OnInitialize(T runner) { }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnUpdate(float deltaTime) { }
        public virtual void OnFixedUpdate(float fixedDeltaTime) { }
    }
}