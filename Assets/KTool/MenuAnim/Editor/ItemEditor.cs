using UnityEditor;
using UnityEngine;

namespace KTool.MenuAnim.Editor
{
    public abstract class ItemEditor
    {
        #region Properties
        private SerializedProperty propertyOnStart,
            propertyOnend;

        private bool iShowEvent;
        #endregion Properties

        #region Construction
        public ItemEditor(SerializedProperty propertyItem)
        {
            propertyOnStart = propertyItem.FindPropertyRelative("onStart");
            propertyOnend = propertyItem.FindPropertyRelative("onEnd");
            iShowEvent = false;
        }
        #endregion

        #region Method
        public abstract void SetActiveItem(bool isActive);
        public abstract void SetStateTaget();
        #endregion

        #region UnityEvent

        public void OnGui()
        {
            OnGui_Chil();
            OnGui_Event();
        }


        protected abstract void OnGui_Chil();

        private void OnGui_Event()
        {
            if (iShowEvent)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Hide Events"))
                    iShowEvent = false;
                EditorGUILayout.EndHorizontal();
                if (!iShowEvent)
                    return;
                EditorGUILayout.PropertyField(propertyOnStart, new GUIContent("On Start"));
                EditorGUILayout.PropertyField(propertyOnend, new GUIContent("On End"));
                return;
            }
            //
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Show Events"))
                iShowEvent = true;
            EditorGUILayout.EndHorizontal();
        }
        #endregion UnityEvent
    }
}
