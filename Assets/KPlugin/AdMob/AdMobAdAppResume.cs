using GoogleMobileAds.Common;
using KTool.Ad;
using KTool.Init;
using System.Collections;
using UnityEngine;

namespace KPlugin.AdMob
{
    public class AdMobAdAppResume : MonoBehaviour, IInit, IAdAppResume
    {
        public enum AdTagetType
        {
            AdAppOpen,
            AdInterstitial,
            AdRewardedInterstitial
        }
        #region Properties
        [SerializeField]
        private InitType initType = InitType.Optional;
        [SerializeField]
        private AdTagetType adTagetType = AdTagetType.AdAppOpen;
        [SerializeField]
        private AdMobAdAppOpen adAppOpen;
        [SerializeField]
        private AdMobAdInterstitial adInterstitial;
        [SerializeField]
        private AdMobAdRewardedInterstitial adRewardedInterstitial;
        [SerializeField]
        private bool isActive = true;

        private bool initComplete,
            initEnd;

        public event IAd.OnAdDisplayed OnAdDisplayed;
        public event IAd.OnAdHidden OnAdHidden;

        public string Name => gameObject.name;
        public InitType InitType => initType;
        public bool InitComplete => initComplete;
        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }
        #endregion

        #region Unity Event
        private void OnApplicationFocus(bool focus)
        {
            if (focus)
                OnAppStateChanged(AppState.Foreground);
            else
                OnAppStateChanged(AppState.Background);
        }
        #endregion

        #region Method
        public void InitBegin()
        {
            IAdAppResume.Instance = this;
            //
            StartCoroutine(IE_Init());
        }

        public void InitEnd()
        {
            initEnd = true;
        }
        #endregion

        #region Ad
        private IEnumerator IE_Init()
        {
            while (AdMobManager.Instance == null || !AdMobManager.Instance.InitComplete)
            {
                yield return new WaitForEndOfFrame();
            }
            //
            switch (adTagetType)
            {
                case AdTagetType.AdAppOpen:
                    if (adAppOpen != null)
                    {
                        adAppOpen.OnAdDisplayedEvent += AdAppOpen_OnAdDisplayedEvent;
                        adAppOpen.OnAdHiddenEvent += AdAppOpen_OnAdHiddenEvent;
                        while (!adAppOpen.IsLoaded)
                            yield return new WaitForEndOfFrame();
                    }
                    break;
                case AdTagetType.AdInterstitial:
                    if (adInterstitial != null)
                    {
                        adInterstitial.OnAdDisplayedEvent += AdAppOpen_OnAdDisplayedEvent;
                        adInterstitial.OnAdHiddenEvent += AdAppOpen_OnAdHiddenEvent;
                        while (!adInterstitial.IsLoaded)
                            yield return new WaitForEndOfFrame();
                    }
                    break;
                case AdTagetType.AdRewardedInterstitial:
                    if (adRewardedInterstitial != null)
                    {
                        adRewardedInterstitial.OnAdDisplayedEvent += AdAppOpen_OnAdDisplayedEvent;
                        adRewardedInterstitial.OnAdHiddenEvent += AdAppOpen_OnAdHiddenEvent;
                        while (!adRewardedInterstitial.IsLoaded)
                            yield return new WaitForEndOfFrame();
                    }
                    break;
            }
            initComplete = true;
            yield break;
        }

        private void AdAppOpen_OnAdDisplayedEvent(bool isSuccess)
        {
            OnAdDisplayed?.Invoke(isSuccess);
        }
        private void AdAppOpen_OnAdHiddenEvent()
        {
            OnAdHidden?.Invoke();
        }

        private void OnAppStateChanged(AppState state)
        {
            if (!InitComplete || !initEnd || !IsActive)
                return;
            // if the app is Foregrounded and the ad is available, show it.
            if (state == AppState.Foreground)
                Show();
        }
        private void Show()
        {
            switch (adTagetType)
            {
                case AdTagetType.AdAppOpen:
                    if (adAppOpen != null)
                        adAppOpen.Show();
                    break;
                case AdTagetType.AdInterstitial:
                    if (adInterstitial != null)
                        adInterstitial.Show();
                    break;
                case AdTagetType.AdRewardedInterstitial:
                    if (adRewardedInterstitial != null)
                        adRewardedInterstitial.Show(null);
                    break;
            }
        }
        #endregion
    }
}
