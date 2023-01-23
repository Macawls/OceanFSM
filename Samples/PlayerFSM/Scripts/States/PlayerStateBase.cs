using System;
using UnityEngine;

namespace OceanFSM.PlayerExample
{
    [Serializable]
    public abstract class PlayerStateBase : State<IPlayer>
    {
        [SerializeField] private string animationStateName;
        [SerializeField] [Range(0f, 1f)] private float transitionDuration = 0.15f;
        
        private int _mAnimationStateHash;

        public override void OnInitialize()
        {
            _mAnimationStateHash = Animator.StringToHash(animationStateName);
        }
        
        public override void OnEnter()
        {
            Runner.Animator.CrossFade(_mAnimationStateHash, transitionDuration, 0);
        }
    }
}