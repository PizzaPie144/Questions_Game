using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.Questions.Loaders
{
    public class LocalCategoryDefinitionLoader : ICategoryDefinitionsLoader
    {
        private string categoriesPath;

        public LocalCategoryDefinitionLoader(string questionCategoriesPath = Paths.DEFAULT_CATEGORY_DEFINITIONS_PATH)
        {
            categoriesPath = questionCategoriesPath;
        }

        public void RequestCategoryDefinitions(Action<CategoryDefinition[]> OnLoadSuccess, Action<string> OnLoadFail)
        {
            var definitionContainers = Resources.LoadAll<CategoriesDefinitionContainer>(categoriesPath);
            var definitions = new CategoryDefinition[definitionContainers.Length];

            for (int i = 0; i < definitionContainers.Length; i++)
            {
                definitions[i] = new CategoryDefinition(definitionContainers[i].category, definitionContainers[i].color, definitionContainers[i].sprite);
                //Resources.UnloadAsset(definitionContainers[i]);
            }

            OnLoadSuccess(definitions);
        }
    }
}
