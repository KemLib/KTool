using UnityEngine;

namespace KTool.Advertisement.Demo
{
    public class AdManagerDemo : MonoBehaviour
    {
        #region Properties
        private const string RESOURCES_PATH = "KTool/Advertisement/Demo/AdManagerDemo";
        private const string GAME_OBJECT_NAME = "KTool_AdManagerDemo";

        private static AdManagerDemo instance;
        public static AdManagerDemo Instance
        {
            get
            {
                if (instance == null)
                {
                    AdManagerDemo prefab = Resources.Load<AdManagerDemo>(RESOURCES_PATH);
                    instance = Instantiate(prefab);
                    instance.gameObject.name = GAME_OBJECT_NAME;
                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }

        [SerializeField]
        private AdBannerDemo adBanner;
        [SerializeField]
        private AdInterstitialDemo adInterstitial;
        [SerializeField]
        private AdRewardedDemo adRewarded;

        public AdBannerDemo AdBanner => adBanner;
        public AdInterstitialDemo AdInterstitial => adInterstitial;
        public AdRewardedDemo AdRewarded => adRewarded;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion
    }
}
