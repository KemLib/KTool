using KTool.Advertisement;
using UnityEngine;

namespace KTool_Demo.Advertisement
{
    public class DemoAdAppOpen : MonoBehaviour
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
            AdAppOpen.Instance.Init();
        }
        public void OnClick_Load()
        {
            AdAppOpen.Instance.Load();
        }
        public void OnClick_Show()
        {
            AdAppOpen.Instance.Show();
        }
        public void OnClick_Destroy()
        {
            AdAppOpen.Instance.Destroy();
        }
        #endregion
    }
}
