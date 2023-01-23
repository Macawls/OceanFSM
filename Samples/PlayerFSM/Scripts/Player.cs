using UnityEngine;

namespace OceanFSM.PlayerExample
{
    public class Player : MonoBehaviour, IPlayer
    {
        [Header("Components")] 
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController characterController;

        [Header("States")] 
        [SerializeField] private Idle idle;
        [SerializeField] private Run run;
        
        public Animator Animator => animator;
        public CharacterController CharacterController => characterController;
        public Vector2 MovementInput => new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        private IAutonomousStateMachine<IPlayer> _mFsm;
        
        private void Awake()
        {
            _mFsm = new AutonomousStateMachineBuilder<IPlayer>(this)
                .SetStartingState(idle)
                .AddState(run)
                .Build();
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
    }
}


