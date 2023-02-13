using System;

namespace OceanFSM
{
    /// <summary>
    /// Aid in the construction of a <see cref="ITransitionCommand"/>.
    /// </summary>
    /// <typeparam name="T">
    /// A reference type used to associate states and machines with a specific instance of a class.
    /// </typeparam>
    public interface ITransitionCommandBuilder<T> where T  : class
    {
        ITransitionCommand Command { get; }
        
        /// <summary>
        /// Sets the condition for this command to be executed.
        /// </summary>
        /// <param name="func">
        /// The condition to be evaluated.
        /// </param>
        ITransitionCommandBuilder<T> SetCondition(Func<bool> func);
        
        /// <summary>
        /// The state to transition to when this command is executed.
        /// </summary>
        /// <param name="targetStateID">
        /// The ID of the state to transition to.
        /// </param>
        ITransitionCommandBuilder<T> SetTargetState(string targetStateID);
        
        /// <inheritdoc cref="SetTargetState"/>
        /// <typeparam name="TState">
        /// The type of the state to transition to.
        /// </typeparam>
        ITransitionCommandBuilder<T> SetTargetState<TState>() where TState : State<T>;
        
        /// <summary>
        /// The action to be executed when the transition is successful.
        /// </summary>
        ITransitionCommandBuilder<T> OnSuccess(Action action);
        
        
        /// <summary>
        /// The action to be executed when this command fails to transition to a new state.
        /// </summary>
        ITransitionCommandBuilder<T> OnFailure(Action action);
    }
}