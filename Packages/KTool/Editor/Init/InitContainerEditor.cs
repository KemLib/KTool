using UnityEditor;
using UnityEngine;

namespace KTool.Init.Editor
{
    [CustomEditor(typeof(InitContainer))]
    public class InitContainerEditor : UnityEditor.Editor
    {
        #region Properties
        private SerializedProperty propertTimeLimit,
            propertSteps,
            propertAfterInit,
            propertNextScene,
            propertLoadSceneMode;
        #endregion
        #region Unity Methods
        private void OnEnable()
        {
            propertTimeLimit = serializedObject.FindProperty("timeLimit");
            propertSteps = serializedObject.FindProperty("steps");
            propertAfterInit = serializedObject.FindProperty("afterInit");
            propertNextScene = serializedObject.FindProperty("nextScene");
            propertLoadSceneMode = serializedObject.FindProperty("loadSceneMode");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //
            EditorGUILayout.PropertyField(propertTimeLimit, new GUIContent("Time Limit"));
            propertTimeLimit.floatValue = Mathf.Max(0, propertTimeLimit.floatValue);
            EditorGUILayout.PropertyField(propertSteps, new GUIContent("Steps"));
            EditorGUILayout.PropertyField(propertAfterInit, new GUIContent("After Init"));
            if (propertAfterInit.boolValue)
            {
                EditorGUILayout.PropertyField(propertNextScene, new GUIContent("Next Scene"));
                EditorGUILayout.PropertyField(propertLoadSceneMode, new GUIContent("Load SceneMode"));
            }
            //
            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}
