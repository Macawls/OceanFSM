namespace OceanFSM
{
    public interface ICommand
    {
        /// <summary>
        /// The unique identifier for this command.
        /// </summary>
        string ID { get; }
    }
}