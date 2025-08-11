using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskBaseApp.Models;

namespace TaskBaseApp.Services
{
	/// <summary>
	/// שירות לניהול משתמשים והעלאת קבצים לשרת.
	/// </summary>
	public class UserAndFileServices
	{
		HttpClient client;
		const string URL = @"https://api.escuelajs.co/api/v1/"; // כתובת הבסיס של ה-API
		JsonSerializerOptions options;

		/// <summary>
		/// בנאי השירות. מאתחל HttpClient ואופציות לסריאליזציה.
		/// </summary>
		public UserAndFileServices()
		{
			options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
				WriteIndented = true,
			};

			CookieContainer cookies = new CookieContainer();
			HttpClientHandler handler = new HttpClientHandler()
			{
				CookieContainer = cookies,
				UseCookies = true
			};
			client = new HttpClient(handler);
		}

		/// <summary>
		/// יוצר משתמש חדש בשרת.
		/// </summary>
		/// <param name="user">אובייקט משתמש למילוי פרטי המשתמש</param>
		/// <returns>מזהה המשתמש החדש אם הצליח, אחרת 0</returns>
		public async Task<int> CreateUserAsync(User user)
		{
			// המרת האובייקט לג'ייסון
			string json = JsonSerializer.Serialize(user, options);

			// יצירת תוכן הבקשה מסוג JSON
			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

			// שליחת הבקשה לשרת
			HttpResponseMessage response = await client.PostAsync(@$"{URL}users", content);

			// בדיקת תקינות התשובה
			if (response.IsSuccessStatusCode)
			{
				// טיפול בתשובת השרת
				json = await response.Content.ReadAsStringAsync();
				User u = JsonSerializer.Deserialize<User>(json, options);
				return u.UserId;
			}
			return 0;
		}

		/// <summary>
		/// מעלה תמונה לשרת.
		/// </summary>
		/// <param name="filePath">הנתיב המלא של הקובץ במכשיר</param>
		/// <returns>מחזיר true אם ההעלאה הצליחה, אחרת false</returns>
		public async Task<bool> UploadImageAsync(string filePath)
		{
			// בדיקה אם הקובץ קיים
			if (!File.Exists(filePath)) { return false; }

			using var multiPartContent = new MultipartFormDataContent();

			// קריאת הקובץ ויצירת StreamContent
			await using var fileStream = File.OpenRead(filePath);
			var fileContent = new StreamContent(fileStream);
			fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

			// הוספת הקובץ ל-MultipartFormDataContent
			multiPartContent.Add(fileContent, "file", Path.GetFileName(filePath));

			// שליחת הבקשה לשרת
			HttpResponseMessage response = await client.PostAsync($@"{URL}files/upload", multiPartContent);

			// בדיקת תקינות התשובה
			if (response.IsSuccessStatusCode)
				return true;

			return false;
		}
	}
}