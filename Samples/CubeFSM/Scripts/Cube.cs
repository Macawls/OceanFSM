using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace OceanFSM.CubeExample
{
    public class Cube : MonoBehaviour, ICube
    {
        [SerializeField] private UnityEvent<State<ICube>> onStateChanged;

        [Header("Components")]
        [SerializeField] private MeshRenderer meshRenderer;
        
        [Header("States")]
        [SerializeField] private FirstCubeState firstCubeState;
        [SerializeField] private SecondCubeState secondCubeState;


        [Header("Controls for changing state")] 
        [SerializeField] private KeyCode firstStateInput;
        [SerializeField] private KeyCode secondStateInput;

        private ITransitionalStateMachine<ICube> _mFsm;

        public void ChangeColor(Color newColor)
        {
            meshRenderer.material.color = newColor;
        }

        private void Awake()
        {
            _mFsm = new TransitionalStateMachineBuilder<ICube>(this)
                .SetStartingState(firstCubeState)
                .AddTransition(firstCubeState, secondCubeState, SecondStateInput, 
                    cube => Debug.Log("Transitioned to second state")) // optional callback
                .AddTransition(secondCubeState, firstCubeState, FirstStateInput)
                .Build();
        }

        private bool SecondStateInput() => Input.GetKeyDown(secondStateInput);

        private bool FirstStateInput() => Input.GetKeyDown(firstStateInput);

        private void OnSecondToFirstTransition(ICube cube)
        {
            Debug.Log($"{cube.GetType().Name} says hello :).  " +
                      $"This comes from the concrete transition added to the machine");
        }
        
        private void OnEnable()
        {
            _mFsm.Start();
        }
        
        private void OnDisable()
        {
            _mFsm.Stop();
        }
        
        private void Update()
        {
            _mFsm.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _mFsm.FixedUpdate(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            _mFsm.Evaluate();
        }
    }
}


