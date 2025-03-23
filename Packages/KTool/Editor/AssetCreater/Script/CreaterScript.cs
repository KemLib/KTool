using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Packages.KTool.Editor.AssetCreater.Script
{
    public class CreaterScript : ICreater
    {
        #region Properties
        public const char CHAR_DOT = '.',
            CHAR_BACKSLASH = '/',
            CHAR_UN_BACKSLASH = '\\',
            CHAR_SPACE = ' ',
            CHAR_SLIP = '_';
        public const string TEXT_ENTER = "\n";
        public static string TextNewLine = Environment.NewLine;
        private const string CREATER_NAME = "Creater Script";
        private const string DEFAULT_CLASS_NAME = "NewScript";
        private const string SCRIPT_FILE_PATH_FORMAT = "{0}/{1}/{2}.cs";
        private const string ERROR_NAMESPACE_IS_EMPTY = "Create script fail: Namespace is empty",
            ERROR_CLASS_NAME_IS_EMPTY = "Create script fail: ClassName is empty",
            ERROR_LOAD_SETTING_FAIL = "Create script fail: canot load Setting",
            ERROR_LOAD_SETTING_TEMPALE_FAIL = "Create script fail: canot load SettingTemplate",
            ERROR_FILE_EXISTS = "Create script fail: File already exists",
            ERROR_WRITE_FAIL = "Create script fail to write data: {0}";

        public static string[] arrayAccessModifier = Enum.GetNames(typeof(AccessModifiers));

        private string txtNameSpace;
        private int indexAccessModifiers;
        private string txtClassName;
        private int indexTemplate;
        private string txtTemplate;
        private Vector2 scrollTemplate;

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
        public static string GetNamespace(string folder)
        {
            folder = folder.Replace(CHAR_BACKSLASH, CHAR_DOT);
            folder = folder.Replace(CHAR_UN_BACKSLASH, CHAR_DOT);
            folder = folder.Replace(CHAR_SPACE, CHAR_SLIP);
            List<string> listFolder = new List<string>(folder.Split(CHAR_DOT));
            string nameSpace = string.Join(CHAR_DOT, listFolder.ToArray());
            return nameSpace;
        }
        public static void WriteScript(string folder, string fileName, string data)
        {
            string path = string.Format(SCRIPT_FILE_PATH_FORMAT, CreateWindow.ProjectPath, folder, fileName);
            StreamWriter sw = null;
            try
            {
                if (File.Exists(path))
                {
                    Debug.LogError(ERROR_FILE_EXISTS);
                    return;
                }
                FileStream fs = File.Open(path, FileMode.Create);
                sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(data);
                sw.Flush();
                sw.Close();
                sw = null;
                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_WRITE_FAIL, ex.Message));
            }
            finally
            {
                sw?.Close();
            }
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
            if (GUILayout.Button("Auto Namespace"))
            {
                txtNameSpace = GetNamespace(createWindow.SelectFolder);
            }
            EditorGUILayout.EndHorizontal();
            //
            indexAccessModifiers = EditorGUILayout.Popup("Access Modifier", indexAccessModifiers, arrayAccessModifier);
            //
            txtClassName = EditorGUILayout.TextField("Class Name", txtClassName);
            //
            if (Setting != null && Setting.Count > 0)
            {
                string[] arrayTemplateName = new string[Setting.Count];
                for (int i = 0; i < Setting.Count; i++)
                    arrayTemplateName[i] = Setting[i].Name;
                if (indexTemplate < 0)
                    indexTemplate = 0;
                else if (indexTemplate >= Setting.Count)
                    indexTemplate = Setting.Count - 1;
                EditorGUI.BeginChangeCheck();
                indexTemplate = EditorGUILayout.Popup("Template", indexTemplate, arrayTemplateName);
                if (EditorGUI.EndChangeCheck())
                    txtTemplate = Setting[indexTemplate].TextTemplate;
            }
            //
            GUILayout.BeginVertical("Script", "window");
            scrollTemplate = EditorGUILayout.BeginScrollView(scrollTemplate);
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
            WriteScript(createWindow.SelectFolder, txtClassName, scriptData);
        }
        public void OnCancel(CreateWindow createWindow)
        {

        }
        #endregion
    }
}
