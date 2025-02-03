using GoogleMobileAds.Api;
using KTool.Ad;
using System;
using System.Collections;
using UnityEngine;

namespace KPlugin.AdMob
{
    public class AdMobAdBanner : AdMobAd, IAdBanner
    {
        #region Properties
        private const int AD_EXPIRE_HOUR = 1;

        [SerializeField]
        [SelectAdId(AdMobAdType.Banner)]
        private int indexAd = 0;
        [SerializeField]
        private bool isAutoCreate = true;
        [SerializeField]
        private KTool.Ad.AdPosition adPosition = KTool.Ad.AdPosition.BotCenter;
        [SerializeField]
        private Vector2 customAdPosition = Vector2.zero;
        [SerializeField]
        private KTool.Ad.AdSize adSize = KTool.Ad.AdSize.Standard;
        [SerializeField]
        private Vector2 customAdSize = Vector2.zero;
        [SerializeField]
        private bool isShowAfterInit = true;

        private KTool.Ad.AdPosition currentAdPosition;
        private KTool.Ad.AdSize currentAdSize;
        private Vector2 currentPosition,
            currentSize;
        private bool isCurrentProperties,
            isLoading,
            isLoadFirst,
            isloaded,
            isShow,
            isExpanded;
        private int attemptLoad;
        private BannerView adObject;
        private DateTime expireTime;

        public event IAd.OnAdExpanded OnAdExpandedEvent;

        public override AdType AdType => AdType.Banner;
        public string AdId
        {
            get
            {
                AdMobSettingAdId settingAdId = AdMobSetting.Instance.Ad_Get(AdMobAdType.Banner, indexAd);
                if (settingAdId != null)
                    return settingAdId.AdID;
                return string.Empty;
            }
        }
        public KTool.Ad.AdPosition PositionType => currentAdPosition;
        public KTool.Ad.AdSize SizeType => currentAdSize;
        public Vector2 Position => currentPosition;
        public Vector2 Size => currentSize;
        public override bool IsAutoReload
        {
            get => true;
            set
            {

            }
        }
        public override bool IsCreated => (adObject != null);
        public override bool IsLoading => isLoading;
        public override bool IsLoaded => isloaded;
        public override bool IsShow => isShow;
        public bool IsExpanded => isExpanded;
        public bool IsReady => IsCreated && IsLoaded;
        #endregion

        #region Unity Event
        private void Update()
        {
            Update_ExpireTime();
        }
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
            if (isShowAfterInit)
                Show();
        }
        #endregion

        #region Method
        public void Create(KTool.Ad.AdPosition adPosition, KTool.Ad.AdSize adSize)
        {
            currentAdPosition = adPosition;
            currentAdSize = adSize;
            if (currentAdSize == KTool.Ad.AdSize.Custom)
                currentSize = customAdSize;
            else
                currentSize = AdMobUtils.Unity_Get(currentAdSize);
            if (currentAdPosition == KTool.Ad.AdPosition.Custom)
                currentPosition = customAdPosition;
            else
                currentPosition = AdMobUtils.Unity_Get(currentAdPosition, currentSize);
            isCurrentProperties = true;
            //
            Ad_Create();
        }
        public void Create(Vector2 position, Vector2 size)
        {
            currentAdPosition = KTool.Ad.AdPosition.Custom;
            currentAdSize = KTool.Ad.AdSize.Custom;
            currentPosition = Vector2Int.RoundToInt(position);
            currentSize = Vector2Int.RoundToInt(size);
            isCurrentProperties = true;
            //
            Ad_Create();
        }
        public override void Create()
        {
            currentAdPosition = adPosition;
            currentAdSize = adSize;
            if (currentAdSize == KTool.Ad.AdSize.Custom)
                currentSize = customAdSize;
            else
                currentSize = AdMobUtils.Unity_Get(currentAdSize);
            if (currentAdPosition == KTool.Ad.AdPosition.Custom)
                currentPosition = customAdPosition;
            else
                currentPosition = AdMobUtils.Unity_Get(currentAdPosition, currentSize);
            isCurrentProperties = true;
            //
            Ad_Create();
        }
        public override void Destroy()
        {
            if (!IsCreated)
                return;
            //
            Ad_Destroy();
        }
        public override void Load()
        {
            if (isCurrentProperties)
                Ad_Create();
            else
                Create();
        }
        public void Show()
        {
            if (IsShow)
                return;
            isShow = true;
            //
            if (IsCreated && IsLoaded)
            {
                adObject.Show();
                //
                PushEvent_OnAdDisplayed(AdMobAdType.Banner, true);
            }
        }
        public void Hide()
        {
            if (!IsShow)
                return;
            isShow = false;
            //
            if (IsCreated && IsLoaded)
            {
                adObject.Hide();
                //
                PushEvent_OnAdHidden(AdMobAdType.Banner);
            }
        }
        private void Update_ExpireTime()
        {
            if (!IsCreated || !IsLoaded || IsLoading || IsShow)
                return;
            if (DateTime.Now < expireTime)
                return;
            Ad_Load();
        }
        #endregion

