using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KTool.Advertisement.Demo
{
    public class AdDemoAppOpen : AdAppOpen
    {
        #region Properties
        private const string ERROR_IS_DESTROY = "Ad is destroy",
            ERROR_NOT_READY = "Ad not ready",
            ERROR_IS_SHOW = "Ad is show";

        public static AdDemoAppOpen InstanceAdDemo => AdDemoManager.Instance.AdAppOpen;

        [SerializeField]
        private Image panelMenu,
            imgProgress;
        [SerializeField]
        private Button btnClose;
        [SerializeField, Min(0)]
        private float showTime;

        private AdAppOpenTrackingSource currentTrackingSource;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods
        private IEnumerator IE_Show()
        {
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
        public override AdAppOpenTracking Show()
        {
            if (IsDestroy)
                return new AdAppOpenTrackingSource(ERROR_IS_DESTROY);
            if (!IsReady)
                return new AdAppOpenTrackingSource(ERROR_NOT_READY);
            if (IsShow)
                return new AdAppOpenTrackingSource(ERROR_IS_SHOW);
            //
            IsShow = true;
            currentTrackingSource = new AdAppOpenTrackingSource(this);
            StartCoroutine(IE_Show());
            //
            return currentTrackingSource;
        }
        public override void Destroy()
        {
            if (IsDestroy)
                return;
            //
            IsDestroy = true;
            if (!IsShow)
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
            IsShow = false;
            panelMenu.gameObject.SetActive(false);
            currentTrackingSource?.Hidden();
            PushEvent_Hidden();
            if (IsDestroy)
                PushEvent_Destroy();
        }
        #endregion
    }
}
