namespace KTool.Ad
{
    public struct AdRevenuePaid
    {
        #region Properties
        private string source;
        private string network_name;
        private string idAd;
        private string countryCode;
        private AdType adType;
        private double value;
        private string currency;

        public string Source => source;
        public string Network_name => network_name;
        public string IdAd => idAd;
        public string CountryCode => countryCode;
        public AdType AdType => adType;
        public double Value => value;
        public string Currency => currency;
        #endregion

        #region Method
        public AdRevenuePaid(string source, string network_name, string idAd, string countryCode, AdType adType, double value, string currency)
        {
            this.source = source;
            this.network_name = network_name;
            this.idAd = idAd;
            this.countryCode = countryCode;
            this.adType = adType;
            this.value = value;
            this.currency = currency;
        }
        #endregion

        #region Method

        #endregion
    }
}