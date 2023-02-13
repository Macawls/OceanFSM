namespace OceanFSM
{
    /// <summary>
    /// Controls the state machine.
    /// </summary>
    public interface IStateMachineController
    {
        /// <summary>
        /// Enters the initial state.
        /// </summary>
        void Start();
        
        /// <summary>
        /// Exits the current state.
        /// </summary>
        void Stop();
        
        /// <summary>
        /// Updates the state machine. If your states do not have any update logic, this does not need to be called.
        /// </summary>
        void Update(float deltaTime);
    }
}