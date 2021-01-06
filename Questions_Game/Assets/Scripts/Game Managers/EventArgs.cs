using System;


namespace PizzaPie
{
    public class LoadEventArgs : EventArgs
    {
        public PizzaPie.Unity.Utils.SequenceLoader SequenceLoader { get; private set; }
        public GameDifficulty Difficulty;

        public LoadEventArgs(Unity.Utils.SequenceLoader sequenceLaoder, GameDifficulty difficulty)
        {
            SequenceLoader = sequenceLaoder;
            Difficulty = difficulty;
        }
    }

    public class PlayButtonEventArgs : EventArgs
    {
        public PlayButtonEventArgs()
        {
        }
    }

    public class GameStartsEventArgs : EventArgs
    {

    }

    public class GameRestartEventArgs : EventArgs { }

    public class EndGameCocurrentEventArgs : EventArgs
    {
        public Unity.Utils.CocurrentRoutineHandler CocurrentRoutine { get; private set; }
        public bool IsWin { get; private set; }
        public EndGameCocurrentEventArgs(bool isWin, Unity.Utils.CocurrentRoutineHandler cocurrentRoutine)
        {
            IsWin = isWin;
            CocurrentRoutine = cocurrentRoutine;
        }
    }

    public class LoseCoccurentEventArgs : EventArgs
    {
        public Unity.Utils.CocurrentRoutineHandler CocurrentRoutine { get; private set; }
        public int RightAnswersCount { get; private set; }

        public LoseCoccurentEventArgs(int rightAnswersCount, Unity.Utils.CocurrentRoutineHandler cocurrentRoutine)
        {
            RightAnswersCount = rightAnswersCount;
            CocurrentRoutine = cocurrentRoutine;
        }
    }

    public class WinCoccurentEventArgs : EventArgs
    {
        public Unity.Utils.CocurrentRoutineHandler CocurrentRoutine { get; private set; }
        public int RightAnswersCount { get; private set; }

        public WinCoccurentEventArgs(int rightAnswersCount, Unity.Utils.CocurrentRoutineHandler cocurrentRoutine)
        {
            RightAnswersCount = rightAnswersCount;
            CocurrentRoutine = cocurrentRoutine;
        }
    }

    public class PlayAgainEventArgs : EventArgs
    {

    }

    public class ResetEventArgs : EventArgs
    {

    }
}