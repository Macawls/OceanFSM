using System;

namespace OceanFSM
{
    public abstract class StateMachineBase<T> : 
        IStateMachine<T> where T : class
    {
        protected State<T> CurrentStateInternal;
        protected State<T> PreviousStateInternal;
        
        public T Runner { get; }
        public State<T> StartingState { get; }
        
        public event Action<State<T>> OnStateChanged;
        
        public State<T> PreviousState => PreviousStateInternal;
        
        public virtual State<T> CurrentState
        {
            get => CurrentStateInternal;
            
            protected set
            {
                CurrentStateInternal?.OnExit();
                PreviousStateInternal = CurrentStateInternal;
                CurrentStateInternal = value;

                CurrentStateInternal.OnEnter();
                OnStateChanged?.Invoke(CurrentStateInternal);
            }
        }

        protected StateMachineBase(T runner, State<T> startingState)
        {
            Runner = runner;
            StartingState = startingState;
        }
        
        protected void InvokeOnStateChanged(State<T> newState)
        {
            OnStateChanged?.Invoke(newState);
        }
        
        public virtual void Start()
        {
            CurrentState = StartingState;
        }

        public virtual void Start(State<T> startingState)
        {
            CurrentState = startingState;
        }

        public virtual void Stop()
        {
            CurrentState.OnExit();
            CurrentStateInternal = null;
        }

        public virtual void Update(float deltaTime)
        {
            CurrentState?.OnUpdate(deltaTime);
        }

        public virtual void FixedUpdate(float fixeDeltaTime)
        {
            CurrentState?.OnFixedUpdate(fixeDeltaTime);
        }
    }
}