using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PizzaPie.QuestionsGame.States
{
    public class StateChangedEventArgs : EventArgs
    {
        public StateType StateType { get; private set; }
        public StateChangedEventArgs(StateType stateType)
        {
            StateType = stateType;
        }
    }
}