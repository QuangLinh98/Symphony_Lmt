using LModels;

namespace Course_Overview.Areas.Admin.Repository
{
    public interface IAboutRepository
    {
        Task<IEnumerable<AboutUs>> GetAllAbout();
        Task<AboutUs> GetOneAbout(int id);
        Task AddAbout(AboutUs aboutUs);
        Task UpdateAbout(AboutUs aboutUs);
        Task DeleteAbout(int id);
    }
}
