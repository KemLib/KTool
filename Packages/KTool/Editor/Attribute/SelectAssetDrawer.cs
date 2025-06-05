using KTool.FileIo;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(SelectAssetAttribute))]
    public class SelectAssetDrawer : PropertyDrawer
    {
        #region Properties
        public const string ERROR_TYPE = "type of property not is UnityEngine.Object or UnityEngine.Object[]",
            ERROR_FOLDER_NOT_FOUND = "Folder not found";
        private const string FILE_NAME_NULL = "<Null>";
        public const string FORMAT_PATH_ASSET = "{0}/{1}.{2}";
        #endregion

        #region Constructor
        public SelectAssetDrawer() : base()
        {

        }
        #endregion Constructor

        #region Unity Event		
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SelectAssetAttribute objectAttribute = attribute as SelectAssetAttribute;
            if (!AssetFinder.Exists(objectAttribute.Folder))
            {
                EditorGUI.LabelField(position, label, new GUIContent(ERROR_FOLDER_NOT_FOUND));
                return;
            }
            if (fieldInfo.FieldType == typeof(string) || fieldInfo.FieldType == typeof(string[]))
            {
                List<string> files = AssetFinder.GetAllFile(objectAttribute.Folder, objectAttribute.Extension, false);
                if (objectAttribute.AllowNull)
                {
                    files.Insert(0, FILE_NAME_NULL);
                    if (string.IsNullOrEmpty(property.stringValue))
                        property.stringValue = FILE_NAME_NULL;
                }
                EditorGui_Draw.DrawPopup_String(position, label, files.ToArray(), property, out int index);
                if (objectAttribute.AllowNull && index == 0)
                    property.stringValue = string.Empty;
                return;
            }
            if (fieldInfo.FieldType.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                OnGUI(objectAttribute, position, label, property, fieldInfo.FieldType);
                return;
            }
            if (fieldInfo.FieldType.IsArray)
            {
                Type typeElement = fieldInfo.FieldType.GetElementType();
                if (typeElement.IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    OnGUI(objectAttribute, position, label, property, typeElement);
                    return;
                }
            }
            //
            EditorGUI.LabelField(position, label, new GUIContent(ERROR_TYPE));
        }
        private void OnGUI(SelectAssetAttribute objectAttribute, Rect position, GUIContent label, SerializedProperty property, Type typeElement)
        {
            List<string> tagetFiles = AssetFinder.GetAllFile(objectAttribute.Folder, objectAttribute.Extension, false);
            int indexTagetFile = 0;
            while (indexTagetFile < tagetFiles.Count)
            {
                UnityEngine.Object obj = Asset_GetObject(objectAttribute.Folder, tagetFiles[indexTagetFile], objectAttribute.Extension, typeElement);
                if (obj == null)
                    tagetFiles.RemoveAt(indexTagetFile);
                else
                    indexTagetFile++;
            }
            if (objectAttribute.AllowNull)
                tagetFiles.Insert(0, FILE_NAME_NULL);
            //
            string fileName;
            if (objectAttribute.AllowNull && property.objectReferenceValue == null)
                fileName = FILE_NAME_NULL;
            else
                fileName = Asset_GetFileName(property, tagetFiles, objectAttribute.Folder, objectAttribute.Extension);
            if (EditorGui_Draw.DrawPopup_String(position, label, tagetFiles.ToArray(), fileName, out int index))
            {
                if (objectAttribute.AllowNull && index == 0)
                    property.objectReferenceValue = null;
                else if (index >= 0)
                    property.objectReferenceValue = Asset_GetObject(objectAttribute.Folder, tagetFiles[index], objectAttribute.Extension, typeElement);
                else
                    property.objectReferenceValue = null;
            }
        }
        #endregion

        #region Method
        public static string Asset_GetFileName(SerializedProperty property, List<string> files, string folderName, string extension)
        {
            UnityEngine.Object asset = property.objectReferenceValue;
            if (asset == null)
                return string.Empty;
            string aseetPath = Asset_GetPath(folderName, asset.name, extension);
            foreach (string file in files)
            {
                if (Asset_GetPath(folderName, file, extension) == aseetPath)
                    return file;
            }
            return string.Empty;
        }
        public static UnityEngine.Object Asset_GetObject(string folderName, string fileName, string extension, Type assetType)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;
            string assetPath = Asset_GetPath(folderName, fileName, extension);
            return AssetDatabase.LoadAssetAtPath(assetPath, assetType);
        }
        public static string Asset_GetPath(string folderName, string fileName, string extension)
        {
            return string.Format(FORMAT_PATH_ASSET, folderName, fileName, extension);
        }
        #endregion
    }
}
