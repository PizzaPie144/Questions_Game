﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.Questions
{
    public struct CategoryDefinition 
    {
        public QuestionCategory Category { get; private set; }
        public Color Color { get; private set; }
        public Texture Texture { get; private set; }
        
        public CategoryDefinition(QuestionCategory category ,Color color, Texture texture)
        {
            Category = category;
            Color = color;
            Texture = texture;
        }

    }
}