// =============================================================================
// Program.cs - Application Entry Point
// Library Management System - ASP.NET MVC
// =============================================================================
// This file configures services, middleware, and the HTTP request pipeline.
// It sets up Entity Framework, session-based authentication, and MVC routing.
// =============================================================================

using LibraryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------
// SERVICE CONFIGURATION
// ---------------------------------------------------------------------------

// Add MVC services (Controllers + Views)
builder.Services.AddControllersWithViews();

// Configure Entity Framework Core with SQLite (local file)
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure session-based authentication
// Sessions are used to track logged-in users across requests
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Session expires after 30 min of inactivity
    options.Cookie.HttpOnly = true;                   // Prevent JavaScript access to session cookie (XSS protection)
    options.Cookie.IsEssential = true;                // Required for GDPR compliance
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// Add HttpContextAccessor so we can access session in views
builder.Services.AddHttpContextAccessor();

// Add anti-forgery services (CSRF protection)
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

var app = builder.Build();

// ---------------------------------------------------------------------------
// MIDDLEWARE PIPELINE
// ---------------------------------------------------------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // HTTP Strict Transport Security
}

app.UseHttpsRedirection();
app.UseStaticFiles();       // Serve wwwroot files (CSS, JS, images)

app.UseRouting();

app.UseSession();           // Enable session middleware (must be before authorization)

app.UseAuthorization();

// ---------------------------------------------------------------------------
// ROUTE CONFIGURATION
// ---------------------------------------------------------------------------
// Default route: HomeController -> Index action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ---------------------------------------------------------------------------
// DATABASE INITIALIZATION
// ---------------------------------------------------------------------------
// Ensure the database is created and seed default admin user
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    context.Database.EnsureCreated();  // Creates DB if it doesn't exist
    DbInitializer.Initialize(context); // Seed default data
}

app.Run();
