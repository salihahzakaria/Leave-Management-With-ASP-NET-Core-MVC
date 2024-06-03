using LeaveManagement.Core.Domain.Entities;

namespace LeaveManagement.Core.Domain.RepositoryContracts
{
    public interface ILeaveTypeRepository
    {
        Task<LeaveType> AddLeaveType(LeaveType leaveType);

        Task<List<LeaveType>> GetAllLeavesType();

        Task<LeaveType?> GetLeaveTypeByID(Guid leaveTypeID);

        Task<LeaveType?> GetLeaveTypeByName(string leaveTypeName);
    }
}
