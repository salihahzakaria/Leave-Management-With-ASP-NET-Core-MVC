using LeaveManagement.Core.Domain.Entities;

namespace LeaveManagement.Core.DTO
{
    public class LeaveTypeResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(LeaveTypeResponse))
            {
                return false;
            }

            LeaveTypeResponse leaveType_to_compare = (LeaveTypeResponse)obj;

            return Id == leaveType_to_compare.Id &&
                Name == leaveType_to_compare.Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Leave Type ID: {Id}, Name: {Name}";
        }
    }

    public static class LeaveTypeExtensions
    {
        public static LeaveTypeResponse ToLeaveTypeResponse(this LeaveType leaveType)
        {
            return new LeaveTypeResponse
            {
                Id = leaveType.Id,
                Name = leaveType.Name
            };
        }
    }
}
