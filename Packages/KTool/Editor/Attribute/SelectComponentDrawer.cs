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
        private const string COMPONENT_NAME_FORMAT = "{0} - Id[{1}]",
            COMPONENT_NAME_IN_CHILD_FORMAT = "{0} - {1} - Id[{2}]";
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
            GetComponentDrawer.GetComponent(mono, typeElement, objectAttribute.GetComponentType, objectAttribute.IncludeInactive, components);
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
                listComponentName.Add(GetComponentName(component, objectAttribute.GetComponentType));
            //
            if (property.objectReferenceValue == null)
                property.objectReferenceValue = components[0];
            Component currentComponent = property.objectReferenceValue as Component;
            string fileName = GetComponentName(currentComponent, objectAttribute.GetComponentType);
            if (EditorGui_Draw.DrawPopup_String(position, label, listComponentName.ToArray(), ref fileName))
            {
                for (int i = 0; i < components.Count; i++)
                    if (listComponentName[i] == fileName)
                    {
                        property.objectReferenceValue = components[i];
                        break;
                    }
            }
        }
        #endregion

        #region Methods
        private string GetComponentName(Component component, GetComponentType getComponentType)
        {
            if (getComponentType == GetComponentType.ThisGameObject)
                return string.Format(COMPONENT_NAME_FORMAT, component.GetType().Name, component.GetInstanceID());
            //
            return string.Format(COMPONENT_NAME_IN_CHILD_FORMAT, component.gameObject.name, component.GetType().Name, component.GetInstanceID());
        }
        #endregion
    }
}
