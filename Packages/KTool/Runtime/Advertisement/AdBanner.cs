using KTool.Advertisement.Demo;
using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class AdBanner : Ad
    {
        #region Properties
        internal const string ERROR_AD_EVENT_EXPANDED_EXCEPTION = "Ad {0} call event Expanded exception: {1}";

        private static AdBanner instance;
        public static AdBanner Instance
        {
            get => instance == null ? AdDemoBanner.InstanceAdDemo : instance;
            protected set => instance = value;
        }

        public delegate void AdExpandedDelegate(AdBanner source, bool isExpanded);

        [SerializeField]
        private AdPosition adPosition;
        [SerializeField]
        private AdSize adSize;
        [SerializeField]
        private Vector2 position,
            size;

        private bool isExpanded;
        public event AdExpandedDelegate OnAdExpanded;

        public override AdType AdType => AdType.Banner;
        public virtual AdPosition PositionType
        {
            get => adPosition;
            protected set => adPosition = value;
        }
        public virtual AdSize SizeType
        {
            get => adSize;
            protected set => adSize = value;
        }
        public virtual Vector2 Position
        {
            get => position;
            protected set => position = value;
        }
        public virtual Vector2 Size
        {
            get => size;
            protected set => size = value;
        }
        public virtual bool IsExpanded
        {
            get => IsShow && isExpanded;
            protected set => isExpanded = value;
        }
        #endregion

        #region Methods
        public abstract AdBannerTracking Show();
        public abstract void Hide();
        #endregion

        #region Event
        protected void PushEvent_Expanded(bool isSuccess)
        {
            try
            {
                OnAdExpanded?.Invoke(this, isSuccess);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_AD_EVENT_EXPANDED_EXCEPTION, AdType.Banner, ex.Message));
            }
        }
        #endregion
    }
}
