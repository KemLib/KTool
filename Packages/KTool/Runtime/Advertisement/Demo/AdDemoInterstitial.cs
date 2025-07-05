using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KTool.Advertisement.Demo
{
    public class AdDemoInterstitial : AdInterstitial
    {
        #region Properties
        private const string ERROR_IS_SHOW = "Ad is show";

        public static AdDemoInterstitial InstanceAdDemo => AdDemoManager.Instance.AdInterstitial;

        [SerializeField]
        private Image panelMenu,
            imgProgress;
        [SerializeField]
        private Button btnClose;
        [SerializeField, Min(0)]
        private float showTime;

        private AdInterstitialTrackingSource currentTrackingSource;
        #endregion

        #region Methods Unity

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
        public override AdInterstitialTracking Show()
        {
            if (IsShow)
                return new AdInterstitialTrackingSource(ERROR_IS_SHOW);
            //
            if (!IsInited)
                Init();
            if (!IsLoaded)
                Load();
            IsShow = true;
            currentTrackingSource = new AdInterstitialTrackingSource(this);
            StartCoroutine(IE_Show());
            //
            return currentTrackingSource;
        }
        public override void Destroy()
        {
            IsDestroy = true;
            if (!IsShow)
                PushEvent_Destroy();
        }
        private IEnumerator IE_Show()
        {
            yield return new WaitForEndOfFrame();
            //
            panelMenu.gameObject.SetActive(true);
            btnClose.gameObject.SetActive(false);
            currentTrackingSource.Displayed(true);
            PushEvent_Displayed(true);
            //
            float delay = Mathf.Clamp(showTime, 3, 60),
                time = 0;
            while (!IsDestroy && time < delay)
            {
                time += Time.deltaTime;
                imgProgress.fillAmount = time / delay;
                yield return new WaitForEndOfFrame();
            }
            //
            AdRevenuePaid adRevenuePaid = new AdRevenuePaid(AdDemoManager.AdSource, string.Empty, AdDemoManager.adCountryCode, string.Empty, AdType.Banner, 1, AdDemoManager.AdCurrency);
            PushEvent_RevenuePaid(adRevenuePaid);
            currentTrackingSource.RevenuePaid(adRevenuePaid);
            //
            btnClose.gameObject.SetActive(true);
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
        public void OnClick_Close()
        {
            if (!IsShow)
                return;
            //
            panelMenu.gameObject.SetActive(false);
            btnClose.gameObject.SetActive(false);
            IsShow = false;
            currentTrackingSource?.Hidden();
            PushEvent_Hidden();
            if (IsDestroy)
                PushEvent_Destroy();
        }
        #endregion
    }
}
