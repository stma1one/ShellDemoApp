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
	public class UserAndFileServices
	{
		HttpClient client;
		const string URL = @"https://api.escuelajs.co/api/v1/";
		JsonSerializerOptions options;

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

		public async Task<int> CreateUserAsync(User user)
		{
			//המרת האובייקט לגייסון
			string json=JsonSerializer.Serialize(user,options);

			//לקחת את הגייסון ולהמיר אותו לחבילת CONTENT
			StringContent content = new StringContent(json,Encoding.UTF8,"application/json");

			//לשלוח לשרת
			HttpResponseMessage response = await client.PostAsync(@$"{URL}users", content);

			//לבדוק שהכל תקין
			if (response.IsSuccessStatusCode)
			{

				//לטפל בתשובה
				json=await response.Content.ReadAsStringAsync();
				User u=JsonSerializer.Deserialize<User>(json,options);
				return u.UserId;
			}
			return 0;
		}

		public async Task<bool> UploadImageAsync(string filePath)
		{
			if (!File.Exists(filePath)) { return false; }

			using var multiPartContent = new MultipartFormDataContent();

			//לקרוא את הקובץ ולהמיר אותו לרצף בתים
			// Open a stream to the file. The 'await using' statement ensures the stream is 
			// properly disposed of even if an error occurs.
			await using var fileStream=File.OpenRead(filePath);
			var fileContent=new StreamContent(fileStream);
			fileContent.Headers.ContentType=new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");


			//לעטוף אותו במוליטפארט
			multiPartContent.Add(fileContent);

			//לשלוח לשרת
			HttpResponseMessage response = await client.PostAsync($@"{URL}files/upload", multiPartContent);

			//טיפול בתשובה
			if (response.IsSuccessStatusCode)
				return true;

			return false;
		}

	}
}
