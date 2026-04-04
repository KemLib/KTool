using KTool.Advertisement.Demo;
using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class AdBanner : Ad
    {
        #region Properties
        protected static AdBanner instance;
        public static AdBanner Instance => instance == null ? AdDemoBanner.InstanceAdDemo : instance;

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
        public virtual bool IsExpanded => IsShow && isExpanded;
        #endregion

        #region Methods
        public abstract IAdBannerTracking Show();
        public abstract void Hide();
        #endregion

        #region Event
        protected void PushEvent_Expanded(bool isExpanded)
        {
            this.isExpanded = isExpanded;
            OnAdExpanded?.Invoke(this, isExpanded);
        }
        #endregion
    }
}
