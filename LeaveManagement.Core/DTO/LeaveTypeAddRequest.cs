using LeaveManagement.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Core.DTO
{
    public class LeaveTypeAddRequest
    {
        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Leave Type")]
        public string? Name { get; set; }

        public LeaveType ToLeaveType()
        {
            return new LeaveType()
            {
                Name = Name
            };
        }
    }
}
