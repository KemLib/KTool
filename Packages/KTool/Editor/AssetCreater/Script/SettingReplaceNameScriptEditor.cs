using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{
    public class SettingReplaceNameScriptEditor : SettingReplace
    {
        #region Properties
        public const string KEY_NAME_SCRIPT = "[NameScriptEditor]";

        [SerializeField]
        private string keyNameScriptEditor = KEY_NAME_SCRIPT;
        [SerializeField]
        private Color colorTextPrivew;
        [SerializeField]
        private bool editFileName;

        public string KeyNameScriptEditor => string.IsNullOrEmpty(keyNameScriptEditor) ? KEY_NAME_SCRIPT : keyNameScriptEditor;
        public Color ColorTextPrivew => colorTextPrivew;
        public bool EditFileName => editFileName;
        #endregion

        #region Methods Unity
        public override Replace GetReplace(CreaterScript createrScript)
        {
            return new ReplaceNameScriptEditor(createrScript, this);
        }
        #endregion

        #region Methods

        #endregion
    }
}
