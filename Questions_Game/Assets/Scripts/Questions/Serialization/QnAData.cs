
namespace PizzaPie.Questions.Serialization
{
    [System.Serializable]
    public struct QnAData
    {
        public string question;
        public string rightAnswer;
        public string[] wrongAnswer;

        public static QnAData GetEmpty()
        {
            var data = new QnAData();
            
            data.question = "";
            data.rightAnswer = "";
            data.wrongAnswer = new string[Settings.ANSWERS_COUNT - 1];
            
            for (int i = 0; i < data.wrongAnswer.Length; i++)
                data.wrongAnswer[i] = "";

            return data;
        }
    }
}