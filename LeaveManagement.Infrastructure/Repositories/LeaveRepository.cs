using LeaveManagement.Core.Domain.Entities;
using LeaveManagement.Core.Domain.RepositoryContracts;
using LeaveManagement.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Leave> AddLeave(Leave leave)
        {
            _db.Leave.Add(leave);
            await _db.SaveChangesAsync();
            return leave;
        }

        public async Task<bool> DeleteLeave(Guid leaveID)
        {
            _db.Leave
                .RemoveRange(_db.Leave
                .Where(temp => temp.Id == leaveID));

            int rowsDeleted = await _db.SaveChangesAsync();
            return rowsDeleted > 0;
        }

        public async Task<List<Leave>> GetAllLeaves()
        {
            return await _db.Leave
                .Include("User")
                .Include("LeaveType")
                .Include("Approver")
                .ToListAsync();
        }

        public async Task<List<Leave>> GetFilteredLeaves(Expression<Func<Leave, bool>> predicate)
        {
            return await _db.Leave
                .Include("User")
                .Include("LeaveType")
                .Include("Approver")
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Leave?> GetLeaveByLeaveID(Guid leaveID)
        {
            return await _db.Leave
                .Include("User")
                .Include("LeaveType")
                .Include("Approver")
                .FirstOrDefaultAsync(temp => temp.Id == leaveID);
        }

        public async Task<List<Leave>> GetLeaveByUserID(Guid? userID)
        {
            return await _db.Leave
                .Include("User")
                .Include("LeaveType")
                .Include("Approver")
                .Where(temp => temp.UserID == userID)
                .ToListAsync();
        }

        public async Task<Leave> UpdateLeave(Leave leave)
        {
            Leave? matchingLeave = await _db.Leave
                .Include("User")
                .Include("LeaveType")
                .Include("Approver")
                .FirstOrDefaultAsync(temp => temp.Id == leave.Id);

            if (matchingLeave == null)
            {
                return leave;
            }

            matchingLeave.UserID = leave.UserID;
            matchingLeave.LeaveTypeID = leave.LeaveTypeID;
            matchingLeave.StartDate = leave.StartDate;
            matchingLeave.EndDate = leave.EndDate;
            matchingLeave.Reason = leave.Reason;
            matchingLeave.Status = leave.Status;
            matchingLeave.ApproverID = leave.ApproverID;

            await _db.SaveChangesAsync();

            return matchingLeave;
        }
    }
}
