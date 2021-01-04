using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.Questions.Loaders
{
    public class RemoteQuestionLoader : IQuestionsLoader 
    {
        public void RequestQuestions(Action<QnABundle[]> OnQuestionLoaded, Action<string> OnQuestionsFailedToLoad)
        {
            throw new NotImplementedException();
        }
    }
}