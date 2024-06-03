using LeaveManagement.Core.Domain.Entities;
using LeaveManagement.Core.Domain.RepositoryContracts;
using LeaveManagement.Core.DTO;
using LeaveManagement.Core.ServiceContracts;

namespace LeaveManagement.Core.Services
{
    public class LeaveTypeService : ILeaveTypeService
    {
        public readonly ILeaveTypeRepository _leaveTypeRepository;

        public LeaveTypeService(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<LeaveTypeResponse> AddLeaveType(LeaveTypeAddRequest? leaveTypeAddRequest)
        {
            if (leaveTypeAddRequest == null)
            {
                throw new ArgumentNullException(nameof(leaveTypeAddRequest));
            }    

            if (leaveTypeAddRequest.Name == null)
            {
                throw new ArgumentException(nameof(leaveTypeAddRequest.Name));
            }

            if (await _leaveTypeRepository.GetLeaveTypeByName(leaveTypeAddRequest.Name) != null)
            {
                throw new ArgumentException("Leave Type already exists");
            }

            LeaveType leaveType = leaveTypeAddRequest.ToLeaveType();
            leaveType.Id = Guid.NewGuid();
            await _leaveTypeRepository.AddLeaveType(leaveType);
            return leaveType.ToLeaveTypeResponse();
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
