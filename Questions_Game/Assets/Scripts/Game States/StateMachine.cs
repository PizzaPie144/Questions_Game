using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.QuestionsGame.States {
    public class StateMachine : MonoBehaviour
    {
        private IState currentState;
        private Dictionary<StateType, IState> states;
        public Dictionary<Type, object> Blackboard { get; private set; }

        [SerializeField]
        private List<MonoBehaviour> monoStates;             //use only for assigning the pre Instatiated IState Monobehaviours

        private void Start()
        {
            Init();
        }

        void Init()
        {
            Blackboard = new Dictionary<Type, object>();
            states = new Dictionary<StateType, IState>();

            foreach(var mono in monoStates)
            {
                if (!typeof(IState).IsAssignableFrom(mono.GetType()))
                {
                    Debug.LogFormat("{0} is Non IState MonoBehaviour and have been assigned to monoStates list", mono.GetType().ToString());
                    continue;
                }
                var state = mono as IState;
                states.Add(state.GetStateType, state);
                state.Init(this);
            }

            ChangeState(StateType.GAME_START);
        }

        public void ChangeState(StateType state)
        {
            if (currentState != null)
            {
                if (IsStateOfType(currentState, state))
                {
                    Debug.LogFormat("State of type {0} is trying to itself", state.ToString());
                    throw new Exception("Trying to transition to same state!");
                }

                currentState.Exit();
            }
            Services.Instance.EventAggregator.Invoke(this, new StateChangedEventArgs(state));
            currentState = states[state];
            currentState.Enter();
        }

        public T GetBlackBoardValue<T>() where T : class
        {
            if (Blackboard.ContainsKey(typeof(T)))
                return Blackboard[typeof(T)] as T;

            Debug.LogFormat("There is no set value for type {0}", typeof(T).Name);
            return null;                        //could throw an exception instead
        }

        public void SetBlackboardValue<T>(T value) where T : class
        {
            if (Blackboard.ContainsKey(typeof(T)))
                Blackboard[typeof(T)] = value;
            else
                Blackboard.Add(typeof(T), value);
        }

        bool IsStateOfType(IState state, StateType stateType)
        {
            return state.GetStateType == stateType; 
        }


    }
}