﻿using KTool.Advertisement.Demo;

namespace KTool.Advertisement
{
    public abstract class AdInterstitial : Ad
    {
        #region Properties
        protected static AdInterstitial instance;
        public static AdInterstitial Instance => instance == null ? AdDemoInterstitial.InstanceAdDemo : instance;

        public override AdType AdType => AdType.Interstitial;
        #endregion

        #region Methods
        public abstract AdInterstitialTracking Show();
        #endregion
    }
}
