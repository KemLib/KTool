using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace KTool.AssetCreater.Editor
{
    public class CreateWindow : EditorWindow
    {
        #region Properties
        private const string WINDOW_TITLE = "Create Asset";
        private static string ERROR_SELECT_EMPTY = "Create Window: select is empty",
            ERROR_SELECT_PATH_EMPTY = "Create Window: select path is empty",
            ERROR_SELECT_PATH_NOT_FILE_OR_FOLDER = "Create Window: select path is not file or folder",
            ERROR_FILE_NAME_IS_EMPTY = "Create file fail: file name is empty",
            ERROR_FILE_EXISTS = "Create file fail: File already exists",
            ERROR_WRITE_FAIL = "Create file fail: to write data: {0}";
        private const string FILE_PATH_FORMAT = "{0}/{1}/{2}.{3}";

        private static string project_path;
        public static string ProjectPath
        {
            get
            {
                if (project_path == null)
                {
                    DirectoryInfo folder = new DirectoryInfo(Application.dataPath);
                    project_path = folder.Parent.FullName;
                }
                return project_path;
            }
        }

        private ICreater creater;
        private string selectFolder,
            selectAsset;
        private Vector2 scrollPos;

        public string SelectFolder => selectFolder;
        public string SelectAsset => selectAsset;
        #endregion

        #region Method
        public void InitAsset(ICreater creater, string path)
        {
            this.creater = creater;
            FileInfo fileInfo = new FileInfo(path);
            selectFolder = fileInfo.Directory.FullName.Remove(0, ProjectPath.Length + 1);
            selectAsset = fileInfo.Name;
            //
            titleContent = new GUIContent(WINDOW_TITLE);
        }
        public void InitFolder(ICreater creater, string path)
        {
            this.creater = creater;
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            selectFolder = directoryInfo.FullName.Remove(0, ProjectPath.Length + 1);
            selectAsset = string.Empty;
            //
            titleContent = new GUIContent(WINDOW_TITLE);
        }
        #endregion

        #region Editor Window
        public static void ShowWindow(ICreater creater)
        {
            if (Selection.assetGUIDs.Length == 0)
            {
                Debug.LogError(ERROR_SELECT_EMPTY);
                return;
            }
            //
            string selectID = Selection.assetGUIDs[0],
                selectPath = AssetDatabase.GUIDToAssetPath(selectID);
            if (string.IsNullOrEmpty(selectPath))
            {
                Debug.LogError(ERROR_SELECT_PATH_EMPTY);
                return;
            }
            //
            string fullPath = Path.Combine(ProjectPath, selectPath);
            if (File.Exists(fullPath))
            {
                CreateWindow createrWindow = EditorWindow.CreateWindow<CreateWindow>();
                createrWindow.InitAsset(creater, fullPath);
                createrWindow.Show();
                creater.OnGuiShow(createrWindow);
            }
            else if (Directory.Exists(fullPath))
            {
                CreateWindow createrWindow = EditorWindow.CreateWindow<CreateWindow>();
                createrWindow.InitFolder(creater, fullPath);
                createrWindow.Show();
                creater.OnGuiShow(createrWindow);
            }
            else
            {
                Debug.LogError(ERROR_SELECT_PATH_NOT_FILE_OR_FOLDER);
            }
        }
        private void OnGUI()
        {
            if (creater == null)
            {
                Close();
                return;
            }
            //
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Select Folder", selectFolder);
            EditorGUILayout.TextField("Select Asset", selectAsset);
            EditorGUI.EndDisabledGroup();
            //
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bool isSave = GUILayout.Button("Save");
            EditorGUILayout.EndHorizontal();
            if (isSave)
            {
                creater.OnSave(this);
                if (creater.SaveAndClose)
                {
                    creater = null;
                    Close();
                    return;
                }
            }
            //
            EditorGUILayout.Space(10);
            GUILayout.BeginVertical(creater.CreaterName, "window");
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            creater.OnGuiDraw(this);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space(10);
            GUILayout.EndVertical();
        }
        private void OnDestroy()
        {
            if (creater == null)
                return;
            //
            creater.OnCancel(this);
            creater = null;
        }
        #endregion

        #region Write File
        public bool WriteFile(string fileName, string fileExtension, string data)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError(ERROR_FILE_NAME_IS_EMPTY);
                return false;
            }
            //
            string path = string.Format(FILE_PATH_FORMAT, ProjectPath, SelectFolder, fileName, fileExtension);
            StreamWriter sw = null;
            try
            {
                if (File.Exists(path))
                {
                    Debug.LogError(ERROR_FILE_EXISTS);
                    return false;
                }
                FileStream fs = File.Open(path, FileMode.Create);
                sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(data);
                sw.Flush();
                sw.Close();
                sw = null;
                AssetDatabase.Refresh();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_WRITE_FAIL, ex.Message));
                return false;
            }
            finally
            {
                sw?.Close();
            }
        }
        public bool WriteFile(string folder, string fileName, string fileExtension, string data)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError(ERROR_FILE_NAME_IS_EMPTY);
                return false;
            }
            //
            string path = string.Format(FILE_PATH_FORMAT, ProjectPath, folder, fileName, fileExtension);
            StreamWriter sw = null;
            try
            {
                if (File.Exists(path))
                {
                    Debug.LogError(ERROR_FILE_EXISTS);
                    return false;
                }
                FileStream fs = File.Open(path, FileMode.Create);
                sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(data);
                sw.Flush();
                sw.Close();
                sw = null;
                AssetDatabase.Refresh();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_WRITE_FAIL, ex.Message));
                return false;
            }
            finally
            {
                sw?.Close();
            }
        }
        #endregion
    }
}
