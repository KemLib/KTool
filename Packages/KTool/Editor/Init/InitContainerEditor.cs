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
            propertyOnBegin,
            propertyOnStep,
            propertyOnProgress,
            propertyOnEnd;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            propertyTimeLimit = serializedObject.FindProperty("timeLimit");
            propertySteps = serializedObject.FindProperty("steps");
            propertyOnBegin = serializedObject.FindProperty("onBegin");
            propertyOnStep = serializedObject.FindProperty("onStep");
            propertyOnProgress = serializedObject.FindProperty("onProgress");
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
            EditorGUILayout.PropertyField(propertyOnBegin, new GUIContent("On Begin"));
            EditorGUILayout.PropertyField(propertyOnStep, new GUIContent("On Step"));
            EditorGUILayout.PropertyField(propertyOnProgress, new GUIContent("On Progress"));
            EditorGUILayout.PropertyField(propertyOnEnd, new GUIContent("On End"));
            //
            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}
