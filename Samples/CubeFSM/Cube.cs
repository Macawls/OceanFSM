using System;
using UnityEngine;
using UnityEngine.Events;

namespace OceanFSM.CubeExample
{
    public class Cube : MonoBehaviour, ICubeMachine
    {
        [SerializeField] private UnityEvent<State<ICubeMachine>> onStateChanged;

        [Header("Components")]
        [SerializeField] private MeshRenderer meshRenderer;
        
        [Header("States")]
        [SerializeField] private FirstCubeState firstCubeState;
        [SerializeField] private SecondCubeState secondCubeState;


        [Header("Keycodes for changing state")] 
        [SerializeField] private KeyCode firstStateKey = KeyCode.UpArrow;
        [SerializeField] private KeyCode secondStateKey = KeyCode.DownArrow;
        
        public ICubeMachine Runner => this;
        private StateMachine<ICubeMachine> _mFsm;
        
        public void ChangeColor(Color newColor)
        {
            meshRenderer.material.color = newColor;
        }

        private void Awake()
        {
            var secondToFirst = new StateTransition<ICubeMachine>(secondCubeState, firstCubeState,
                condition: () => Input.GetKeyDown(firstStateKey), 
                OnSecondToFirstTransition);

            _mFsm = new StateMachineBuilder<ICubeMachine>(this)
                .SetStartingState(firstCubeState)
                .AddTransition(firstCubeState, secondCubeState, 
                    condition: () => Input.GetKeyDown(secondStateKey))
                .AddTransition(secondToFirst)
                .Build();


            _mFsm.OnStateChanged += state => onStateChanged?.Invoke(state);
        }

        private void OnSecondToFirstTransition(ICubeMachine cubeMachine)
        {
            Debug.Log($"{cubeMachine.GetType().Name} says hello :).  " +
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


