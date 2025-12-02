using EndPoint.Site.Models;
using EndPoint.Site.Models.ViewModels.HomePages;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Common.Queries.GetSlider;
using System.Diagnostics;

namespace EndPoint.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGetSliderService _sliderService;

        public HomeController(ILogger<HomeController> logger, IGetSliderService sliderService)
        {
            _logger = logger;
            _sliderService = sliderService;
        }

        public IActionResult Index()
        {
            HomePageViewModel homePage = new HomePageViewModel()
            {
                Sliders = _sliderService.Execute().Data,
            };

            return View(homePage);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
