using System;
using UnityEngine;

namespace OceanFSM.CubeExample
{
    [Serializable]
    public class SecondCubeState : BaseCubeState
    { 
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log($"{GetType().Name} OnEnter");
        }
    }
}