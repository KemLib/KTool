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
                EditorGui_Draw.DrawPopup_String(position, label, files.ToArray(), property);
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
            //
            string fileName = Asset_GetFileName(property, tagetFiles, objectAttribute.Folder, objectAttribute.Extension);
            if (EditorGui_Draw.DrawPopup_String(position, label, tagetFiles.ToArray(), fileName, out int index))
            {
                fileName = tagetFiles[index];
                property.objectReferenceValue = Asset_GetObject(objectAttribute.Folder, fileName, objectAttribute.Extension, typeElement);
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
