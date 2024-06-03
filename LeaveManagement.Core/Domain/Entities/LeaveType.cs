using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Core.Domain.Entities
{
    public class LeaveType
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(20)]
        public string? Name { get; set; }

        public virtual ICollection<Leave>? Leaves { get; set; }
    }
}
