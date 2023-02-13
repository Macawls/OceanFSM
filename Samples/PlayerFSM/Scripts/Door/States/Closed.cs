using UnityEngine;

namespace OceanFSM.PlayerExample
{
    public class Closed : State<IDoor>
    {
        public override void OnInitialize()
        {
            Debug.Log($"{nameof(Closed)} init");
        }
        
        public override void OnEnter()
        {
            Runner.Close();
        }
    }
}