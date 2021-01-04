using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using UnityEngine;
using UnityEditor;

using PizzaPie.Editor.Util;
using PizzaPie.Questions;
using PizzaPie.Questions.Serialization;


namespace PizzaPie.Editor.Questions
{
    public class QuestionEditorHelperWindow : EditorWindow
    {
        private int tab;
        private QuestionCategory category;
        private int questionsCount = 1;

        private static IJsonAdapter jsonAdapter;

        [MenuItem("Window/Questions Helper")]
        private static void Init()
        {
            var window = EditorWindow.GetWindow<QuestionEditorHelperWindow>();
            jsonAdapter = new UnityJsonAdapter();
            window.Show();
        }


        private void OnGUI()
        {
            tab = GUILayout.Toolbar(tab, new GUIContent[] { new GUIContent("QnA Bundles"), new GUIContent("Categories") });

            switch (tab)
            {
                case 0:
                    OnQnABundlesTab();
                    break;
                case 1:
                    OnCategoriesTab();
                    break;
            }
        }


        private void OnQnABundlesTab()
        {
            GUILayout.Label(new GUIContent("Generate QnA Bundle"));
            
            category = (QuestionCategory)EditorGUILayout.EnumPopup(new GUIContent("Select Category"), category);
            questionsCount = EditorGUILayout.IntField(new GUIContent("Question Fields To Add"), questionsCount);

            if(GUILayout.Button(new GUIContent("Generate")))
            {
                string dirPath = string.Concat(Paths.RESOURCES_PATH, "/", Paths.DEFAULT_QnABUNDLES_PATH);

                if (!Directory.Exists(string.Concat(Application.dataPath, "/", dirPath)))
                    Assets.CreateFolder(dirPath);

                var filePath = string.Concat(Application.dataPath, "/", dirPath, "/", category.ToString(), "_Bundle.json");


                if (File.Exists(filePath))
                {
                    Debug.Log("Bundle with this category already exists, please edit it directly to add more questions!");
                    return;
                }
                
                using(var sr = File.CreateText(filePath))
                {
                    if (jsonAdapter == null) jsonAdapter = new UnityJsonAdapter();

                    var bundle = new QnADataBundle() { category = category, QnADatas = new QnAData[questionsCount] };
                    for (int i = 0; i < questionsCount; i++)
                        bundle.QnADatas[i] = QnAData.GetEmpty();

                    var json = jsonAdapter.Serialize(bundle);

                    sr.Write(json);
                    AssetDatabase.Refresh();

                    Debug.LogFormat("Bundle successfuly created at {0}", filePath);
                }
            }
        }

        private void OnCategoriesTab()
        {
            if(GUILayout.Button(new GUIContent("Generate Category Definition Assets")))
            {
                var filePathTrimmed = string.Concat(Paths.RESOURCES_PATH, "/", Paths.DEFAULT_CATEGORY_DEFINITIONS_PATH);
                var filePath = string.Concat(Application.dataPath, "/", filePathTrimmed);

                if (!File.Exists(filePath))
                    Assets.CreateFolder(filePathTrimmed);

                var categories = Enum.GetValues(typeof(QuestionCategory)) as QuestionCategory[];

                foreach(var cat in categories)
                {
                    var fileName = string.Concat(cat.ToString(), "_Category.asset");
                    if (!File.Exists(string.Concat(filePath, '/', fileName)))
                    {
                        var asset = Assets.CreateScriptableAsset<CategoriesDefinitionContainer>(filePathTrimmed, true, string.Concat(cat.ToString(), "_Category"));
                        asset.category = cat;
                        EditorUtility.SetDirty(asset);

                        Debug.LogFormat("Category Definition Created {0}", cat.ToString());
                    }
                }
            }
        }
    }
}
