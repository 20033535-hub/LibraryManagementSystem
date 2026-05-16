// =============================================================================
// LibraryDbContext.cs - Entity Framework Database Context
// Library Management System
// =============================================================================
// This class is the bridge between the application and SQL Server.
// Entity Framework uses this context to:
//   - Map C# classes to database tables
//   - Execute LINQ queries as SQL (preventing SQL injection)
//   - Track changes and save data
// =============================================================================

using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data
{
    /// <summary>
    /// Database context for the Library Management System.
    /// Configures entity mappings and table relationships.
    /// </summary>
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        // ---------------------------------------------------------------------------
        // DbSets - Each DbSet maps to a database table
        // ---------------------------------------------------------------------------

        /// <summary>
        /// Users table - stores all registered users (Admins and Members)
        /// </summary>
        public DbSet<User> Users { get; set; }

        // Future tables can be added here:
        // public DbSet<Book> Books { get; set; }
        // public DbSet<BorrowRecord> BorrowRecords { get; set; }
        // public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Configure entity mappings and constraints using Fluent API.
        /// This provides additional configuration beyond Data Annotations.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure Email is unique across all users
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Set default value for Role
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("Member");

            // Set default value for CreatedAt
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
