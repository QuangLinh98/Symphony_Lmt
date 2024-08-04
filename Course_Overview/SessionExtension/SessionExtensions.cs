using Microsoft.AspNetCore.Http;
namespace Course_Overview.SessionExtension

{
	public static class SessionExtensions
	{
	public static int? GetStudentId(this ISession session)
	{
		return session.GetInt32("StudentID");
	}
}
}
