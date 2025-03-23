using UnityEngine;
using UnityEngine.UI;

namespace KTool.Advertisement.Demo
{
    public class AdBannerDemo : AdBanner
    {
        #region Properties
        public static AdBannerDemo InstanceAdBanner => AdManagerDemo.Instance.AdBanner;

        [SerializeField]
        private Image bannerBot,
            bannerMid,
            bannerTop;
        [SerializeField]
        private Vector2 sizeStandard,
            sizeMedium,
            sizeFullSize;

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
        private Vector2 BannerSize
        {
            get
            {
                switch (SizeType)
                {
                    case AdSize.Standard:
                        return sizeStandard;
                    case AdSize.Medium:
                        return sizeMedium;
                    case AdSize.FullSize:
                        return sizeFullSize;
                    default:
                        return sizeStandard;
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
            if (State != AdState.None)
                return;
            //
            State = AdState.Inited;
            PushEvent_Inited();
        }
        public override void Load()
        {
            if (State != AdState.Inited)
                return;
            //
            State = AdState.Loaded;
            PushEvent_Loaded(true);
            State = AdState.Ready;
        }
        public override AdBannerTracking Show()
        {
            if (!IsReady)
                return null;
            //
            currentTrackingSource = new AdBannerTrackingSource();
            //
            IsExpanded = false;
            currentBanner = BannerSelect;
            RectTransform rtfBanner = currentBanner.GetComponent<RectTransform>();
            rtfBanner.sizeDelta = BannerSize;
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
            //
            currentBanner.gameObject.SetActive(false);
            currentTrackingSource?.Hidden();
            PushEvent_Hidden();
            //
            State = AdState.Inited;
        }
        public override void Destroy()
        {
            if (State == AdState.None)
                return;
            //
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
