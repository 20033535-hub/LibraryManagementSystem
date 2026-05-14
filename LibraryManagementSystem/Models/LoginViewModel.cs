// =============================================================================
// LoginViewModel.cs - Login Form Model
// Library Management System
// =============================================================================
// ViewModel used for the login form.
// Validates that email and password are provided before
// attempting authentication.
// =============================================================================

using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// ViewModel for user login.
    /// Only requires email and password for authentication.
    /// </summary>
    public class LoginViewModel
    {
        // Email used as the login identifier
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        // Password - will be verified against BCrypt hash
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}
