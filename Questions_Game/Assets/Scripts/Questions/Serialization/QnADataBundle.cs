
namespace PizzaPie.QuestionsGame.Questions.Serialization
{
    [System.Serializable]
    public struct QnADataBundle
    {
        public QuestionCategory category;
        public QnAData[] QnADatas;
    }
}