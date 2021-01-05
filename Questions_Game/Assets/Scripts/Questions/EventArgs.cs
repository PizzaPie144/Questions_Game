using System;

namespace PizzaPie.Questions
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

    public class AfterQuestionSelected : EventArgs
    {

    }

    public class BeforeAnswerPickedEventArgs : EventArgs
    {
        public bool IsRight { get; private set; }
        //public PizzaPie.Unity.Utils.SequenceLoader SequenceLoader { get; private set; }

        public BeforeAnswerPickedEventArgs(bool isRight)
        {
            IsRight = isRight;
        }
    }

    public class AfterAnswerPickedEventArgs : EventArgs
    {
        public bool IsRight { get; private set; }

        public AfterAnswerPickedEventArgs(bool isRigth)
        {
            IsRight = isRigth;
        }
    }

    public class AnswerPickedCoccurentEventArgs : EventArgs
    {
        public bool IsRight { get; private set; }
        public PizzaPie.Unity.Utils.CocurrentRoutineHandler CocurrentRoutineHandler { get; private set; }

        public AnswerPickedCoccurentEventArgs(bool isRight, Unity.Utils.CocurrentRoutineHandler cocurrentRoutineHandler)
        {
            IsRight = isRight;
            CocurrentRoutineHandler = cocurrentRoutineHandler;
        }
    }

}