using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.QuestionsGame.Questions
{
    public class CategoryDefinition 
    {
        public QuestionCategory Category { get; private set; }
        public Color Color { get; private set; }
        public Sprite Sprite { get; private set; }
        
        public CategoryDefinition(QuestionCategory category ,Color color, Sprite sprite)
        {
            Category = category;
            Color = color;
            Sprite = sprite;
        }

    }
}