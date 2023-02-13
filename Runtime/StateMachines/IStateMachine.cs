namespace OceanFSM
{
    /// <summary>
    /// A marker interface for the components of a state machine.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStateMachine<T> : 
        IStateMachineContext<T>, 
        IStateMachineController where T : class
    {
        
    }
}