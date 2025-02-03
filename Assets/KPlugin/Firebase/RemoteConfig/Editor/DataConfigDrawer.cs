using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using KTool.Attribute.Editor;

namespace KPlugin.Firebase.RemoteConfig.Editor
{
    [CustomPropertyDrawer(typeof(DataConfigAttribute))]
    public class DataConfigDrawer : PropertyDrawer
    {
        #region Properties
        private bool isShow;
        private List<bool> listIsShow;
        #endregion Properties

        #region Constructor
        public DataConfigDrawer() : base()
        {
            isShow = false;
            listIsShow = new List<bool>();
        }

        #endregion Constructor

        #region Unity Event		
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType == typeof(DataConfig))
            {
                OnGUI_Title(position, label, ref isShow);
                if (isShow)
                    OnGui_Properties(property);
                return;
            }
            if (fieldInfo.FieldType == typeof(DataConfig[]))
            {
                ListIsShow_Async(property);
                if (ListIsShow_Get(position, label, property))
                    OnGui_Properties(property);
                return;
            }
            EditorGUI.LabelField(position, label, new GUIContent("type of property not is Int"));
        }

        private void OnGUI_Title(Rect position, GUIContent label, ref bool isShow)
        {
            Rect rectLable = new Rect(position.x, position.y, position.width - 50, position.height);
            Rect rectButton = new Rect(Mathf.Max(position.x, position.x + position.width - 50), position.y, 50, position.height);
            GUILayout.BeginHorizontal();
            GUI.Label(rectLable, label);
            if (isShow)
            {
                if (GUI.Button(rectButton, "Hide"))
                    isShow = false;
            }
            else
            {
                if (GUI.Button(rectButton, "Show"))
                    isShow = true;
            }
            GUILayout.EndHorizontal();
        }
        private void OnGui_Properties(SerializedProperty property)
        {
            SerializedProperty propertyKey = property.FindPropertyRelative("key"),
                propertyDataType = property.FindPropertyRelative("dataType");
            EditorGUILayout.PropertyField(propertyKey, new GUIContent("Key"));
            EditorGUILayout.PropertyField(propertyDataType, new GUIContent("Data Type"));
            DataType dataType = (DataType)propertyDataType.enumValueIndex;
            switch (dataType)
            {
                case DataType.String:
                    SerializedProperty propertyValueString = property.FindPropertyRelative("valueString");
                    EditorGUILayout.PropertyField(propertyValueString, new GUIContent("Value String"));
                    break;
                case DataType.Long:
                    SerializedProperty propertyValueLong = property.FindPropertyRelative("valueLong");
                    EditorGUILayout.PropertyField(propertyValueLong, new GUIContent("Value Long"));
                    break;
                case DataType.Double:
                    SerializedProperty propertyValueDouble = property.FindPropertyRelative("valueDouble");
                    EditorGUILayout.PropertyField(propertyValueDouble, new GUIContent("Value Double"));
                    break;
                case DataType.Boolean:
                    SerializedProperty propertyValueBoolean = property.FindPropertyRelative("valueBoolean");
                    EditorGUILayout.PropertyField(propertyValueBoolean, new GUIContent("Value Boolean"));
                    break;
                case DataType.Json:
                    SerializedProperty propertyValueJson = property.FindPropertyRelative("valueJson");
                    EditorGUILayout.PropertyField(propertyValueJson, new GUIContent("Value Json"));
                    break;
            }
        }
        #endregion Unity Event

        #region Method
        private void ListIsShow_Async(SerializedProperty property)
        {
            SerializedProperty rootproperty = EditorGui_Draw.PropertyElement_GetPropertyRoot(property);
            int length = rootproperty.arraySize;
            while (listIsShow.Count > length)
                listIsShow.RemoveAt(listIsShow.Count - 1);
            while (listIsShow.Count < length)
                listIsShow.Add(false);
        }
        private bool ListIsShow_Get(Rect position, GUIContent label, SerializedProperty property)
        {
            int index = EditorGui_Draw.PropertyElement_IndexOf(property);
            bool isShow = listIsShow[index];
            OnGUI_Title(position, label, ref isShow);
            listIsShow[index] = isShow;
            return isShow;
        }
        #endregion Method
    }
}
