using UnityEngine;

namespace Packages.KTool.Editor.AssetCreater.Script
{
    [System.Serializable]
    public class SettingTemplate
    {
        #region Properties
        public const string KEY_NAMESPACE = "[Namespace]",
            KEY_ACCESS_MODIFIERS = "[ClassAccessModifiers]",
            KEY_CLASS_NAME = "[ClassName]";

        [SerializeField]
        private string displayName;
        [SerializeField]
        private string keyNamespace = KEY_NAMESPACE,
            keyAccessModifiers = KEY_ACCESS_MODIFIERS,
            keyClassname = KEY_CLASS_NAME;
        [SerializeField]
        private TextAsset assetTemplate;

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(displayName))
                {
                    if (assetTemplate == null)
                        return string.Empty;
                    return assetTemplate.name;
                }
                else
                    return displayName;
            }
        }
        public string KeyNamespace => string.IsNullOrEmpty(keyNamespace) ? KEY_NAMESPACE : keyNamespace;
        public string KeyAccessModifiers => string.IsNullOrEmpty(keyAccessModifiers) ? KEY_ACCESS_MODIFIERS : keyAccessModifiers;
        public string KeyClassname => string.IsNullOrEmpty(keyClassname) ? KEY_CLASS_NAME : keyClassname;
        public string TextTemplate => assetTemplate == null ? string.Empty : assetTemplate.text;
        #endregion

        #region Methods
        public void OnValidate()
        {
            if (string.IsNullOrEmpty(displayName) && assetTemplate != null)
            {
                displayName = assetTemplate.name;
            }
        }
        #endregion
    }
}
