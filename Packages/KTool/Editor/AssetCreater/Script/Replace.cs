using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{
    public abstract class Replace
    {
        #region Properties
        public const char CHAR_DOT = '.',
            CHAR_BACKSLASH = '/',
            CHAR_UN_BACKSLASH = '\\',
            CHAR_SPACE = ' ',
            CHAR_SLIP = '_';
        public const string FOLDER_NAME_PACKAGES = "Packages",
            FOLDER_NAME_ASSETS = "Assets",
            FOLDER_NAME_RUNTIME = "Runtime",
            FOLDER_NAME_EDITOR = "Editor";
        private const string FORMAT_TEXT_PRIVIEW_COLOR = "<color=#{0}>{1}</color>";

        public readonly CreaterScript CreaterScript;
        public abstract SettingReplace Setting
        {
            get;
        }
        #endregion

        #region Construction
        public Replace(CreaterScript createrScript)
        {
            CreaterScript = createrScript;
        }
        #endregion

        #region Methods
        public abstract string OnGuiDraw(string preview);
        public abstract string OnSave(string text);
        public abstract void OnCancel();
        #endregion

        #region Utility
        public static string GetTextPreviewColor(string text, Color color)
        {
            return string.Format(FORMAT_TEXT_PRIVIEW_COLOR, ColorUtility.ToHtmlStringRGB(color), text);
        }
        public static string StringRemoveStartSpace(string text)
        {
            while (text.StartsWith(CHAR_SPACE))
            {
                if (text.Length <= 1)
                    return string.Empty;
                text = text.Substring(1);
            }
            return text;
        }
        public static string StringRemoveEndSpace(string text)
        {
            while (text.EndsWith(CHAR_SPACE))
            {
                if (text.Length <= 1)
                    return string.Empty;
                text = text.Substring(0, text.Length - 1);
            }
            return text;
        }
        #endregion
    }
}
