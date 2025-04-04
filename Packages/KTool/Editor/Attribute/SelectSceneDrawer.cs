using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(SelectSceneAttribute))]
    public class SelectSceneDrawer : PropertyDrawer
    {
        #region Properties

        #endregion Properties

        #region Constructor
        public SelectSceneDrawer() : base()
        {

        }
        #endregion Constructor

        #region UnityEvent
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string[] options = AttributeUnit.Array_Scene();
            if (options.Length == 0)
            {
                EditorGUI.LabelField(position, label, new GUIContent("List scene in build setting is empty"));
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
        #endregion UnityEvent

        #region Method

        #endregion Method
    }
}
