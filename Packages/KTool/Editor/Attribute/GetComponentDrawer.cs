using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(GetComponentAttribute))]
    public class GetComponentDrawer : PropertyDrawer
    {
        #region Properties
        public const string ERROR_TYPE = "type of property not is Component or Component[]";
        #endregion Properties

        #region Constructor
        public GetComponentDrawer() : base()
        {

        }
        #endregion Constructor

        #region Unity Event		
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType.IsSubclassOf(typeof(Component)))
            {
                OnGui_Field(position, label, property);
                return;
            }
            if (fieldInfo.FieldType.IsArray)
            {
                Type typeElement = fieldInfo.FieldType.GetElementType();
                if (typeElement.IsSubclassOf(typeof(Component)))
                {
                    OnGui_Fields(position, label, property, typeElement);
                    return;
                }
            }
            EditorGUI.LabelField(position, label, new GUIContent(ERROR_TYPE));
        }

        private void OnGui_Field(Rect position, GUIContent label, SerializedProperty property)
        {
            MonoBehaviour mono = property.serializedObject.targetObject as MonoBehaviour;
            Component component;
            if (property.objectReferenceValue != null)
            {
                component = property.objectReferenceValue as Component;
                if (component.gameObject.GetInstanceID() == mono.gameObject.GetInstanceID())
                {
                    EditorGUI.PropertyField(position, property, label);
                    return;
                }
            }
            //
            component = mono.GetComponent(fieldInfo.FieldType);
            //
            property.objectReferenceValue = component;
            EditorGUI.PropertyField(position, property, label);
        }
        private void OnGui_Fields(Rect position, GUIContent label, SerializedProperty property, Type typeElement)
        {
            int indexElement = EditorGui_Draw.PropertyElement_IndexOf(property);
            if (indexElement == 0)
            {
                if (!Reload_PropertyRoot(property, typeElement))
                    return;
            }
            //
            EditorGUI.PropertyField(position, property, label);
        }
        #endregion Unity Event

        #region Method
        private bool Reload_PropertyRoot(SerializedProperty property, Type typeElement)
        {
            List<Component> components = new List<Component>();
            MonoBehaviour mono = property.serializedObject.targetObject as MonoBehaviour;
            mono.GetComponents(typeElement, components);
            //
            SerializedProperty propertyRoot = EditorGui_Draw.PropertyElement_GetPropertyRoot(property);
            EditorGui_Draw.ArrayAsync_Component(components, propertyRoot);
            if (propertyRoot.arraySize == 0)
                return false;
            return true;
        }
        #endregion
    }
}
