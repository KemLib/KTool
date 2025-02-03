using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace KTool.MenuAnim.Editor
{
    public class ItemEditorColor : ItemEditor
    {
        #region Properties
        private SerializedProperty propertyGraphic,
            propertyUseOrigin,
            propertyOrigin,
            propertyTaget,
            propertyDelay,
            propertyDuration,
            propertyDoEase;
        #endregion Properties

        #region Construction
        public ItemEditorColor(SerializedProperty propertyItem) : base(propertyItem)
        {
            propertyGraphic = propertyItem.FindPropertyRelative("graphic");
            propertyUseOrigin = propertyItem.FindPropertyRelative("useOrigin");
            propertyOrigin = propertyItem.FindPropertyRelative("origin");
            propertyTaget = propertyItem.FindPropertyRelative("taget");
            propertyDelay = propertyItem.FindPropertyRelative("delay");
            propertyDuration = propertyItem.FindPropertyRelative("duration");
            propertyDoEase = propertyItem.FindPropertyRelative("doEase");
        }
        #endregion

        #region Unity Event		
        protected override void OnGui_Chil()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(propertyGraphic, new GUIContent("Graphic"));
            if (EditorGUI.EndChangeCheck())
            {
                MaskableGraphic graphic = propertyGraphic.objectReferenceValue as MaskableGraphic;
                propertyTaget.colorValue = graphic.color;
            }
            EditorGUILayout.PropertyField(propertyUseOrigin, new GUIContent("Use Origi"));
            if (propertyUseOrigin.boolValue)
                EditorGUILayout.PropertyField(propertyOrigin, new GUIContent("Origin"));
            EditorGUILayout.PropertyField(propertyTaget, new GUIContent("Taget"));
            EditorGUILayout.PropertyField(propertyDelay, new GUIContent("Delay"));
            EditorGUILayout.PropertyField(propertyDuration, new GUIContent("Duration"));
            if (propertyDuration.floatValue <= 0)
                propertyDuration.floatValue = 0.5f;
            EditorGUILayout.PropertyField(propertyDoEase, new GUIContent("Do Ease"));
        }
        #endregion Unity Event

        #region Method
        public override void SetActiveItem(bool isActive)
        {
            if (propertyGraphic.objectReferenceValue == null)
                return;
            MaskableGraphic graphic = propertyGraphic.objectReferenceValue as MaskableGraphic;
            graphic.gameObject.SetActive(isActive);
            EditorUtility.SetDirty(graphic.gameObject);
        }
        public override void SetStateTaget()
        {
            if (propertyGraphic.objectReferenceValue == null)
                return;
            MaskableGraphic graphic = propertyGraphic.objectReferenceValue as MaskableGraphic;
            graphic.color = propertyTaget.colorValue;
        }
        #endregion Method
    }
}
