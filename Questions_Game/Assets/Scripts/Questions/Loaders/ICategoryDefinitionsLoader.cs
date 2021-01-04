using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PizzaPie.Questions.Loaders
{
    public interface ICategoryDefinitionsLoader
    {
        void RequestCategoryDefinitions(Action<CategoryDefinition[]> OnLoadSuccess, Action<string> OnLoadFail);
    }
}
