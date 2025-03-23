using UnityEngine;
using KTool.Advertisement;

namespace KTool_Demo.Advertisement
{
    public class DemoAdRewarded : MonoBehaviour
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
            AdRewarded.Instance.Init();
        }
        public void OnClick_Load()
        {
            AdRewarded.Instance.Load();
        }
        public void OnClick_Show()
        {
            AdRewarded.Instance.Show();
        }
        public void OnClick_Destroy()
        {
            AdRewarded.Instance.Destroy();
        }
        #endregion
    }
}
