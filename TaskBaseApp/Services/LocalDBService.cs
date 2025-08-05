using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using TaskBaseApp.Models;
using TaskBaseApp.Models.DTOS;

namespace TaskBaseApp.Services;


public class LocalDBService
{
	 const string DatabaseFilename = "FinalTodoSQLite.db3";

	public const SQLite.SQLiteOpenFlags Flags =
		// open the database in read/write mode
		SQLite.SQLiteOpenFlags.ReadWrite |
		// create the database if it doesn't exist
		SQLite.SQLiteOpenFlags.Create |
		// enable multi-threaded database access
		SQLite.SQLiteOpenFlags.SharedCache;
	string DatabasePath =>
	Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
	//this is the local database connection
	SQLiteAsyncConnection database;

	async Task Init()
	{		
		if (database is not null)
			return;
		try
		{
			database = new SQLiteAsyncConnection(DatabasePath, Flags);
			await database.CreateTableAsync<User>();
			await database.CreateTableAsync<UrgencyLevel>();
			await database.CreateTableAsync<UserTaskDTO>();
			await database.CreateTableAsync<TaskCommentDTO>();
		}
		catch(Exception ex)
		{
			// Handle exceptions such as database creation failure
			Console.WriteLine($"Unable to create database: {ex.Message}");
			throw;
		}	
	}

	public async Task LoadSampleDataAsync(){
		var users = new List<User>
			{
				new User { UserId=1, Username = "admin", Password = "admin", IsAdmin=true },
				new User { UserId=2, Username = "user1", Password = "password1" },
				new User { UserId=3, Username = "user2", Password = "password2" }
			};

		var urgencyLevels = new List<UrgencyLevel>
			{
				new UrgencyLevel { UrgencyLevelId=1, UrgencyLevelName = "נמוכה" },
				new UrgencyLevel { UrgencyLevelId=2, UrgencyLevelName = "בינונית" },
				new UrgencyLevel { UrgencyLevelId=3, UrgencyLevelName = "גבוהה" }
			};
		var tasks = new List<UserTaskDTO>
			{
				new UserTaskDTO
				{
					UserId = 1, // admin
					User=users[0], // admin
					UrgencyLevel = urgencyLevels.FirstOrDefault(u=>u.UrgencyLevelName == "גבוהה"), // גבוהה
					UrgencyLevelId = 3, // גבוהה
					TaskDescription = "לסיים את הדוח השבועי",
					TaskDueDate = DateTime.Now.AddDays(1),
					TaskImage = "https://picsum.photos/seed/report/300/200"
				},
				new UserTaskDTO
				{
					UserId =1, // admin
					User=users[0], // admin
				UrgencyLevel = urgencyLevels.FirstOrDefault(u=>u.UrgencyLevelName == "בינונית"), // גבוהה
					UrgencyLevelId = 2, // בינונית
					
					TaskDescription = "להתכונן לפגישת צוות",
					TaskDueDate = DateTime.Now.AddDays(3),
					TaskImage = "https://picsum.photos/seed/meeting/300/200"
				},
				new UserTaskDTO
				{
					UserId = 2, // user1
					User=users[1], // admin
					UrgencyLevel = urgencyLevels.FirstOrDefault(u=>u.UrgencyLevelName == "נמוכה"), // גבוהה
					UrgencyLevelId = 1, // נמוכה
					
					TaskDescription = "לעדכן את תיעוד הפרויקט",
					TaskDueDate = DateTime.Now.AddDays(7),
					TaskActualDate = DateTime.Now.AddDays(-1), // משימה שהושלמה
					TaskImage = "https://picsum.photos/seed/docs/300/200"
				}
			};
		
		await Init();
		await database.DeleteAllAsync<User>();
		await database.DeleteAllAsync<UrgencyLevel>();
		await database.DeleteAllAsync<UserTaskDTO>();
		await database.DeleteAllAsync<TaskCommentDTO>();
		// Load sample data into the database if needed
		
		if ((await database.Table<User>().CountAsync()) == 0)
		{
			
			await database.InsertAllAsync(users);
		}
		if(await database.Table<UrgencyLevel>().CountAsync() == 0)
		{
			
			await database.InsertAllAsync(urgencyLevels);
		}
		
		
		if(await database.Table<UserTaskDTO>().CountAsync() == 0)
		{
			
			try
			{
				await database.InsertAllWithChildrenAsync(tasks, false);
				
			}
			catch(Exception ex)
			{
				Console.WriteLine($"Error inserting tasks: {ex.Message}");
				throw;
			}
			
		}
		if(await database.Table<TaskCommentDTO>().CountAsync() == 0)
		{
			var comments = new List<TaskCommentDTO>
			{
				new TaskCommentDTO
				{
					TaskId = tasks[0].TaskId, // משימה 1
					Task=tasks[0],
					Comment = "התחלתי לעבוד על המשימה",
					CommentDate = DateTime.Now.AddDays(-2)
				},
				new TaskCommentDTO
				{
					TaskId = tasks[0].TaskId, // משימה 2
					Task=tasks[0],
					Comment = "המשימה מתקדמת כמתוכנן",
					CommentDate = DateTime.Now.AddDays(-1)
				}
			};
			await database.InsertAllWithChildrenAsync(comments,false);
		}
	}
	public async Task<List<UserTaskDTO>> GetUserTasksAsync(int userId)
	{
		await Init();
		var list = (await database.GetAllWithChildrenAsync<UserTaskDTO>(task=>task.UserId==userId));
		

		
		return list;
	}

	public async Task<bool> SaveTaskAsync(UserTaskDTO task)
	{
		await Init();
		int result = 0;
		if (task.TaskId != 0)
		{
			result = await database.UpdateAsync(task);
		}
		else
		{
			result = await database.InsertAsync(task);
		}
		return result == 0;
	}

}
