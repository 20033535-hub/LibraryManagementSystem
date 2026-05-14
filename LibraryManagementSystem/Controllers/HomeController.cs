// =============================================================================
// HomeController.cs - Home & Public Pages Controller
// Library Management System
// =============================================================================
// Handles the public-facing pages of the application:
//   - Home page (library introduction)
//   - Error page
// These pages are accessible without authentication.
// =============================================================================

using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LibraryManagementSystem.Controllers
{
    /// <summary>
    /// Controller for public pages accessible to all visitors.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: /Home/Index
        /// Displays the library home page with introduction and features.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: /Home/About
        /// Displays information about the library system.
        /// </summary>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Displays the error page with request details.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
