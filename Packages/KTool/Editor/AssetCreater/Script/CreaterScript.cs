using KTool.AssetCreater.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{
    public class CreaterScript : ICreater
    {
        #region Properties
        private const string CREATER_NAME = "Creater Script";
        private const string SCRIPT_FILE_EXTENSION = "cs";
        private const string FILE_PATH_FORMAT = "{0}\\{1}.{2}";
        private const string FILE_PATH_TITLE = "Path";
        private const string GUI_ERROR_TITLE = "Error!",
            ERROR_LOAD_SETTING_FAIL = "Canot load Setting",
            ERROR_LOAD_SETTING_TEMPALE_FAIL = "Canot load SettingTemplate";

        private SettingCreateScript setting;
        private string selectFolder,
            selectAsset,
            folderName,
            fileName;
        private SettingTemplate settingTemplate;
        private List<Replace> listReplace;
        private Vector2 scrollTextTemplate;
        private string[] arrayTemplateName;
        private int indexTemplate;

        public string CreaterName => CREATER_NAME;
        public bool SaveAndClose => true;
        public string SelectFolder => selectFolder;
        public string SelectAsset => selectAsset;
        public string FolderName
        {
            get => folderName;
            set => folderName = value;
        }
        public string FileName
        {
            get => fileName;
            set => fileName = value;
        }
        public SettingTemplate SettingTemplate => settingTemplate;
        public int Count => listReplace.Count;
        public Replace this[int index] => listReplace[index];
        #endregion

        #region Construction
        public CreaterScript()
        {

        }
        #endregion

        #region Method
        private void LoadSettingTemplate()
        {
            if (indexTemplate < 0)
                indexTemplate = 0;
            else if (indexTemplate >= setting.Count)
                indexTemplate = setting.Count - 1;
            //
            settingTemplate = setting[indexTemplate];
            listReplace.Clear();
            for (int i = 0; i < settingTemplate.Count; i++)
                listReplace.Add(settingTemplate[i].GetReplace(this));
        }
        private static string FixFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;
            //
            fileName = fileName.Substring(0, fileName.LastIndexOf(Replace.CHAR_DOT));
            //
            return fileName;
        }
        [MenuItem("Assets/KTool/Create Script")]
        private static void MenuItem_ShowWindow()
        {
            ICreater creater = new CreaterScript();
            CreateWindow.ShowWindow(creater);
        }
        #endregion

        #region Method Creater
        public void OnGuiShow(CreateWindow createWindow)
        {
            setting = SettingCreateScript.Instance;
            selectFolder = createWindow.SelectFolder;
            selectAsset = createWindow.SelectAsset;
            folderName = createWindow.SelectFolder;
            fileName = FixFileName(createWindow.SelectAsset);
            settingTemplate = null;
            listReplace = new List<Replace>();
            if (setting == null || setting.Count == 0)
            {
                arrayTemplateName = new string[0];
                indexTemplate = -1;
            }
            else
            {
                arrayTemplateName = new string[setting.Count];
                for (int i = 0; i < setting.Count; i++)
                    arrayTemplateName[i] = setting[i].Name;
                indexTemplate = 0;
                //
                LoadSettingTemplate();
            }
            //
            scrollTextTemplate = Vector2.zero;
        }
        public void OnGuiDraw(CreateWindow createWindow)
        {
            EditorGUI.BeginDisabledGroup(true);
            string path = string.Format(FILE_PATH_FORMAT, folderName, fileName, SCRIPT_FILE_EXTENSION);
            EditorGUILayout.TextField(FILE_PATH_TITLE, path);
            EditorGUI.EndDisabledGroup();
            //
            OnGuiDrawSelectTemplate();
            //
            GUILayout.Space(10);
            //
            OnGuiDrawReplace();
            //
            GUILayout.FlexibleSpace();
        }
        private void OnGuiDrawSelectTemplate()
        {
            if (setting == null || setting.Count == 0)
            {
                if (setting == null)
                    EditorGUILayout.LabelField(GUI_ERROR_TITLE, ERROR_LOAD_SETTING_FAIL);
                else
                    EditorGUILayout.LabelField(GUI_ERROR_TITLE, ERROR_LOAD_SETTING_TEMPALE_FAIL);
                return;
            }
            //
            EditorGUI.BeginChangeCheck();
            indexTemplate = EditorGUILayout.Popup("Template", indexTemplate, arrayTemplateName);
            if (EditorGUI.EndChangeCheck())
            {
                LoadSettingTemplate();
            }
        }
        private void OnGuiDrawReplace()
        {
            if (settingTemplate == null)
                return;
            //
            string previewText = settingTemplate.TextTemplate;
            foreach (var replace in listReplace)
            {
                try
                {
                    previewText = replace.OnGuiDraw(previewText);
                }
                catch (Exception ex)
                {
                    EditorGUILayout.LabelField(replace.Setting.name, ex.Message);
                }
            }
            //
            GUILayout.Space(10);
            //
            GUIStyle textStyle = new GUIStyle();
            textStyle.normal.textColor = Color.white;
            textStyle.wordWrap = true;
            textStyle.richText = true;
            GUILayout.BeginVertical("Script " + settingTemplate.Name, "window");
            scrollTextTemplate = EditorGUILayout.BeginScrollView(scrollTextTemplate);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextArea(previewText, textStyle);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
        public void OnSave(CreateWindow createWindow)
        {
            if (settingTemplate == null)
                return;
            //
            string text = settingTemplate.TextTemplate;
            foreach (var replace in listReplace)
                text = replace.OnSave(text);
            createWindow.WriteFile(FolderName, FileName, SCRIPT_FILE_EXTENSION, text);
        }
        public void OnCancel(CreateWindow createWindow)
        {
            foreach (var replace in listReplace)
            {
                replace.OnCancel();
            }
        }
        #endregion
    }
}
