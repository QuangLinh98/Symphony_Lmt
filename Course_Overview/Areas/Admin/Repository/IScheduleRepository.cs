using LModels;

namespace Course_Overview.Areas.Admin.Repository
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>> GetAllScheduleAsync();
        Task<Schedule> GetScheduleByIdAsync(int id);
        Task AddScheduleAsync(Schedule schedule);
        Task UpdateScheduleAsync(Schedule schedule);
        Task DeleteScheduleAsync(int id);
    }
}
