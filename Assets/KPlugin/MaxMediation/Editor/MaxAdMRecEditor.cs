using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using KTool.Ad;

namespace KPlugin.MaxMediation.Editor
{
	[CustomEditor(typeof(MaxAdMRec))]
	public class MaxAdMRecEditor : UnityEditor.Editor
	{
		#region Properties
		private SerializedProperty propertyIndexAd,
            propertyIsAutoCreate,
            propertyAdPosition,
            propertyCustomAdPosition,
            propertyIsAutoReload,
            propertyIsShowAfterInit;
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
            EditorGUILayout.PropertyField(propertyIndexAd, new GUIContent("Index Ad"));
            EditorGUILayout.PropertyField(propertyIsAutoCreate, new GUIContent("Is Auto Create"));
            EditorGUILayout.PropertyField(propertyAdPosition, new GUIContent("Ad Position"));
            AdPosition adPosition = (AdPosition)propertyAdPosition.enumValueIndex;
            if (adPosition == AdPosition.Custom)
                EditorGUILayout.PropertyField(propertyCustomAdPosition, new GUIContent("Custom Ad Position"));
            EditorGUILayout.PropertyField(propertyIsAutoReload, new GUIContent("Is Auto Reload"));
            EditorGUILayout.PropertyField(propertyIsShowAfterInit, new GUIContent("Is Show After Init"));
            //
            serializedObject.ApplyModifiedProperties();
        }
		#endregion
		
		#region Method
		private void Init()
		{
            propertyIndexAd = serializedObject.FindProperty("indexAd");
            propertyIsAutoCreate = serializedObject.FindProperty("isAutoCreate");
            propertyAdPosition = serializedObject.FindProperty("adPosition");
            propertyCustomAdPosition = serializedObject.FindProperty("customAdPosition");
            propertyIsAutoReload = serializedObject.FindProperty("isAutoReload");
            propertyIsShowAfterInit = serializedObject.FindProperty("isShowAfterInit");
        }
		#endregion
	}
}
