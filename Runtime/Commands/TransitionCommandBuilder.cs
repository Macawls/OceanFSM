using System;

namespace OceanFSM
{
    public class TransitionCommandBuilder<T> : 
        ITransitionCommandBuilder<T> where T : class
    {
        public ITransitionCommand Command { get; }
        
        public TransitionCommandBuilder(ITransitionCommand command)
        {
            Command = command;
        }
        
        public ITransitionCommandBuilder<T> SetCondition(Func<bool> func)
        {
            Command.Predicate = func;
            return this;
        }
        
        public ITransitionCommandBuilder<T> SetTargetState(string targetStateID)
        {
            Command.TargetStateID = targetStateID;
            return this;
        }

        public ITransitionCommandBuilder<T> SetTargetState<TState>() where TState : State<T>
        {
            Command.TargetStateID = typeof(TState).Name;
            return this;
        }

        public ITransitionCommandBuilder<T> OnSuccess(Action action)
        {
            Command.OnSuccess = action;
            return this;
        }

        public ITransitionCommandBuilder<T> OnFailure(Action action)
        {
            Command.OnFailure = action;
            return this;
        }
    }
}