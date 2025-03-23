namespace Packages.KTool.Editor.AssetCreater
{
    public interface ICreater
    {
        #region Properties
        public string CreaterName
        {
            get;
        }
        public bool SaveAndClose
        {
            get;
        }
        #endregion

        #region Method
        public void OnGuiShow(CreateWindow createWindow);
        public void OnGuiDraw(CreateWindow createWindow);
        public void OnSave(CreateWindow createWindow);
        public void OnCancel(CreateWindow createWindow);
        #endregion
    }
}
