namespace MajhiPaithani.Application.Models.Response
{
    public class UpdateCustomerResponse
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Message { get; set; } = "Profile updated successfully";
    }
}
