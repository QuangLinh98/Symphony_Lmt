using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Service
{
    public class ClassService : IClassRepository
    {
        private readonly DatabaseContext _dbContext;
        public ClassService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddClass(Class room)
        {
            await _dbContext.Classes.AddAsync(room);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteClass(int id)
        {
            var room = await GetOneClass(id);
            if (room != null)
            {
                _dbContext.Classes.Remove(room);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Class>> GetAllClass()
        {
            var classes = await _dbContext.Classes.Include(c => c.Teacher).ToListAsync();
            return classes;
        }

        public async Task<Class> GetOneClass(int id)
        {
            var room = await _dbContext.Classes.FindAsync(id);
            return room;
        }

        public async Task UpdateClass(Class room)
        {
            _dbContext.Classes.Update(room);
            await _dbContext.SaveChangesAsync();
        }
    }

}
