using System;

namespace OceanFSM
{
    public class TransitionCommand : ITransitionCommand
    {
        public string ID { get; }

        public TransitionCommand(string id)
        {
            ID = id;
        }
        
        string ITransitionCommand.TargetStateID { get; set; }
        Action ITransitionCommand.OnFailure { get; set; }
        Action ITransitionCommand.OnSuccess { get; set; }
        Func<bool> ITransitionCommand.Predicate { get; set; }
    }
}