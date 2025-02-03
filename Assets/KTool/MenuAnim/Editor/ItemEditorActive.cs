using UnityEditor;
using UnityEngine;

namespace KTool.MenuAnim.Editor
{
    public class ItemEditorActive : ItemEditor
    {
        #region Properties
        private SerializedProperty propertyGameObject,
            propertyDelay,
            propertyIsActive;
        #endregion Properties

        #region Constrution
        public ItemEditorActive(SerializedProperty propertyItem) : base(propertyItem)
        {
            propertyGameObject = propertyItem.FindPropertyRelative("gameObject");
            propertyDelay = propertyItem.FindPropertyRelative("delay");
            propertyIsActive = propertyItem.FindPropertyRelative("isActive");
        }
        #endregion

        #region UnityEvent
        protected override void OnGui_Chil()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(propertyGameObject, new GUIContent("Game Object"));
            if (EditorGUI.EndChangeCheck() && propertyGameObject.objectReferenceValue != null)
            {
                GameObject gameObject = propertyGameObject.objectReferenceValue as GameObject;
                propertyIsActive.boolValue = gameObject.activeSelf;
            }
            EditorGUILayout.PropertyField(propertyDelay, new GUIContent("Delay"));
            EditorGUILayout.PropertyField(propertyIsActive, new GUIContent("Is Active"));
        }
        #endregion UnityEvent

        #region Method
        public override void SetActiveItem(bool isActive)
        {
            if (propertyGameObject.objectReferenceValue == null)
                return;
            GameObject go = propertyGameObject.objectReferenceValue as GameObject;
            go.SetActive(isActive);
            EditorUtility.SetDirty(go);
        }
        public override void SetStateTaget()
        {
            if (propertyGameObject.objectReferenceValue == null)
                return;
            GameObject go = propertyGameObject.objectReferenceValue as GameObject;
            go.SetActive(propertyIsActive.boolValue);
            EditorUtility.SetDirty(go);
        }
        #endregion Method
    }
}
