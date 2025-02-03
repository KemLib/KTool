using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(GetComponentInChildrenAttribute))]
    public class GetComponentInChildrenDrawer : PropertyDrawer
    {
        #region Properties
        #endregion Properties

        #region Constructor
        public GetComponentInChildrenDrawer() : base()
        {
        }
        #endregion Constructor

        #region Unity Event		
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType.IsSubclassOf(typeof(Component)))
            {
                GetComponentInChildrenAttribute objectAttribute = attribute as GetComponentInChildrenAttribute;
                OnGui_Field(position, label, property, objectAttribute.GetInTree);
                return;
            }
            if (fieldInfo.FieldType.IsArray)
            {
                Type typeElement = fieldInfo.FieldType.GetElementType();
                if (typeElement.IsSubclassOf(typeof(Component)))
                {
                    GetComponentInChildrenAttribute objectAttribute = attribute as GetComponentInChildrenAttribute;
                    OnGui_Fields(position, label, property, typeElement, objectAttribute.GetInTree);
                    return;
                }
            }
            EditorGUI.LabelField(position, label, new GUIContent(GetComponentDrawer.ERROR_TYPE));
        }

        private void OnGui_Field(Rect position, GUIContent label, SerializedProperty property, bool getInTree)
        {
            MonoBehaviour mono = property.serializedObject.targetObject as MonoBehaviour;
            Component component;
            if (property.objectReferenceValue != null)
            {
                component = property.objectReferenceValue as Component;
                if (Children_Check(component.gameObject.transform, mono.transform, getInTree))
                {
                    EditorGUI.PropertyField(position, property, label);
                    return;
                }
            }
            //
            component = Children_GetComponent(mono, fieldInfo.FieldType, getInTree);
            //
            property.objectReferenceValue = component;
            EditorGUI.PropertyField(position, property, label);
        }
        private void OnGui_Fields(Rect position, GUIContent label, SerializedProperty property, Type typeElement, bool getInTree)
        {
            int indexElement = EditorGui_Draw.PropertyElement_IndexOf(property);
            if (indexElement == 0)
                Reload_PropertyRoot(property, typeElement, getInTree);
            EditorGUI.PropertyField(position, property, label);
        }
        #endregion Unity Event

        #region Method
        private void Reload_PropertyRoot(SerializedProperty property, Type typeElement, bool getInTree)
        {
            List<Component> components = new List<Component>();
            MonoBehaviour mono = property.serializedObject.targetObject as MonoBehaviour;
            Children_GetComponents(mono, typeElement, getInTree, components);
            //
            SerializedProperty propertyRoot = EditorGui_Draw.PropertyElement_GetPropertyRoot(property);
            EditorGui_Draw.ArrayAsync_Component(components, propertyRoot);
        }
        #endregion

        #region Children
        public static Component Children_GetComponent(MonoBehaviour mono, Type type, bool getInTree)
        {
            int childCount = mono.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = mono.transform.GetChild(i);
                Component component = Children_GetComponent(child, type, getInTree);
                if (component != null)
                    return component;
            }
            return null;
        }
        public static Component Children_GetComponent(Transform transform, Type type, bool getInTree)
        {
            Component component = transform.GetComponent(type);
            if (component != null)
                return component;
            //
            if (!getInTree)
                return null;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                component = Children_GetComponent(child, type, getInTree);
                if (component != null)
                    return component;
            }
            return null;
        }
        public static void Children_GetComponents(MonoBehaviour mono, Type type, bool getInTree, List<Component> components)
        {
            int childCount = mono.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = mono.transform.GetChild(i);
                Children_GetComponents(child, type, getInTree, components);
            }
        }
        public static void Children_GetComponents(Transform transform, Type type, bool getInTree, List<Component> components)
        {
            List<Component> tmp = new List<Component>();
            transform.GetComponents(type, tmp);
            if (tmp.Count > 0)
                components.AddRange(tmp);
            //
            if (!getInTree)
                return;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Children_GetComponents(child, type, getInTree, components);
            }
        }
        public static bool Children_Check(Transform child, Transform parent, bool getInTree)
        {
            Transform childParent = child.parent;
            if (childParent == null)
                return false;
            if (childParent.GetInstanceID() == parent.GetInstanceID())
                return true;
            if (getInTree)
                return Children_Check(childParent, parent, getInTree);
            return false;
        }
        #endregion
    }
}
