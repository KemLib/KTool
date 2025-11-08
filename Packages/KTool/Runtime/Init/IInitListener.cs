namespace KTool.Init
{
    public interface IInitListener
    {
        #region Properties

        #endregion

        #region Methods
        public void Init_OnShow();
        public void Init_OnHide();
        public void Init_OnProgress(float progress);
        public void Init_OnTitle(string title);
        #endregion
    }
}
