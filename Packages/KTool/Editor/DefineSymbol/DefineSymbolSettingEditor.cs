using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace KTool.DefineSymbol.Editor
{

    [CustomEditor(typeof(DefineSymbolSetting))]
    public class DefineSymbolSettingEditor : UnityEditor.Editor
    {
        #region Properties
        private SerializedProperty propertyDefineSymbols;
        private string newDefineSymbol;
        private bool showSettingDefineSymbols,
            showDefineSymbols;
        #endregion

        #region Methods Unity        
        private void OnEnable()
        {
            propertyDefineSymbols = serializedObject.FindProperty("defineSymbols");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //
            NamedBuildTarget namedBuildTarget = GetCurrentNamedBuildTarget();
            string[] defineSymbols = GetCurrentDefineSymbol(namedBuildTarget);
            OnInspectorGUI_Setting(namedBuildTarget, defineSymbols);
            //
            EditorGUILayout.Space(10);
            OnInspectorGUI_Create();
            //
            EditorGUILayout.Space(10);
            OnInspectorGUI_List(namedBuildTarget, defineSymbols);
            //
            serializedObject.ApplyModifiedProperties();
        }
        private void OnInspectorGUI_Setting(NamedBuildTarget namedBuildTarget, string[] defineSymbols)
        {
            GUILayout.BeginVertical("Setting", "window");
            //
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Build Taget", namedBuildTarget.TargetName);
            EditorGUILayout.TextField("Define Symbol Count", defineSymbols.Length.ToString());
            EditorGUI.EndDisabledGroup();
            //
            if (defineSymbols.Length > 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (showSettingDefineSymbols)
                {
                    if (GUILayout.Button("Hide"))
                    {
                        showSettingDefineSymbols = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("Show"))
                    {
                        showSettingDefineSymbols = true;
                    }
                }
                GUILayout.EndHorizontal();
                //
                if (showSettingDefineSymbols)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    for (int i = 0; i < defineSymbols.Length; i++)
                    {
                        string lable = string.Format("Index {0}", i);
                        EditorGUILayout.TextField(lable, defineSymbols[i]);
                    }
                    EditorGUI.EndDisabledGroup();
                }
            }
            //
            GUILayout.EndVertical();
        }
        private void OnInspectorGUI_Create()
        {
            EditorGUILayout.BeginHorizontal();
            newDefineSymbol = EditorGUILayout.TextField("Enter New Value", newDefineSymbol);
            if (!string.IsNullOrEmpty(newDefineSymbol) && GUILayout.Button("Add"))
            {
                propertyDefineSymbols.arraySize += 1;
                propertyDefineSymbols.GetArrayElementAtIndex(propertyDefineSymbols.arraySize - 1).stringValue = newDefineSymbol;
                newDefineSymbol = string.Empty;
            }
            EditorGUILayout.EndHorizontal();
        }
        private void OnInspectorGUI_List(NamedBuildTarget namedBuildTarget, string[] defineSymbols)
        {
            if (propertyDefineSymbols.arraySize == 0)
                return;
            //
            GUILayout.BeginVertical("Define Symbols", "window");
            //
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Define Symbol Count", propertyDefineSymbols.arraySize.ToString());
            EditorGUI.EndDisabledGroup();
            //
            if (propertyDefineSymbols.arraySize > 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (showDefineSymbols)
                {
                    if (GUILayout.Button("Hide"))
                    {
                        showDefineSymbols = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("Show"))
                    {
                        showDefineSymbols = true;
                    }
                }
                GUILayout.EndHorizontal();
                //
                if (showDefineSymbols)
                {
                    int index = 0;
                    while (index < propertyDefineSymbols.arraySize)
                    {
                        SerializedProperty propertyDefineSymbol = propertyDefineSymbols.GetArrayElementAtIndex(index);
                        OnInspectorGUI(propertyDefineSymbol, index, namedBuildTarget, ref defineSymbols, out bool remove);
                        if (remove)
                        {
                            propertyDefineSymbols.DeleteArrayElementAtIndex(index);
                            continue;
                        }
                        else
                        {
                            index++;
                        }
                    }
                }
            }
            GUILayout.EndVertical();
        }
        private void OnInspectorGUI(SerializedProperty propertyDefineSymbol, int index, NamedBuildTarget namedBuildTarget, ref string[] defineSymbols, out bool remove)
        {
            string defineSymbol = propertyDefineSymbol.stringValue;
            //
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(defineSymbol);
            if (IndexOf(defineSymbol, defineSymbols) == -1)
            {
                if (GUILayout.Button("Active"))
                {
                    remove = false;
                    AddDefineSymbol(namedBuildTarget, defineSymbol, ref defineSymbols);
                }
                else if (GUILayout.Button("Remove"))
                {
                    remove = true;
                }
                else
                {
                    remove = false;
                }
            }
            else
            {
                if (GUILayout.Button("Deactive"))
                {
                    remove = false;
                    RemoveDefineSymbol(namedBuildTarget, defineSymbol, ref defineSymbols);
                }
                else if (GUILayout.Button("Remove"))
                {
                    remove = true;
                    RemoveDefineSymbol(namedBuildTarget, defineSymbol, ref defineSymbols);
                }
                else
                {
                    remove = false;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region Methods
        private int IndexOf(string value, string[] array)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i] == value)
                    return i;
            return -1;
        }
        private void AddDefineSymbol(NamedBuildTarget namedBuildTarget, string value, ref string[] array)
        {
            string[] newArray = new string[array.Length + 1];
            array.CopyTo(newArray, 0);
            newArray[array.Length] = value;
            array = newArray;
            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, array);
        }
        private void RemoveDefineSymbol(NamedBuildTarget namedBuildTarget, string value, ref string[] array)
        {
            string[] newArray = new string[array.Length - 1];
            int index = 0;
            for (int i = 0; i < array.Length; i++)
                if (array[i] != value)
                {
                    newArray[index] = array[i];
                    index++;
                }
            array = newArray;
            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, array);
        }
        private string[] GetCurrentDefineSymbol(NamedBuildTarget namedBuildTarget)
        {
            PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget, out string[] defineSymbols);
            return defineSymbols;
        }
        private NamedBuildTarget GetCurrentNamedBuildTarget()
        {
#if UNITY_SERVER
        return NamedBuildTarget.Server;
#else
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            NamedBuildTarget namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
            return namedBuildTarget;
#endif
        }
        #endregion
    }
}
