using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{
    public class ReplaceNamespace : Replace
    {
        #region Properties
        private const string GUI_TITLE = "Namespace";

        private SettingReplaceNamespace setting;
        private string textNamespace;

        public override SettingReplace Setting => setting;
        public string TextNamespace => textNamespace;
        #endregion

        #region Construction
        public ReplaceNamespace(CreaterScript createrScript, SettingReplaceNamespace setting) : base(createrScript)
        {
            this.setting = setting;
            textNamespace = GetNamespace(createrScript.FolderName);
            textNamespace = FixName(textNamespace);
        }
        #endregion

        #region Methods
        public override string OnGuiDraw(string preview)
        {
            EditorGUI.BeginChangeCheck();
            textNamespace = EditorGUILayout.TextField(new GUIContent(GUI_TITLE), textNamespace);
            if (EditorGUI.EndChangeCheck())
            {
                textNamespace = FixName(textNamespace);
            }
            //
            return preview.Replace(setting.KeyNamespace, GetTextPreviewColor(textNamespace, setting.ColorTextPrivew));
        }
        public override string OnSave(string text)
        {
            return text.Replace(setting.KeyNamespace, textNamespace);
        }
        public override void OnCancel()
        {

        }
        #endregion
        #region Utility
        public static string GetNamespace(string folder)
        {
            if (string.IsNullOrEmpty(folder))
                return string.Empty;
            //
            // Split by path separators first so dotted package ids
            // (e.g. com.kem.ktool) stay as a single segment.
            folder = StringRemoveStartSpace(folder);
            folder = StringRemoveEndSpace(folder);
            List<string> listFolder = new List<string>(folder.Split(
                new char[] { CHAR_BACKSLASH, CHAR_UN_BACKSLASH },
                System.StringSplitOptions.RemoveEmptyEntries));
            if (listFolder.Count > 0
                && (listFolder[0] == FOLDER_NAME_ASSETS || listFolder[0] == FOLDER_NAME_PACKAGES))
            {
                if (listFolder.Count >= 3)
                {
                    if (listFolder[2] == FOLDER_NAME_EDITOR)
                    {
                        listFolder.RemoveAt(2);
                        listFolder.Add(FOLDER_NAME_EDITOR);
                    }
                    else if (listFolder[2] == FOLDER_NAME_RUNTIME)
                    {
                        listFolder.RemoveAt(2);
                    }
                }
                listFolder.RemoveAt(0);
            }
            for (int i = 0; i < listFolder.Count; i++)
                listFolder[i] = listFolder[i].Replace(CHAR_SPACE, CHAR_SLIP);
            return string.Join(CHAR_DOT.ToString(), listFolder.ToArray());
        }
        public static string FixName(string name)
        {
            name = StringRemoveStartSpace(name);
            name = StringRemoveEndSpace(name);
            name = name.Replace(CHAR_BACKSLASH, CHAR_DOT);
            name = name.Replace(CHAR_UN_BACKSLASH, CHAR_DOT);
            name = name.Replace(CHAR_SPACE, CHAR_SLIP);
            return name;
        }
        #endregion
    }
}
