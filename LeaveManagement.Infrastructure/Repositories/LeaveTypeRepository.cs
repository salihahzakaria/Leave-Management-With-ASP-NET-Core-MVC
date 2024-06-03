using LeaveManagement.Core.Domain.Entities;
using LeaveManagement.Core.Domain.RepositoryContracts;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        public Task<LeaveType> AddLeaveType(LeaveType leaveType)
        {
            throw new NotImplementedException();
        }

        public Task<List<LeaveType>> GetAllLeavesType()
        {
            throw new NotImplementedException();
        }

        public Task<LeaveType?> GetLeaveTypeByID(Guid leaveTypeID)
        {
            throw new NotImplementedException();
        }

        public Task<LeaveType?> GetLeaveTypeByName(string leaveTypeName)
        {
            throw new NotImplementedException();
        }
    }
}
