using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using TaskBaseApp.Models;

namespace TaskBaseApp.Services;

/// <summary>
/// שירות לקבלת ושליחת בדיחות מהשרת.
/// </summary>
public class JokeService
{
	#region שדות פרטיים
	HttpClient httpClient; // אובייקט לשליחת בקשות וקבלת תשובות מהשרת
	JsonSerializerOptions options; // פרמטרים שישמשו אותנו להגדרות ה-JSON
	const string URL = $@"https://v2.jokeapi.dev/"; // כתובת השרת
	#endregion

	#region בנאי
	/// <summary>
	/// בנאי השירות, מאתחל את ה-HttpClient והגדרות ה-JSON.
	/// </summary>
	public JokeService()
	{
		// יצירת אובייקט HttpClient
		httpClient = new HttpClient();

		// הגדרות סידור/פענוח JSON
		options = new JsonSerializerOptions()
		{
			PropertyNameCaseInsensitive = true,
			WriteIndented = true
		};
	}
	#endregion

	#region מתודות עיקריות
	/// <summary>
	/// מביא בדיחה אקראית מהשרת.
	/// </summary>
	/// <returns>אובייקט בדיחה</returns>
	public async Task<Joke> GetRandomJoke()
	{
		Joke? j = null;
		HttpResponseMessage response = await httpClient.GetAsync($"{URL}joke/Any?safe-mode");

		 // בדיקה אם הבקשה הצליחה
		if (response.IsSuccessStatusCode)
		{
			string jsonString = await response.Content.ReadAsStringAsync();

			j = JsonSerializer.Deserialize<Joke>(jsonString, options);

			JsonNodeOptions nodeOptions = new JsonNodeOptions() { PropertyNameCaseInsensitive = true };
			JsonNode? node = JsonNode.Parse(jsonString, nodeOptions);
			{
				// בדיקה אם התקבלה שגיאה מהשרת
				if (node?["error"]?.GetValue<bool>() == true)
				{
					j = new Joke()
					{
						ServiceError = JsonSerializer.Deserialize<ServiceError>(jsonString)
					};
				}
				else if (node?["type"]?.GetValue<string>() == "single")
					j = JsonSerializer.Deserialize<OneLiner>(jsonString, options);
				else
					j = JsonSerializer.Deserialize<TwoPartJoke>(jsonString, options);
			}
		}
		return j;
	}

	/// <summary>
	/// שליחת בדיחה חדשה לשרת.
	/// </summary>
	/// <param name="j">אובייקט בדיחה</param>
	/// <returns>האם הבדיחה התקבלה בהצלחה</returns>
	public async Task<bool> SubmitJokeAsync(MyJoke j)
	{
		JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

		// סידור האובייקט ל-JSON
		string jsonString = JsonSerializer.Serialize(j, options);

		// הכנסת ה-JSON לאובייקט StringContent
		StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

		// שליחת הבדיחה לשרת
		var response = await httpClient.PostAsync($"{URL}Submit?dry-run", content);

		// בדיקת סטטוס התשובה
		if (response.StatusCode == System.Net.HttpStatusCode.Created)
			return true;
		else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
			return false;
		return false;
	}
	#endregion
}
