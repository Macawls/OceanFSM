using System;
using UnityEngine;

namespace OceanFSM.CubeExample
{
    [Serializable]
    public class FirstCubeState : BaseCubeState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log($"{GetType().Name} OnEnter");
        }
    }
}