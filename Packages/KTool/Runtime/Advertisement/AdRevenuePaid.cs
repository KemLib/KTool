namespace KTool.Advertisement
{
    public struct AdRevenuePaid
    {
        #region Properties
        public readonly string Source;
        public readonly string NetworkName;
        public readonly string IdAd;
        public readonly string CountryCode;
        public readonly AdType AdType;
        public readonly double Value;
        public readonly string Currency;
        #endregion

        #region Method
        public AdRevenuePaid(string source, string network_name, string idAd, string countryCode, AdType adType, double value, string currency)
        {
            Source = source;
            NetworkName = network_name;
            IdAd = idAd;
            CountryCode = countryCode;
            AdType = adType;
            Value = value;
            Currency = currency;
        }
        #endregion

        #region Method

        #endregion
    }
}