using MajhiPaithani.Web.ViewModels.Home;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace MajhiPaithani.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7006/");
        }
        public IActionResult Index()
        {
            var model = new LandingPageViewModel
            {
                HeroTitle = "Authentic Paithani Sarees Direct From Artisans",
                HeroSubtitle = "Connecting traditional Paithani artisans with customers without middlemen.",

                CustomerBenefits = new List<string>
            {
                "Buy authentic Paithani directly from artisans",
                "No middleman commission",
                "Transparent pricing",
                "Customization options available"
            },

                SellerBenefits = new List<string>
            {
                "Reach customers across India",
                "Sell directly without middlemen",
                "Showcase your craftsmanship",
                "Get better profit margins"
            },

                SellerButtonText = "Become a Seller",
                ExploreButtonText = "Explore Sarees"
            };

            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Map the View Model to the API Request Object with the 's' prefix attributes
            var request = new RegisterViewModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Password = model.Password,
                RoleId = model.RoleId
            };

            // Serialize normally - the [JsonPropertyName] attributes will handle the mapping
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("api/auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }

                // Read the validation error if it fails again
                var errorJson = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "API Validation Error: " + errorJson);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Connection Error: " + ex.Message);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var request = new LoginViewModel
            {
                Email = model.Email,
                Password = model.Password
            };

            // Use System.Text.Json with camelCase naming policy to match API expectations
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(request, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Use a leading slash to ensure it appends to the BaseAddress correctly
                var response = await _httpClient.PostAsync("api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }

                // If 404, it means the URL is wrong. If 401, credentials.
                ModelState.AddModelError("", $"Server returned {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", "Could not connect to the API. Is it running?");
            }

            return View(model);
        }
    }
}

