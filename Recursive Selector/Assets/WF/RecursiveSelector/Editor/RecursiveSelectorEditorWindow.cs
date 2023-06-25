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
        Vector2 scrollPos = Vector2.zero;
        readonly Dictionary<string, string> classCache = new();
        readonly List<string> classCacheList = new();

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

            // Use local functions to avoid errors caused by not having EndHorizontal() / EndVertical() calls due to
            // use of return statements.
            void typeSelection()
            {
                textFieldVal = EditorGUILayout.TextField(textFieldVal);

                if (GUILayout.Button("Select"))
                {
                    bool exists = classCache.TryGetValue(textFieldVal, out string assemblyName);
                    if (!exists)
                    {
                        Debug.LogWarning(@$"Type '{textFieldVal}' does not exist in the class cache.
Check for compile errors and missing namespace input.");
                        return;
                    }

                    Type foundType = Type.GetType($"{textFieldVal}, {assemblyName}");
                    if (foundType == null)
                    {
                        Debug.LogError($"Could not load type '{textFieldVal} from {assemblyName}'. Contact developer.");
                        return;
                    }
                    UpdateSelection(foundType);
                }

                if (GUILayout.Button("Clear"))
                {
                    textFieldVal = string.Empty;
                    GUI.FocusControl(null);
                    Repaint();
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
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
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
                        classCache[type.FullName] = type.Assembly.GetName().Name;
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
