using UnityEngine;

namespace OceanFSM.CubeExample
{
    public interface ICubeMachine : IStateMachineRunner<ICubeMachine>
    {
        void ChangeColor(Color newColor);
    }
}