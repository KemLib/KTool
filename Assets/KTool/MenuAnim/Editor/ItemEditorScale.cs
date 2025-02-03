using UnityEditor;
using UnityEngine;

namespace KTool.MenuAnim.Editor
{
    public class ItemEditorScale : ItemEditor
    {
        #region Properties
        private SerializedProperty propertyRtfObject,
            propertyRtfType,
            propertyUseOrigin,
            propertyOrigin,
            propertyTaget,
            propertyDelay,
            propertyDuration,
            propertyDoEase;
        #endregion Properties

        #region Construction
        public ItemEditorScale(SerializedProperty propertyItem) : base(propertyItem)
        {
            propertyRtfObject = propertyItem.FindPropertyRelative("rtfObject");
            propertyRtfType = propertyItem.FindPropertyRelative("rtfType");
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
                propertyTaget.vector2Value = rtfObject.localScale;
            }
            EditorGUILayout.PropertyField(propertyRtfType, new GUIContent("rtf Type"));
            EditorGUILayout.PropertyField(propertyUseOrigin, new GUIContent("Use Origin"));
            RtfType rtfType = (RtfType)propertyRtfType.enumValueIndex;
            switch (rtfType)
            {
                case RtfType.XY:
                    if (propertyUseOrigin.boolValue)
                        EditorGUILayout.PropertyField(propertyOrigin, new GUIContent("Origin"));
                    EditorGUILayout.PropertyField(propertyTaget, new GUIContent("Taget"));
                    break;
                case RtfType.X:
                    if (propertyUseOrigin.boolValue)
                    {
                        float originX = propertyOrigin.vector2Value.x;
                        originX = EditorGUILayout.FloatField(new GUIContent("Origin X"), originX);
                        propertyOrigin.vector2Value = new Vector2(originX, propertyOrigin.vector2Value.y);
                    }
                    float tagetX = propertyTaget.vector2Value.x;
                    tagetX = EditorGUILayout.FloatField(new GUIContent("Taget X"), tagetX);
                    propertyTaget.vector2Value = new Vector2(tagetX, propertyTaget.vector2Value.y);
                    break;
                case RtfType.Y:
                    if (propertyUseOrigin.boolValue)
                    {
                        float originY = propertyOrigin.vector2Value.y;
                        originY = EditorGUILayout.FloatField(new GUIContent("Origin Y"), originY);
                        propertyOrigin.vector2Value = new Vector2(propertyOrigin.vector2Value.x, originY);
                    }
                    float tagetY = propertyTaget.vector2Value.y;
                    tagetY = EditorGUILayout.FloatField(new GUIContent("Taget Y"), tagetY);
                    propertyTaget.vector2Value = new Vector2(propertyTaget.vector2Value.x, tagetY);

                    break;
            }
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
            RtfType rtfType = (RtfType)propertyRtfType.enumValueIndex;
            switch (rtfType)
            {
                case RtfType.XY:
                    rtf.localScale = propertyTaget.vector2Value;
                    break;
                case RtfType.X:
                    rtf.localScale = new Vector2(propertyTaget.vector2Value.x, rtf.localScale.y);
                    break;
                case RtfType.Y:
                    rtf.localScale = new Vector2(rtf.localScale.x, propertyTaget.vector2Value.y);
                    break;
            }
        }
        #endregion Method
    }
}
