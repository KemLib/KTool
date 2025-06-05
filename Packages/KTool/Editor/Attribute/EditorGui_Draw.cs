using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    public static class EditorGui_Draw
    {
        #region Properties
        public const string ERROR_OPTION_EMPTY = "No items to select";
        private const string PROPERTIES_ARRAY = "Array";
        #endregion

        #region Unity Event
        public static void DrawPopup_String(Rect position, GUIContent label, string[] options, SerializedProperty property, out int index)
        {
            if (options.Length == 0)
            {
                property.stringValue = string.Empty;
                EditorGUI.LabelField(position, label, new GUIContent(ERROR_OPTION_EMPTY));
                index = -1;
                return;
            }
            //
            index = ArrayString_IndexOf(property.stringValue, options);
            if (index < 0)
            {
                index = 0;
                property.stringValue = options[index];
            }
            //
            int newIndex = EditorGUI.Popup(position, label.text, index, options);
            if (newIndex != index)
            {
                index = newIndex;
                property.stringValue = options[index];
            }
        }
        public static bool DrawPopup_String(Rect position, GUIContent label, string[] options, string value, out int index)
        {
            if (options.Length == 0)
            {
                EditorGUI.LabelField(position, label, new GUIContent(ERROR_OPTION_EMPTY));
                index = -1;
                return true;
            }
            //
            bool isChange = false;
            index = ArrayString_IndexOf(value, options);
            if (index < 0)
            {
                index = 0;
                isChange = true;
            }
            //
            int newIndex = EditorGUI.Popup(position, label.text, index, options);
            if (newIndex != index)
            {
                index = newIndex;
                isChange = true;
            }
            return isChange;
        }
        public static void DrawPopup_Int(Rect position, GUIContent label, string[] options, SerializedProperty property)
        {
            if (options.Length == 0)
            {
                property.intValue = 0;
                EditorGUI.LabelField(position, label, new GUIContent(ERROR_OPTION_EMPTY));
                return;
            }
            //
            int index = property.intValue;
            if (index < 0)
            {
                index = 0;
                property.intValue = index;
            }
            else if (index >= options.Length)
            {
                index = options.Length - 1;
                property.intValue = index;
            }
            //
            int newIndex = EditorGUI.Popup(position, label.text, index, options);
            if (newIndex != index)
            {
                index = newIndex;
                property.intValue = index;
            }
        }
        public static bool DrawPopup_Int(Rect position, GUIContent label, string[] options, ref int index)
        {
            if (options.Length == 0)
            {
                index = 0;
                EditorGUI.LabelField(position, label, new GUIContent(ERROR_OPTION_EMPTY));
                return true;
            }
            //
            bool isChange = false;
            if (index < 0)
            {
                index = 0;
                isChange = true;
            }
            else if (index >= options.Length)
            {
                index = options.Length - 1;
                isChange = true;
            }
            //
            int newIndex = EditorGUI.Popup(position, label.text, index, options);
            if (newIndex != index)
            {
                index = newIndex;
                isChange = true;
            }
            return isChange;
        }
        #endregion

        #region Method
        public static void ArrayAsync_Component(List<Component> tagetComponents, SerializedProperty originProperty)
        {
            if (originProperty.arraySize != tagetComponents.Count)
                originProperty.arraySize = tagetComponents.Count;
            //
            foreach (SerializedProperty propertyItem in originProperty)
            {
                if (propertyItem.objectReferenceValue == null)
                    continue;
                Component component = propertyItem.objectReferenceValue as Component;
                int index = ArrayComponent_IndexOf(component, tagetComponents);
                if (index < 0)
                    propertyItem.objectReferenceValue = null;
                else
                    tagetComponents.RemoveAt(index);
            }
            //
            if (tagetComponents.Count == 0)
                return;
            foreach (SerializedProperty propertyItem in originProperty)
            {
                if (propertyItem.objectReferenceValue == null)
                {
                    propertyItem.objectReferenceValue = tagetComponents[0];
                    tagetComponents.RemoveAt(0);
                }
            }
        }
        public static void ArrayAsync_String(List<string> tagetStrings, SerializedProperty originProperty)
        {
            if (originProperty.arraySize != tagetStrings.Count)
                originProperty.arraySize = tagetStrings.Count;
            //
            foreach (SerializedProperty propertyItem in originProperty)
            {
                if (string.IsNullOrEmpty(propertyItem.stringValue))
                    continue;
                string value = propertyItem.stringValue;
                int index = ArrayString_IndexOf(value, tagetStrings);
                if (index < 0)
                    propertyItem.stringValue = string.Empty;
                else
                    tagetStrings.RemoveAt(index);
            }
            //
            if (tagetStrings.Count == 0)
                return;
            foreach (SerializedProperty propertyItem in originProperty)
            {
                if (string.IsNullOrEmpty(propertyItem.stringValue))
                {
                    propertyItem.stringValue = tagetStrings[0];
                    tagetStrings.RemoveAt(0);
                }
            }
        }
        public static void ArrayAsync_String(List<string> tagetStrings, List<string> originStrings)
        {
            while (originStrings.Count > tagetStrings.Count)
                originStrings.RemoveAt(0);
            //
            int index = 0;
            while (index < originStrings.Count)
            {
                if (tagetStrings.Contains(originStrings[index]))
                {
                    index++;
                    continue;
                }
                originStrings.RemoveAt(index);
            }
            //
            index = 0;
            while (index < tagetStrings.Count)
            {
                if (!originStrings.Contains(tagetStrings[index]))
                {
                    originStrings.Add(tagetStrings[index]);
                }
                index++;
            }
        }
        private static int ArrayString_IndexOf(string value, string[] array)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i] == value)
                    return i;
            return -1;
        }
        private static int ArrayString_IndexOf(string value, List<string> array)
        {
            int index = 0;
            foreach (string var in array)
            {
                if (var == value)
                    return index;
                index++;
            }
            return -1;
        }
        private static int ArrayComponent_IndexOf(Component component, Component[] components)
        {
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].GetInstanceID() == component.GetInstanceID())
                    return i;
            }
            return -1;
        }
        private static int ArrayComponent_IndexOf(Component component, List<Component> components)
        {
            int index = 0;
            foreach (Component componentItem in components)
            {
                if (componentItem.GetInstanceID() == component.GetInstanceID())
                    return index;
                index++;
            }
            return -1;
        }
        #endregion

        #region PropertyElement
        public static int PropertyElement_IndexOf(SerializedProperty property)
        {
            string path = property.propertyPath;
            string intdexString = path.Substring(path.Length - 2, 1);
            if (int.TryParse(intdexString, out int index))
                return index;
            return -1;
        }
        public static SerializedProperty PropertyElement_GetPropertyRoot(SerializedProperty property)
        {
            string path = property.propertyPath;
            int indexOfArray = path.LastIndexOf(PROPERTIES_ARRAY);
            string tmp = path.Substring(0, indexOfArray - 1);
            return property.serializedObject.FindProperty(tmp);
        }
        #endregion
    }
}
