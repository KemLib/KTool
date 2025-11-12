namespace KTool.Advertisement
{
    public struct AdRevenuePaid
    {
        #region Properties
        public readonly string Source;
        public readonly string NetworkName;
        public readonly string IdAd;
        public readonly AdType AdType;
        public readonly string CountryCode;
        public readonly string Placement;
        public readonly double Value;
        public readonly string Currency;
        #endregion

        #region Method
        public AdRevenuePaid(string source, string network_name, string idAd, AdType adType, string countryCode, string placement, double value, string currency)
        {
            Source = source;
            NetworkName = network_name;
            IdAd = idAd;
            AdType = adType;
            CountryCode = countryCode;
            Placement = placement;
            Value = value;
            Currency = currency;
        }
        #endregion

        #region Method

        #endregion
    }
}