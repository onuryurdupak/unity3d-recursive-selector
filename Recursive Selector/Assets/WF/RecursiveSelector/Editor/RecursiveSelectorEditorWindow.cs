using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

namespace WF.RecursiveSelector
{
    public class RecursiveSelectorEditorWindow : EditorWindow
    {
        string textFieldVal = string.Empty;
        bool wasCompiling = EditorApplication.isCompiling;
        bool foldout = false;
        Vector2 scollPos = Vector2.zero;
        readonly HashSet<string> classCache = new(); // For O(1) lookup.
        readonly List<string> classCacheList = new(); // For ordering.

        [MenuItem("Window/Recursive Selector")]
        static void Init()
        {
            RecursiveSelectorEditorWindow window = (RecursiveSelectorEditorWindow)GetWindow(typeof(RecursiveSelectorEditorWindow));
            window.titleContent = new GUIContent("Recursive Selector");
            window.Show();
        }

        private void OnEnable()
        {
            RefreshClassCache();
        }

        private void Update()
        {
            if (wasCompiling && !EditorApplication.isCompiling)
            {
                OnCompleteCompiling();
            }

            wasCompiling = EditorApplication.isCompiling;
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();

            // Use local functions to avoid errors caused by not closing groups when return prevents code from 
            // executing EndHorizontal() / EndVertical() calls.
            void typeSelection()
            {
                textFieldVal = EditorGUILayout.TextField(textFieldVal);

                if (GUILayout.Button("Select"))
                {
                    bool exists = classCache.TryGetValue(textFieldVal, out string foundValue);
                    if (!exists)
                    {
                        Debug.LogError($"Type '{textFieldVal}' does not exist in class cache. Check for compile errors and missing namespace portion.");
                        return;
                    }

                    Type foundType = Type.GetType(textFieldVal);
                    if (foundType == null)
                    {
                        Debug.LogError($"Internal error: Type '{textFieldVal}' does not exist in cache.");
                        return;
                    }
                    UpdateSelection(foundType);
                }

                if (GUILayout.Button("Clear"))
                {
                    textFieldVal = string.Empty;
                }
            }
            typeSelection();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            string[] searchResults = classCacheList
                .Where(option => option.IndexOf(textFieldVal, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToArray();

            EditorGUILayout.EndHorizontal();

            foldout = EditorGUILayout.Foldout(foldout, "Filtered Results");
            if (foldout)
            {
                scollPos = EditorGUILayout.BeginScrollView(scollPos);
                foreach (string s in searchResults)
                {
                    if (GUILayout.Button($"     {s}", Styles.LeftAlignedWhiteFont))
                    {
                        textFieldVal = s;
                        GUI.FocusControl(null);
                        Repaint();
                    }
                }
                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.EndVertical();
        }

        private void OnCompleteCompiling()
        {
            RefreshClassCache();
        }

        private void RefreshClassCache()
        {
            classCache.Clear();
            classCacheList.Clear();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.FullName.StartsWith("System") &&
                    !assembly.FullName.StartsWith("Microsoft") &&
                    !assembly.FullName.StartsWith("mscorlib") &&
                    !assembly.FullName.StartsWith("netstandard") &&
                    !assembly.FullName.StartsWith("UnityEditor"))
                {
                    var types = assembly.GetTypes()
                        .Where(type => (type.IsSubclassOf(typeof(MonoBehaviour)) || type.IsSubclassOf(typeof(Component))) && !type.IsAbstract);

                    foreach (var type in types)
                    {
                        classCache.Add(type.FullName);
                        classCacheList.Add(type.FullName);
                    }
                }
            }
        }

        private void UpdateSelection(Type t)
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            List<GameObject> componentObjects = new();

            foreach (GameObject selectedObject in selectedObjects)
            {
                Component[] componentsInChildren = selectedObject.GetComponentsInChildren(t);

                foreach (Component component in componentsInChildren)
                {
                    componentObjects.Add(component.gameObject);
                }
            }

            Selection.objects = componentObjects.ToArray();
        }
    }
}
