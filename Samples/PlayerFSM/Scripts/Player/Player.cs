using System;
using UnityEngine;
using UnityEngine.Events;

namespace OceanFSM.PlayerExample
{
    public class Player : MonoBehaviour, IPlayer
    {
        [Header("Settings")]
        [SerializeField] private float jumpCooldown = 2f;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private string groundLayerName = "Ground";
        [SerializeField] private float groundCheckDistance = 1.1f;
        
        [Header("States")] 
        [SerializeField] private Idle idle;
        [SerializeField] private Run run;
        [SerializeField] private Jump jump;
        
        [Header("Components")] 
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController characterController;
        
        
        [Header("Events")]
        [SerializeField] private UnityEvent<string> stateChanged;

        
        public Animator Animator => animator;
        public CharacterController CharacterController => characterController;
        public Vector2 MovementInput => new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        public bool IsGrounded
        {
            get
            {
                var ray = new Ray(transform.position, Vector3.down);
                
                Debug.DrawRay(ray.origin, ray.direction * groundCheckDistance, 
                    color: Physics.Raycast(ray, groundCheckDistance, LayerMask.GetMask(groundLayerName)) ? 
                        Color.green : 
                        Color.red);
                
                return Physics.Raycast(ray, groundCheckDistance, LayerMask.GetMask(groundLayerName));
            }
        }
        
        private IAutonomousMachine<IPlayer> _mFsm;
        
        private float _mJumpTimer;
        
        private void Awake()
        {
            ////////////////////////////////////////////////////////////////////
            // Machine Setup
            ////////////////////////////////////////////////////////////////////
            _mFsm = new AutonomousBuilder<IPlayer>(this)
                .SetInitialState(nameof(Idle)) // or "Idle" or idle.GetType().Name
                .AddState(idle)
                .AddState(run)
                .AddState(jump)
                .Build();
            
            // can also be done like this
            // _mFsm = new AutonomousBuilder<IPlayer>(this)
            //     .SetInitialState("Idle")
            //     .AddStates(idle, run, jump)
            //     .Build();

            
            ////////////////////////////////////////////////////////////////////
            // Transitional Commands Setup
            ////////////////////////////////////////////////////////////////////
            
            _mFsm.AddCommand("Jump")
                .SetTargetState<Jump>()
                .SetCondition(() => _mFsm.CurrentState is Run && IsGrounded)
                .OnSuccess(() => Debug.Log("Hi mom")) // maybe visual fx for example
                .OnFailure(() => Debug.Log("depression")); // maybe play a negative sound effect for example
            
            //  Or 
            //  var jumpCommand = new TransitionCommand(id: "Jump"); 
            //  
            //  new TransitionCommandBuilder<IPlayer>(jumpCommand)
            //      .SetTargetState<Jump>()
            //      .SetCondition(() => )
            //      .OnSuccess(() => ) 
            //      .OnFailure(() => ); 
            //  
            // _mFsm.AddCommand(jumpCommand);
            
            ////////////////////////////////////////////////////////////////////
            
            // Modifying the command after it has been added
            // _mFsm.GetCommand("Jump").OnSuccess(() => Debug.Log("Hi dad"));
            // Retrieving data from the command
            // var jumpCondition = _mFsm.GetCommand("Jump").Command.Predicate;
            
            ////////////////////////////////////////////////////////////////////
        }

        private void OnStateChanged(State<IPlayer> state)
        {
            stateChanged?.Invoke(state.GetType().Name);
        }
        
        private void OnEnable()
        {
            _mFsm.Start();
            _mFsm.StateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            _mFsm.Stop();
            _mFsm.StateChanged -= OnStateChanged;
        }
        
        private void Update()
        {
            _mFsm.Update(Time.deltaTime);
            
            if (Input.GetKeyDown(jumpKey)) 
            {
                _mFsm.ExecuteCommand("Jump");
            }
        }
    }
}


