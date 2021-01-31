using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PizzaPie.QuestionsGame.Editor 
{
    [CustomEditor(typeof(CategoriesDefinitionContainer))]
    public class CategoryDefinitionCustomInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save"))
            {
                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }

        }
    }
}
