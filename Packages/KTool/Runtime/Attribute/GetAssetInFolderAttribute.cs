using KTool.FileIo;
using System;
using UnityEngine;

namespace KTool.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class GetAssetInFolderAttribute : PropertyAttribute
    {
        #region Properties
        private string folder;
        private string extension;
        private string search_pattern;

        public string Folder => folder;
        public string Extension => extension;
        public string SearchPattern => search_pattern;
        #endregion

        #region Constructor
        public GetAssetInFolderAttribute(string folder, ExtensionType extension) : base()
        {
            this.folder = folder;
            this.extension = PathUnit.GetExtension(extension);
            search_pattern = PathUnit.GetSearchPattern(extension);
        }
        public GetAssetInFolderAttribute(string folder, string extension) : base()
        {
            this.folder = folder;
            this.extension = PathUnit.GetExtension(extension);
            search_pattern = PathUnit.GetSearchPattern(extension);
        }
        #endregion

        #region Method

        #endregion
    }
}
