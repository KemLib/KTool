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
        public const char CHAR_DOT = '.',
            CHAR_BACKSLASH = '/',
            CHAR_UN_BACKSLASH = '\\',
            CHAR_SPACE = ' ',
            CHAR_SLIP = '_';
        private const string FOLDER_NAME_PACKAGES = "Packages",
            FOLDER_NAME_ASSETS = "Assets",
            FOLDER_NAME_RUNTIME = "Runtime",
            FOLDER_NAME_EDITOR = "Editor";
        private const string CREATER_NAME = "Creater Script";
        private const string DEFAULT_CLASS_NAME = "NewScript";
        private const string SCRIPT_FILE_EXTENSION = "cs";
        private const string ERROR_NAMESPACE_IS_EMPTY = "Create script fail: Namespace is empty",
            ERROR_CLASS_NAME_IS_EMPTY = "Create script fail: ClassName is empty",
            ERROR_LOAD_SETTING_FAIL = "Create script fail: canot load Setting",
            ERROR_LOAD_SETTING_TEMPALE_FAIL = "Create script fail: canot load SettingTemplate";

        public static string[] arrayAccessModifier = Enum.GetNames(typeof(AccessModifiers));

        private string txtNameSpace;
        private int indexAccessModifiers;
        private string txtClassName;
        private int indexTemplate;
        private string txtTemplate;
        private Vector2 scrollTextTemplate;

        public string CreaterName => CREATER_NAME;
        public bool SaveAndClose => true;
        private SettingCreateScript Setting => SettingCreateScript.Instance;
        private AccessModifiers AccessModifier
        {
            get
            {
                if (Enum.TryParse(arrayAccessModifier[indexAccessModifiers], out AccessModifiers accessModifier))
                    return accessModifier;
                return AccessModifiers.PUBLIC;
            }
        }
        #endregion

        #region Construction
        public CreaterScript()
        {

        }
        #endregion

        #region Method
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
            txtNameSpace = GetNamespace(createWindow.SelectFolder);
            indexAccessModifiers = 0;
            txtClassName = DEFAULT_CLASS_NAME;
            if (Setting == null || Setting.Count == 0)
            {
                indexTemplate = -1;
                txtTemplate = string.Empty;
            }
            else
            {
                indexTemplate = 0;
                txtTemplate = Setting[indexTemplate].TextTemplate;
            }
        }
        public void OnGuiDraw(CreateWindow createWindow)
        {
            EditorGUILayout.BeginHorizontal();
            txtNameSpace = EditorGUILayout.TextField("NameSpace", txtNameSpace);
            txtNameSpace = FixName(txtNameSpace);
            if (GUILayout.Button("Auto Namespace"))
            {
                txtNameSpace = GetNamespace(createWindow.SelectFolder);
            }
            EditorGUILayout.EndHorizontal();
            //
            indexAccessModifiers = EditorGUILayout.Popup("Access Modifier", indexAccessModifiers, arrayAccessModifier);
            //
            txtClassName = EditorGUILayout.TextField("Class Name", txtClassName);
            txtClassName = FixName(txtClassName);
            //
            string templateName = string.Empty;
            if (Setting != null && Setting.Count > 0)
            {
                EditorGUILayout.Space(10);
                string[] arrayTemplateName = new string[Setting.Count];
                for (int i = 0; i < Setting.Count; i++)
                    arrayTemplateName[i] = Setting[i].Name;
                if (indexTemplate < 0)
                    indexTemplate = 0;
                else if (indexTemplate >= Setting.Count)
                    indexTemplate = Setting.Count - 1;
                //
                EditorGUI.BeginChangeCheck();
                indexTemplate = EditorGUILayout.Popup("Template", indexTemplate, arrayTemplateName);
                if (EditorGUI.EndChangeCheck())
                    txtTemplate = Setting[indexTemplate].TextTemplate;
                EditorGUILayout.Space(10);
                //
                templateName = Setting[indexTemplate].Name;
            }
            //
            GUILayout.BeginVertical("Script " + templateName, "window");
            scrollTextTemplate = EditorGUILayout.BeginScrollView(scrollTextTemplate);
            txtTemplate = EditorGUILayout.TextArea(txtTemplate, GUILayout.Height(createWindow.position.height));
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
            //
            EditorGUILayout.Space(10);
        }
        public void OnSave(CreateWindow createWindow)
        {
            if (Setting == null)
            {
                Debug.LogError(ERROR_LOAD_SETTING_FAIL);
                return;
            }
            if (indexTemplate < 0 || indexTemplate >= Setting.Count)
            {
                Debug.LogError(ERROR_LOAD_SETTING_TEMPALE_FAIL);
                return;
            }
            if (string.IsNullOrEmpty(txtNameSpace))
            {
                Debug.LogError(ERROR_NAMESPACE_IS_EMPTY);
                return;
            }
            //
            if (string.IsNullOrEmpty(txtClassName))
            {
                Debug.LogError(ERROR_CLASS_NAME_IS_EMPTY);
                return;
            }
            //
            string txtAccessModifier = AccessModifier.ToString().ToLower().Replace(CHAR_SLIP, CHAR_SPACE),
                scriptData = txtTemplate;
            scriptData = scriptData.Replace(Setting[indexTemplate].KeyNamespace, txtNameSpace);
            scriptData = scriptData.Replace(Setting[indexTemplate].KeyAccessModifiers, txtAccessModifier);
            scriptData = scriptData.Replace(Setting[indexTemplate].KeyClassname, txtClassName);
            createWindow.WriteFile(txtClassName, SCRIPT_FILE_EXTENSION, scriptData);
        }
        public void OnCancel(CreateWindow createWindow)
        {

        }
        #endregion

        #region Utility
        public static string GetNamespace(string folder)
        {
            folder = FixName(folder);
            //
            List<string> listFolder = new List<string>(folder.Split(CHAR_DOT));
            if (listFolder.Count > 0)
            {
                if (listFolder[0] == FOLDER_NAME_ASSETS)
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
                else if (listFolder[0] == FOLDER_NAME_PACKAGES)
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
            }
            string nameSpace = string.Join(CHAR_DOT, listFolder.ToArray());
            return nameSpace;
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
        private static string StringRemoveStartSpace(string text)
        {
            while (text.StartsWith(CHAR_SPACE))
            {
                if (text.Length <= 1)
                    return string.Empty;
                text = text.Substring(1);
            }
            return text;
        }
        private static string StringRemoveEndSpace(string text)
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
