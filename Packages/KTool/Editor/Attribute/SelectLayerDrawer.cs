using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(SelectLayerAttribute))]
    public class SelectLayerDrawer : PropertyDrawer
    {
        #region Properties
        #endregion Properties

        #region Constructor
        public SelectLayerDrawer() : base()
        {

        }
        #endregion Constructor

        #region Unity Event	
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string[] options = AttributeUnit.Array_Layer();
            int[] optionsId = AttributeUnit.Array_LayerId();
            if (options.Length == 0)
            {
                EditorGUI.LabelField(position, label, new GUIContent("List Layer in setting is empty"));
                return;
            }
            //
            if (fieldInfo.FieldType == typeof(string) || fieldInfo.FieldType == typeof(string[]))
            {
                EditorGui_Draw.DrawPopup_String(position, label, options, property, out _);
                return;
            }
            if (fieldInfo.FieldType == typeof(int) || fieldInfo.FieldType == typeof(int[]))
            {
                int indexLayer = IndexOf(property.intValue, optionsId);
                EditorGui_Draw.DrawPopup_Int(position, label, options, ref indexLayer);
                int newId = optionsId[indexLayer];
                if (newId != property.intValue)
                {
                    property.intValue = newId;
                }
                return;
            }
            //
            EditorGUI.LabelField(position, label, new GUIContent("type of property not is String or Int"));
        }
        #endregion Unity Event

        #region Method
        private int IndexOf(int id, int[] optionsId)
        {
            for (int i = 0; i < optionsId.Length; i++)
            {
                if (optionsId[i] == id)
                    return i;
            }
            return -1;
        }
        #endregion Method
    }
}
