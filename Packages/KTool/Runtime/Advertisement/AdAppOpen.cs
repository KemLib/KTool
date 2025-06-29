using KTool.Advertisement.Demo;

namespace KTool.Advertisement
{
    public abstract class AdAppOpen : Ad
    {
        #region Properties
        private static AdAppOpen instance;
        public static AdAppOpen Instance
        {
            get => instance == null ? AdDemoAppOpen.InstanceAdDemo : instance;
            protected set => instance = value;
        }

        public override AdType AdType => AdType.AppOpen;
        #endregion

        #region Methods
        public abstract AdAppOpenTracking Show();
        #endregion
    }
}
