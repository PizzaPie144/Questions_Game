using System.Collections;
using System.Collections.Generic;
using System;

namespace PizzaPie.Questions
{
    public class QnA
    {
        public string Question { get; private set; }

        public string RightAnswer { get; private set; }

        public IEnumerable<string> WrongAnswers { get { return wrongAnswers; } }

        private string[] wrongAnswers; 

        public QnA(Serialization.QnAData data)
        {
            Question = data.question;
            RightAnswer = data.rightAnswer;
            wrongAnswers = data.wrongAnswer;
        }
    }
}
