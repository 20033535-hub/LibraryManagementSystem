// =============================================================================
// RegisterViewModel.cs - Registration Form Model
// Library Management System
// =============================================================================
// ViewModel used for the user registration form.
// Contains validation rules via Data Annotations to ensure
// proper input before data reaches the controller.
// =============================================================================

using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// ViewModel for user registration.
    /// Separates form input from the database entity for security.
    /// </summary>
    public class RegisterViewModel
    {
        // User's full name
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        // Email address - validated for proper format
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        // Password - minimum 6 characters for security
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        // Confirm Password - must match the Password field
        // [Compare] attribute ensures both fields match
        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
