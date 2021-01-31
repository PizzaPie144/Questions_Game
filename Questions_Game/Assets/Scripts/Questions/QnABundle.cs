using System.Collections;
using System.Collections.Generic;
using System;

namespace PizzaPie.QuestionsGame.Questions
{
    //might make sense to remove the enumerator interface, after all it won;t get iterated 
    public class QnABundle 
    {
        private QnA[] QnAs;
        private int position = -1;

        public QuestionCategory Category { get; private set; }
        public QnA this[int index] {get { return QnAs[index]; } }
        public int Lenght { get { return QnAs.Length; } }

        private QnA Current
        {
            get
            {
                return QnAs[position];
            }
        }

        public int RemainingCount { get { return Lenght - position + 1; } }

        public QnABundle(Serialization.QnADataBundle data)
        {
            Category = data.category;
            QnAs = new QnA[data.QnADatas.Length];

            //error handling?
            for(int i = 0; i < data.QnADatas.Length; i++)
                QnAs[i] = new QnA(data.QnADatas[i]);
        }

        public void Shuffle()
        {
            var rng = new Random();
            int n = QnAs.Length;
            while (n > 1)
            {
                int m = rng.Next(n);
                n--;
                QnA temp = QnAs[n];
                QnAs[n] = QnAs[m];
                QnAs[m] = temp;
            }
        }

        public QnA GetNext()
        {
            if (++position >= Lenght)       
                position = 0;               

            var q = Current;
            MoveNext();
            return q;
        }

        private bool MoveNext()
        {
            position++;
            return position < QnAs.Length;
        }
    }

}
