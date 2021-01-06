using PizzaPie.Events;
using PizzaPie.QuestionsGame.Sound;

namespace PizzaPie
{
    public class Services : Unity.Utils.Singleton<Services>
    {
        public IEventAggregatorService EventAggregator { get; private set; }
        public Questions.Providers.IQuestionsProvider QuestionsProvider { get; private set; }
        public ISoundService soundService { get; private set; }

        private void Awake()
        {
            EventAggregator = new EventAggregator();

            //local loaders, no fallbacks
            QuestionsProvider = new Questions.Providers.QuestionsProvider(
                new Questions.Loaders.LocalQuestionLoader(new Questions.Serialization.UnityJsonAdapter()),
                new Questions.Loaders.LocalCategoryDefinitionLoader());

            soundService = new NormalSoundService();
        }

    }
}
