﻿using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{
    [System.Serializable]
    public class SettingTemplate
    {
        #region Properties

        [SerializeField]
        private string displayName;
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
