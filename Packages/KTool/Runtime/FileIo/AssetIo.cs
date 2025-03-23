namespace KTool.FileIo
{
    public static class AssetIo
    {
        #region Properties

        #endregion Properties

        #region Text IO
        public static ResultReadText Text_Read(string folder, string file, ExtensionType extension, object state = null)
        {
            string extensionString = PathUnit.GetExtension(extension);
            return Text_Read(folder, file, extensionString, state);
        }
        public static ResultReadText Text_Read(string folder, string file, string extension, object state = null)
        {
            string path = AssetFinder.GetPath(folder, file, extension);
            return IoText.Read(path, state);
        }
        public static ResultWrite Text_Write(string folder, string file, ExtensionType extension, string text, object state = null)
        {
            string extensionString = PathUnit.GetExtension(extension);
            return Text_Write(folder, file, extensionString, text, state);
        }
        public static ResultWrite Text_Write(string folder, string file, string extension, string text, object state = null)
        {
            string path = AssetFinder.GetPath(folder, file, extension);
            return IoText.Write(path, text, state);
        }
        #endregion

        #region Binary IO
        public static ResultRead Binary_Read(string folder, string file, ExtensionType extension, object state = null)
        {
            string extensionString = PathUnit.GetExtension(extension);
            return Binary_Read(folder, file, extensionString, state);
        }
        public static ResultRead Binary_Read(string folder, string file, string extension, object state = null)
        {
            string path = AssetFinder.GetPath(folder, file, extension);
            return IoBinary.Read(path, state);
        }
        public static ResultWrite Binary_Write(string folder, string file, ExtensionType extension, byte[] data, object state = null)
        {
            string extensionString = PathUnit.GetExtension(extension);
            return Binary_Write(folder, file, extensionString, data, state);
        }
        public static ResultWrite Binary_Write(string folder, string file, string extension, byte[] data, object state = null)
        {
            string path = AssetFinder.GetPath(folder, file, extension);
            return IoBinary.Write(path, data, state);
        }
        #endregion
    }
}
