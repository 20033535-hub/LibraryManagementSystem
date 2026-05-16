// =============================================================================
// User.cs - User Entity Model
// Library Management System
// =============================================================================
// Represents a user in the system (Admin or Member).
// This is the primary entity for authentication and authorization.
// Maps to the "Users" table in SQL Server via Entity Framework Code First.
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// User entity representing a library system user.
    /// Supports two roles: Admin and Member.
    /// </summary>
    [Table("Users")]
    public class User
    {
        // Primary Key - Auto-generated identity column
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        // Full name of the user
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        // Email address - used as login credential (must be unique)
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(150)]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        // BCrypt hashed password - never stored in plain text
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        // User role: "Admin" or "Member"
        // Determines dashboard access and permissions
        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "Member";

        // Timestamp when the account was created
        [Display(Name = "Registered On")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
