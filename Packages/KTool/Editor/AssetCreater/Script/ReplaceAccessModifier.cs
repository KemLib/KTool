using System;
using UnityEditor;
using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{
    public class ReplaceAccessModifier : Replace
    {
        #region Properties
        private const string GUI_TITLE = "Access Modifier";
        public static string[] arrayAccessModifier = Enum.GetNames(typeof(AccessModifiers));

        private readonly SettingReplaceAccessModifier setting;
        private int indexAccessModifiers;
        private AccessModifiers accessModifier;
        private string textAccessModifier;

        public override SettingReplace Setting => setting;
        public AccessModifiers AccessModifier => accessModifier;
        public string TextAccessModifier => textAccessModifier;
        #endregion

        #region Construction
        public ReplaceAccessModifier(CreaterScript createrScript, SettingReplaceAccessModifier setting) : base(createrScript)
        {
            this.setting = setting;
            //
            indexAccessModifiers = 0;
            accessModifier = GetAccessModifier(indexAccessModifiers);
            textAccessModifier = GetTextAccessModifier(accessModifier);
        }
        #endregion

        #region Methods
        public override string OnGuiDraw(string preview)
        {
            EditorGUI.BeginChangeCheck();
            indexAccessModifiers = EditorGUILayout.Popup(new GUIContent(GUI_TITLE), indexAccessModifiers, arrayAccessModifier);
            if (EditorGUI.EndChangeCheck())
            {
                accessModifier = GetAccessModifier(indexAccessModifiers);
                textAccessModifier = GetTextAccessModifier(accessModifier);
            }
            //
            return preview.Replace(setting.KeyAccessModifiers, GetTextPreviewColor(textAccessModifier, setting.ColorTextPrivew));
        }
        public override string OnSave(string text)
        {
            return text.Replace(setting.KeyAccessModifiers, textAccessModifier);
        }
        public override void OnCancel()
        {

        }

        private static AccessModifiers GetAccessModifier(int index)
        {
            if (Enum.TryParse(arrayAccessModifier[index], out AccessModifiers accessModifier))
                return accessModifier;
            return AccessModifiers.PUBLIC;
        }
        private static string GetTextAccessModifier(AccessModifiers accessModifier)
        {
            return accessModifier.ToString().ToLower().Replace(CHAR_SLIP, CHAR_SPACE);
        }
        #endregion
    }
}
