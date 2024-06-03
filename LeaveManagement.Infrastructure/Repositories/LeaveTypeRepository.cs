using LeaveManagement.Core.Domain.Entities;
using LeaveManagement.Core.Domain.RepositoryContracts;
using LeaveManagement.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<LeaveType> AddLeaveType(LeaveType leaveType)
        {
            _db.LeaveType.Add(leaveType);
            await _db.SaveChangesAsync();
            return leaveType;
        }

        public async Task<List<LeaveType>> GetAllLeavesType()
        {
            return await _db.LeaveType.ToListAsync();
        }

        public async Task<LeaveType?> GetLeaveTypeByID(Guid leaveTypeID)
        {
            return await _db.LeaveType.FirstOrDefaultAsync(temp => temp.Id == leaveTypeID);
        }

        public async Task<LeaveType?> GetLeaveTypeByName(string leaveTypeName)
        {
            return await _db.LeaveType.FirstOrDefaultAsync(temp => temp.Name == leaveTypeName);
        }
    }
}
