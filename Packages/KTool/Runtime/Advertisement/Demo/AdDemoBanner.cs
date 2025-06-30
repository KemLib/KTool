using UnityEngine;
using UnityEngine.UI;

namespace KTool.Advertisement.Demo
{
    public class AdDemoBanner : AdBanner
    {
        #region Properties
        private const string ERROR_IS_DESTROY = "Ad is destroy",
            ERROR_NOT_READY = "Ad not ready",
            ERROR_IS_SHOW = "Ad is show";

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
            if (IsDestroy || IsInited)
                return;
            //
            IsInited = true;
            PushEvent_Inited();
        }
        public override void Load()
        {
            if (!IsInited || IsLoaded)
                return;
            //
            IsLoaded = true;
            PushEvent_Loaded(true);
        }
        public override AdBannerTracking Show()
        {
            if (IsDestroy)
                return new AdBannerTrackingSource(ERROR_IS_DESTROY);
            if (!IsReady)
                return new AdBannerTrackingSource(ERROR_NOT_READY);
            if (IsShow)
                return new AdBannerTrackingSource(ERROR_IS_SHOW);
            //
            currentTrackingSource = new AdBannerTrackingSource(this);
            //
            IsShow = true;
            //
            currentBanner = BannerSelect;
            currentBanner.gameObject.SetActive(true);
            currentTrackingSource.Displayed(true);
            PushEvent_Displayed(true);
            //
            return currentTrackingSource;
        }
        public override void Hide()
        {
            if (!IsShow)
                return;
            //
            AdRevenuePaid adRevenuePaid = new AdRevenuePaid(AdDemoManager.AdSource, string.Empty, AdDemoManager.adCountryCode, string.Empty, AdType.Banner, 1, AdDemoManager.AdCurrency);
            PushEvent_RevenuePaid(adRevenuePaid);
            currentTrackingSource.RevenuePaid(adRevenuePaid);
            //
            IsShow = false;
            //
            currentBanner.gameObject.SetActive(false);
            currentTrackingSource?.Hidden();
            PushEvent_Hidden();
        }
        public override void Destroy()
        {
            if (IsDestroy)
                return;
            //
            IsDestroy = true;
            if (IsShow)
                Hide();
            PushEvent_Destroy();
        }
        #endregion

        #region Unity Ui Event
        public void OnClick_Ad()
        {
            if (!IsShow)
                return;
            //
            currentTrackingSource?.Clicked();
            PushEvent_Clicked();
        }
        public void OnClick_Expanded()
        {
            if (!IsShow)
                return;
            //
            currentTrackingSource?.Expanded(IsExpanded);
        }
        #endregion
    }
}
