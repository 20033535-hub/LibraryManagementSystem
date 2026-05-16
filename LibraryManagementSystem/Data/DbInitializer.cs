// =============================================================================
// DbInitializer.cs - Database Seed Data
// Library Management System
// =============================================================================
// Seeds the database with a default Admin user on first run.
// This ensures there is always an admin account available
// to manage the system.
// =============================================================================

using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data
{
    /// <summary>
    /// Initializes the database with seed data.
    /// Called once during application startup.
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Seeds a default Admin user if the Users table is empty.
        /// Default credentials: admin@library.com / Admin@123
        /// </summary>
        public static void Initialize(LibraryDbContext context)
        {
            // Check if any users already exist
            if (context.Users.Any())
            {
                return; // Database has been seeded already
            }

            // Create default admin user with BCrypt hashed password
            var adminUser = new User
            {
                Name = "System Administrator",
                Email = "admin@library.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin",
                CreatedAt = DateTime.Now
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
        }
    }
}
