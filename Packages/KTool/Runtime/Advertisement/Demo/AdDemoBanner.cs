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
            State = AdState.Inited;
            PushEvent_Inited();
        }
        public override void Load()
        {
            State = AdState.Loaded;
            PushEvent_Loaded(true);
            State = AdState.Ready;
        }
        public override AdBannerTracking Show()
        {
            if (IsShow)
                return new AdBannerTrackingSource(ERROR_IS_SHOW);
            //
            currentTrackingSource = new AdBannerTrackingSource();
            //
            IsExpanded = false;
            currentBanner = BannerSelect;
            currentBanner.gameObject.SetActive(true);
            //
            State = AdState.Show;
            currentTrackingSource.Displayed(true);
            PushEvent_Displayed(true);
            return currentTrackingSource;
        }
        public override void Hide()
        {
            if (!IsShow)
                return;
            //
            IsExpanded = false;
            //
            currentTrackingSource?.ShowComplete(true);
            PushEvent_ShowComplete(true);
            AdRevenuePaid adRevenuePaid = new AdRevenuePaid(AdDemoManager.AdSource, string.Empty, AdDemoManager.adCountryCode, string.Empty, AdType.Banner, 1, AdDemoManager.AdCurrency);
            PushEvent_RevenuePaid(adRevenuePaid);
            currentTrackingSource.RevenuePaid(adRevenuePaid);
            //
            currentBanner.gameObject.SetActive(false);
            currentTrackingSource?.Hidden();
            PushEvent_Hidden();
            //
            State = AdState.Inited;
        }
        public override void Destroy()
        {
            State = AdState.Destroy;
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
            IsExpanded = !IsExpanded;
            currentTrackingSource?.Expanded(IsExpanded);
        }
        #endregion
    }
}
