using KTool.Advertisement;
using UnityEngine;

namespace KTool_Demo.Advertisement
{
    public class DemoAdBanner : MonoBehaviour
    {
        #region Properties

        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion

        #region Methods Unity Ui
        public void OnClick_Init()
        {
            AdBanner.Instance.Init();
        }
        public void OnClick_Load()
        {
            AdBanner.Instance.Load();
        }
        public void OnClick_Show()
        {
            AdBanner.Instance.Show();
        }
        public void OnClick_Hide()
        {
            AdBanner.Instance.Hide();
        }
        public void OnClick_Destroy()
        {
            AdBanner.Instance.Destroy();
        }
        #endregion
    }
}
