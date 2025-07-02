using UnityEditor;
using UnityEngine;

namespace KTool.Init.Editor
{
    [CustomEditor(typeof(InitContainer))]
    public class InitContainerEditor : UnityEditor.Editor
    {
        #region Properties
        private SerializedProperty propertyTimeLimit,
            propertySteps,
            propertyAfterInit,
            propertyNextScene,
            propertyLoadSceneMode,
            propertyOnBegin,
            propertyOnEnd;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            propertyTimeLimit = serializedObject.FindProperty("timeLimit");
            propertySteps = serializedObject.FindProperty("steps");
            propertyAfterInit = serializedObject.FindProperty("afterInit");
            propertyNextScene = serializedObject.FindProperty("nextScene");
            propertyLoadSceneMode = serializedObject.FindProperty("loadSceneMode");
            propertyOnBegin = serializedObject.FindProperty("onBegin");
            propertyOnEnd = serializedObject.FindProperty("onEnd");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //
            EditorGUILayout.PropertyField(propertyTimeLimit, new GUIContent("Time Limit"));
            propertyTimeLimit.floatValue = Mathf.Max(0, propertyTimeLimit.floatValue);
            EditorGUILayout.PropertyField(propertySteps, new GUIContent("Steps"));
            //
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(propertyAfterInit, new GUIContent("After Init"));
            if (propertyAfterInit.boolValue)
            {
                EditorGUILayout.PropertyField(propertyNextScene, new GUIContent("Next Scene"));
                EditorGUILayout.PropertyField(propertyLoadSceneMode, new GUIContent("Load SceneMode"));
            }
            //
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(propertyOnBegin, new GUIContent("On Begin"));
            EditorGUILayout.PropertyField(propertyOnEnd, new GUIContent("On End"));
            //
            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}
