// =============================================================================
// MemberController.cs - Member Dashboard Controller
// Library Management System
// =============================================================================
// Handles Member-only pages and functionality.
// All actions require the user to be logged in with the "Member" role.
// This controller is prepared for future expansion with:
//   - Browse & Search Books
//   - Borrow & Return Books
//   - View Borrowing History
// =============================================================================

using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    /// <summary>
    /// Controller for Member dashboard and member functions.
    /// Access restricted to users with "Member" role.
    /// </summary>
    public class MemberController : Controller
    {
        private readonly LibraryDbContext _context;

        public MemberController(LibraryDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: /Member/Index
        /// Displays the Member Dashboard with personalized information.
        /// </summary>
        public IActionResult Index()
        {
            // Check if user is logged in and is a Member
            if (!IsMember())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");

            // Future: Add member-specific stats
            // ViewBag.BooksCheckedOut = await _context.BorrowRecords
            //     .CountAsync(b => b.UserId == userId && !b.IsReturned);
            // ViewBag.OverdueBooks = await _context.BorrowRecords
            //     .CountAsync(b => b.UserId == userId && !b.IsReturned && b.DueDate < DateTime.Now);

            return View();
        }

        // =====================================================================
        // HELPER METHODS
        // =====================================================================

        /// <summary>
        /// Checks if the current session belongs to a Member user.
        /// </summary>
        private bool IsMember()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Member" || role == "Admin"; // Admins can also view member pages
        }
    }
}
