using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{
    [CreateAssetMenu(fileName = "SettingReplaceNameScriptEditor", menuName = "ScriptableObject/SettingReplaceNameScriptEditor")]
    public class SettingReplaceNameScriptEditor : SettingReplace
    {
        #region Properties
        public const string KEY_NAME_SCRIPT = "[NameScriptEditor]";

        [SerializeField]
        private string keyNameScriptEditor = KEY_NAME_SCRIPT;
        [SerializeField]
        private Color colorTextPrivew;

        public string KeyNameScriptEditor => string.IsNullOrEmpty(keyNameScriptEditor) ? KEY_NAME_SCRIPT : keyNameScriptEditor;
        public Color ColorTextPrivew => colorTextPrivew;
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
