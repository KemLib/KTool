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

        #region Methods
        private IEnumerator IE_Show()
        {
            float delay = Mathf.Clamp(showTime, 3, 60),
                time = 0;
            while (time < delay)
            {
                time += Time.deltaTime;
                imgProgress.fillAmount = time / delay;
                yield return new WaitForEndOfFrame();
            }
            //
            currentTrackingSource?.ShowComplete(true);
            PushEvent_ShowComplete(true);
            AdRevenuePaid adRevenuePaid = new AdRevenuePaid(AdDemoManager.AdSource, string.Empty, AdDemoManager.adCountryCode, string.Empty, AdType.Banner, 1, AdDemoManager.AdCurrency);
            PushEvent_RevenuePaid(adRevenuePaid);
            currentTrackingSource.RevenuePaid(adRevenuePaid);
            //
            btnClose.gameObject.SetActive(true);
        }
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
        public override AdInterstitialTracking Show()
        {
            if (IsShow)
                return new AdInterstitialTrackingSource(ERROR_IS_SHOW);
            //
            currentTrackingSource = new AdInterstitialTrackingSource();
            //
            panelMenu.gameObject.SetActive(true);
            btnClose.gameObject.SetActive(false);
            StartCoroutine(IE_Show());
            //
            State = AdState.Show;
            currentTrackingSource.Displayed(true);
            PushEvent_Displayed(true);
            return currentTrackingSource;
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
        public void OnClick_Close()
        {
            if (!IsShow)
                return;
            //
            panelMenu.gameObject.SetActive(false);
            currentTrackingSource?.Hidden();
            PushEvent_Hidden();
            //
            State = AdState.Inited;
        }
        #endregion
    }
}
