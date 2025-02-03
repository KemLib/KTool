using KTool.FileIo;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(SelectFileNameInFolderAttribute))]
    public class SelectFileNameInFolderDrawer : PropertyDrawer
    {
        #region Properties
        public const string ERROR_TYPE = "type of property not is string or string[]";
        #endregion

        #region Constructor
        public SelectFileNameInFolderDrawer() : base()
        {

        }
        #endregion Constructor

        #region Unity Event		
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType == typeof(string) || fieldInfo.FieldType == typeof(string[]))
            {
                SelectFileNameInFolderAttribute objectAttribute = attribute as SelectFileNameInFolderAttribute;
                List<string> files = AssetFinder.GetAllFile(objectAttribute.Folder, objectAttribute.Extension, false);
                EditorGui_Draw.DrawPopup_String(position, label, files.ToArray(), property);
                return;
            }
            EditorGUI.LabelField(position, label, new GUIContent(ERROR_TYPE));
        }
        #endregion

        #region Method

        #endregion
    }
}
