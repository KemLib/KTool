using UnityEditor;
using UnityEngine;

namespace KTool.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(DisabledChangeAttribute))]
    public class DisabledChangeDrawer : PropertyDrawer
    {
        #region Properties
        private bool isEnable;
        #endregion

        #region Constructor
        public DisabledChangeDrawer() : base()
        {
            isEnable = false;
        }
        #endregion Constructor

        #region Unity Event		
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DisabledChangeAttribute objectAttribute = attribute as DisabledChangeAttribute;
            bool enableChange = objectAttribute.EnableChange;
            if (enableChange)
            {
                EditorGUILayout.BeginHorizontal();
                //
                GUILayout.FlexibleSpace();
                if (isEnable)
                {
                    if (GUILayout.Button("Disable"))
                        isEnable = false;
                }
                else
                {
                    if (GUILayout.Button("Enable"))
                        isEnable = true;
                }
                //
                EditorGUI.BeginDisabledGroup(!isEnable);
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndDisabledGroup();
                //
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndDisabledGroup();
            }
        }
        #endregion
    }
}
