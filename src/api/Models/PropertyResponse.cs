namespace api.Models
{
    public class PropertyResponse
    {
        public PropertyAttributes Attributes { get; set; }
        public SalesHistory SalesHistory { get; set; }
        public AdditionalInfo AdditionalInfo { get; set; }
    }

    public class PropertyAttributes
    {
        public string? PropertyID { get; set; }
        public string? FlatNumber { get; set; }
        public string? StreetNumber { get; set; }
        public string? StreetName { get; set; }
        public string? Suburb { get; set; }
        public string? State { get; set; }
        public string? Postcode { get; set; }
        public string? PropertyType { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? CarSpaces { get; set; }
        public decimal? LandSize { get; set; }
        public decimal? BuildingSize { get; set; }
    }

    public class SalesHistory
    {
        public DateTime? LastListedDate { get; set; }
        public DateTime? LastSoldDate { get; set; }
        public DateTime? LastRentedDate { get; set; }
        public decimal? LastListedPrice { get; set; }
        public decimal? LastSoldPrice { get; set; }
        public decimal? LastRentedPrice { get; set; }
    }

    public class AdditionalInfo
    {
        public string OwnerRenter { get; set; }
        public string MarketStatus { get; set; }
    }
} 