using System;

namespace OceanFSM
{
    public abstract class StateMachineBase<T> : 
        IStateMachine<T> where T : class
    {
        protected State<T> CurrentStateInternal;
        protected State<T> PreviousStateInternal;
        
        public T Runner { get; }
        public string InitialStateID { get; protected set; }

        public event Action<State<T>> StateChanged;
        
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
                StateChanged?.Invoke(CurrentStateInternal);
            }
        }

        protected StateMachineBase(T runner, string initialStateID)
        {
            InitialStateID = initialStateID;
            Runner = runner;
        }
        
        protected void InvokeOnStateChanged(State<T> newState)
        {
            StateChanged?.Invoke(newState);
        }
        
        public abstract void Start();
        
        public virtual void Stop()
        {
            CurrentState?.OnExit();
            CurrentStateInternal = null;
        }

        public virtual void Update(float deltaTime)
        {
            CurrentState?.OnUpdate(deltaTime);
        }
        
        protected static bool MatchesID(State<T> state, string stateID)
        {
            return state.GetType().Name == stateID;
        }
        
        protected void HandleCommandExecution(ITransitionCommand command, State<T> targetState)
        {
            if (command.Predicate == null)
            {
                CurrentState = targetState;
                command.OnSuccess?.Invoke();
                return;
            }

            if (!command.Predicate())
            {
                command.OnFailure?.Invoke();
                return;
            }
            
            CurrentState = targetState;
            command.OnSuccess?.Invoke();
        }
        
    }
}