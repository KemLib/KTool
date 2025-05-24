using UnityEditor;
using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{
    public class ReplaceNameScript : Replace
    {
        #region Properties
        private const string GUI_TITLE = "Name Script";
        private const string DEFAULT_NAME_SCRIPT_FORMAT = "New{0}";

        private readonly SettingReplaceNameScript setting;
        private string textNameScript;

        public override SettingReplace Setting => setting;
        public string TextNameScript => textNameScript;
        #endregion

        #region Construction
        public ReplaceNameScript(CreaterScript createrScript, SettingReplaceNameScript setting) : base(createrScript)
        {
            this.setting = setting;
            if (string.IsNullOrEmpty(CreaterScript.FileName))
                textNameScript = string.Format(DEFAULT_NAME_SCRIPT_FORMAT, CreaterScript.SettingTemplate.Name);
            else
                textNameScript = CreaterScript.FileName;
            textNameScript = FixName(textNameScript);
            if(setting.EditFileName)
                CreaterScript.FileName = TextNameScript;
        }
        #endregion

        #region Methods
        public override string OnGuiDraw(string preview)
        {
            EditorGUI.BeginChangeCheck();
            textNameScript = EditorGUILayout.TextField(new GUIContent(GUI_TITLE), textNameScript);
            if (EditorGUI.EndChangeCheck())
            {
                textNameScript = FixName(textNameScript);
                if (setting.EditFileName)
                    CreaterScript.FileName = TextNameScript;
            }
            //
            return preview.Replace(setting.KeyNameScript, GetTextPreviewColor(textNameScript, setting.ColorTextPrivew));
        }
        public override string OnSave(string text)
        {
            return text.Replace(setting.KeyNameScript, textNameScript);
        }
        public override void OnCancel()
        {

        }
        #endregion

        #region Utility
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
