using LeaveManagement.Core.Domain.Entities;
using LeaveManagement.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Core.DTO
{
    public class LeaveAddRequest
    {
        [Required(ErrorMessage = "Please select a {0}")]
        [Display(Name = "Name")]
        public Guid? UserID { get; set; }

        [Required(ErrorMessage = "Please select a {0}")]
        [Display(Name = "Leave Type")]
        public Guid? LeaveTypeID { get; set; }

        [Required(ErrorMessage = "Please select a {0}")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Please select a {0}")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public string? Reason { get; set; }
        public StatusOptions? Status { get; set; } = StatusOptions.Pending;

        public Leave ToLeave()
        {
            return new Leave()
            {
                UserID = UserID,
                LeaveTypeID = LeaveTypeID,
                StartDate = StartDate,
                EndDate = EndDate,
                Reason = Reason,
                Status = Status.ToString()
            };
        }
    }
}
