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

    public class AnswerPickedSequence : EventArgs
    {
        public bool IsRight { get; private set; }
        public PizzaPie.Unity.Utils.SequenceLoader SequenceLoader { get; private set; }

        public AnswerPickedSequence(bool isRight, Unity.Utils.SequenceLoader sequence)
        {
            IsRight = isRight;
            SequenceLoader = sequence;
        }
    }

}