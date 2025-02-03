using System.Collections.Generic;
using UnityEngine;

namespace KTool.Ad
{
    public class AdNativeData
    {
        #region Properties
        [SerializeField]
        private readonly Texture2D icon,
            adChoicesLogo;
        [SerializeField]
        private readonly string star,
            headline,
            body,
            callToAction,
            advertiser,
            price,
            store;

        public List<GameObject> Images;
        public Texture2D Icon => icon;
        public Texture2D AdChoicesLogo => adChoicesLogo;
        public string Star => star;
        public string Headline => headline;
        public string Body => body;
        public string CallToAction => callToAction;
        public string Advertiser => advertiser;
        public string Price => price;
        public string Store => store;
        #endregion

        #region Construction
        public AdNativeData(Texture2D icon,
            Texture2D adChoicesLogo,
            string star = null,
            string headline = null,
            string body = null,
            string callToAction = null,
            string advertiser = null,
            string price = null,
            string store = null)
        {
            this.icon = icon;
            this.adChoicesLogo = adChoicesLogo;
            this.star = star;
            this.headline = headline;
            this.body = body;
            this.callToAction = callToAction;
            this.advertiser = advertiser;
            this.price = price;
            this.store = store;
        }
        #endregion

        #region Method

        #endregion
    }
}
