using UnityEngine;
using KTool.Advertisement;

namespace KTool_Demo.Advertisement
{
    public class DemoAdInterstitial : MonoBehaviour
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
            AdInterstitial.Instance.Init();
        }
        public void OnClick_Load()
        {
            AdInterstitial.Instance.Load();
        }
        public void OnClick_Show()
        {
            AdInterstitial.Instance.Show();
        }
        public void OnClick_Destroy()
        {
            AdInterstitial.Instance.Destroy();
        }
        #endregion
    }
}
