using System;
using UnityEngine;

namespace OceanFSM.PlayerExample
{
    [Serializable]
    public class Jump : PlayerStateBase
    {
        [SerializeField] private float jumpSpeed = 10f;
        [SerializeField] private float gravity = -9.8f;
        
        private float _mTimeElapsed;
        private Vector3 _mVelocity;
        
        
        public override void OnEnter()
        {
            base.OnEnter();
            _mTimeElapsed = 0f;
            
            _mVelocity = Controller.velocity;
            _mVelocity.y = jumpSpeed;
        }

        public override void OnUpdate(float deltaTime)
        {
            _mTimeElapsed += deltaTime;
            _mVelocity.y += gravity * deltaTime;
            
            Controller.Move(_mVelocity * deltaTime);
            if (!(_mTimeElapsed > 0.3f)) return;
            
            if (Runner.IsGrounded)
            {
                Machine.ChangeState<Idle>();
            }
        }
        
        public override void OnExit()
        {
            base.OnExit();
            _mVelocity = Vector3.zero;
        }
    }
}