using LeaveManagement.Core.Domain.Entities;
using System.Linq.Expressions;

namespace LeaveManagement.Core.Domain.RepositoryContracts
{
    public interface ILeaveRepository
    {
        Task<Leave> AddLeave(Leave leave);

        Task<List<Leave>> GetAllLeaves();

        Task<Leave?> GetLeaveByLeaveID(Guid leaveID);
        Task<List<Leave>> GetLeaveByUserID(Guid? userID);

        Task<List<Leave>> GetFilteredLeaves(Expression<Func<Leave, bool>> predicate);

        Task<Leave> UpdateLeave(Leave leave);

        Task<bool> DeleteLeave(Guid leaveID);
    }
}
