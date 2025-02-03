using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace KTool.Localized.Editor
{
    [CustomEditor(typeof(TextsUIControl))]
    public class TextsUIControlEditor : UnityEditor.Editor
    {
        #region Properties
        private SerializedProperty propertyItems;
        #endregion

        #region Unity Event
        private void OnEnable()
        {
            Init();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Reload Items In Children"))
                ReloadItems();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(propertyItems);
            //
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Method
        private void Init()
        {
            propertyItems = serializedObject.FindProperty("items");
        }

        private void ReloadItems()
        {
            TextsUIControl textsUIControl = serializedObject.targetObject as TextsUIControl;
            List<Text> texts = new List<Text>();
            textsUIControl.GetComponentsInChildren<Text>(texts);
            propertyItems.arraySize = texts.Count;
            int index = 0;
            foreach (SerializedProperty propertyItem in propertyItems)
            {
                SerializedProperty propertyText = propertyItem.FindPropertyRelative("text");
                propertyText.objectReferenceValue = texts[index];
                index++;
            }
        }
        #endregion
    }
}
