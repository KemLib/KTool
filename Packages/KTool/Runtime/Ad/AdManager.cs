using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace KTool.Ad
{
    public class AdManager : MonoBehaviour
    {
        #region Properties
        private const string INSTANCE_GAME_OBEJECT_NAME = "AdManager";
        private static AdManager instance;
        public static AdManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject(INSTANCE_GAME_OBEJECT_NAME);
                    AdManager adManager = go.AddComponent<AdManager>();
                    adManager.Init();
                }
                return instance;
            }
        }

        private bool isInit;
        private List<IAdBanner> banners;
        private List<IAdMRec> mRecs;
        private List<IAdNative> natives;
        private List<IAdNativeBanner> nativeBanners;
        private List<IAdInterstitial> interstitials;
        private List<IAdRewarded> rewardeds;
        private List<IAdRewardedInterstitial> rewardedInterstitials;
        private TypeInfo typeInfo_Banner,
            typeInfo_MRec,
            typeInfo_Natives,
            typeInfo_NativeBanners,
            typeInfo_Interstitials,
            typeInfo_Rewardeds,
            typeInfo_RewardedInterstitials;

        #endregion

        #region Unity Event		
        private void Awake()
        {
            if (instance == null)
            {
                Init();
                return;
            }
            if (instance.GetInstanceID() == GetInstanceID())
                return;
            Destroy(gameObject);
        }
        private void OnDestroy()
        {
            if (instance != null && instance.GetInstanceID() == GetInstanceID())
                instance = null;
        }
        #endregion

        #region Method
        private void Init()
        {
            if (isInit)
                return;
            isInit = true;
            //
            instance = this;
            DontDestroyOnLoad(gameObject);
            //
            banners = new List<IAdBanner>();
            mRecs = new List<IAdMRec>();
            natives = new List<IAdNative>();
            nativeBanners = new List<IAdNativeBanner>();
            interstitials = new List<IAdInterstitial>();
            rewardeds = new List<IAdRewarded>();
            rewardedInterstitials = new List<IAdRewardedInterstitial>();
            typeInfo_Banner = typeof(IAdBanner).Ge‌​tTypeInfo();
            typeInfo_MRec = typeof(IAdMRec).Ge‌​tTypeInfo();
            typeInfo_Natives = typeof(IAdNative).Ge‌​tTypeInfo();
            typeInfo_NativeBanners = typeof(IAdNativeBanner).Ge‌​tTypeInfo();
            typeInfo_Interstitials = typeof(IAdInterstitial).Ge‌​tTypeInfo();
            typeInfo_Rewardeds = typeof(IAdRewarded).Ge‌​tTypeInfo();
            typeInfo_RewardedInterstitials = typeof(IAdRewardedInterstitial).Ge‌​tTypeInfo();
        }

        public int GetCount<T>() where T : IAd
        {
            Init();
            //
            TypeInfo typeT = typeof(T).Ge‌​tTypeInfo();
            if (typeInfo_Banner.IsAssignableFrom(typeT))
                return banners.Count;
            if (typeInfo_MRec.IsAssignableFrom(typeT))
                return mRecs.Count;
            if (typeInfo_Natives.IsAssignableFrom(typeT))
                return natives.Count;
            if (typeInfo_NativeBanners.IsAssignableFrom(typeT))
                return nativeBanners.Count;
            if (typeInfo_Interstitials.IsAssignableFrom(typeT))
                return interstitials.Count;
            if (typeInfo_Rewardeds.IsAssignableFrom(typeT))
                return rewardeds.Count;
            if (typeInfo_RewardedInterstitials.IsAssignableFrom(typeT))
                return rewardedInterstitials.Count;
            return -1;
        }
        public T Get<T>(int index) where T : IAd
        {
            Init();
            //
            TypeInfo typeT = typeof(T).Ge‌​tTypeInfo();
            if (typeInfo_Banner.IsAssignableFrom(typeT))
                return (T)banners[index];
            if (typeInfo_MRec.IsAssignableFrom(typeT))
                return (T)mRecs[index];
            if (typeInfo_Natives.IsAssignableFrom(typeT))
                return (T)natives[index];
            if (typeInfo_NativeBanners.IsAssignableFrom(typeT))
                return (T)nativeBanners[index];
            if (typeInfo_Interstitials.IsAssignableFrom(typeT))
                return (T)interstitials[index];
            if (typeInfo_Rewardeds.IsAssignableFrom(typeT))
                return (T)rewardeds[index];
            if (typeInfo_RewardedInterstitials.IsAssignableFrom(typeT))
                return (T)rewardedInterstitials[index];
            return default(T);
        }
        public T Get<T>(string name) where T : IAd
        {
            Init();
            //
            TypeInfo typeT = typeof(T).Ge‌​tTypeInfo();
            if (typeInfo_Banner.IsAssignableFrom(typeT))
            {
                foreach (var ad in banners)
                    if (ad.Name == name)
                        return (T)ad;
            }
            else if (typeInfo_MRec.IsAssignableFrom(typeT))
            {
                foreach (var ad in mRecs)
                    if (ad.Name == name)
                        return (T)ad;
            }
            else if (typeInfo_Natives.IsAssignableFrom(typeT))
            {
                foreach (var ad in natives)
                    if (ad.Name == name)
                        return (T)ad;
            }
            else if (typeInfo_NativeBanners.IsAssignableFrom(typeT))
            {
                foreach (var ad in nativeBanners)
                    if (ad.Name == name)
                        return (T)ad;
            }
            else if (typeInfo_Interstitials.IsAssignableFrom(typeT))
            {
                foreach (var ad in interstitials)
                    if (ad.Name == name)
                        return (T)ad;
            }
            else if (typeInfo_Rewardeds.IsAssignableFrom(typeT))
            {
                foreach (var ad in rewardeds)
                    if (ad.Name == name)
                        return (T)ad;
            }
            else if (typeInfo_RewardedInterstitials.IsAssignableFrom(typeT))
            {
                foreach (var ad in rewardedInterstitials)
                    if (ad.Name == name)
                        return (T)ad;
            }
            return default(T);
        }
        public void Add(IAd ad)
        {
            Init();
            //
            if (ad is IAdBanner)
                banners.Add(ad as IAdBanner);
            if (ad is IAdMRec)
                mRecs.Add(ad as IAdMRec);
            if (ad is IAdNative)
                natives.Add(ad as IAdNative);
            if (ad is IAdNativeBanner)
                nativeBanners.Add(ad as IAdNativeBanner);
            if (ad is IAdInterstitial)
                interstitials.Add(ad as IAdInterstitial);
            if (ad is IAdRewarded)
                rewardeds.Add(ad as IAdRewarded);
            if (ad is IAdRewardedInterstitial)
                rewardedInterstitials.Add(ad as IAdRewardedInterstitial);
        }
        #endregion
    }
}
