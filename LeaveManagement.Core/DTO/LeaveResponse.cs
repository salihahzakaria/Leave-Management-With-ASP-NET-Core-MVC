using LeaveManagement.Core.Domain.Entities;
using LeaveManagement.Core.Enums;

namespace LeaveManagement.Core.DTO
{
    public class LeaveResponse
    {
        public Guid Id { get; set; }
        public Guid? UserID { get; set; }
        public string? User { get; set; }
        public Guid? LeaveTypeID { get; set; }
        public string? LeaveType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Days { get; set; }
        public string? Reason { get; set; }
        public string? Status { get; set; }
        public Guid? ApproverID { get; set; }
        public string? Approver { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(LeaveResponse))
            {
                return false;
            }

            LeaveResponse leave_to_compare = (LeaveResponse)obj;

            return Id == leave_to_compare.Id &&
                UserID == leave_to_compare.UserID &&
                User == leave_to_compare.User &&
                LeaveTypeID == leave_to_compare.LeaveTypeID &&
                LeaveType == leave_to_compare.LeaveType &&
                StartDate == leave_to_compare.StartDate &&
                EndDate == leave_to_compare.EndDate &&
                Reason == leave_to_compare.Reason &&
                Status == leave_to_compare.Status &&
                ApproverID == leave_to_compare.ApproverID &&
                Approver == leave_to_compare.Approver; ;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Leave ID: {Id}, User ID: {UserID}, User: {User}, Leave Type ID: {LeaveTypeID}, " +
                $"Leave Type: {LeaveType}, Start Date: {StartDate}, End Date: {EndDate}, " +
                $"Reason: {Reason}, Status: {Status}, Approver ID: {ApproverID}, Approver: {Approver}";
        }

        public LeaveUpdateRequest ToLeaveUpdateRequest()
        {
            return new LeaveUpdateRequest()
            {
                Id = Id,
                UserID = UserID,
                LeaveTypeID = LeaveTypeID,
                StartDate = StartDate,
                EndDate = EndDate,
                Reason = Reason,
                Status = (StatusOptions)Enum.Parse(typeof(StatusOptions), Status, true),
                ApproverID = ApproverID
            };
        }
    }

    public static class LeaveExtensions
    {
        public static LeaveResponse ToLeaveResponse(this Leave leave)
        {
            return new LeaveResponse()
            {
                Id = leave.Id,
                UserID = leave.UserID,
                LeaveTypeID = leave.LeaveTypeID,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Days = (leave.StartDate != null && leave.EndDate != null) ? (int)(leave.EndDate.Value.Day - leave.StartDate.Value.Day) : null,
                Reason = leave.Reason,
                Status = leave.Status,
                ApproverID = leave.ApproverID,
                LeaveType = leave.LeaveType?.Name,
                User = leave.User?.EmployeeName,
                Approver = leave.Approver?.EmployeeName
            };
        }
    }
}
