﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PizzaPie.QuestionsGame.Questions.Loaders
{
    public interface IQuestionsLoader 
    {
        void RequestQuestions(Action<QnABundle[]> OnQuestionLoaded, Action<string> OnQuestionsFailedToLoad);
    }

}

