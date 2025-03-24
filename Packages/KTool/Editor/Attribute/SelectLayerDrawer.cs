using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(SelectLayerAttribute))]
    public class SelectLayerDrawer : PropertyDrawer
    {
        #region Properties
        private string[] options;
        #endregion Properties

        #region Constructor
        public SelectLayerDrawer() : base()
        {
            options = AttributeUnit.Array_Layer();
        }
        #endregion Constructor

        #region Unity Event	
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (options.Length == 0)
            {
                EditorGUI.LabelField(position, label, new GUIContent("List Layer in setting is empty"));
                return;
            }
            //
            if (fieldInfo.FieldType == typeof(string) || fieldInfo.FieldType == typeof(string[]))
            {
                EditorGui_Draw.DrawPopup_String(position, label, options, property);
                return;
            }
            if (fieldInfo.FieldType == typeof(int) || fieldInfo.FieldType == typeof(int[]))
            {
                EditorGui_Draw.DrawPopup_Int(position, label, options, property);
                return;
            }
            //
            EditorGUI.LabelField(position, label, new GUIContent("type of property not is String or Int"));
        }
        #endregion Unity Event

        #region Method

        #endregion Method
    }
}
