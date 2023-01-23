using System;
using UnityEngine;

namespace OceanFSM.PlayerExample
{
    [Serializable]
    public class Idle : PlayerStateBase
    {
        public override void OnUpdate(float deltaTime)
        {
            if (Runner.MovementInput == Vector2.zero) return;
            
            if (Runner.MovementInput.y > 0.1f)
            {
                // or Machine.ChangeState("Run");
                Machine.ChangeState<Run>(() => 
                {
                    Debug.Log("Transitioned to Run State");
                }); 
            }
        }
    }
}