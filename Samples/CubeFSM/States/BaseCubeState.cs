using System;
using UnityEngine;

namespace OceanFSM.CubeExample
{
    [Serializable]
    public class BaseCubeState : State<ICube>
    {
        [SerializeField] private Color color;

        public override void OnInitialize(ICube runner)
        {
            Debug.Log($"This cube has {runner.Sides} sides");
        }

        public override void OnEnter()
        {
            Runner.ChangeColor(color);
        }
    }
}