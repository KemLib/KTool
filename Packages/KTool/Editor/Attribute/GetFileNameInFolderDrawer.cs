using KTool.FileIo;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(GetFileNameInFolderAttribute))]
    public class GetFileNameInFolderDrawer : PropertyDrawer
    {
        #region Properties
        public const string ERROR_TYPE = "type of property not is string[]",
            ERROR_FOLDER_NOT_FOUND = "Folder not found: {0}";
        #endregion

        #region Constructor
        public GetFileNameInFolderDrawer() : base()
        {

        }
        #endregion Constructor

        #region Unity Event		
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType == typeof(string[]))
            {
                OnGui_Fields(position, label, property);
                return;
            }
            EditorGUI.LabelField(position, label, new GUIContent(ERROR_TYPE));
        }
        private void OnGui_Fields(Rect position, GUIContent label, SerializedProperty property)
        {
            int indexElement = EditorGui_Draw.PropertyElement_IndexOf(property);
            if (indexElement == 0)
            {
                GetFileNameInFolderAttribute objectAttribute = attribute as GetFileNameInFolderAttribute;
                string folder = objectAttribute.Folder,
                    extension = objectAttribute.Extension;
                SerializedProperty propertyRoot = EditorGui_Draw.PropertyElement_GetPropertyRoot(property);
                if (!AssetFinder.Exists(folder))
                {
                    propertyRoot.arraySize = 0;
                    Debug.LogError(string.Format(ERROR_FOLDER_NOT_FOUND, folder));
                    return;
                }
                if (!Reload_PropertyRoot(folder, extension, propertyRoot))
                    return;
            }
            //
            EditorGUI.PropertyField(position, property, label);
        }
        #endregion

        #region Method
        private bool Reload_PropertyRoot(string folder, string extension, SerializedProperty propertyRoot)
        {
            List<string> files = AssetFinder.GetAllFile(folder, extension, false);
            //
            EditorGui_Draw.ArrayAsync_String(files, propertyRoot);
            if (propertyRoot.arraySize == 0)
                return false;
            return true;
        }
        #endregion
    }
}
