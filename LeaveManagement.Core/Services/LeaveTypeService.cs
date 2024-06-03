using LeaveManagement.Core.DTO;
using LeaveManagement.Core.ServiceContracts;

namespace LeaveManagement.Core.Services
{
    public class LeaveTypeService : ILeaveTypeService
    {
        public Task<LeaveTypeResponse> AddLeaveType(LeaveTypeAddRequest? leaveTypeAddRequest)
        {
            throw new NotImplementedException();
        }

        public Task<List<LeaveTypeResponse>> GetAllLeavesType()
        {
            throw new NotImplementedException();
        }

        public Task<LeaveTypeResponse?> GetLeaveTypeByLeaveTypeID(Guid? leaveTypeID)
        {
            throw new NotImplementedException();
        }
    }
}
