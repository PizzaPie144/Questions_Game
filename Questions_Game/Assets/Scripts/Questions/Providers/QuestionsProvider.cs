using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


using PizzaPie.Questions.Loaders;

namespace PizzaPie.Questions.Providers
{
    public class QuestionsProvider : IQuestionsProvider, Events.ISubscriber<BeforeLoadEventArgs>
    {
        private IQuestionsLoader questionsLoader;
        private ICategoryDefinitionsLoader categoriesLoader;

        private IQuestionsLoader fallbackQuestionsLoader;
        private ICategoryDefinitionsLoader fallbackCategoriesLoader;

        private Dictionary<QuestionCategory, QnABundle> questions;
        private Dictionary<QuestionCategory, CategoryDefinition> definitions;

        private Unity.Utils.CouroutinesHandler coroutineHandler;

        private int QnALoadAttemps;
        private int QnAFallbackLoadAttempts;
        private int definitionsLoadAttempts;
        private int fallbackDefinitionsLoadAttempts;

        private bool QnAIsLoaded;
        private bool categoriesIsLoaded;

        public bool IsInit { get; private set; }

        #region ctors
        public QuestionsProvider(IQuestionsLoader questionsLoader, ICategoryDefinitionsLoader categoriesLoader, IQuestionsLoader fallbackQuestionsLoader = null, ICategoryDefinitionsLoader fallbackCategoriesLoader = null)
        {
            this.questionsLoader = questionsLoader;
            this.categoriesLoader = categoriesLoader;

            this.fallbackQuestionsLoader = fallbackQuestionsLoader;
            this.fallbackCategoriesLoader = fallbackCategoriesLoader;

            Services.Instance.EventAggregator.Subscribe<BeforeLoadEventArgs>(this);     //dispose or destructor to unsub?
        }

        #endregion

        #region IQuestionsProvider
        //public void Init()
        //{
        //    coroutineHandler = Unity.Utils.CouroutinesHandler.Init();
        //    coroutineHandler.StartCoroutine(InitSequence());
        //}

        public QnA GetNextQuestion(QuestionCategory category)
        {
            return questions[category].GetNext();
        }

        public CategoryDefinition GetCategoryDefinition(QuestionCategory category)
        {
            return definitions[category];
        }

        public CategoryDefinition[] GetCategoryDefinitions()
        {
            List<CategoryDefinition> defs = new List<CategoryDefinition>();
            foreach (var d in definitions.Values)
                defs.Add(d);

            return defs.ToArray();
        }

        public Dictionary<QuestionCategory, int> GetQuestionsRemainingCounts()
        {
            var dictionary = new Dictionary<QuestionCategory, int>();
            foreach (var kvp in questions)
                dictionary.Add(kvp.Key, kvp.Value.RemainingCount);

            return dictionary;
        }

        public int GetQustionsRemaining(QuestionCategory category)
        {
            return questions[category].RemainingCount;
        }



        #endregion

        #region Data Load
        private IEnumerator InitSequence()
        {
            if (coroutineHandler == null)
                coroutineHandler = Unity.Utils.CouroutinesHandler.Init();

            questionsLoader.RequestQuestions(OnSuccessQnALoad, OnFailQnALoad);
            while (!QnAIsLoaded)
                yield return null;

            categoriesLoader.RequestCategoryDefinitions(OnSuccessDefinitionsLoad, OnFailDefinitionsLoad);
            while (!categoriesIsLoaded)
                yield return null;

            IsInit = true;
            coroutineHandler.Dispose();
        }

        private void OnSuccessQnALoad(QnABundle[] bundles)
        {
            questions = new Dictionary<QuestionCategory, QnABundle>();

            foreach (var bundle in bundles)
            {
                if (questions.ContainsKey(bundle.Category))
                {
                    Debug.LogFormat("Multiple files with same category are not supported, a bundle of {0} did not get loaded", bundle.Category.ToString());
                    continue;
                }
                questions.Add(bundle.Category, bundle);
            }

            QnAIsLoaded = true;
        }

        private void OnFailQnALoad(string error)
        {
            if (QnALoadAttemps < Settings.loadMaxAttempts)
            {
                Debug.LogFormat("QnA bundles failed to load with error {0}, attempting to load again in {1}", error, Settings.attemptDelayRemote.ToString());

                coroutineHandler.StartCoroutine(DelayedAttempt(Settings.attemptDelayRemote,
                    () => questionsLoader.RequestQuestions(OnSuccessQnALoad, OnFailQnALoad)));
                QnALoadAttemps++;

            }
            else if (fallbackQuestionsLoader != null && QnAFallbackLoadAttempts < Settings.fallbackLoadMaxAttempts)
            {
                Debug.LogFormat("QnA bundles failed to load with error {0}, attempting to load using fallback loader.", error);
                QnAFallbackLoadAttempts++;
                fallbackQuestionsLoader.RequestQuestions(OnSuccessQnALoad, OnFailDefinitionsLoad);
            }
            else
            {
                Debug.Log("Failed to load QnA Bundles, reset sequense initiated");
                //init delayed reset sequense (invoke via event)
                //probably re load the app make sure to kill singletons
                //kill the coroutines
                //kill load attempts
            }
        }

        private IEnumerator DelayedAttempt(float delay, Action loadAction)
        {
            yield return new WaitForSeconds(delay);
            loadAction();
        }

        private void OnSuccessDefinitionsLoad(CategoryDefinition[] categoryDefinitions)
        {
            definitions = new Dictionary<QuestionCategory, CategoryDefinition>();

            foreach (var def in categoryDefinitions)
                if (!definitions.ContainsKey(def.Category))
                    definitions.Add(def.Category, def);
                else
                    Debug.LogFormat("Multiple definitions for the same category are not supported, definition of category {0} did not loaded.", def.Category);

            categoriesIsLoaded = true;
        }

        private void OnFailDefinitionsLoad(string error)
        {
            if (definitionsLoadAttempts < Settings.loadMaxAttempts)
            {
                Debug.LogFormat("Category Definitions failed to load, re attempting in {0} with error {1}", Settings.attemptDelayRemote, error);

                coroutineHandler.StartCoroutine(DelayedAttempt(Settings.attemptDelayRemote,
                    () => categoriesLoader.RequestCategoryDefinitions(OnSuccessDefinitionsLoad, OnFailDefinitionsLoad)));
                definitionsLoadAttempts++;
            }
            else if (fallbackCategoriesLoader != null && fallbackDefinitionsLoadAttempts < Settings.fallbackLoadMaxAttempts)
            {
                Debug.LogFormat("Failed to load category definitions with error {0}, attempting to load with fallback loader", error);
                fallbackCategoriesLoader.RequestCategoryDefinitions(OnSuccessDefinitionsLoad, OnFailDefinitionsLoad);
                fallbackDefinitionsLoadAttempts++;
            }
            else
            {
                Debug.Log("Failed to load category definitions, reset sequence initiated");
            }
        }


        #endregion

        #region ISubscriber
        public void Handler(object sender, BeforeLoadEventArgs e)
        {
            e.SequenceLoader.AddEnumerator(InitSequence());
        }


        #endregion
    }
}