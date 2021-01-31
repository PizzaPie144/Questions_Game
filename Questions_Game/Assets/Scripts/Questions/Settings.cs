using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.QuestionsGame.Questions
{
    public static class Settings
    {
        public const int ANSWERS_COUNT = 3;
        public const int loadMaxAttempts = 5;
        public const int fallbackLoadMaxAttempts = 1;
        public const float attemptDelayRemote = 2f;
    }
}