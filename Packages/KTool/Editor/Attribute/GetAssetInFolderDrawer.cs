using KTool.FileIo;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(GetAssetInFolderAttribute))]
    public class GetAssetInFolderDrawer : PropertyDrawer
    {
        #region Properties
        public const string ERROR_TYPE = "type of property not is UnityEngine.Object[]";
        #endregion

        #region Constructor
        public GetAssetInFolderDrawer() : base()
        {

        }
        #endregion Constructor

        #region Unity Event		
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType.IsArray)
            {
                Type typeElement = fieldInfo.FieldType.GetElementType();
                if (typeElement.IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    OnGui_Fields(position, label, property, typeElement);
                    return;
                }
            }
            EditorGUI.LabelField(position, label, new GUIContent(ERROR_TYPE));
        }
        private void OnGui_Fields(Rect position, GUIContent label, SerializedProperty property, Type typeElement)
        {
            int indexElement = EditorGui_Draw.PropertyElement_IndexOf(property);
            if (indexElement == 0)
                Reload_PropertyRoot(property, typeElement);
            EditorGUI.PropertyField(position, property, label);
        }
        #endregion

        #region Method
        private void Reload_PropertyRoot(SerializedProperty property, Type typeElement)
        {
            GetAssetInFolderAttribute objectAttribute = attribute as GetAssetInFolderAttribute;
            string fodler = objectAttribute.Folder,
                extension = objectAttribute.Extension;
            //
            SerializedProperty propertyRoot = EditorGui_Draw.PropertyElement_GetPropertyRoot(property);
            List<string> tagetFiles = AssetFinder.GetAllFile(fodler, extension, false);
            int indexTagetFile = 0;
            while (indexTagetFile < tagetFiles.Count)
            {
                UnityEngine.Object obj = SelectAssetInFolderDrawer.Asset_GetObject(fodler, tagetFiles[indexTagetFile], extension, typeElement);
                if (obj == null)
                    tagetFiles.RemoveAt(indexTagetFile);
                else
                    indexTagetFile++;
            }
            //
            List<string> originFile = new List<string>();
            foreach (SerializedProperty propertyItem in propertyRoot)
            {
                string fileName = SelectAssetInFolderDrawer.Asset_GetFileName(propertyItem, tagetFiles, fodler, extension);
                if (!originFile.Contains(fileName))
                    originFile.Add(fileName);
            }
            EditorGui_Draw.ArrayAsync_String(tagetFiles, originFile);
            //
            propertyRoot.arraySize = originFile.Count;
            int index = 0;
            foreach (SerializedProperty propertyItem in propertyRoot)
            {
                propertyItem.objectReferenceValue = SelectAssetInFolderDrawer.Asset_GetObject(fodler, originFile[index], extension, typeElement);
                index++;
            }

        }
        #endregion
    }
}
