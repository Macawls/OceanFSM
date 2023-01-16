namespace OceanFSM
{
    public abstract class State<T> where T : class, IStateMachineRunner<T>
    {
        protected T Runner { get; private set; }
        internal void SetRunner(T runner)
        {
            Runner = runner;
        }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnUpdate(float deltaTime) { }
        public virtual void OnFixedUpdate(float fixedDeltaTime) { }
    }
}