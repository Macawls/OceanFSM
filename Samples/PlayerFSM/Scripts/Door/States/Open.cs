using UnityEngine;

namespace OceanFSM.PlayerExample
{
    public class Open : State<IDoor>
    {
        public override void OnInitialize()
        {
            Debug.Log($"{nameof(Open)} init");
        }
        
        public override void OnEnter()
        {
            Runner.Open();
        }
    }
}