        #region Ad
        private void Ad_Create()
        {
            Ad_Destroy();
            //
            StartCoroutine(Ad_IE_Create());
        }
        private void Ad_Destroy()
        {
            if (adObject == null)
                return;
            //
            isExpanded = false;
            isShow = false;
            isloaded = false;
            isLoading = false;
            adObject.Destroy();
            adObject = null;
            PushEvent_OnAdDestroy(AdMobAdType.AppOpen);
        }
        private IEnumerator Ad_IE_Create()
        {
            while (AdMobManager.Instance == null || !AdMobManager.Instance.InitComplete)
            {
                yield return new WaitForEndOfFrame();
            }
            //
            Vector2Int size = Vector2Int.RoundToInt(AdMobUtils.Convert_UnityToAdMob(currentSize)),
                position = Vector2Int.RoundToInt(AdMobUtils.Convert_UnityToAdMob(currentPosition));
            GoogleMobileAds.Api.AdSize adSize = new GoogleMobileAds.Api.AdSize(size.x, size.y);
            adObject = new BannerView(AdId, adSize, position.x, position.y);
            Ad_EventRegister();
            PushEvent_OnAdCreated(AdMobAdType.Banner, true);
            //
            isLoadFirst = true;
            Ad_Load();
        }
        private void Ad_EventRegister()
        {
            adObject.OnBannerAdLoaded += Ad_OnBannerAdLoaded;
            adObject.OnBannerAdLoadFailed += Ad_OnBannerAdLoadFailed;
            adObject.OnAdFullScreenContentOpened += Ad_OnAdFullScreenContentOpened;
            adObject.OnAdClicked += Ad_OnAdClicked;
            adObject.OnAdPaid += Ad_OnAdPaid;
            adObject.OnAdFullScreenContentClosed += Ad_OnAdFullScreenContentClosed;
            adObject.OnAdImpressionRecorded += Ad_OnAdImpressionRecorded;
        }
        private void Ad_Load()
        {
            if (IsLoading)
                return;
            isLoading = true;
            //
            AdRequest adRequest = new AdRequest();
            adObject.LoadAd(adRequest);
        }
        private IEnumerator IE_Ad_Load(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            Ad_Load();
        }
        private void Ad_OnBannerAdLoadFailed(LoadAdError obj)
        {
            isLoading = false;
            if (isLoadFirst)
            {
                attemptLoad = Mathf.Min(attemptLoad + 1, 6);
                float delay = Mathf.Pow(2, attemptLoad);
                StartCoroutine(IE_Ad_Load(delay));
            }
            PushEvent_OnAdLoaded(AdMobAdType.Banner, false);
        }

        private void Ad_OnBannerAdLoaded()
        {
            isLoading = false;
            isloaded = true;
            isLoadFirst = false;
            attemptLoad = 0;
            expireTime = DateTime.Now + TimeSpan.FromHours(AD_EXPIRE_HOUR);
            if (!InitComplete)
                InitComplete = true;
            adObject.Hide();
            //
            PushEvent_OnAdLoaded(AdMobAdType.Banner, true);
            //
            if (IsShow)
            {
                adObject.Show();
                //
                PushEvent_OnAdDisplayed(AdMobAdType.Banner, true);
            }
        }

        private void Ad_OnAdFullScreenContentOpened()
        {
            isExpanded = true;
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
        private void Ad_OnAdClicked()
        {
            PushEvent_OnAdClicked(AdMobAdType.Banner);
        }
        private void Ad_OnAdFullScreenContentClosed()
        {
            isExpanded = false;
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
        private void Ad_OnAdPaid(AdValue adValue)
        {
            AdRevenuePaid revenuePaid = new AdRevenuePaid(
                AdMobManager.ADMOB_SCOURCE,
                AdMobManager.ADMOB_SCOURCE,
                AdId,
                AdMobManager.ADMOB_COUNTRY_CODE,
                AdType,
                adValue.Value,
                adValue.CurrencyCode);
            PushEvent_OnAdRevenuePaid(AdMobAdType.Banner, revenuePaid);
        }
        private void Ad_OnAdImpressionRecorded()
        {

        }
        #endregion
    }
}
