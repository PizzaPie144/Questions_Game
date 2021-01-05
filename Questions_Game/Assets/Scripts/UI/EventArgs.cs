using System;

namespace PizzaPie.UI
{
    public class DifficultySelectedEventArgs : EventArgs
    {
        public DifficultyDefinition DifficultyDefinition { get; private set; }
        public PizzaPie.Unity.Utils.CocurrentRoutineHandler CoccurentRoutine { get; private set; }

        public DifficultySelectedEventArgs(DifficultyDefinition definition, Unity.Utils.CocurrentRoutineHandler coccurentRoutine)
        {
            DifficultyDefinition = definition;
            CoccurentRoutine = coccurentRoutine;
        }
    }
}