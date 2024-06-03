using LeaveManagement.Core.DTO;
using LeaveManagement.Core.Enums;

namespace LeaveManagement.Core.ServiceContracts
{
    public interface ILeaveService
    {
        Task<LeaveResponse> AddLeave(LeaveAddRequest? leaveAddRequest);
        Task<List<LeaveResponse>> GetAllLeaves();

        Task<LeaveResponse?> GetLeaveByLeaveID(Guid? leaveID);

        Task<List<LeaveResponse>> GetLeaveByUserID(Guid? userID);

        Task<List<LeaveResponse>> GetFilteredLeaves(string searchBy, string? searchString);

        Task<LeaveResponse> UpdateLeave(LeaveUpdateRequest? leaveUpdateRequest);

        Task<bool> DeleteLeave(Guid? leaveID);

        Task<List<LeaveResponse>> GetSortedLeaves(List<LeaveResponse> allLeaves, string sortBy, SortOrderOptions sortOrderOptions);

    }
}
