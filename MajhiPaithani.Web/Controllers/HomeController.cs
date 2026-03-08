using MajhiPaithani.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MajhiPaithani.Web.Controllers
{
    public class HomeController : Controller
    {
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
    }
}
