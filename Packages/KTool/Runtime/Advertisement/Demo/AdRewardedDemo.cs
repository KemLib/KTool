using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KTool.Advertisement.Demo
{
    public class AdRewardedDemo : AdRewarded
    {
        #region Properties
        public static AdRewardedDemo InstanceAdRewarded => AdManagerDemo.Instance.AdRewarded;

        [SerializeField]
        private Image rewarded,
            imgProgress;
        [SerializeField, Min(0)]
        private float showTime;

        private AdRewardedTrackingSource currentTrackingSource;
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
            PushEvent_ReceivedReward(AdRewardReceived.RewardSuccess);
            currentTrackingSource.ReceivedReward(AdRewardReceived.RewardSuccess);
            AdRevenuePaid adRevenuePaid = new AdRevenuePaid(AdManagerDemo.AdSource, string.Empty, AdManagerDemo.adCountryCode, string.Empty, AdType.Banner, 1, AdManagerDemo.AdCurrency);
            PushEvent_RevenuePaid(adRevenuePaid);
            currentTrackingSource.RevenuePaid(adRevenuePaid);
            //
            rewarded.gameObject.SetActive(false);
            currentTrackingSource?.Hidden();
            PushEvent_Hidden();
            //
            State = AdState.Inited;
        }
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
        public override AdRewardedTracking Show()
        {
            if (!IsReady)
                return null;
            //
            currentTrackingSource = new AdRewardedTrackingSource();
            //
            rewarded.gameObject.SetActive(true);
            StartCoroutine(IE_Show());
            //
            State = AdState.Show;
            currentTrackingSource.Displayed(true);
            PushEvent_Displayed(true);
            return currentTrackingSource;
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
        #endregion
    }
}
