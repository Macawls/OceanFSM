namespace OceanFSM
{
    /// <summary>
    /// Handles commands for a state machine.
    /// </summary>
    /// <typeparam name="T">
    /// A reference type used to associate states and machines with a specific instance of a class.
    /// </typeparam>
    public interface ITransitionCommandHandler<T> 
        where T : class
    {
        /// <summary>
        /// Adds a command to the state machine.
        /// </summary>
        /// <param name="commandID">
        /// The name of the command held internally by the state machine.
        /// </param>
        /// <typeparam name="T">
        /// The generic reference type used to associate states and machines.
        /// </typeparam>
        ITransitionCommandBuilder<T> AddCommand(string commandID);
        
        /// <summary>
        /// Adds a command to the state machine.
        /// </summary>
        /// <param name="command">
        /// The command to add.
        /// </param>
        void AddCommand(ITransitionCommand command);
        
        /// <summary>
        /// Gets a command from the state machine.
        /// </summary>
        /// <param name="commandID">
        /// The ID of the command to retrieve.
        /// </param>
        /// <returns></returns>
        ITransitionCommandBuilder<T> GetCommand(string commandID);
        
        /// <summary>
        /// Removes a command from the state machine.
        /// </summary>
        /// <param name="commandID">
        /// The ID of the command to remove.
        /// </param>
        void RemoveCommand(string commandID);

        /// <summary>
        /// Attempts to execute a command.
        /// </summary>
        /// <param name="commandID">
        /// The name of the command to execute.
        /// </param>
        void ExecuteCommand(string commandID);
    }
}