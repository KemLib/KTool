using System.Collections.Generic;
using System.IO;

namespace KTool.FileIo
{
    public static class AssetFinder
    {
        #region Properties

        #endregion

        #region File
        public static string GetPath(string folder, string file, ExtensionType extension)
        {
            string extensionString = PathUnit.GetExtension(extension);
            return GetPath(folder, file, extensionString);
        }
        public static bool Exists(string folder, string file, ExtensionType extension)
        {
            string extensionString = PathUnit.GetExtension(extension);
            return Exists(folder, file, extensionString);
        }
        public static void Delete(string folder, string file, ExtensionType extension)
        {
            string extensionString = PathUnit.GetExtension(extension);
            Delete(folder, file, extensionString);
        }
        public static List<string> GetAllFile(string folder, ExtensionType extension, bool isSort)
        {
            string extensionString = PathUnit.GetExtension(extension);
            return GetAllFile(folder, extensionString, isSort);
        }
        public static string GetPath(string folder, string file, string extension)
        {
            extension = PathUnit.GetExtension(extension);
            return string.Format(PathUnit.FOTMAT_FULL_PATH_FILE, PathUnit.GetFullFolder_Asset(folder), file, extension);
        }
        public static bool Exists(string folder, string file, string extension)
        {
            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(file))
                return false;
            string path = GetPath(folder, file, extension);
            return File.Exists(path);
        }
        public static void Delete(string folder, string file, string extension)
        {
            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(file))
                return;
            string path = GetPath(folder, file, extension);
            if (File.Exists(path))
                File.Delete(path);
        }
        public static List<string> GetAllFile(string folder, string extension, bool isSort)
        {
            if (string.IsNullOrEmpty(folder))
                return new List<string>();
            string fullFolder = PathUnit.GetFullFolder_Asset(folder);
            string searchPattern = PathUnit.GetSearchPattern(extension);
            DirectoryInfo directory = new DirectoryInfo(fullFolder);
            FileInfo[] files = directory.GetFiles(searchPattern);
            if (isSort)
                SortTmp.SortFileInfo(files);

            //
            List<string> result = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                string name = files[i].Name.Substring(0, files[i].Name.LastIndexOf(PathUnit.CHAR_SPECIAL_DOT));
                if (string.IsNullOrEmpty(name))
                    continue;
                result.Add(name);
            }
            return result;
        }
        #endregion

        #region Folder
        public static bool Exists(string folder)
        {
            if (string.IsNullOrEmpty(folder))
                return false;
            string fullFolder = PathUnit.GetFullFolder_Asset(folder);
            return Directory.Exists(fullFolder);
        }
        public static void Delete(string folder, bool recursive)
        {
            if (string.IsNullOrEmpty(folder))
                return;
            string fullFolder = PathUnit.GetFullFolder_Asset(folder);
            if (Directory.Exists(fullFolder))
                Directory.Delete(fullFolder, recursive);
        }
        public static void CreateFolder(string folder)
        {
            if (string.IsNullOrEmpty(folder))
                return;
            string fullFolder = PathUnit.GetFullFolder_Asset(folder);
            if (!Directory.Exists(fullFolder))
                Directory.CreateDirectory(fullFolder);
        }
        public static List<string> GetAllFolder(string folder, bool isSort)
        {
            List<string> result = new List<string>();
            //
            string fullFolder = PathUnit.GetFullFolder_Asset(folder);
            DirectoryInfo directory = new DirectoryInfo(fullFolder);
            DirectoryInfo[] folders = directory.GetDirectories();
            if (isSort)
                SortTmp.SortDirectoryInfo(folders);
            for (int i = 0; i < folders.Length; i++)
            {
                if (string.IsNullOrEmpty(folders[i].Name))
                    continue;
                result.Add(folders[i].Name);
            }
            //
            return result;
        }
        #endregion
    }
}
