using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PizzaPie.QuestionsGame.Wheel
{
    public class WheelStopSpinEventArgs : EventArgs
    {
        public Questions.QuestionCategory QuestionCategory { get; private set; }
        public Unity.Utils.CocurrentRoutineHandler CocurrentRoutine { get; private set; }
        
        public WheelStopSpinEventArgs(Questions.QuestionCategory questionCategory, Unity.Utils.CocurrentRoutineHandler cocurrentRoutine)
        {
            QuestionCategory = questionCategory;
            CocurrentRoutine = cocurrentRoutine;
        }
    }

    public class StopWheelButtonEventArgs : EventArgs
    {

    }
}
