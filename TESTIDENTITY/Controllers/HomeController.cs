using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TESTIDENTITY.Data;
using TESTIDENTITY.Models;

namespace TESTIDENTITY.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _repo;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _repo = context;
        }

        public IActionResult Index()
        {
            var user = User.FindFirst(ClaimTypes.Email)?.Value;
            var existDb = _repo.Clients.FirstOrDefault(x => x.EmailAddress == user);
            if(existDb != null) 
            {
                ViewData["IsAdmin"] = existDb.IsAdmin;
                ViewData["Email"] = existDb.EmailAddress;
            }
            ViewData["User"] = user;
            return View();
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
