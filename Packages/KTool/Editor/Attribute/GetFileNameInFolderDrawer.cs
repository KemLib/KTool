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
        public const string ERROR_TYPE = "type of property not is string[]";
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
                Reload_PropertyRoot(property);
            EditorGUI.PropertyField(position, property, label);
        }
        #endregion

        #region Method
        private void Reload_PropertyRoot(SerializedProperty property)
        {
            GetFileNameInFolderAttribute objectAttribute = attribute as GetFileNameInFolderAttribute;
            List<string> files = AssetFinder.GetAllFile(objectAttribute.Folder, objectAttribute.Extension, false);
            //
            SerializedProperty propertyRoot = EditorGui_Draw.PropertyElement_GetPropertyRoot(property);
            EditorGui_Draw.ArrayAsync_String(files, propertyRoot);
        }
        #endregion
    }
}
