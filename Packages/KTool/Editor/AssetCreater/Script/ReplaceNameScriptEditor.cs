using UnityEditor;
using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{
    public class ReplaceNameScriptEditor : Replace
    {
        #region Properties
        private const string GUI_TITLE = "Name Script Editor";
        private const string DEFAULT_NAME_SCRIPT_EDITOR_FORMAT = "New{0}Editor",
            NAME_SCRIPT_EDITO_FORMAT = "{0}Editor";

        private readonly SettingReplaceNameScriptEditor setting;
        private string textNameScriptEditor;

        public override SettingReplace Setting => setting;
        public string TextNameScriptEditor => textNameScriptEditor;
        #endregion

        #region Construction
        public ReplaceNameScriptEditor(CreaterScript createrScript, SettingReplaceNameScriptEditor setting) : base(createrScript)
        {
            this.setting = setting;
            if (string.IsNullOrEmpty(CreaterScript.FileName))
                textNameScriptEditor = string.Format(DEFAULT_NAME_SCRIPT_EDITOR_FORMAT, CreaterScript.SettingTemplate.Name);
            else
                textNameScriptEditor = string.Format(NAME_SCRIPT_EDITO_FORMAT, CreaterScript.FileName);
            textNameScriptEditor = ReplaceNameScript.FixName(textNameScriptEditor);
            if (setting.EditFileName)
                CreaterScript.FileName = TextNameScriptEditor;
        }
        #endregion

        #region Methods
        public override string OnGuiDraw(string preview)
        {
            EditorGUI.BeginChangeCheck();
            textNameScriptEditor = EditorGUILayout.TextField(new GUIContent(GUI_TITLE), textNameScriptEditor);
            if (EditorGUI.EndChangeCheck())
            {
                textNameScriptEditor = ReplaceNameScript.FixName(textNameScriptEditor);
                if (setting.EditFileName)
                    CreaterScript.FileName = TextNameScriptEditor;
            }
            //
            return preview.Replace(setting.KeyNameScriptEditor, GetTextPreviewColor(textNameScriptEditor, setting.ColorTextPrivew));
        }
        public override string OnSave(string text)
        {
            return text.Replace(setting.KeyNameScriptEditor, textNameScriptEditor);
        }
        public override void OnCancel()
        {

        }
        #endregion
    }
}
