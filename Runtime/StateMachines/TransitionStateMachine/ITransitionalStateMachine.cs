namespace OceanFSM
{
    public interface ITransitionalStateMachine<T> : 
        IStateMachine<T> where T : class
    {
        void Evaluate();
    }
}