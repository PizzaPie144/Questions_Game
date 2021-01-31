using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.QuestionsGame.Questions.Providers 
{
    public interface IQuestionsProvider
    {
        QnA GetNextQuestion(QuestionCategory category);
        CategoryDefinition GetCategoryDefinition(QuestionCategory category);
        CategoryDefinition[] GetCategoryDefinitions();
        Dictionary<QuestionCategory, int> GetQuestionsRemainingCounts();
        int GetQustionsRemaining(QuestionCategory category);
    }
}
