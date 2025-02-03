using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Ad;
using KTool.Init;

namespace KPlugin.MaxMediation
{
    public class MaxAdAppResume : MonoBehaviour, IInit, IAdAppResume
    {
        public enum AdTagetType
        {
            AdAppOpen,
            AdInterstitial
        }
        #region Properties
        [SerializeField]
        private InitType initType = InitType.Optional;
        [SerializeField]
        private AdTagetType adTagetType = AdTagetType.AdAppOpen;
        [SerializeField]
        private MaxAdAppOpen adAppOpen;
        [SerializeField]
        private MaxAdInterstitial adInterstitial;
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
                OnAppStateChanged(true);
            else
                OnAppStateChanged(false);
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
            while (MaxManager.Instance == null || !MaxManager.Instance.InitComplete)
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

        private void OnAppStateChanged(bool focus)
        {
            if (!InitComplete || !initEnd || !IsActive)
                return;
            // if the app is Foregrounded and the ad is available, show it.
            if (focus)
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
            }
        }
        #endregion
    }
}
