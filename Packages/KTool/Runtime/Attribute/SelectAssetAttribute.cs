using KTool.FileIo;
using System;
using UnityEngine;

namespace KTool.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SelectAssetAttribute : PropertyAttribute
    {
        #region Properties
        private string folder;
        private string extension;
        private string search_pattern;
        private bool allowNull;

        public string Folder => folder;
        public string Extension => extension;
        public string SearchPattern => search_pattern;
        public bool AllowNull => allowNull;
        #endregion

        #region Constructor
        public SelectAssetAttribute(string folder, ExtensionType extension, bool allowNull = false) : base()
        {
            this.folder = folder;
            this.extension = PathUnit.GetExtension(extension);
            search_pattern = PathUnit.GetSearchPattern(extension);
            this.allowNull = allowNull;
        }
        public SelectAssetAttribute(string folder, string extension, bool allowNull = false) : base()
        {
            this.folder = folder;
            this.extension = PathUnit.GetExtension(extension);
            search_pattern = PathUnit.GetSearchPattern(extension);
            this.allowNull = allowNull;
        }
        #endregion

        #region Method

        #endregion
    }
}
