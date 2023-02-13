using UnityEngine;

namespace OceanFSM.PlayerExample
{
    public interface IPlayerInput
    {
        Vector2 MovementInput { get; }
    }
}