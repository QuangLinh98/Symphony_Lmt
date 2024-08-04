using LModels;

namespace Course_Overview.Areas.Admin.Repository
{
    public interface IFAQRepository
    {
        Task<IEnumerable<FAQ>> GetAllFAQ();
        Task<FAQ> GetOneFAQ(int id);
        Task AddFAQ(FAQ fAQ);
        Task UpdateFAQ(FAQ fAQ);
        Task DeleteFAQ(int id);
    }
}
