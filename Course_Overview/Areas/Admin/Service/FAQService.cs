using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Service
{
    public class FAQService : IFAQRepository
    {
        private readonly DatabaseContext _dbContext;
        public FAQService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddFAQ(FAQ fAQ)
        {
            await _dbContext.FAQs.AddAsync(fAQ);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteFAQ(int id)
        {
            var FaqExisting = await GetOneFAQ(id);
            if (FaqExisting != null)
            {
                _dbContext.FAQs.Remove(FaqExisting);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<FAQ>> GetAllFAQ()
        {
            var Faqs = await _dbContext.FAQs.ToListAsync();
            return Faqs;
        }

        public async Task<FAQ> GetOneFAQ(int id)
        {
            var FaqExisting = await _dbContext.FAQs.FindAsync(id);
            return FaqExisting;
        }

        public async Task UpdateFAQ(FAQ fAQ)
        {
            _dbContext.FAQs.Update(fAQ);
            await _dbContext.SaveChangesAsync();
        }
    }
}
