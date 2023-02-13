using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OceanFSM
{
    public class PollingMachine<T> : StateMachineBase<T>, 
        IPollingMachine<T> where T : class
    {
        private List<Transition<T>> _mCurrentTransitions = new();
        private readonly Dictionary<State<T>, List<Transition<T>>> _mTransitions;
        
        public IReadOnlyDictionary<State<T>, List<Transition<T>>> Transitions => _mTransitions;
        
        public override State<T> CurrentState
        {
            get => CurrentStateInternal;

            protected set
            {
                CurrentStateInternal?.OnExit();
                PreviousStateInternal = CurrentStateInternal;
                CurrentStateInternal = value;
                _mCurrentTransitions = _mTransitions[CurrentStateInternal];
                
                CurrentStateInternal.OnEnter();
                InvokeOnStateChanged(CurrentStateInternal);
            }
        }
        
        public PollingMachine(T runner, string initialStateID, Dictionary<State<T>, List<Transition<T>>> transitions) : 
            base(runner, initialStateID)
        {
            InitialStateID = initialStateID;
            _mTransitions = transitions;
            
            foreach (var state in _mTransitions.Keys)
            {
                state.InitializeInternal(runner, this);
            }
        }
        
        public override void Start()
        {
            var initialState = _mTransitions.Keys.FirstOrDefault(e => e.GetType().Name == InitialStateID);
            
            if (initialState == null)
            {
                Debug.LogError($"No state with the name:[{InitialStateID}] found.");
                return;
            }
            
            CurrentState = initialState;
        }
        
        public void Evaluate()
        {
            _mCurrentTransitions.ForEach(OnTransitionEvaluate);
        }
        
        private void OnTransitionEvaluate(Transition<T> transition)
        {
            if (!transition.Condition())
            {
                return;
            }
            
            CurrentState = transition.To;
            transition.OnTransition?.Invoke();
        }
    }
}