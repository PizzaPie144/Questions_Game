﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PizzaPie {

    [CreateAssetMenu(menuName ="Category Definition")]
    public class CategoriesDefinitionContainer : ScriptableObject
    {
        public Questions.QuestionCategory category;
        
        public Color color;

        public Texture texture;
    }
}