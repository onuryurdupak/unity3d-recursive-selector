using UnityEngine;
using UnityEditor;

namespace WF.QuerySelector
{
    public class QuerySelectorEditorWindow : EditorWindow
    {
        string query = string.Empty;

        const string TOOLTIP = @"Usage of Query Selector: https://github.com/onuryurdupak/unity3d-query-selector#readme";

        [MenuItem("Window/Query Selector")]
        static void Init()
        {
            QuerySelectorEditorWindow window = (QuerySelectorEditorWindow)GetWindow(typeof(QuerySelectorEditorWindow));
            window.titleContent = new GUIContent("Query Selector");
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
