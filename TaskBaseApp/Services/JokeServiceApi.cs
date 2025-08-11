using JokesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace TaskBaseApp.Services
{
	/// <summary>
	/// שירות לקבלת בדיחות מ-API חיצוני.
	/// </summary>
	public class JokeServiceApi
	{
		HttpClient _httpClient;
		const string URL = $@"https://v2.jokeapi.dev/joke/"; // כתובת השרת של ה-API לבדיחות
		JsonSerializerOptions options;

		/// <summary>
		/// בנאי השירות. מאתחל HttpClient ואופציות לסריאליזציה.
		/// </summary>
		public JokeServiceApi()
		{
			_httpClient = new HttpClient();
			options = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true,
				WriteIndented = true
			};
		}

		/// <summary>
		/// מביא בדיחה אקראית מה-API.
		/// </summary>
		/// <returns>אובייקט מסוג Joke (יכול להיות OneLiner, TwoPartJoke או שגיאה)</returns>
		public async Task<Joke> GetRandomJoke()
		{
			Joke? joke = null;
			try
			{
				// שליחת בקשת GET לשרת לבדיחה אקראית במצב בטוח
				HttpResponseMessage response = await _httpClient.GetAsync($@"{URL}Any?safe-mode");

				// בדיקת תקינות התשובה מהשרת
				if (response.IsSuccessStatusCode)
				{
					string jsonString = await response.Content.ReadAsStringAsync();

					// המרת התשובה לאובייקט JsonNode
					JsonNodeOptions jsonNodeOptions = new JsonNodeOptions() { PropertyNameCaseInsensitive = true };
					JsonNode node = JsonNode.Parse(jsonString, jsonNodeOptions);

					// בדיקה אם חזרה שגיאה מהשירות
					if (node?["error"]?.GetValue<bool>() == true)
					{
						joke = new Joke()
						{
							ServiceError = JsonSerializer.Deserialize<ServiceError>(jsonString, options)
						};
					}
					else if ((node?["type"]?.GetValue<string>() == "single"))
					{
						// בדיחה מסוג שורה אחת
						joke = JsonSerializer.Deserialize<OneLiner>(jsonString, options);
					}
					else
					{
						// בדיחה מסוג שתי חלקים
						joke = JsonSerializer.Deserialize<TwoPartJoke>(jsonString, options);
					}
				}

				// החזרת הבדיחה או השגיאה למי שקרא לשירות
				return joke;
			}
			catch (Exception ex)
			{
				// טיפול בשגיאות כלליות
				Console.WriteLine("error");
			}
			return joke;
		}
	}
}