using System;

namespace OceanFSM
{
    /// <summary>
    /// A command that transitions to a new state.
    /// </summary>
    public interface ITransitionCommand : ICommand
    {
        /// <summary>
        /// The ID of the state to transition to.
        /// </summary>
        string TargetStateID { get; internal set; }
        
        /// <summary>
        /// Action to be executed when this command fails to transition to a new state.
        /// </summary>
        Action OnFailure { get; internal set; }
        
        /// <summary>
        /// Action to be executed when the transition is successful.
        /// </summary>
        Action OnSuccess { get; internal set; }
        
        /// <summary>
        /// The condition to be evaluated before this command is executed.
        /// </summary>
        Func<bool> Predicate { get; internal set; }
    }
}