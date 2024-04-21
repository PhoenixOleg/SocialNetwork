using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Models;
using SocialNetwork.ViewModels.Account;
using System.Diagnostics;

namespace SocialNetwork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("")] //Маршрут по умолчанию
        [Route("[controller]/[action]")]
        public IActionResult Index() // Отображает представление (страницу) /Views/Home/Index.cshtml
        {
            return View(new StoreOfModels()); //Собирает представление из моделей представлений,
                                              //инициализируемых конструкторе класса StoreOfModels (регистрация и вход)
        }

        [Route("[action]")]
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
