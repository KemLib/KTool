using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KTool.Advertisement.Demo
{
    public class AdDemoBanner : AdBanner
    {
        #region Properties
        private const string ERROR_IS_SHOW = "Ad is show";

        public static AdDemoBanner InstanceAdDemo => AdDemoManager.Instance.AdBanner;

        [SerializeField]
        private Image bannerBot,
            bannerMid,
            bannerTop;

        private Image currentBanner;
        private AdBannerTrackingSource currentTrackingSource;

        private Image BannerSelect
        {
            get
            {
                switch (PositionType)
                {
                    case AdPosition.TopLeft:
                    case AdPosition.TopCenter:
                    case AdPosition.TopRight:
                        return bannerTop;
                    case AdPosition.MidLeft:
                    case AdPosition.MidCenter:
                    case AdPosition.MidRight:
                        return bannerMid;
                    case AdPosition.BotLeft:
                    case AdPosition.BotCenter:
                    case AdPosition.BotRight:
                        return bannerBot;
                    default:
                        return bannerBot;
                }
            }
        }
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion

        #region Ad
        public override void Init()
        {
            IsInited = true;
            PushEvent_Inited(true);
        }
        public override void Load()
        {
            IsLoaded = true;
            PushEvent_Loaded(true);
        }
        public override IAdBannerTracking Show()
        {
            if (IsShow)
                return new AdBannerTrackingSource(this, ERROR_IS_SHOW);
            //
            if (!IsInited)
                Init();
            if (!IsLoaded)
                Load();
            IsShow = true;
            currentTrackingSource = new AdBannerTrackingSource(this);
            StartCoroutine(IE_Show());
            //
            return currentTrackingSource;
        }
        public override void Hide()
        {
            if (!IsShow)
                return;
            //
            currentBanner.gameObject.SetActive(false);
            IsShow = false;
            currentTrackingSource?.PushEvent_Hidden();
            PushEvent_Hidden();
        }
        public override void Destroy()
        {
            IsDestroy = true;
            if (IsShow)
                Hide();
            PushEvent_Destroy();
        }
        private IEnumerator IE_Show()
        {
            yield return new WaitForEndOfFrame();
            //
            currentBanner = BannerSelect;
            currentBanner.gameObject.SetActive(true);
            currentTrackingSource.PushEvent_Displayed(true);
            PushEvent_Displayed(true);
            //
            yield return new WaitForEndOfFrame();
            //
            AdRevenuePaid adRevenuePaid = new AdRevenuePaid(AdDemoManager.AdSource, string.Empty, AdDemoManager.adCountryCode, string.Empty, AdType.Banner, 0, AdDemoManager.AdCurrency);
            PushEvent_RevenuePaid(adRevenuePaid);
            currentTrackingSource.PushEvent_RevenuePaid(adRevenuePaid);
        }
        #endregion

        #region Unity Ui Event
        public void OnClick_Ad()
        {
            if (!IsShow)
                return;
            //
            currentTrackingSource?.PushEvent_Clicked();
            PushEvent_Clicked();
        }
        public void OnClick_Expanded()
        {
            if (!IsShow)
                return;
            //
            currentTrackingSource?.PushEvent_Expanded(IsExpanded);
        }
        #endregion
    }
}
