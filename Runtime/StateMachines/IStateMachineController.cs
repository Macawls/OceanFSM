namespace OceanFSM
{
    public interface IStateMachineController<T> where T : class
    {
        void Start();
        void Start(State<T> startingState);
        void Stop();
        void Update(float deltaTime);
        void FixedUpdate(float fixeDeltaTime);
    }
}