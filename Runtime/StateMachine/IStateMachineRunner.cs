namespace OceanFSM
{
    public interface IStateMachineRunner<out T> where T : class
    { 
        T Runner { get; }
    }
}