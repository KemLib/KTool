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
            //
            EditorGUI.LabelField(position, label, new GUIContent(ERROR_TYPE));
        }

        private void OnGui_Field(Rect position, GUIContent label, SerializedProperty property)
        {
            GetComponentAttribute objectAttribute = attribute as GetComponentAttribute;
            MonoBehaviour mono = property.serializedObject.targetObject as MonoBehaviour;
            if (property.objectReferenceValue != null)
            {
                Component component = property.objectReferenceValue as Component;
                if (Children_Check(mono, component.transform, objectAttribute.GetComponentType, objectAttribute.IncludeInactive))
                {
                    EditorGUI.PropertyField(position, property, label);
                    return;
                }
            }
            //
            property.objectReferenceValue = GetComponent(mono, fieldInfo.FieldType, objectAttribute.GetComponentType, objectAttribute.IncludeInactive);
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
            GetComponentAttribute objectAttribute = attribute as GetComponentAttribute;
            MonoBehaviour mono = property.serializedObject.targetObject as MonoBehaviour;
            List<Component> components = new List<Component>();
            GetComponent(mono, typeElement, objectAttribute.GetComponentType, objectAttribute.IncludeInactive, components);
            //
            SerializedProperty propertyRoot = EditorGui_Draw.PropertyElement_GetPropertyRoot(property);
            EditorGui_Draw.ArrayAsync_Component(components, propertyRoot);
            if (propertyRoot.arraySize == 0)
                return false;
            return true;
        }
        #endregion

        #region Children
        public static Component GetComponent(MonoBehaviour mono, Type typeElement, GetComponentType getComponentType, bool includeInactive)
        {
            if (getComponentType == GetComponentType.InGameObject)
                return mono.GetComponent(typeElement);
            //
            return GetComponent_InChildren(mono, typeElement, getComponentType, includeInactive);
        }
        public static Component GetComponent_InChildren(MonoBehaviour mono, Type typeElement, GetComponentType getComponentType, bool includeInactive)
        {
            int childCount = mono.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = mono.transform.GetChild(i);
                if (!includeInactive && !child.gameObject.activeSelf)
                    continue;
                Component component = GetComponent_InChildren(child, typeElement, getComponentType, includeInactive);
                if (component != null)
                    return component;
            }
            return null;
        }
        public static Component GetComponent_InChildren(Transform transform, Type typeElement, GetComponentType getComponentType, bool includeInactive)
        {
            Component component = transform.GetComponent(typeElement);
            if (component != null)
                return component;
            //
            if (getComponentType != GetComponentType.InAllChildren)
                return null;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (!includeInactive && !child.gameObject.activeSelf)
                    continue;
                component = GetComponent_InChildren(child, typeElement, getComponentType, includeInactive);
                if (component != null)
                    return component;
            }
            return null;
        }
        public static void GetComponent(MonoBehaviour mono, Type typeElement, GetComponentType getComponentType, bool includeInactive, List<Component> components)
        {
            if (getComponentType == GetComponentType.InGameObject)
            {
                mono.GetComponents(typeElement, components);
                return;
            }
            //
            GetComponent_InChildren(mono, typeElement, getComponentType, includeInactive, components);
        }
        public static void GetComponent_InChildren(MonoBehaviour mono, Type typeElement, GetComponentType getComponentType, bool includeInactive, List<Component> components)
        {
            int childCount = mono.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = mono.transform.GetChild(i);
                if (!includeInactive && !child.gameObject.activeSelf)
                    continue;
                GetComponent_InChildren(child, typeElement, getComponentType, includeInactive, components);
            }
        }
        public static void GetComponent_InChildren(Transform transform, Type typeElement, GetComponentType getComponentType, bool includeInactive, List<Component> components)
        {
            List<Component> tmp = new List<Component>();
            transform.GetComponents(typeElement, tmp);
            if (tmp.Count > 0)
                components.AddRange(tmp);
            //
            if (getComponentType != GetComponentType.InAllChildren)
                return;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (!includeInactive && !child.gameObject.activeSelf)
                    continue;
                GetComponent_InChildren(child, typeElement, getComponentType, includeInactive, components);
            }
        }
        public static bool Children_Check(MonoBehaviour mono, Transform child, GetComponentType getComponentType, bool includeInactive)
        {
            if (getComponentType == GetComponentType.InGameObject)
                return mono.gameObject.GetInstanceID() == child.gameObject.GetInstanceID();
            //
            if (!includeInactive && !child.gameObject.activeSelf)
                return false;
            Transform childParent = child.parent;
            if (childParent == null)
                return false;
            if (childParent.GetInstanceID() == mono.transform.GetInstanceID())
                return true;
            if (getComponentType == GetComponentType.InAllChildren)
                return Children_Check(mono, childParent, getComponentType, includeInactive);
            return false;
        }
        #endregion
    }
}
