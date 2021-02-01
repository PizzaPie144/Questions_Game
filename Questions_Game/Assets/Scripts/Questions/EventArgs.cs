using System;
using PizzaPie.QuestionsGame.Unity.Utils;

namespace PizzaPie.QuestionsGame.Questions
{
    public class BeforeQuestionSelected : EventArgs
    {
        public QnA QnA { get; private set; }
        public QuestionCategory Category { get; private set; }

        public BeforeQuestionSelected(QnA QnA, QuestionCategory category)
        {
            this.QnA = QnA;
            Category = category;
        }
    }

    public class AnswerPickedCoccurentEventArgs : EventArgs
    {
        public bool IsRight { get; private set; }
        public CocurrentRoutineHandler CocurrentRoutineHandler { get; private set; }

        public AnswerPickedCoccurentEventArgs(bool isRight, CocurrentRoutineHandler cocurrentRoutineHandler)
        {
            IsRight = isRight;
            CocurrentRoutineHandler = cocurrentRoutineHandler;
        }

        
    }

    public class AnswerPickedEventArgs : EventArgs
    {
        public bool IsRight { get; private set; }
        public AnswerPickedEventArgs(bool isRight)
        {
            IsRight = isRight;
        }
    }

}