using LModels;

namespace Course_Overview.Areas.Admin.Repository
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetAllClass();
        Task<Class> GetOneClass(int id);
        Task AddClass(Class room);   
        Task UpdateClass(Class room);
        Task DeleteClass(int id);
    }
}
