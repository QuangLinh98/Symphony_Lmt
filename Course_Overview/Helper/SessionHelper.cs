using LModels;
using Newtonsoft.Json;

namespace Course_Overview.Helper
{
	public static class SessionHelper
	{
		public static void SetSession(HttpContext context , string key,  string value)
		{
			context.Session.SetString(key, value);
		}

		public static string GetSession(HttpContext context , string key)
		{
			return context.Session.GetString(key);
		}

		public static void ClearSession (HttpContext context)
		{
			context.Session.Clear();
		}

		public static User GetUserFromSession(HttpContext context)
		{
			var userJson = context.Session.GetString("userSession");
            if (string.IsNullOrEmpty(userJson))
            {
				return null;
            }
			return JsonConvert.DeserializeObject<User>(userJson);
		}
	}
}
