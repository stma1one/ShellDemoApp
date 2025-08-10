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
	public class JokeServiceApi
	{
		HttpClient _httpClient;
		const string URL= $@"https://v2.jokeapi.dev/joke/"; // כתובת השרת
		JsonSerializerOptions options;

		public JokeServiceApi()
		{
			_httpClient = new HttpClient();
			options = new JsonSerializerOptions()
			{
				  PropertyNameCaseInsensitive = true,
				   WriteIndented = true
				  
			};
		}

		public async Task<Joke> GetRandomJoke()
		{
				Joke? joke = null;
			try
			{
				//שליחת בקשה
				HttpResponseMessage response = await _httpClient.GetAsync($@"{URL}Any?safe-mode");
				//בדיקת תקינות
				if (response.IsSuccessStatusCode)
				{

					string jsonString= await response.Content.ReadAsStringAsync();
					//המרת התשובה לאובייקט
					//joke=JsonSerializer.Deserialize<Joke>(jsonString,options);

					JsonNodeOptions jsonNodeOptions = new JsonNodeOptions(){ PropertyNameCaseInsensitive = true };
					JsonNode node = JsonNode.Parse(jsonString,jsonNodeOptions);
					if (node?["error"]?.GetValue<bool>() == true)
					{
						joke = new Joke()
						{
							ServiceError = JsonSerializer.Deserialize<ServiceError>(jsonString, options)

						};
					}
					else
						if((node?["type"]?.GetValue<string>() == "single"))
							joke=JsonSerializer.Deserialize<OneLiner>(jsonString,options);
					else
						joke=JsonSerializer.Deserialize<TwoPartJoke>(jsonString,options);


				}

				//החזרת התשובה למי שביקש את השרות
				return joke;
			}
			catch (Exception ex) {
				Console.WriteLine("error");
			}
			return joke;
			
		}


	}
}
