using System;
using System.Collections;
using System.Collections.Generic;
using PizzaPie.Questions.Serialization;
using UnityEngine;

namespace PizzaPie.Questions.Loaders
{
    public class LocalQuestionLoader : IQuestionsLoader
    {
        private IJsonAdapter jsonAdapter;
        private string bundlesPath;

        public LocalQuestionLoader(IJsonAdapter jsonAdapter, string questionBundlesPath = Paths.DEFAULT_QnABUNDLES_PATH)
        {
            this.jsonAdapter = jsonAdapter;
            bundlesPath = questionBundlesPath;
        }


        public void RequestQuestions(Action<QnABundle[]> OnQuestionLoaded, Action<string> OnQuestionsFailedToLoad)
        {
            var jsons = Resources.LoadAll<TextAsset>(bundlesPath);
            QnABundle[] bundles = new QnABundle[jsons.Length];

            for (int i = 0; i < jsons.Length; i++)
            {
                var data = jsonAdapter.Deserialize<QnADataBundle>(jsons[i].text);
                bundles[i] = new QnABundle(data);
                Resources.UnloadAsset(jsons[i]);
            }

            OnQuestionLoaded(bundles);
        }
    }
}
