using KTool.Advertisement.Demo;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class AdBanner : Ad
    {
        #region Properties
        private static AdBanner instance;
        public static AdBanner Instance
        {
            get
            {
                if (instance == null)
                    return AdBannerDemo.InstanceAdBanner;
                else
                    return instance;
            }
            protected set => instance = value;
        }

        public delegate void AdExpandedDelegate(bool isExpanded);

        [SerializeField]
        private AdPosition adPosition;
        [SerializeField]
        private AdSize adSize;

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
            get => isExpanded;
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
            OnAdExpanded?.Invoke(isSuccess);
        }
        #endregion
    }
}
