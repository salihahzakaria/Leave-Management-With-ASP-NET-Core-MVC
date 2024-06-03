﻿using LeaveManagement.Core.Domain.Entities;
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
