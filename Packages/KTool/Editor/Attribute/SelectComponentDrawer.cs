using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(SelectComponentAttribute))]
    public class SelectComponentDrawer : PropertyDrawer
    {
        #region Properties
        public const string ERROR_TYPE = "type of property not is Component or Component[]",
            ERROR_LIST_COMPONENT_IS_EMPTY_FORMAT = "List Component[{0}] in setting is empty";
        private const string COMPONENT_NAME_NULL = "Null",
            COMPONENT_NAME_FORMAT = "{0} - {1} - Id[{2}]";
        #endregion

        #region Constructor
        public SelectComponentDrawer() : base()
        {

        }
        #endregion Constructor

        #region Unity Event		
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType.IsSubclassOf(typeof(Component)))
            {
                OnGUI(position, label, property, fieldInfo.FieldType);
                return;
            }
            if (fieldInfo.FieldType.IsArray)
            {
                Type typeElement = fieldInfo.FieldType.GetElementType();
                if (typeElement.IsSubclassOf(typeof(Component)))
                {
                    OnGUI(position, label, property, typeElement);
                    return;
                }
            }
            //
            EditorGUI.LabelField(position, label, new GUIContent(ERROR_TYPE));
        }
        private void OnGUI(Rect position, GUIContent label, SerializedProperty property, Type typeElement)
        {
            SelectComponentAttribute objectAttribute = attribute as SelectComponentAttribute;
            MonoBehaviour mono = property.serializedObject.targetObject as MonoBehaviour;
            List<Component> components = new List<Component>();
            if (objectAttribute.AllowNull)
                components.Add(null);
            GetComponentDrawer.GetComponent(mono, typeElement, objectAttribute.GetComponentType, objectAttribute.AllowInactive, components);
            if (components.Count == 0)
            {
                property.objectReferenceValue = null;
                string error = string.Format(ERROR_LIST_COMPONENT_IS_EMPTY_FORMAT, typeElement.Name);
                EditorGUI.LabelField(position, label, new GUIContent(error));
                return;
            }
            //
            List<string> listComponentName = new List<string>();
            foreach (Component component in components)
                listComponentName.Add(GetComponentName(component));
            //
            Component currentComponent;
            if (property.objectReferenceValue == null && !objectAttribute.AllowNull)
                property.objectReferenceValue = components[0];
            if (property.objectReferenceValue == null)
                currentComponent = null;
            else
                currentComponent = property.objectReferenceValue as Component;
            if (currentComponent != null && !GetComponentDrawer.Children_Check(mono, currentComponent, objectAttribute.GetComponentType, objectAttribute.AllowInactive))
            {
                property.objectReferenceValue = components[0];
                currentComponent = property.objectReferenceValue as Component;
            }
            string fileName = GetComponentName(currentComponent);
            if (EditorGui_Draw.DrawPopup_String(position, label, listComponentName.ToArray(), fileName, out int index))
            {
                property.objectReferenceValue = components[index];
            }
        }
        #endregion

        #region Methods
        private string GetComponentName(Component component)
        {
            if (component == null)
                return COMPONENT_NAME_NULL;
            return string.Format(COMPONENT_NAME_FORMAT, component.gameObject.name, component.GetType().Name, component.GetInstanceID());
        }
        #endregion
    }
}
