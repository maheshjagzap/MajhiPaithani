namespace MajhiPaithani.Application.Models.Request
{
    public class UpdateCustomerAddressRequest
    {
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? AddressType { get; set; }
        public bool? IsDefault { get; set; }
    }
}
