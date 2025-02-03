using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KPlugin.MaxMediation.Editor
{
	[CustomEditor(typeof(MaxAdAppResume))]
	public class MaxAdAppResumeEditor : UnityEditor.Editor
	{
        #region Properties
        private SerializedProperty propertyInitType,
            propertyAdTagetType,
            propertyAdAppOpen,
            propertyAdInterstitial,
            propertyIsActive;
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
            EditorGUILayout.PropertyField(propertyInitType, new GUIContent("Init Type"));
            EditorGUILayout.PropertyField(propertyAdTagetType, new GUIContent("Ad Taget Type"));
            MaxAdAppResume.AdTagetType adType = (MaxAdAppResume.AdTagetType)propertyAdTagetType.enumValueIndex;
            switch (adType)
            {
                case MaxAdAppResume.AdTagetType.AdAppOpen:
                    EditorGUILayout.PropertyField(propertyAdAppOpen, new GUIContent("Ad Taget"));
                    break;
                case MaxAdAppResume.AdTagetType.AdInterstitial:
                    EditorGUILayout.PropertyField(propertyAdInterstitial, new GUIContent("Ad Taget"));
                    break;
            }
            EditorGUILayout.PropertyField(propertyIsActive, new GUIContent("Is Active"));
            //
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Method
        private void Init()
        {
            propertyInitType = serializedObject.FindProperty("initType");
            propertyAdTagetType = serializedObject.FindProperty("adTagetType");
            propertyAdAppOpen = serializedObject.FindProperty("adAppOpen");
            propertyAdInterstitial = serializedObject.FindProperty("adInterstitial");
            propertyIsActive = serializedObject.FindProperty("isActive");
        }
        #endregion
    }
}
