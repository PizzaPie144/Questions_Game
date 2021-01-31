using System;
using PizzaPie.QuestionsGame.Unity.Utils;

namespace PizzaPie.QuestionsGame
{
    public class LoadEventArgs : EventArgs
    {
        public SequenceLoader SequenceLoader { get; private set; }
        public GameDifficulty Difficulty;

        public LoadEventArgs(SequenceLoader sequenceLaoder, GameDifficulty difficulty)
        {
            SequenceLoader = sequenceLaoder;
            Difficulty = difficulty;
        }
    }

    public class PlayButtonEventArgs : EventArgs
    {
    }

    public class GameStartsEventArgs : EventArgs
    {
    }

    public class GameRestartEventArgs : EventArgs { }

    public class EndGameCocurrentEventArgs : EventArgs
    {
        public CocurrentRoutineHandler CocurrentRoutine { get; private set; }
        public bool IsWin { get; private set; }
        public EndGameCocurrentEventArgs(bool isWin, CocurrentRoutineHandler cocurrentRoutine)
        {
            IsWin = isWin;
            CocurrentRoutine = cocurrentRoutine;
        }
    }

    public class LoseCoccurentEventArgs : EventArgs
    {
        public CocurrentRoutineHandler CocurrentRoutine { get; private set; }
        public int RightAnswersCount { get; private set; }

        public LoseCoccurentEventArgs(int rightAnswersCount, CocurrentRoutineHandler cocurrentRoutine)
        {
            RightAnswersCount = rightAnswersCount;
            CocurrentRoutine = cocurrentRoutine;
        }
    }

    public class WinCoccurentEventArgs : EventArgs
    {
        public CocurrentRoutineHandler CocurrentRoutine { get; private set; }
        public int RightAnswersCount { get; private set; }

        public WinCoccurentEventArgs(int rightAnswersCount, CocurrentRoutineHandler cocurrentRoutine)
        {
            RightAnswersCount = rightAnswersCount;
            CocurrentRoutine = cocurrentRoutine;
        }
    }

    public class PlayAgainEventArgs : EventArgs
    {
    }

    
}