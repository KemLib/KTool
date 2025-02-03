using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KTool.Localized.Editor
{
    [CustomEditor(typeof(LocalizedText))]
    public class LanguageControlEditor : UnityEditor.Editor
    {
        #region Properties
        private SerializedProperty propertyTexts;
        #endregion Properties

        #region Unity Event
        private void OnEnable()
        {
            propertyTexts = serializedObject.FindProperty("texts");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Reload Texts"))
                ReloadTexts();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(propertyTexts);
            //
            serializedObject.ApplyModifiedProperties();
        }
        #endregion Unity Event

        #region Method
        private void ReloadTexts()
        {
            TextControl[] texts_Resource = Resources.FindObjectsOfTypeAll<TextControl>();
            List<TextControl> texts_Scene = new List<TextControl>();
            for (int i = 0; i < texts_Resource.Length; i++)
                if (!EditorUtility.IsPersistent(texts_Resource[i].transform.root.gameObject))
                    texts_Scene.Add(texts_Resource[i]);
            //
            propertyTexts.arraySize = texts_Scene.Count;
            if (propertyTexts.arraySize <= 0)
                return;
            int index = 0;
            foreach (SerializedProperty property in propertyTexts)
            {
                property.objectReferenceValue = texts_Scene[index];
                index++;
            }
        }
        #endregion Method
    }
}
