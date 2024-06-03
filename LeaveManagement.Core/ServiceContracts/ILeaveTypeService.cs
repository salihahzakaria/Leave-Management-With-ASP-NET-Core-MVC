using LeaveManagement.Core.DTO;

namespace LeaveManagement.Core.ServiceContracts
{
    public interface ILeaveTypeService
    {
        Task<LeaveTypeResponse> AddLeaveType(LeaveTypeAddRequest? leaveTypeAddRequest);
        Task<List<LeaveTypeResponse>> GetAllLeavesType();
        Task<LeaveTypeResponse?> GetLeaveTypeByLeaveTypeID(Guid? leaveTypeID);
    }
}
