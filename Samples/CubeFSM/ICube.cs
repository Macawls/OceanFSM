using UnityEngine;

namespace OceanFSM.CubeExample
{
    public interface ICube
    {
        int Sides { get; }
        void ChangeColor(Color newColor);
    }
}