using LModels;

namespace Course_Overview.Areas.Admin.Repository
{
	public interface ITopicRepository
	{
		Task<IEnumerable<Topic>> GetAllTopic();
		Task<Topic> GetOneTopic(int id);
		Task AddTopic(Topic topic);
		Task UpdateTopic(Topic topic);
		Task DeleteTopic(int id);
		Task<IEnumerable<Topic>> GetTopicsByCourseId(int courseId);
	}
}
