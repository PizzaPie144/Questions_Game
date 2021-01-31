

namespace PizzaPie.QuestionsGame.Questions.Serialization
{
    public interface IJsonAdapter
    {
        string Serialize(object obj);

        object Deserialize(string json, System.Type type);

        T Deserialize<T>(string json);
    }
}
