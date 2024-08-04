using LModels;

namespace Course_Overview.Areas.Admin.Repository
{
	public interface ITeacherRepository
	{
		Task<IEnumerable<Teacher>> GetAllTeacher();
		Task<Teacher> GetOneTeacher(int id);
		Task AddTeacher(Teacher teacher);
		Task UpdateTeacher(Teacher teacher);
		Task DeleteTeacher(int id);
    }
}
