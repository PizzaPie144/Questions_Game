using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie
{
    [CreateAssetMenu(menuName = "Difficulty Defintion")]
    public class DifficultyDefinition : ScriptableObject ,System.IComparable<DifficultyDefinition>
    {
        [SerializeField]
        private string displayName;
        [SerializeField]
        private int order;
        [SerializeField]
        private GameDifficulty difficulty;
        [SerializeField]
        private int maxWrongAnswers;
        [SerializeField]
        private int maxRightAnswers;

        public string DisplayName { get { return displayName; } }
        public int Order { get { return order; } }
        public GameDifficulty GameDifficulty { get { return difficulty; } }
        public int MaxWrongAnswers { get { return maxWrongAnswers; } }
        public int MaxRightAnswers { get { return maxRightAnswers; } }

        

        public bool IsLose(int wrongAnswersCount)
        {
            return wrongAnswersCount > maxWrongAnswers;
        }

        public bool IsWin(int rightAnswersCount)
        {
            return rightAnswersCount > maxRightAnswers;
        }

        public int CompareTo(DifficultyDefinition other)
        {
            if (order > other.order)
                return 1;
            else if (order == other.order)
                return 0;
            else
                return -1;
        }
    }
}
