using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Ad;
using KTool.Init;

namespace KPlugin.MaxMediation
{
    public class MaxAdBanner : MaxAd, IAdBanner
    {
        #region Properties
        [SerializeField]
        [SelectAdId(MaxAdType.Banner)]
        private int indexAd = 0;
        [SerializeField]
        private bool isAutoCreate = true;
        [SerializeField]
        private AdPosition adPosition = AdPosition.BotCenter;
        [SerializeField]
        private Vector2 customAdPosition = Vector2.zero;
        [SerializeField]
        private Color backgroundColor = Color.white;
        [SerializeField]
        private bool isAutoReload = true;
        [SerializeField]
        private bool isShowAfterInit = true;

        private AdPosition currentAdPosition;
        private Vector2 currentPosition;
        private bool isCreated,
            isLoading,
            isLoaded,
            isShow;
        private bool isExpanded;
        private bool isShowOnLoaded;

        public event IAd.OnAdExpanded OnAdExpandedEvent;

        public override AdType AdType => AdType.Banner;
        private string AdId
        {
            get
            {
                MaxSettingId settingAdId = MaxSetting.Instance.Ad_Get(MaxAdType.Banner, indexAd);
                if (settingAdId != null)
                    return settingAdId.AdID;
                return string.Empty;
            }
        }
        public override bool IsAutoReload
        {
            get => isAutoReload;
            set
            {
                isAutoReload = value;
                if (IsAutoReload)
                    MaxSdk.StartBannerAutoRefresh(AdId);
                else
                    MaxSdk.StopBannerAutoRefresh(AdId);
            }
        }
        public Color BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                MaxSdk.SetBannerBackgroundColor(AdId, backgroundColor);
            }
        }
        public AdPosition PositionType => currentAdPosition;
        public Vector2 Position => currentPosition;
        public AdSize SizeType => AdSize.Standard;
        public Vector2 Size => MaxUtils.GetUnityScreen_BannerSize();
        public override bool IsCreated => isCreated;
        public override bool IsLoaded => isLoaded;
        public override bool IsLoading => IsAutoReload || isLoading;
        public override bool IsShow => isShow;
        public bool IsExpanded => isExpanded;
        public bool IsReady => IsLoaded;
        #endregion

        #region Unity Event

        #endregion

        #region Method

        #endregion

        #region Init
        protected override void OnInitBegin()
        {
            AdManager.Instance.Add(this);
            //
            if (isAutoCreate)
                Create();
            else
                InitComplete = true;
        }
        protected override void OnInitEnd()
        {
            if (IsCreated && IsLoaded && (isShowAfterInit || isShowOnLoaded))
                Show();
        }
        #endregion

        #region Ad
        public void Create(AdPosition adPosition, AdSize adSize)
        {
            currentAdPosition = adPosition;
            if (currentAdPosition == AdPosition.Custom)
                currentPosition = customAdPosition;
            else
                currentPosition = MaxUtils.GetUnityScreen_BannerPosition(currentAdPosition);
            //
            Banner_Create();
        }
        public void Create(Vector2 position, Vector2 size)
        {
            currentAdPosition = AdPosition.Custom;
            currentPosition = Vector2Int.RoundToInt(position);
            //
            Banner_Create();
        }
        public override void Create()
        {
            currentAdPosition = adPosition;
            if (currentAdPosition == AdPosition.Custom)
                currentPosition = customAdPosition;
            else
                currentPosition = MaxUtils.GetUnityScreen_BannerPosition(currentAdPosition);
            //
            Banner_Create();
        }
        public override void Load()
        {
            if (!IsCreated)
                return;
            //
            if (IsAutoReload || IsLoading)
                return;
            isLoading = true;
            //
            MaxSdk.LoadBanner(AdId);
        }
        public override void Destroy()
        {
            if (!IsCreated)
                return;
            isCreated = false;
            //
            isLoaded = false;
            isLoading = false;
            if (IsShow)
            {
                isShow = false;
                Banner_OnHide();
            }
            else
                isShow = false;
            isShowOnLoaded = false;
            isExpanded = false;
            MaxSdk.DestroyBanner(AdId);
            Banner_EventUnRegister();
            //
            PushEvent_OnAdDestroy(MaxAdType.Banner);
        }
        public void Show()
        {
            if (IsShow)
                return;
            //
            if (IsLoaded)
            {
                isShow = true;
                isShowOnLoaded = false;
                MaxSdk.ShowBanner(AdId);
                Banner_OnDisplayed();
            }
            else
            {
                isShowOnLoaded = true;
            }
        }
        public void Hide()
        {
            if (!IsShow)
            {
                isShowOnLoaded = false;
                return;
            }
            //
            isShow = false;
            isShowOnLoaded = false;
            MaxSdk.HideBanner(AdId);
            Banner_OnHide();
        }
        #endregion

        #region Max Banner
        private void Banner_Create()
        {
            StartCoroutine(IE_Create());
        }
        private IEnumerator IE_Create()
        {
            while (MaxManager.Instance == null || !MaxManager.Instance.InitComplete)
            {
                yield return new WaitForEndOfFrame();
            }
            //
            if(currentAdPosition == AdPosition.Custom)
                Banner_Create(currentPosition);
            else
                Banner_Create(currentAdPosition);
        }
        private void Banner_Create(AdPosition positionType)
        {
            if (IsCreated)
                return;
            isCreated = true;
            //
            MaxSdkBase.BannerPosition positionMaxBanner = MaxUtils.Convert_BannerPosition(positionType);
            MaxSdk.CreateBanner(AdId, positionMaxBanner);
            //
            isLoading = true;
            Banner_Setup();
            Banner_EventRegister();
            //
            PushEvent_OnAdCreated(MaxAdType.Banner, true);
        }
        private void Banner_Create(Vector2 position)
        {
            if (IsCreated)
                return;
            isCreated = true;
            //
            Vector2 positionMaxScreen = MaxUtils.Convert_UnityScreen_To_MaxScreen(position);
            MaxSdk.CreateBanner(AdId, positionMaxScreen.x, positionMaxScreen.y);
            //
            isLoading = true;
            Banner_Setup();
            Banner_EventRegister();
            //
            PushEvent_OnAdCreated(MaxAdType.Banner, true);
        }
        private void Banner_Setup()
        {
            MaxSdk.SetBannerBackgroundColor(AdId, backgroundColor);
            if (IsAutoReload)
                MaxSdk.StartBannerAutoRefresh(AdId);
        }
        private void Banner_EventRegister()
        {
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += Banner_OnAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += Banner_OnAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += Banner_OnAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += Banner_OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent += Banner_OnAdExpandedEvent;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent += Banner_OnAdCollapsedEvent;
            MaxSdkCallbacks.Banner.OnAdReviewCreativeIdGeneratedEvent += Banner_OnAdReviewCreativeIdGeneratedEvent;
        }
        private void Banner_EventUnRegister()
        {
            MaxSdkCallbacks.Banner.OnAdLoadedEvent -= Banner_OnAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent -= Banner_OnAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent -= Banner_OnAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= Banner_OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent -= Banner_OnAdExpandedEvent;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent -= Banner_OnAdCollapsedEvent;
            MaxSdkCallbacks.Banner.OnAdReviewCreativeIdGeneratedEvent += Banner_OnAdReviewCreativeIdGeneratedEvent;
        }
        private void Banner_OnAdLoadedEvent(string adId, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            isLoading = false;
            isLoaded = true;
            //
            if (InitComplete)
            {
                if (isShowOnLoaded)
                {
                    Show();
                }
            }
            else
            {
                InitComplete = true;
                if (IsInitEnded && (isShowAfterInit || isShowOnLoaded))
                {
                    Show();
                }
            }
            //
            PushEvent_OnAdLoaded(MaxAdType.Banner, true);
        }
        private void Banner_OnAdLoadFailedEvent(string adId, MaxSdkBase.ErrorInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            isLoading = false;
            isLoaded = false;
            //
            PushEvent_OnAdLoaded(MaxAdType.Banner, false);
        }
        private void Banner_OnAdClickedEvent(string adId, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            PushEvent_OnAdClicked(MaxAdType.Banner);
        }
        private void Banner_OnAdRevenuePaidEvent(string adId, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            AdRevenuePaid revenuePaid = new AdRevenuePaid(
                MaxManager.MAX_SCOURCE,
                adInfo.NetworkName,
                AdId,
                MaxManager.Instance.CountryCode,
                AdType,
                adInfo.Revenue,
                MaxManager.MAX_CURRENCY);
            PushEvent_OnAdRevenuePaid(MaxAdType.Banner, revenuePaid);
        }
        private void Banner_OnAdExpandedEvent(string adId, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            isExpanded = true;
            //
            try
            {
                OnAdExpandedEvent?.Invoke(true);
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, AdType.ToString(), EVENT_NAME_ON_AD_EXPANDED, ex.Message);
                Debug.LogError(message);
            }
        }
        private void Banner_OnAdCollapsedEvent(string adId, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            isExpanded = false;
            //
            try
            {
                OnAdExpandedEvent?.Invoke(false);
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, AdType.ToString(), EVENT_NAME_ON_AD_EXPANDED, ex.Message);
                Debug.LogError(message);
            }
        }
        private void Banner_OnDisplayed()
        {
            PushEvent_OnAdDisplayed(MaxAdType.Banner, true);
        }
        private void Banner_OnDisplayFailed()
        {
            PushEvent_OnAdDisplayed(MaxAdType.Banner, false);
        }
        private void Banner_OnHide()
        {
            PushEvent_OnAdHidden(MaxAdType.Banner);
        }
        private void Banner_OnAdReviewCreativeIdGeneratedEvent(string adId, string adReviewCreativeId, MaxSdkBase.AdInfo adInfo)
        {

        }
        #endregion
    }
}
