using System;
using UnityEngine;

namespace OceanFSM.CubeExample
{
    [Serializable]
    public class BaseCubeState : State<ICubeMachine>
    {
        [SerializeField] private Color color;
        
        public override void OnEnter()
        {
            Runner.ChangeColor(color);
        }
    }
}