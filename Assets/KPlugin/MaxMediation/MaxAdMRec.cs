using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Ad;
using KTool.Init;

namespace KPlugin.MaxMediation
{
	public class MaxAdMRec : MaxAd, IAdMRec
    {
        #region Properties
        [SerializeField]
        [SelectAdId(MaxAdType.MRec)]
        private int indexAd = 0;
        [SerializeField]
        private bool isAutoCreate = true;
        [SerializeField]
        private AdPosition adPosition = AdPosition.BotCenter;
        [SerializeField]
        private Vector2 customAdPosition = Vector2.zero;
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

        public override AdType AdType => AdType.MRec;
        private string AdId
        {
            get
            {
                MaxSettingId settingAdId = MaxSetting.Instance.Ad_Get(MaxAdType.MRec, indexAd);
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
                    MaxSdk.StartMRecAutoRefresh(AdId);
                else
                    MaxSdk.StopMRecAutoRefresh(AdId);
            }
        }
        public AdPosition PositionType => currentAdPosition;
        public Vector2 Position => currentPosition;
        public AdSize SizeType => AdSize.Standard;
        public Vector2 Size => MaxUtils.GetUnityScreen_MRecSize();
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
                currentPosition = MaxUtils.GetUnityScreen_MRecPosition(currentAdPosition);
            //
            MRec_Create();
        }
        public void Create(Vector2 position, Vector2 size)
        {
            currentAdPosition = AdPosition.Custom;
            currentPosition = Vector2Int.RoundToInt(position);
            //
            MRec_Create();
        }
        public override void Create()
        {
            currentAdPosition = adPosition;
            if (currentAdPosition == AdPosition.Custom)
                currentPosition = customAdPosition;
            else
                currentPosition = MaxUtils.GetUnityScreen_MRecPosition(currentAdPosition);
            //
            MRec_Create();
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
            MaxSdk.LoadMRec(AdId);
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
                MRec_OnHide();
            }
            else
                isShow = false;
            isShowOnLoaded = false;
            isExpanded = false;
            MaxSdk.DestroyMRec(AdId);
            MRec_EventUnRegister();
            //
            PushEvent_OnAdDestroy(MaxAdType.MRec);
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
                MaxSdk.ShowMRec(AdId);
                MRec_OnDisplayed();
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
            MaxSdk.HideMRec(AdId);
            MRec_OnHide();
        }
        #endregion

        #region Max MRec
        private void MRec_Create()
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
            if (currentAdPosition == AdPosition.Custom)
                MRec_Create(currentPosition);
            else
                MRec_Create(currentAdPosition);
        }
        private void MRec_Create(AdPosition positionType)
        {
            if (IsCreated)
                return;
            isCreated = true;
            //
            MaxSdkBase.AdViewPosition positionMaxMRec = MaxUtils.Convert_AdViewPosition(positionType);
            MaxSdk.CreateMRec(AdId, positionMaxMRec);
            //
            isLoading = true;
            MRec_Setup();
            MRec_EventRegister();
            //
            PushEvent_OnAdCreated(MaxAdType.MRec, true);
        }
        private void MRec_Create(Vector2 position)
        {
            if (IsCreated)
                return;
            isCreated = true;
            //
            Vector2 positionMaxScreen = MaxUtils.Convert_UnityScreen_To_MaxScreen(position);
            MaxSdk.CreateMRec(AdId, positionMaxScreen.x, positionMaxScreen.y);
            //
            isLoading = true;
            MRec_Setup();
            MRec_EventRegister();
            //
            PushEvent_OnAdCreated(MaxAdType.MRec, true);
        }
        private void MRec_Setup()
        {
            if (IsAutoReload)
                MaxSdk.StartMRecAutoRefresh(AdId);
        }
        private void MRec_EventRegister()
        {
            MaxSdkCallbacks.MRec.OnAdLoadedEvent += MRec_OnAdLoadedEvent;
            MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += MRec_OnAdLoadFailedEvent;
            MaxSdkCallbacks.MRec.OnAdClickedEvent += MRec_OnAdClickedEvent;
            MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += MRec_OnAdRevenuePaidEvent;
            MaxSdkCallbacks.MRec.OnAdExpandedEvent += MRec_OnAdExpandedEvent;
            MaxSdkCallbacks.MRec.OnAdCollapsedEvent += MRec_OnAdCollapsedEvent;
            MaxSdkCallbacks.MRec.OnAdReviewCreativeIdGeneratedEvent += MRec_OnAdReviewCreativeIdGeneratedEvent;
        }
        private void MRec_EventUnRegister()
        {
            MaxSdkCallbacks.MRec.OnAdLoadedEvent -= MRec_OnAdLoadedEvent;
            MaxSdkCallbacks.MRec.OnAdLoadFailedEvent -= MRec_OnAdLoadFailedEvent;
            MaxSdkCallbacks.MRec.OnAdClickedEvent -= MRec_OnAdClickedEvent;
            MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent -= MRec_OnAdRevenuePaidEvent;
            MaxSdkCallbacks.MRec.OnAdExpandedEvent -= MRec_OnAdExpandedEvent;
            MaxSdkCallbacks.MRec.OnAdCollapsedEvent -= MRec_OnAdCollapsedEvent;
            MaxSdkCallbacks.MRec.OnAdReviewCreativeIdGeneratedEvent += MRec_OnAdReviewCreativeIdGeneratedEvent;
        }
        private void MRec_OnAdLoadedEvent(string adId, MaxSdkBase.AdInfo adInfo)
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
            PushEvent_OnAdLoaded(MaxAdType.MRec, true);
        }
        private void MRec_OnAdLoadFailedEvent(string adId, MaxSdkBase.ErrorInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            isLoading = false;
            isLoaded = false;
            //
            PushEvent_OnAdLoaded(MaxAdType.MRec, false);
        }
        private void MRec_OnAdClickedEvent(string adId, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            PushEvent_OnAdClicked(MaxAdType.MRec);
        }
        private void MRec_OnAdRevenuePaidEvent(string adId, MaxSdkBase.AdInfo adInfo)
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
            PushEvent_OnAdRevenuePaid(MaxAdType.MRec, revenuePaid);
        }
        private void MRec_OnAdExpandedEvent(string adId, MaxSdkBase.AdInfo adInfo)
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
        private void MRec_OnAdCollapsedEvent(string adId, MaxSdkBase.AdInfo adInfo)
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
        private void MRec_OnDisplayed()
        {
            PushEvent_OnAdDisplayed(MaxAdType.MRec, true);
        }
        private void MRec_OnDisplayFailed()
        {
            PushEvent_OnAdDisplayed(MaxAdType.MRec, false);
        }
        private void MRec_OnHide()
        {
            PushEvent_OnAdHidden(MaxAdType.MRec);
        }
        private void MRec_OnAdReviewCreativeIdGeneratedEvent(string adId, string adReviewCreativeId, MaxSdkBase.AdInfo adInfo)
        {

        }
        #endregion
    }
}
