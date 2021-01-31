using PizzaPie.QuestionsGame.Sound;
using PizzaPie.QuestionsGame.Events;

namespace PizzaPie.QuestionsGame
{
    public class Services : Unity.Utils.Singleton<Services>
    {
        public IEventAggregatorService EventAggregator { get; private set; }
        public Questions.Providers.IQuestionsProvider QuestionsProvider { get; private set; }
        public ISoundService SoundService { get; private set; }

        private void Awake()
        {
            EventAggregator = new EventAggregator();

            //local loaders, no fallbacks
            QuestionsProvider = new Questions.Providers.QuestionsProvider(
                new Questions.Loaders.LocalQuestionLoader(new Questions.Serialization.UnityJsonAdapter()),
                new Questions.Loaders.LocalCategoryDefinitionLoader());

            SoundService = new NormalSoundService();
        }

    }
}
