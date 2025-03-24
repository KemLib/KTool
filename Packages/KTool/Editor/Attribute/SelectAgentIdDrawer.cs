using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(SelectAgentIdAttribute))]
    public class SelectAgentIdDrawer : PropertyDrawer
    {
        #region Properties
        private string[] agents;
        #endregion Properties

        #region Constructor
        public SelectAgentIdDrawer() : base()
        {
            agents = AttributeUnit.Array_Agent();
        }

        #endregion Constructor

        #region Unity Event		
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (agents.Length == 0)
            {
                EditorGUI.LabelField(position, label, new GUIContent("List agent is empty"));
                return;
            }
            //
            if (fieldInfo.FieldType == typeof(int) || fieldInfo.FieldType == typeof(int[]))
            {
                int id = property.intValue,
                    index = AttributeUnit.Agent_GetIndex(id);
                if (index < 0)
                {
                    index = 0;
                    property.intValue = AttributeUnit.Agent_GetId(index);
                }
                else if (index >= agents.Length)
                {
                    index = agents.Length - 1;
                    property.intValue = AttributeUnit.Agent_GetId(index);
                }
                if (EditorGui_Draw.DrawPopup_Int(position, label, agents, ref index))
                    property.intValue = AttributeUnit.Agent_GetId(index);
                return;
            }
            //
            EditorGUI.LabelField(position, label, new GUIContent("type of property not is Int"));
        }
        #endregion Unity Event

        #region Method

        #endregion Method
    }
}
