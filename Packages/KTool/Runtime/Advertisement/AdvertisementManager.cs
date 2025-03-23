using UnityEngine;
using System.Collections.Generic;

namespace KTool.Advertisement
{
    public class AdvertisementManager : MonoBehaviour
    {
        #region Properties
        public static AdvertisementManager Instance
        {
            get;
            private set;
        }

        [SerializeField]
        private AdBanner[] banners;
        [SerializeField]
        private AdInterstitial[] interstitials;
        [SerializeField]
        private AdRewarded[] rewardeds;
        #endregion

        #region Methods Unity
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
            //
            Destroy(gameObject);
        }
        private void OnDestroy()
        {
            if (Instance != null && Instance.GetInstanceID() == GetInstanceID())
                Instance = null;
        }
        #endregion

        #region Methods Ad
        public Ad AdGet(string nameGameObject)
        {
            if (string.IsNullOrEmpty(nameGameObject))
                return null;
            //
            foreach (var ad in banners)
                if (ad.gameObject.name == nameGameObject)
                    return ad;
            foreach (var ad in interstitials)
                if (ad.gameObject.name == nameGameObject)
                    return ad;
            foreach (var ad in rewardeds)
                if (ad.gameObject.name == nameGameObject)
                    return ad;
            return null;
        }
        public T AdGet<T>() where T : Ad
        {
            foreach (var ad in banners)
                if (ad is T)
                    return ad as T;
            foreach (var ad in interstitials)
                if (ad is T)
                    return ad as T;
            foreach (var ad in rewardeds)
                if (ad is T)
                    return ad as T;
            return null;
        }
        public List<T> AdGets<T>() where T : Ad
        {
            List<T> ads = new List<T>();
            foreach (var ad in banners)
                if (ad is T)
                    ads.Add(ad as T);
            foreach (var ad in interstitials)
                if (ad is T)
                    ads.Add(ad as T);
            foreach (var ad in rewardeds)
                if (ad is T)
                    ads.Add(ad as T);
            return null;
        }
        #endregion

        #region Methods Banners
        public AdBanner BannerGet(int index)
        {
            if (index < 0 || index >= banners.Length)
                return null;
            return banners[index];
        }
        public AdBanner BannerGet(string nameGameObject)
        {
            if (string.IsNullOrEmpty(nameGameObject))
                return null;
            //
            foreach (var banner in banners)
                if (banner.gameObject.name == nameGameObject)
                    return banner;
            return null;
        }
        public T BannerGet<T>() where T : AdBanner
        {
            foreach (var banner in banners)
                if (banner is T)
                    return banner as T;
            return null;
        }
        public List<T> BannerGets<T>() where T : AdBanner
        {
            List<T> ads = new List<T>(banners.Length);
            foreach (var banner in banners)
                if (banner is T)
                    ads.Add(banner as T);
            return null;
        }
        #endregion

        #region Methods Interstitials
        public AdInterstitial InterstitialGet(int index)
        {
            if (index < 0 || index >= interstitials.Length)
                return null;
            return interstitials[index];
        }
        public AdInterstitial InterstitialGet(string nameGameObject)
        {
            if (string.IsNullOrEmpty(nameGameObject))
                return null;
            //
            foreach (var interstitial in interstitials)
                if (interstitial.gameObject.name == nameGameObject)
                    return interstitial;
            return null;
        }
        public T InterstitialGet<T>() where T : AdInterstitial
        {
            foreach (var interstitial in interstitials)
                if (interstitial is T)
                    return interstitial as T;
            return null;
        }
        public List<T> InterstitialGets<T>() where T : AdInterstitial
        {
            List<T> ads = new List<T>(interstitials.Length);
            foreach (var interstitial in interstitials)
                if (interstitial is T)
                    ads.Add(interstitial as T);
            return null;
        }
        #endregion

        #region Methods Rewardeds
        public AdRewarded RewardedGet(int index)
        {
            if (index < 0 || index >= rewardeds.Length)
                return null;
            return rewardeds[index];
        }
        public AdRewarded RewardedGet(string nameGameObject)
        {
            if (string.IsNullOrEmpty(nameGameObject))
                return null;
            //
            foreach (var rewarded in rewardeds)
                if (rewarded.gameObject.name == nameGameObject)
                    return rewarded;
            return null;
        }
        public T RewardedGet<T>() where T : AdRewarded
        {
            foreach (var rewarded in rewardeds)
                if (rewarded is T)
                    return rewarded as T;
            return null;
        }
        public List<T> RewardedGets<T>() where T : AdRewarded
        {
            List<T> ads = new List<T>(rewardeds.Length);
            foreach (var rewarded in rewardeds)
                if (rewarded is T)
                    ads.Add(rewarded as T);
            return null;
        }
        #endregion
    }
}
