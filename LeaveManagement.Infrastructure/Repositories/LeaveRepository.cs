using LeaveManagement.Core.Domain.Entities;
using LeaveManagement.Core.Domain.RepositoryContracts;
using System.Linq.Expressions;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        public Task<Leave> AddLeave(Leave leave)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteLeave(Guid leaveID)
        {
            throw new NotImplementedException();
        }

        public Task<List<Leave>> GetAllLeaves()
        {
            throw new NotImplementedException();
        }

        public Task<List<Leave>> GetFilteredLeaves(Expression<Func<Leave, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Leave?> GetLeaveByLeaveID(Guid leaveID)
        {
            throw new NotImplementedException();
        }

        public Task<List<Leave>> GetLeaveByUserID(Guid? userID)
        {
            throw new NotImplementedException();
        }

        public Task<Leave> UpdateLeave(Leave leave)
        {
            throw new NotImplementedException();
        }
    }
}
