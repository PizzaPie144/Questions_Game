using System;

namespace PizzaPie.QuestionsGame.UI
{
    public class DifficultySelectedEventArgs : EventArgs
    {
        public DifficultyDefinition DifficultyDefinition { get; private set; }
        public PizzaPie.QuestionsGame.Unity.Utils.CocurrentRoutineHandler CoccurentRoutine { get; private set; }

        public DifficultySelectedEventArgs(DifficultyDefinition definition, Unity.Utils.CocurrentRoutineHandler coccurentRoutine)
        {
            DifficultyDefinition = definition;
            CoccurentRoutine = coccurentRoutine;
        }
    }
}