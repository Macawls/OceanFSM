using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace OceanFSM.CubeExample
{
    public class DisplayCurrentState : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmpText;

        public void OnStateChange(State<ICubeMachine> newState)
        {
            if (!tmpText)
            {
                return;
            }
            
            tmpText.text = $"State: {newState.GetType().Name}";
        }
    }
}


