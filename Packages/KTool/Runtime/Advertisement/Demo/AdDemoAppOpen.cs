using UnityEngine;
using UnityEngine.UI;

namespace KTool.Advertisement.Demo
{
    public class AdDemoAppOpen : AdAppOpen
    {
        #region Properties
        public const string ERROR_IS_SHOW = "Ad is show";

        public static AdDemoAppOpen InstanceAdDemo => AdDemoManager.Instance.AdAppOpen;

        [SerializeField]
        private Image panelMenu,
            imgProgress;
        [SerializeField, Min(0)]
        private float timeShow;

        private bool isClick;
        private float currentTime,
            tagetTime;

        private bool IsDisplayed
        {
            get => panelMenu.gameObject.activeSelf;
            set
            {
                panelMenu.gameObject.SetActive(value);
            }
        }
        #endregion

        #region Methods Unity
        private void Update()
        {
            if (IsShow)
            {
                if (IsDisplayed)
                {
                    currentTime += Time.unscaledDeltaTime;
                    if (currentTime >= tagetTime)
                    {
                        imgProgress.fillAmount = 1;
                        //
                        Hide();
                    }
                    else
                    {
                        imgProgress.fillAmount = currentTime / tagetTime;
                    }
                }
                else
                {
                    IsDisplayed = true;
                    currentTime = 0;
                    tagetTime = Mathf.Max(1, timeShow);
                    //
                    PushEvent_Displayed(true);
                }
            }
        }
        #endregion

        #region Ad
        public override void Load()
        {
            IsLoaded = true;
            PushEvent_Loaded(true);
        }
        protected override bool OnShow(out string error)
        {
            if (IsShow)
            {
                error = ERROR_IS_SHOW;
                return false;
            }
            //
            if (!IsLoaded)
                Load();
            isClick = false;
            IsShow = true;
            //
            error = string.Empty;
            return true;
        }
        public override void Destroy()
        {
            IsDestroy = true;
            if (!IsShow)
                PushEvent_Destroy();
        }
        private void Hide()
        {
            IsDisplayed = false;
            IsShow = false;
            //
            AdRevenuePaid revenue = new AdRevenuePaid(
                source: AdDemoManager.AdSource,
                network_name: AdDemoManager.AdNetwork,
                idAd: AdType.ToString(),
                adType: AdType,
                countryCode: AdDemoManager.adCountryCode,
                placement: Placement,
                value: 0,
                currency: AdDemoManager.AdCurrency);
            PushEvent_RevenuePaid(revenue);
            //
            PushEvent_Hidden();
            if (IsDestroy)
                PushEvent_Destroy();
        }
        #endregion

        #region Unity Ui Event
        public void OnClick_Ad()
        {
            if (!IsShow || isClick)
                return;
            //
            isClick = true;
            //
            PushEvent_Clicked();
        }
        public void OnClick_Close()
        {
            if (!IsShow)
                return;
            //
            Hide();
        }
        #endregion
    }
}
