// =============================================================================
// AdminController.cs - Admin Dashboard Controller
// Library Management System
// =============================================================================
// Handles Admin-only pages and functionality.
// All actions require the user to be logged in with the "Admin" role.
// This controller is prepared for future expansion with:
//   - Book Management
//   - User Management
//   - Reports & Analytics
// =============================================================================

using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    /// <summary>
    /// Controller for Admin dashboard and administrative functions.
    /// Access restricted to users with "Admin" role.
    /// </summary>
    public class AdminController : Controller
    {
        private readonly LibraryDbContext _context;

        public AdminController(LibraryDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: /Admin/Index
        /// Displays the Admin Dashboard with system statistics.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Check if user is logged in and is Admin
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Gather dashboard statistics
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalMembers = await _context.Users.CountAsync(u => u.Role == "Member");
            ViewBag.TotalAdmins = await _context.Users.CountAsync(u => u.Role == "Admin");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            // Future: Add book counts, borrow stats, etc.
            // ViewBag.TotalBooks = await _context.Books.CountAsync();
            // ViewBag.ActiveBorrows = await _context.BorrowRecords.CountAsync(b => !b.IsReturned);

            return View();
        }

        /// <summary>
        /// GET: /Admin/ManageUsers
        /// Displays a list of all registered users.
        /// Placeholder for future user management features.
        /// </summary>
        public async Task<IActionResult> ManageUsers()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var users = await _context.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();
            return View(users);
        }

        // =====================================================================
        // HELPER METHODS
        // =====================================================================

        /// <summary>
        /// Checks if the current session belongs to an Admin user.
        /// </summary>
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }
    }
}
