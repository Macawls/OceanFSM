using UnityEngine;

namespace OceanFSM.PlayerExample
{
    public interface IPlayer : IPlayerInput
    {
        Animator Animator { get; }
        CharacterController CharacterController { get; }
        bool IsGrounded { get; }
    }
}