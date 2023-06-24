using UnityEngine;
using UnityEditor;

namespace WF.RecursiveSelector
{
    public class RecursiveSelectorEditorWindow : EditorWindow
    {
        string query = string.Empty;

        const string TOOLTIP = @"TODO(onur): Explain how it works.";

        [MenuItem("Window/Recursive Selector")]
        static void Init()
        {
            RecursiveSelectorEditorWindow window = (RecursiveSelectorEditorWindow)GetWindow(typeof(RecursiveSelectorEditorWindow));
            window.titleContent = new GUIContent("Recursive Selector");
            window.Show();
        }

        void OnGUI()
        {
            query = EditorGUILayout.TextField(query);
            
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                Debug.Log("Enter key pressed! Query: " + query);
                // TODO(onur): Check current selection, create type from string.
                // Check existense, then find all children of same type.
                // Deselect everything and select matching elements.
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.HelpBox(TOOLTIP, MessageType.Info);
        }
    }
}
