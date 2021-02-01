using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.QuestionsGame.States
{
    public interface IState
    {
        StateType GetStateType { get; }
        void Init(StateMachine stateMachine);
        void Enter();
        void Exit();
        void _Reset();           
        void Interupt();        
    }
}