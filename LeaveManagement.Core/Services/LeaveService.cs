using LeaveManagement.Core.DTO;
using LeaveManagement.Core.Enums;
using LeaveManagement.Core.ServiceContracts;

namespace LeaveManagement.Core.Services
{
    public class LeaveService : ILeaveService
    {
        public Task<LeaveResponse> AddLeave(LeaveAddRequest? leaveAddRequest)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteLeave(Guid? leaveID)
        {
            throw new NotImplementedException();
        }

        public Task<List<LeaveResponse>> GetAllLeaves()
        {
            throw new NotImplementedException();
        }

        public Task<List<LeaveResponse>> GetFilteredLeaves(string searchBy, string? searchString)
        {
            throw new NotImplementedException();
        }

        public Task<LeaveResponse?> GetLeaveByLeaveID(Guid? leaveID)
        {
            throw new NotImplementedException();
        }

        public Task<List<LeaveResponse>> GetLeaveByUserID(Guid? userID)
        {
            throw new NotImplementedException();
        }

        public Task<List<LeaveResponse>> GetSortedLeaves(List<LeaveResponse> allLeaves, string sortBy, SortOrderOptions sortOrderOptions)
        {
            throw new NotImplementedException();
        }

        public Task<LeaveResponse> UpdateLeave(LeaveUpdateRequest? leaveUpdateRequest)
        {
            throw new NotImplementedException();
        }
    }
}
