using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace OceanFSM
{
    public class AutonomousMachine<T> : StateMachineBase<T>, 
        IAutonomousMachine<T> where T : class
    {
        private readonly Dictionary<Type, State<T>> _mStateMap = new();
        private readonly Dictionary<string, ITransitionCommand> _mCommands = new();
        
        public AutonomousMachine(T runner, string initialStateID, IEnumerable<State<T>> states) : 
            base(runner, initialStateID)
        {
            foreach (var state in states)
            {
                AddState(state);
                state.InitializeInternal(runner, this);
            }
        }
        
        private void AddState(State<T> state)
        {
            var type = state.GetType();
            _mStateMap.Add(type, state);
        }
        
        public TState ChangeState<TState>(Action onTransition = null) where TState : State<T>
        {
            var type = typeof(TState);
            
            if (!_mStateMap.ContainsKey(type))
            {
                Debug.LogWarning($"State with type: [{type.Name}] does not exist in the machine, cannot change to it.");
                return null;
            }
            
            CurrentState = _mStateMap[type];
            onTransition?.Invoke();
            return CurrentState as TState;
        }

        public State<T> ChangeState(string stateID, Action onTransition = null)
        {
            var type = _mStateMap.Values.FirstOrDefault(state => MatchesID(state, stateID))?.GetType();
            
            if (type == null)
            {
                Debug.LogWarning($"State: [{stateID}] does not exist in the machine, cannot change to it.");
                return null;
            }
            
            if (!_mStateMap.ContainsKey(type))
            {
                Debug.LogWarning($"State: [{type.Name}] does not exist in the machine, cannot change to it.");
                return null;
            }
            
            CurrentState = _mStateMap[type];
            onTransition?.Invoke();
            return CurrentState;
        }
        
        public ITransitionCommandBuilder<T> AddCommand(string commandID)
        {
            var command = new TransitionCommand(commandID);
            
            if (_mCommands.ContainsKey(commandID))
            {
                Debug.Log($"Command with ID: [{commandID}] already exists in the machine, will be replaced.");
                return new TransitionCommandBuilder<T>(_mCommands[commandID] = command);
            }
            
            _mCommands.Add(commandID, command);
            return new TransitionCommandBuilder<T>(command);
        }

        public void AddCommand(ITransitionCommand command)
        {
            if (_mCommands.ContainsKey(command.ID))
            {
                Debug.Log($"Command with ID: [{command.ID}] already exists in the machine, will be replaced.");
                _mCommands[command.ID] = command;
                return;
            }
            
            _mCommands.Add(command.ID, command);
        }

        public ITransitionCommandBuilder<T> GetCommand(string commandID)
        {
            if (_mCommands.ContainsKey(commandID))
            {
                return new TransitionCommandBuilder<T>(_mCommands[commandID]);
            }
            
            Debug.LogWarning($"Command with ID: [{commandID}] does not exist in the machine, cannot get it.");
            return null;
        }
        

        public void RemoveCommand(string commandID)
        {
            if (_mCommands.ContainsKey(commandID))
            {
                _mCommands.Remove(commandID);
                return;
            }
            
            Debug.LogWarning($"Command with ID: [{commandID}] does not exist in the machine, cannot remove it.");
        }

        public void ExecuteCommand(string commandID)
        {
            var command = _mCommands[commandID];
            var targetState = _mStateMap.Values.FirstOrDefault(state => MatchesID(state, command.TargetStateID));

            if (targetState == null)
            {
                Debug.LogWarning($"State with name: [{command.TargetStateID}] does not exist in the machine, cannot execute command.");
                return;
            }

            HandleCommandExecution(command, targetState);
        }
        
        public override void Start()
        {
            var initialState = _mStateMap.Values.FirstOrDefault(state => MatchesID(state, InitialStateID));
            
            if (initialState == null)
            {
                Debug.LogWarning($"State with name: [{InitialStateID}] does not exist in the machine, cannot start.");
                return;
            }
            
            CurrentState = initialState;
        }
    }
}