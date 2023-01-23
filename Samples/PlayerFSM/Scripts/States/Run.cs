using System;
using UnityEngine;

namespace OceanFSM.PlayerExample
{
    [Serializable]
    public class Run : PlayerStateBase
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float rotateSpeed = 1f;

        private Transform _mTransform;
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            _mTransform = Runner.CharacterController.transform;
        }
        
        public override void OnUpdate(float deltaTime)
        {
            _mTransform.Rotate(0, Runner.MovementInput.x * rotateSpeed, 0);
            
            var forward = _mTransform.TransformDirection(Vector3.forward);
            float curSpeed = speed * Runner.MovementInput.y;
            
            Runner.CharacterController.SimpleMove(forward * curSpeed);
            
            if (Runner.MovementInput is {x: 0, y: 0})
            {
                Machine.ChangeState<Idle>(() =>
                {
                    Debug.Log("Transitioned to Idle State");
                });
            }
        }
    }
}