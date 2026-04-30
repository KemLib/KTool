using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KTool.Advertisement.Demo
{
    public class AdDemoBanner : AdBanner
    {
        #region Properties
        public static AdDemoBanner InstanceAdDemo => AdDemoManager.Instance.AdBanner;

        [SerializeField]
        private Image bannerBot,
            bannerMid,
            bannerTop;

        private Image currentBanner;

        private Image BannerSelect
        {
            get
            {
                switch (PositionType)
                {
                    case AdPosition.TopLeft:
                    case AdPosition.TopCenter:
                    case AdPosition.TopRight:
                        return bannerTop;
                    case AdPosition.MidLeft:
                    case AdPosition.MidCenter:
                    case AdPosition.MidRight:
                        return bannerMid;
                    case AdPosition.BotLeft:
                    case AdPosition.BotCenter:
                    case AdPosition.BotRight:
                        return bannerBot;
                    default:
                        return bannerBot;
                }
            }
        }
        private bool IsDisplayed
        {
            get => currentBanner == null ? false : currentBanner.gameObject.activeSelf;
            set
            {
                if (currentBanner == null)
                    return;
                currentBanner.gameObject.SetActive(value);
            }
        }
        #endregion

        #region Methods Unity
        private void Update()
        {
            if(IsShow)
            {
                if (!IsDisplayed)
                    IsDisplayed = true;
            }
        }
        #endregion

        #region Methods

        #endregion

        #region Ad
        public override void Load()
        {
            IsLoaded = true;
            PushEvent_Loaded(true);
        }
        protected override bool OnShow(out string error)
        {
            if (IsShow)
            {
                error = AdDemoAppOpen.ERROR_IS_SHOW;
                return false;
            }
            //
            if (!IsLoaded)
                Load();
            currentBanner = BannerSelect;
            IsShow = true;
            //
            error = string.Empty;
            return true;
        }
        protected override bool OnHide()
        {
            if (!IsShow)
                return false;
            //
            IsDisplayed = false;
            IsShow = false;
            PushEvent_Hidden();
            return true;
        }
        public override void Destroy()
        {
            IsDestroy = true;
            if (IsShow)
                Hide();
            PushEvent_Destroy();
        }
        #endregion

        #region Unity Ui Event
        public void OnClick_Ad()
        {
            if (!IsShow)
                return;
            //
            PushEvent_Clicked();
        }
        public void OnClick_Expanded()
        {
            if (!IsShow)
                return;
            //
            PushEvent_Expanded(IsExpanded);
        }
        #endregion
    }
}
