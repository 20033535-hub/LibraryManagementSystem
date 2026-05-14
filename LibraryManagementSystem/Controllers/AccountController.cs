// =============================================================================
// AccountController.cs - Authentication Controller
// Library Management System
// =============================================================================
// Handles all authentication-related actions:
//   - User Registration (with BCrypt password hashing)
//   - User Login (with credential verification)
//   - User Logout (session clearing)
//   - Role-based redirection (Admin vs Member dashboards)
//
// SECURITY FEATURES:
//   - BCrypt password hashing (one-way, salted)
//   - Anti-forgery tokens on all POST actions ([ValidateAntiForgeryToken])
//   - Input validation via Data Annotations on ViewModels
//   - Parameterized queries via Entity Framework (SQL Injection prevention)
//   - Razor auto-encoding (XSS prevention)
// =============================================================================

using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    /// <summary>
    /// Controller managing user authentication and account operations.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly LibraryDbContext _context;

        public AccountController(LibraryDbContext context)
        {
            _context = context;
        }

        // =====================================================================
        // REGISTRATION
        // =====================================================================

        /// <summary>
        /// GET: /Account/Register
        /// Displays the registration form.
        /// </summary>
        public IActionResult Register()
        {
            // Redirect if already logged in
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("RedirectToDashboard");
            }
            return View();
        }

        /// <summary>
        /// POST: /Account/Register
        /// Processes the registration form submission.
        /// Validates input, checks for duplicate emails, hashes password,
        /// and creates a new user account.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]  // CSRF protection
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Validate all Data Annotation rules on the ViewModel
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if email already exists in the database
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "An account with this email already exists");
                return View(model);
            }

            // Create new user with BCrypt hashed password
            // BCrypt automatically generates a unique salt for each password
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = "Member",        // Default role for self-registration
                CreatedAt = DateTime.Now
            };

            // Save to database (EF Core uses parameterized queries - SQL Injection safe)
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Set success message and redirect to login
            TempData["SuccessMessage"] = "Registration successful! Please log in.";
            return RedirectToAction("Login");
        }

        // =====================================================================
        // LOGIN
        // =====================================================================

        /// <summary>
        /// GET: /Account/Login
        /// Displays the login form.
        /// </summary>
        public IActionResult Login()
        {
            // Redirect if already logged in
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("RedirectToDashboard");
            }
            return View();
        }

        /// <summary>
        /// POST: /Account/Login
        /// Processes login form submission.
        /// Verifies credentials and creates a session for the user.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]  // CSRF protection
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Find user by email (case-insensitive search)
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            // Verify user exists AND password matches the BCrypt hash
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            // ---- Authentication Successful ----
            // Store user information in session
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role);

            // Redirect based on role
            return RedirectToAction("RedirectToDashboard");
        }

        // =====================================================================
        // LOGOUT
        // =====================================================================

        /// <summary>
        /// POST: /Account/Logout
        /// Clears the user session and redirects to home page.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Clear all session data
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }

        // =====================================================================
        // ROLE-BASED REDIRECTION
        // =====================================================================

        /// <summary>
        /// Redirects the user to the appropriate dashboard based on their role.
        /// Admin → AdminController/Index
        /// Member → MemberController/Index
        /// </summary>
        public IActionResult RedirectToDashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");

            return role switch
            {
                "Admin" => RedirectToAction("Index", "Admin"),
                "Member" => RedirectToAction("Index", "Member"),
                _ => RedirectToAction("Login")
            };
        }

        /// <summary>
        /// GET: /Account/AccessDenied
        /// Displayed when a user tries to access a restricted page.
        /// </summary>
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
