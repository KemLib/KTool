using System.IO;
using UnityEditor;

namespace KTool.ScriptTemplate.Editor
{
    [InitializeOnLoad]
    public class ScriptTemplateReplacer : AssetModificationProcessor
    {
        private const string FILE_META = ".meta",
            FILE_CS = ".cs";
        private const string CLASS_EDITOR = "Editor";
        private const char CHAR_DOT = '.',
            CHAR_BACKSLASH = '/';
        private const string FOLDER_PACKAGE = "Packages/KTool",
            FOLDER_PACKAGE_CACHE = "Library/PackageCache",
            FOLDER_PACKAGE_CACHE_KTOOL = "com.infinity.world.ktool",
            PATH_SCRIPTTEMPLATE_NEW_BEHAVIOURSCRIPT = "{0}/Runtime/ScriptTemplate/ScriptTemplate_NewBehaviourScript.txt",
            PATH_SCRIPTTEMPLATE_CLASS = "{0}/Runtime/ScriptTemplate/ScriptTemplate_Class.txt",
            PATH_SCRIPTTEMPLATE_CLASS_EDITOR = "{0}/Runtime/ScriptTemplate/ScriptTemplate_ClassEditer.txt";
        private const string KEY_NAMESPACE = "#NAMESPACE#",
            KEY_CLASS_NAME = "#SCRIPTNAME#",
            KEY_CLASS_EDITOR_NAME = "#SCRIPTEDITORNAME#";

        public static void OnWillCreateAsset(string pathMeta)
        {
            if (!Check_MetaCS(pathMeta))
                return;
            string pathCs = pathMeta.Replace(FILE_META, string.Empty),
                fileName = GetFileName(pathCs);
            if (!Check_NewBehaviourScript(pathCs, fileName))
                return;
            //
            string folderName = GetFolderName(pathCs),
                nameSpace = GetNameSpace(folderName);
            string data;
            if (Check_ClassEditor(fileName, nameSpace))
            {
                data = GetData_ClassEditor(fileName, nameSpace);
            }
            else
            {
                data = GetData_Class(fileName, nameSpace);
            }
            //
            if (string.IsNullOrEmpty(data))
                return;
            File.WriteAllText(pathCs, data);
            AssetDatabase.Refresh();
        }

        private static string GetData_NewBehaviourScript(string fileName)
        {
            string data = PackageFile_Read(PATH_SCRIPTTEMPLATE_NEW_BEHAVIOURSCRIPT);
            data = data.Replace(KEY_CLASS_NAME, fileName);
            return data;
        }
        private static string GetData_Class(string fileName, string nameSpace)
        {
            string data = PackageFile_Read(PATH_SCRIPTTEMPLATE_CLASS);
            data = data.Replace(KEY_NAMESPACE, nameSpace);
            data = data.Replace(KEY_CLASS_NAME, fileName);
            return data;
        }
        private static string GetData_ClassEditor(string fileName, string nameSpace)
        {
            string classname = fileName.Replace(CLASS_EDITOR, string.Empty);
            string data = PackageFile_Read(PATH_SCRIPTTEMPLATE_CLASS_EDITOR);
            data = data.Replace(KEY_NAMESPACE, nameSpace);
            data = data.Replace(KEY_CLASS_NAME, classname);
            data = data.Replace(KEY_CLASS_EDITOR_NAME, fileName);
            return data;
        }
        private static bool Check_MetaCS(string pathMeta)
        {
            return pathMeta.EndsWith(FILE_CS + FILE_META);
        }
        private static bool Check_NewBehaviourScript(string pathCs, string fileName)
        {
            string data = File.ReadAllText(pathCs);
            return data == GetData_NewBehaviourScript(fileName);
        }
        private static bool Check_ClassEditor(string fileName, string nameSpace)
        {
            return fileName.EndsWith(CLASS_EDITOR) && nameSpace.Contains(CLASS_EDITOR);
        }
        private static string GetFileName(string pathCs)
        {
            int indexStart = pathCs.LastIndexOf(CHAR_BACKSLASH),
                indexEnd = pathCs.LastIndexOf(FILE_CS),
                length = indexEnd - indexStart - 1;
            return pathCs.Substring(indexStart + 1, length);
        }
        private static string GetFolderName(string pathCs)
        {
            int indexEnd = pathCs.LastIndexOf(CHAR_BACKSLASH);
            string folder = pathCs.Substring(0, indexEnd);
            return folder;
        }
        private static string GetNameSpace(string folderName)
        {
            return folderName.Replace(CHAR_BACKSLASH, CHAR_DOT);
        }

        private static string PackageFile_Read(string pathFormat)
        {
            try
            {
                string path = string.Format(pathFormat, FOLDER_PACKAGE);
                if (File.Exists(path))
                {
                    return File.ReadAllText(path);
                }
                else
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(FOLDER_PACKAGE_CACHE);
                    DirectoryInfo[] folders = directoryInfo.GetDirectories();
                    foreach (DirectoryInfo folder in folders)
                        if (folder.Name.StartsWith(FOLDER_PACKAGE_CACHE_KTOOL))
                        {
                            path = string.Format(pathFormat, folder.FullName);
                            return File.ReadAllText(path);
                        }
                }
                return string.Empty;
            }
            catch (System.Exception)
            {
                return string.Empty;
            }
        }
    }
}
