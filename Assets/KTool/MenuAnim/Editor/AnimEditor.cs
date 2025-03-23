using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KTool.MenuAnim.Editor
{
    public class AnimEditor
    {
        #region Properties
        private SerializedProperty propertyAnim,
            propertyName,
            propertyItems;
        private bool isShowName;
        private bool isShowItem;
        private string[] listType;
        private List<ItemEditor> itemEditors;

        public string FieldName
        {
            get
            {
                return propertyName.stringValue;
            }
            private set
            {
                propertyName.stringValue = value;
            }
        }
        #endregion Properties

        #region Construction
        public AnimEditor(SerializedProperty propertyAnim)
        {
            Init_Property(propertyAnim);
            Init_ItemEditor();
            isShowName = true;
        }
        public AnimEditor(SerializedProperty propertyAnim, string fieldName)
        {
            Init_Property(propertyAnim);
            Init_Type();
            Init_ItemEditor();
            FieldName = fieldName;
            isShowName = false;
        }
        private void Init_Property(SerializedProperty propertyAnim)
        {
            this.propertyAnim = propertyAnim;
            propertyName = propertyAnim.FindPropertyRelative("name");
            propertyItems = propertyAnim.FindPropertyRelative("items");
        }
        private void Init_Type()
        {
            Array array = Enum.GetValues(typeof(ItemType));
            listType = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
                listType[i] = array.GetValue(i).ToString();
        }
        private void Init_ItemEditor()
        {
            itemEditors = new List<ItemEditor>();
            foreach (SerializedProperty propertyItem in propertyItems)
            {
                itemEditors.Add(GetItemEditorOfType(propertyItem));
            }
        }
        #endregion

        #region Method
        public void SetActiveItem(bool isActive)
        {
            foreach (var item in itemEditors)
                item.SetActiveItem(isActive);
        }
        public void SetStateTaget()
        {
            foreach (var item in itemEditors)
                item.SetStateTaget();
        }

        private Item GetItemOfType(ItemType type)
        {
            switch (type)
            {
                case ItemType.Active:
                    return new ItemActive();
                case ItemType.Event:
                    return new ItemEvent();
                case ItemType.Move:
                    return new ItemMove();
                case ItemType.Rotate:
                    return new ItemRotate();
                case ItemType.Scale:
                    return new ItemScale();
                case ItemType.Resize:
                    return new ItemResize();
                case ItemType.Color:
                    return new ItemColor();
                case ItemType.ColorAlpha:
                    return new ItemColorAlpha();
                default:
                    return new ItemActive();
            }
        }
        private ItemEditor GetItemEditorOfType(SerializedProperty propertyItem)
        {
            if (propertyItem.managedReferenceValue is ItemActive)
                return new ItemEditorActive(propertyItem);
            if (propertyItem.managedReferenceValue is ItemEvent)
                return new ItemEditorEvent(propertyItem);
            else if (propertyItem.managedReferenceValue is ItemMove)
                return new ItemEditorMove(propertyItem);
            else if (propertyItem.managedReferenceValue is ItemRotate)
                return new ItemEditorRotate(propertyItem);
            else if (propertyItem.managedReferenceValue is ItemScale)
                return new ItemEditorScale(propertyItem);
            else if (propertyItem.managedReferenceValue is ItemResize)
                return new ItemEditorResize(propertyItem);
            else if (propertyItem.managedReferenceValue is ItemColor)
                return new ItemEditorColor(propertyItem);
            else if (propertyItem.managedReferenceValue is ItemColorAlpha)
                return new ItemEditorColorAlpha(propertyItem);
            else
                return new ItemEditorActive(propertyItem);
        }
        private ItemType GetType(int index)
        {
            switch (index)
            {
                case 0:
                    return ItemType.Active;
                case 1:
                    return ItemType.Event;
                case 2:
                    return ItemType.Move;
                case 3:
                    return ItemType.Rotate;
                case 4:
                    return ItemType.Scale;
                case 5:
                    return ItemType.Resize;
                case 6:
                    return ItemType.Color;
                case 7:
                    return ItemType.ColorAlpha;
                default:
                    return ItemType.Active;
            }
        }
        private int GetTypeIndex(SerializedProperty propertyItem)
        {
            if (propertyItem.managedReferenceValue == null)
                return -1;
            //
            if (propertyItem.managedReferenceValue is ItemActive)
                return 0;
            if (propertyItem.managedReferenceValue is ItemEvent)
                return 1;
            if (propertyItem.managedReferenceValue is ItemMove)
                return 2;
            if (propertyItem.managedReferenceValue is ItemRotate)
                return 3;
            if (propertyItem.managedReferenceValue is ItemScale)
                return 4;
            if (propertyItem.managedReferenceValue is ItemResize)
                return 5;
            if (propertyItem.managedReferenceValue is ItemColor)
                return 6;
            if (propertyItem.managedReferenceValue is ItemColorAlpha)
                return 7;
            return -1;
        }
        #endregion

        #region UnityEvent
        public void OnGui()
        {
            GUILayout.BeginVertical(FieldName, "window");
            //
            OnGui_Name();
            OnGui_Items();
            //
            GUILayout.EndVertical();
        }
        private void OnGui_Name()
        {
            if (!isShowName)
                return;
            EditorGUILayout.PropertyField(propertyName, new GUIContent("Name"));
        }
        private void OnGui_Items()
        {
            GUILayout.BeginVertical("Items", "window");
            //
            int count = propertyItems.arraySize;
            count = EditorGUILayout.IntField(new GUIContent("Count"), count);
            if (count != propertyItems.arraySize)
            {
                propertyItems.arraySize = count;
                int index = 0;
                foreach (SerializedProperty propertyItem in propertyItems)
                {
                    if (propertyItem.managedReferenceValue == null)
                    {
                        propertyItem.managedReferenceValue = GetItemOfType(ItemType.Active);
                        if (index < itemEditors.Count)
                            itemEditors[index] = GetItemEditorOfType(propertyItem);
                        else
                            itemEditors.Add(GetItemEditorOfType(propertyItem));
                    }
                    index++;
                }
            }
            //
            if (count > 0)
            {
                if (isShowItem)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Hide"))
                    {
                        isShowItem = false;
                    }
                    GUILayout.EndHorizontal();
                    //
                    int index = 0;
                    foreach (SerializedProperty propertyItem in propertyItems)
                    {
                        OnGui_Item(propertyItem, index);
                        index++;
                    }
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Show"))
                    {
                        isShowItem = true;
                    }
                    GUILayout.EndHorizontal();
                }
            }
            //
            GUILayout.EndVertical();
        }
        private void OnGui_Item(SerializedProperty propertyItem, int index)
        {
            string title = string.Format("Item: {0}", index);
            GUILayout.BeginVertical(title, "window");
            //
            OnGui_Type(propertyItem, index);
            itemEditors[index].OnGui();
            //
            GUILayout.EndVertical();
        }
        private void OnGui_Type(SerializedProperty propertyItem, int index)
        {
            int indexType = GetTypeIndex(propertyItem);
            int newIndexType;
            if (indexType == -1)
                newIndexType = 0;
            else
                newIndexType = EditorGUILayout.Popup(new GUIContent("Type"), indexType, listType);
            if (newIndexType != indexType)
            {
                SerializedProperty propertyDelay = propertyItem.FindPropertyRelative("delay");
                float oldDelay = (propertyDelay == null ? 0 : propertyDelay.floatValue);
                //
                ItemType type = GetType(newIndexType);
                propertyItem.managedReferenceValue = GetItemOfType(type);
                itemEditors[index] = GetItemEditorOfType(propertyItem);
                //
                propertyItem.FindPropertyRelative("delay").floatValue = oldDelay;
            }
        }
        #endregion UnityEvent
    }
}
