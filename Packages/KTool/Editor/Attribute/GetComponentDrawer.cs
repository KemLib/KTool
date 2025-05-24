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
                if (Children_Check(mono, component, objectAttribute.GetComponentType, objectAttribute.AllowInactive))
                {
                    EditorGUI.PropertyField(position, property, label);
                    return;
                }
            }
            //
            property.objectReferenceValue = GetComponent(mono, fieldInfo.FieldType, objectAttribute.GetComponentType, objectAttribute.AllowInactive);
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
            GetComponent(mono, typeElement, objectAttribute.GetComponentType, objectAttribute.AllowInactive, components);
            //
            SerializedProperty propertyRoot = EditorGui_Draw.PropertyElement_GetPropertyRoot(property);
            EditorGui_Draw.ArrayAsync_Component(components, propertyRoot);
            if (propertyRoot.arraySize == 0)
                return false;
            return true;
        }
        #endregion

        #region GetComponent
        public static Component GetComponent(MonoBehaviour mono, Type typeElement, GetComponentType getComponentType, bool allowInactive)
        {
            Component component;
            if (IsInGameObject(getComponentType))
            {
                component = mono.GetComponent(typeElement);
                if (component != null)
                    return component;
            }
            if (IsInChildren(getComponentType))
                component = GetComponent_InChildren(mono, typeElement, getComponentType, allowInactive);
            else
                component = null;
            return component;
        }
        private static Component GetComponent_InChildren(MonoBehaviour mono, Type typeElement, GetComponentType getComponentType, bool allowInactive)
        {
            int childCount = mono.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = mono.transform.GetChild(i);
                if (!allowInactive && !child.gameObject.activeSelf)
                    continue;
                Component component = GetComponent_InChildren(child, typeElement, getComponentType, allowInactive);
                if (component != null)
                    return component;
            }
            return null;
        }
        private static Component GetComponent_InChildren(Transform transform, Type typeElement, GetComponentType getComponentType, bool allowInactive)
        {
            Component component = transform.GetComponent(typeElement);
            if (component != null)
                return component;
            if (!IsInAllChildren(getComponentType))
                return null;
            //
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (!allowInactive && !child.gameObject.activeSelf)
                    continue;
                component = GetComponent_InChildren(child, typeElement, getComponentType, allowInactive);
                if (component != null)
                    return component;
            }
            return null;
        }

        public static void GetComponent(MonoBehaviour mono, Type typeElement, GetComponentType getComponentType, bool includeInactive, List<Component> components)
        {
            List<Component> tmps = new List<Component>();
            if (IsInGameObject(getComponentType))
            {
                mono.GetComponents(typeElement, tmps);
                if (tmps.Count > 0)
                    components.AddRange(tmps);
            }
            //
            if (IsInChildren(getComponentType))
                GetComponent_InChildren(mono, typeElement, getComponentType, includeInactive, components, tmps);
        }
        private static void GetComponent_InChildren(MonoBehaviour mono, Type typeElement, GetComponentType getComponentType, bool includeInactive, List<Component> components, List<Component> tmps)
        {
            int childCount = mono.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = mono.transform.GetChild(i);
                if (!includeInactive && !child.gameObject.activeSelf)
                    continue;
                GetComponent_InChildren(child, typeElement, getComponentType, includeInactive, components, tmps);
            }
        }
        private static void GetComponent_InChildren(Transform transform, Type typeElement, GetComponentType getComponentType, bool includeInactive, List<Component> components, List<Component> tmps)
        {
            transform.GetComponents(typeElement, tmps);
            if (tmps.Count > 0)
                components.AddRange(tmps);
            if (!IsInAllChildren(getComponentType))
                return;
            //
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (!includeInactive && !child.gameObject.activeSelf)
                    continue;
                GetComponent_InChildren(child, typeElement, getComponentType, includeInactive, components, tmps);
            }
        }
        private static bool IsInGameObject(GetComponentType getComponentType)
        {
            return getComponentType == GetComponentType.InGameObject || getComponentType == GetComponentType.InGameObject_Children || getComponentType == GetComponentType.InGameObject_AllChildren;
        }
        private static bool IsInChildren(GetComponentType getComponentType)
        {
            return getComponentType == GetComponentType.InChildren || getComponentType == GetComponentType.InAllChildren || getComponentType == GetComponentType.InGameObject_Children || getComponentType == GetComponentType.InGameObject_AllChildren;
        }
        private static bool IsInAllChildren(GetComponentType getComponentType)
        {
            return getComponentType == GetComponentType.InAllChildren || getComponentType == GetComponentType.InGameObject_AllChildren;
        }

        public static bool Children_Check(MonoBehaviour mono, Component component, GetComponentType getComponentType, bool allowInactive)
        {
            if (IsInGameObject(getComponentType) && mono.gameObject.GetInstanceID() == component.gameObject.GetInstanceID())
                return true;
            if (IsInChildren(getComponentType))
                return Children_Check(mono, component.transform, getComponentType, allowInactive);
            return false;
        }
        private static bool Children_Check(MonoBehaviour mono, Transform child, GetComponentType getComponentType, bool allowInactive)
        {
            if (!allowInactive && !child.gameObject.activeSelf)
                return false;
            Transform childParent = child.parent;
            if (childParent == null)
                return false;
            if (childParent.GetInstanceID() == mono.transform.GetInstanceID())
                return true;
            if (IsInAllChildren(getComponentType))
                return Children_Check(mono, childParent, getComponentType, allowInactive);
            return false;
        }
        #endregion
    }
}
