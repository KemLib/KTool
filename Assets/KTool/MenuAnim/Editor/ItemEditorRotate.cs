using UnityEditor;
using UnityEngine;

namespace KTool.MenuAnim.Editor
{
    public class ItemEditorRotate : ItemEditor
    {
        #region Properties
        private SerializedProperty propertyRtfObject,
            propertyRotateMode,
            propertyUseOrigin,
            propertyOrigin,
            propertyTaget,
            propertyDelay,
            propertyDuration,
            propertyDoEase;
        #endregion Properties

        #region Construction
        public ItemEditorRotate(SerializedProperty propertyItem) : base(propertyItem)
        {
            propertyRtfObject = propertyItem.FindPropertyRelative("rtfObject");
            propertyRotateMode = propertyItem.FindPropertyRelative("rotateMode");
            propertyUseOrigin = propertyItem.FindPropertyRelative("useOrigin");
            propertyOrigin = propertyItem.FindPropertyRelative("origin");
            propertyTaget = propertyItem.FindPropertyRelative("taget");
            propertyDelay = propertyItem.FindPropertyRelative("delay");
            propertyDuration = propertyItem.FindPropertyRelative("duration");
            propertyDoEase = propertyItem.FindPropertyRelative("doEase");
        }
        #endregion

        #region UnityEvent
        protected override void OnGui_Chil()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(propertyRtfObject, new GUIContent("Rtf Object"));
            if (EditorGUI.EndChangeCheck() && propertyRtfObject.objectReferenceValue != null)
            {
                RectTransform rtfObject = propertyRtfObject.objectReferenceValue as RectTransform;
                propertyTaget.floatValue = rtfObject.rotation.eulerAngles.z;
            }
            EditorGUILayout.PropertyField(propertyRotateMode, new GUIContent("Rotate Mode"));
            EditorGUILayout.PropertyField(propertyUseOrigin, new GUIContent("Use Origin"));
            if (propertyUseOrigin.boolValue)
                EditorGUILayout.PropertyField(propertyOrigin, new GUIContent("Origin"));
            EditorGUILayout.PropertyField(propertyTaget, new GUIContent("Taget"));
            EditorGUILayout.PropertyField(propertyDelay, new GUIContent("Delay"));
            EditorGUILayout.PropertyField(propertyDuration, new GUIContent("Duration"));
            if (propertyDuration.floatValue <= 0)
                propertyDuration.floatValue = 0.5f;
            EditorGUILayout.PropertyField(propertyDoEase, new GUIContent("Do Ease"));
        }
        #endregion UnityEvent

        #region Method
        public override void SetActiveItem(bool isActive)
        {
            if (propertyRtfObject.objectReferenceValue == null)
                return;
            RectTransform rtf = propertyRtfObject.objectReferenceValue as RectTransform;
            GameObject go = rtf.gameObject;
            go.SetActive(isActive);
            EditorUtility.SetDirty(go);
        }
        public override void SetStateTaget()
        {
            if (propertyRtfObject.objectReferenceValue == null)
                return;
            RectTransform rtf = propertyRtfObject.objectReferenceValue as RectTransform;
            rtf.rotation = Quaternion.Euler(rtf.rotation.eulerAngles.x, rtf.rotation.eulerAngles.y, propertyTaget.floatValue);
        }
        #endregion Method
    }
}
