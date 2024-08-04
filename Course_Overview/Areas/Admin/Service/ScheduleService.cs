using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Service
{
    public class ScheduleService : IScheduleRepository
    {
        private readonly DatabaseContext _dbContext;
        public ScheduleService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddScheduleAsync(Schedule schedule)
        {
            await _dbContext.Schedules.AddAsync(schedule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteScheduleAsync(int id)
        {
            var schedule = await _dbContext.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _dbContext.Schedules.Remove(schedule);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Schedule>> GetAllScheduleAsync()
        {
            return await _dbContext.Schedules
                .Include(sc => sc.Course)
                .Include(sc => sc.Class)
                .ToListAsync();
        }

        public async Task<Schedule> GetScheduleByIdAsync(int id)
        {
            return await _dbContext.Schedules.Include(s => s.Course).Include(s => s.Class).FirstOrDefaultAsync(s => s.ScheduleID == id);
        }

        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            _dbContext.Schedules.Update(schedule);
            await _dbContext.SaveChangesAsync() ;
        }
    }
}
