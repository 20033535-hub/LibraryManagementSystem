// =============================================================================
// ErrorViewModel.cs - Error Page Model
// Library Management System
// =============================================================================

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// ViewModel for displaying error information on the error page.
    /// </summary>
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
