namespace MajhiPaithani.Application.Models.Response
{
    public class AddCustomerAddressResponse
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? AddressType { get; set; }
        public bool? IsDefault { get; set; }
        public string Message { get; set; } = "Address added successfully";
    }
}
