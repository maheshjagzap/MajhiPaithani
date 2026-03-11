using System.Text.Json.Serialization;

namespace MajhiPaithani.Web.ViewModels.Home
{
    public class RegisterViewModel
    {

        [JsonPropertyName("sFirstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("sLastName")]
        public string LastName { get; set; }

        [JsonPropertyName("sEmail")]
        public string Email { get; set; }

        [JsonPropertyName("sPhoneNumber")]
        public string Phone { get; set; }

        [JsonPropertyName("sPassword")]
        public string Password { get; set; }


        [JsonPropertyName("RoleId")]
        public string RoleId { get; set; }
    }
}
