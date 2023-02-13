using System.Collections;
using UnityEngine;

namespace OceanFSM.PlayerExample
{
    public class Door : MonoBehaviour, IDoor
    {
        [Header("Settings")]
        [SerializeField] private Transform doorPivot;
        [SerializeField, Range(50f, 200f)] private float openAngle = 120f;
        [SerializeField, Range(0.1f, 2f)] private float rotationDuration = 0.5f;
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        [SerializeField] private float interactDistance = 5f;
        [SerializeField] private Color gizmoColor = Color.green;
        
        private IPollingMachine<IDoor> _mMachine;
        private Coroutine _mRotationRoutine;
        
        private bool PlayerIsNear => 
            Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position) < interactDistance;
        private bool InteractKeyPressed => Input.GetKeyDown(interactKey);
        
        private void Awake()
        {
            var open = new Open();
            var closed = new Closed();
            
           /////////////////////////////////////////////////////////////////////////////////////////////////
           
           // we can do this
           // var transitions = new Transition<IDoor>[]
           // {
           //     new (open, closed, CanInteract),
           //     new (closed, open, CanInteract)
           // };
           //
           //
           // _mMachine = new PollingBuilder<IDoor>(this)
           //     .SetInitialState(nameof(Closed))
           //     .AddTransitions(transitions)
           //     .Build();


           // standalone transition, action is optional
            var openToClosed = new Transition<IDoor>(open, closed, CanInteract, 
                  () => Debug.Log("Hi mom")); 

            // we can also do this
            _mMachine = new PollingBuilder<IDoor>(this)
                .SetInitialState(nameof(Closed))
                .AddTransition(openToClosed)
                .AddTransition(from: closed, to: open, condition: CanInteract) // or inline transition
                .Build();
            
            
            /////////////////////////////////////////////////////////////////////////////////////////////////
        }

        
        private void OnEnable()
        {
            _mMachine.Start();
            _mMachine.StateChanged += OnStateChanged;
        }
        
        private void OnDisable()
        {
            _mMachine.Stop();
            _mMachine.StateChanged -= OnStateChanged;
        }
        
        private void LateUpdate()
        {
            _mMachine.Evaluate();
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, interactDistance);
        }
        
        private void OnStateChanged(State<IDoor> newState)
        {
            Debug.Log($"Entered State: {newState.GetType().Name}");
        }
        
        private bool CanInteract()
        {
            return PlayerIsNear && InteractKeyPressed;
        }
        
        public void Open()
        {
            Toggle(true);
        }

        public void Close()
        {
            Toggle(false);
        }

        private void Toggle(bool open)
        {
            if (_mRotationRoutine != null)
            {
                StopCoroutine(_mRotationRoutine);
            }

            _mRotationRoutine = StartCoroutine(RotateDoor(open ? openAngle : 0f, rotationDuration));
        }
        
        private IEnumerator RotateDoor(float angle, float duration)
        {
            var startRotation = doorPivot.localRotation;
            var endRotation = Quaternion.Euler(0f, angle, 0f);
            
            float time = 0f;
            
            while (time < duration)
            {
                time += Time.deltaTime;
                doorPivot.localRotation = Quaternion.Lerp(startRotation, endRotation, time);
                yield return null;
            }
            
            doorPivot.localRotation = endRotation;
        }
    }
}