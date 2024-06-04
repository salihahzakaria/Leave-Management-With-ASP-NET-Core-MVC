using LeaveManagement.Core.Domain.Entities;
using LeaveManagement.Core.Domain.RepositoryContracts;
using LeaveManagement.Core.DTO;
using LeaveManagement.Core.Enums;
using LeaveManagement.Core.Helpers;
using LeaveManagement.Core.ServiceContracts;

namespace LeaveManagement.Core.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeaveService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public async Task<LeaveResponse> AddLeave(LeaveAddRequest? leaveAddRequest)
        {
            if (leaveAddRequest == null)
            {
                throw new ArgumentNullException(nameof(leaveAddRequest));
            }

            ValidationHelper.ModelValidation(leaveAddRequest);

            Leave leave = leaveAddRequest.ToLeave();
            leave.Id = Guid.NewGuid();
            await _leaveRepository.AddLeave(leave);
            return leave.ToLeaveResponse();
        }

        public Task<bool> DeleteLeave(Guid? leaveID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LeaveResponse>> GetAllLeaves()
        {
            List<Leave> leaves = await _leaveRepository.GetAllLeaves();
            return leaves.Select(temp => temp.ToLeaveResponse()).ToList();
        }

        public Task<List<LeaveResponse>> GetFilteredLeaves(string searchBy, string? searchString)
        {
            throw new NotImplementedException();
        }

        public async Task<LeaveResponse?> GetLeaveByLeaveID(Guid? leaveID)
        {
            if (leaveID == null)
            {
                return null;
            }

            Leave? leave = await _leaveRepository.GetLeaveByLeaveID(leaveID.Value);

            if (leave == null)
            {
                return null;
            }

            return leave.ToLeaveResponse();
        }

        public async Task<List<LeaveResponse>> GetLeaveByUserID(Guid? userID)
        {
            if (userID == null)
            {
                return null;
            }

            List<Leave> leaves = await _leaveRepository.GetLeaveByUserID(userID.Value);

            if (leaves == null)
            {
                return null;
            }

            return leaves.Select(temp => temp.ToLeaveResponse()).ToList();
        }

        public Task<List<LeaveResponse>> GetSortedLeaves(List<LeaveResponse> allLeaves, string sortBy, SortOrderOptions sortOrderOptions)
        {
            throw new NotImplementedException();
        }

        public async Task<LeaveResponse> UpdateLeave(LeaveUpdateRequest? leaveUpdateRequest)
        {
            if (leaveUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(leaveUpdateRequest));
            }

            //Validation
            ValidationHelper.ModelValidation(leaveUpdateRequest);

            Leave? matchingLeave = await _leaveRepository.GetLeaveByLeaveID(leaveUpdateRequest.Id);

            if (matchingLeave == null)
            {
                throw new ArgumentException("No leave were found");
            }

            //Update all details
            matchingLeave.UserID = leaveUpdateRequest.UserID;
            matchingLeave.LeaveTypeID = leaveUpdateRequest.LeaveTypeID;
            matchingLeave.StartDate = leaveUpdateRequest.StartDate;
            matchingLeave.EndDate = leaveUpdateRequest.EndDate;
            matchingLeave.Reason = leaveUpdateRequest.Reason;
            matchingLeave.Status = leaveUpdateRequest.Status.ToString();
            matchingLeave.ApproverID = leaveUpdateRequest.ApproverID;

            await _leaveRepository.UpdateLeave(matchingLeave);

            return matchingLeave.ToLeaveResponse();
        }
    }
}
