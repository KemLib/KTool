using KTool.FileIo;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(SelectAssetInFolderAttribute))]
    public class SelectAssetInFolderDrawer : PropertyDrawer
    {
        #region Properties
        public const string ERROR_TYPE = "type of property not is UnityEngine.Object or UnityEngine.Object[]";
        public const string FORMAT_PATH_ASSET = "{0}/{1}.{2}";
        #endregion

        #region Constructor
        public SelectAssetInFolderDrawer() : base()
        {

        }
        #endregion Constructor

        #region Unity Event		
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                OnGUI(position, label, property, fieldInfo.FieldType);
                return;
            }
            if (fieldInfo.FieldType.IsArray)
            {
                Type typeElement = fieldInfo.FieldType.GetElementType();
                if (typeElement.IsSubclassOf(typeof(UnityEngine.Object)))
                    OnGUI(position, label, property, typeElement);
                return;
            }
            EditorGUI.LabelField(position, label, new GUIContent(ERROR_TYPE));
        }
        private void OnGUI(Rect position, GUIContent label, SerializedProperty property, Type typeElement)
        {
            SelectAssetInFolderAttribute objectAttribute = attribute as SelectAssetInFolderAttribute;
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
            if (EditorGui_Draw.DrawPopup_String(position, label, tagetFiles.ToArray(), ref fileName))
                property.objectReferenceValue = Asset_GetObject(objectAttribute.Folder, fileName, objectAttribute.Extension, typeElement);
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
