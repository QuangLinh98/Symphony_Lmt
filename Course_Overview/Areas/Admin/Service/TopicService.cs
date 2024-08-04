using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Service
{
	public class TopicService : ITopicRepository
	{
		private readonly DatabaseContext _dbContext;
		public TopicService(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task AddTopic(Topic topic)
		{
			await _dbContext.AddAsync(topic);
			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteTopic(int id)
		{
			var topicExisting = await GetOneTopic(id);
			if (topicExisting != null)
			{
				_dbContext.Topics.Remove(topicExisting);
				await _dbContext.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Topic>> GetAllTopic()
		{
			var topics = await _dbContext.Topics.Include(t => t.Course).ToListAsync();
			return topics;
		}

		public async Task<Topic> GetOneTopic(int id)
		{
			var topic = await _dbContext.Topics.FindAsync(id);
			return topic;
		}

		public async Task<IEnumerable<Topic>> GetTopicsByCourseId(int courseId)
		{
			return await _dbContext.Topics
		           .Where(t => t.CourseID == courseId) 
		           .ToListAsync();
		}

		public async Task UpdateTopic(Topic topic)
		{
			_dbContext.Topics.Update(topic);
			await _dbContext.SaveChangesAsync();
		}
	}
}
