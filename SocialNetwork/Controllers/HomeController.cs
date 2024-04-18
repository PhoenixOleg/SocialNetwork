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

        [Route("")] //������� �� ���������
        public IActionResult Index() // ���������� ������������� (��������) /Views/Home/Index.cshtml
        {
            return View(new StoreOfModels()); //�������� ������������� �� ������� �������������,
                                              //���������������� ������������ ������ StoreOfModels (����������� � ����)
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