using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? EmployeeName { get; set; }
    }
}
