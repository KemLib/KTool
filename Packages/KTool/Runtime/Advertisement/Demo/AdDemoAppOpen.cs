using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KTool.Advertisement.Demo
{
    public class AdDemoAppOpen : AdAppOpen
    {
        #region Properties
        private const string ERROR_IS_SHOW = "Ad is show";

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
        public override IAdTracking Show()
        {
            if (IsShow)
                return new AdAppOpenTrackingSource(this, ERROR_IS_SHOW);
            //
            if (!IsInited)
                Init();
            if (!IsLoaded)
                Load();
            IsShow = true;
            currentTrackingSource = new AdAppOpenTrackingSource(this);
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
            currentTrackingSource.PushEvent_Displayed(true);
            PushEvent_Displayed(true);
            //
            float delay = Mathf.Clamp(showTime, 3, 60),
                time = 0;
            while (!IsDestroy && time < delay)
            {
                time += Time.unscaledDeltaTime;
                imgProgress.fillAmount = time / delay;
                yield return new WaitForEndOfFrame();
            }
            //
            AdRevenuePaid adRevenuePaid = new AdRevenuePaid(AdDemoManager.AdSource, string.Empty, AdDemoManager.adCountryCode, string.Empty, AdType.Banner, 0, AdDemoManager.AdCurrency);
            PushEvent_RevenuePaid(adRevenuePaid);
            currentTrackingSource.PushEvent_RevenuePaid(adRevenuePaid);
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
            currentTrackingSource?.PushEvent_Clicked();
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
            currentTrackingSource?.PushEvent_Clicked();
            PushEvent_Hidden();
            if (IsDestroy)
                PushEvent_Destroy();
        }
        #endregion
    }
}
