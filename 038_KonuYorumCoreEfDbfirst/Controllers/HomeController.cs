using _038_KonuYorumCoreEfDbfirst.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _038_KonuYorumCoreEfDbfirst.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        // https://localhost:44301            Program.cs altındaki route tanımı: controller/action/id?
        // https://localhost:44301/Home       
        // https://localhost:44301/Home/Index Program.cs altındaki route tanımı: "{controller=Home}/{action=Index}/{id?}"
        public IActionResult Index()
        {
            return View();
        }
        // https://localhost:44301/Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }
        // https://localhost:44301/Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}