using UnityEditor;
using UnityEngine;

namespace KTool.MenuAnim.Editor
{
    [CustomEditor(typeof(MenuAnimControl))]
    public class MenuControlEditor : UnityEditor.Editor
    {
        #region Properties
        private SerializedProperty propertyIsShow,
            propertyUpdateType,
            propertyUnscaleTime;
        private AnimEditor aeHide,
            aeShow;
        private bool isShowAnims;
        #endregion Properties

        #region UnityEvent
        private void OnEnable()
        {
            Init();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(propertyIsShow, new GUIContent("Is Show"));
            if (EditorGUI.EndChangeCheck())
            {
                if (propertyIsShow.boolValue)
                    aeShow.SetStateTaget();
                else
                    aeHide.SetStateTaget();
            }
            //
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Active Items"))
                aeShow.SetActiveItem(true);
            if (GUILayout.Button("Deactive Items"))
                aeHide.SetActiveItem(false);
            EditorGUILayout.EndHorizontal();
            //
            EditorGUILayout.PropertyField(propertyUpdateType, new GUIContent("Update Type"));
            EditorGUILayout.PropertyField(propertyUnscaleTime, new GUIContent("Unscale Time"));
            //
            aeHide.OnGui();
            aeShow.OnGui();
            GUILayout.Space(10);
            //
            serializedObject.ApplyModifiedProperties();
        }
        #endregion UnityEvent

        #region Method
        private void Init()
        {
            propertyIsShow = serializedObject.FindProperty("isShow");
            propertyUpdateType = serializedObject.FindProperty("updateType");
            propertyUnscaleTime = serializedObject.FindProperty("unscaleTime");
            SerializedProperty propertyAnimHide = serializedObject.FindProperty("animHide"),
                propertyAnimShow = serializedObject.FindProperty("animShow");
            //
            serializedObject.Update();
            aeHide = new AnimEditor(propertyAnimHide, "Hide");
            aeShow = new AnimEditor(propertyAnimShow, "Show");
            serializedObject.ApplyModifiedProperties();
        }
        #endregion Method
    }
}
