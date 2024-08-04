using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Service
{
    public class AboutService : IAboutRepository
    {
        private readonly DatabaseContext _dbContext;
        public AboutService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAbout(AboutUs aboutUs)
        {
            await _dbContext.AboutUs.AddAsync(aboutUs);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAbout(int id)
        {
            var about = await GetOneAbout(id);
            if (about != null)
            {
                _dbContext.AboutUs.Remove(about);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AboutUs>> GetAllAbout()
        {
            var abouts = await _dbContext.AboutUs.ToListAsync();
            return abouts;
        }

        public async Task<AboutUs> GetOneAbout(int id)
        {
            var aboutExisting = await _dbContext.AboutUs.FindAsync(id);
            return aboutExisting;
        }

        public async Task UpdateAbout(AboutUs aboutUs)
        {
            _dbContext.AboutUs.Update(aboutUs);
            await _dbContext.SaveChangesAsync();
        }
    }
}
