namespace KTool.Init
{
    public interface IInit
    {
        #region Properties
        string Name
        {
            get;
        }
        InitType InitType
        {
            get;
        }
        bool InitComplete
        {
            get;
        }
        #endregion

        #region Method
        void InitBegin();
        void InitEnd();
        #endregion
    }
}
