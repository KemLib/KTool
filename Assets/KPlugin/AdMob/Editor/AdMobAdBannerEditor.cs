using KTool.Ad;
using UnityEditor;
using UnityEngine;

namespace KPlugin.AdMob.Editor
{
    [CustomEditor(typeof(AdMobAdBanner))]
    public class AdMobAdBannerEditor : UnityEditor.Editor
    {
        #region Properties
        private SerializedProperty propertyInitType,
            propertyIndexAd,
            propertyIsAutoCreate,
            propertyAdPosition,
            propertyCustomAdPosition,
            propertyAdSize,
            propertyCustomAdSize,
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
            EditorGUILayout.PropertyField(propertyInitType, new GUIContent("Init Type"));
            EditorGUILayout.PropertyField(propertyIndexAd, new GUIContent("Index Ad"));
            //
            EditorGUILayout.PropertyField(propertyIsAutoCreate, new GUIContent("Auto Create"));
            if (propertyIsAutoCreate.boolValue)
            {
                EditorGUILayout.PropertyField(propertyAdPosition, new GUIContent("Ad Position"));
                AdPosition adPosition = (AdPosition)propertyAdPosition.enumValueIndex;
                if (adPosition == AdPosition.Custom)
                    EditorGUILayout.PropertyField(propertyCustomAdPosition, new GUIContent("Position"));
                //
                EditorGUILayout.PropertyField(propertyAdSize, new GUIContent("Ad Size"));
                AdSize adSize = (AdSize)propertyAdSize.enumValueIndex;
                if (adSize == AdSize.Custom)
                    EditorGUILayout.PropertyField(propertyCustomAdSize, new GUIContent("Size"));
                //
                EditorGUILayout.PropertyField(propertyIsShowAfterInit, new GUIContent("Show After Init"));
            }
            //
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Method
        private void Init()
        {
            propertyInitType = serializedObject.FindProperty("initType");
            propertyIndexAd = serializedObject.FindProperty("indexAd");
            propertyAdPosition = serializedObject.FindProperty("adPosition");
            propertyCustomAdPosition = serializedObject.FindProperty("customAdPosition");
            propertyAdSize = serializedObject.FindProperty("adSize");
            propertyCustomAdSize = serializedObject.FindProperty("customAdSize");
            propertyIsAutoCreate = serializedObject.FindProperty("isAutoCreate");
            propertyIsShowAfterInit = serializedObject.FindProperty("isShowAfterInit");
        }
        #endregion
    }
}
