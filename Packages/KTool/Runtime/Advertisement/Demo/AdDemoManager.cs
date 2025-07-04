using UnityEngine;

namespace KTool.Advertisement.Demo
{
    public class AdDemoManager : MonoBehaviour
    {
        #region Properties
        private const string RESOURCES_PATH = "KTool/Advertisement/Demo/AdManagerDemo";
        private const string GAME_OBJECT_NAME = "KTool_AdManagerDemo";
        public const string AdSource = "KTool Ad Demo",
            AdCurrency = "tmp",
            adCountryCode = "VN";

        private static AdDemoManager instance;
        public static AdDemoManager Instance
        {
            get
            {
                if (instance == null)
                {
                    AdDemoManager prefab = Resources.Load<AdDemoManager>(RESOURCES_PATH);
                    instance = Instantiate(prefab);
                    instance.gameObject.name = GAME_OBJECT_NAME;
                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }

        [SerializeField]
        private AdDemoAppOpen adAppOpen;
        [SerializeField]
        private AdDemoBanner adBanner;
        [SerializeField]
        private AdDemoInterstitial adInterstitial;
        [SerializeField]
        private AdDemoRewarded adRewarded;

        public AdDemoAppOpen AdAppOpen => adAppOpen;
        public AdDemoBanner AdBanner => adBanner;
        public AdDemoInterstitial AdInterstitial => adInterstitial;
        public AdDemoRewarded AdRewarded => adRewarded;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion
    }
}
