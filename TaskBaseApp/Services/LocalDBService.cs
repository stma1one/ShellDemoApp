using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBaseApp.Models;
using TaskBaseApp.Models.DTOS;
using TaskBaseApp.Service;

namespace TaskBaseApp.Services;

//במצב אמת המחלקה הזו
//תממש את ממשק ITaskServices
//ואז כל מה שישאר זה להחליף את המימוש ב
//DI CONTAINER ב MAUIPROGRAM.CS 
public class LocalDBService
{
	#region קבועים לניהול DB
	const string DatabaseFilename = "FinalTodoSQLite.db3";
	/// <summary>
	/// דגלים לפתיחת מסד הנתונים.
	/// </summary>
	public const SQLite.SQLiteOpenFlags Flags =
	// open the database in read/write mode
	SQLite.SQLiteOpenFlags.ReadWrite |
	// create the database if it doesn't exist
	SQLite.SQLiteOpenFlags.Create |
	// enable multi-threaded database access
	SQLite.SQLiteOpenFlags.SharedCache;
	#endregion

	#region Properties אזור מאפיינים
	/// <summary>

	/// הנתיב לקובץ מסד הנתונים.
	/// </summary>
	string DatabasePath =>
		Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

	/// <summary>
	/// חיבור למסד הנתונים המקומי.
	/// </summary>
	SQLiteAsyncConnection database;
	#endregion

	private async Task Init()
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
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}


	}
	#region Sample Data אזור נתוני דוגמה
	/// <summary>
	/// טעינת נתוני דוגמה למסד הנתונים.
	/// </summary>
	public async Task LoadSampleDataAsync()
	{
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
		TaskDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
		TaskImage = "https://picsum.photos/seed/report/300/200"
	},
	new UserTaskDTO
	{
		UserId =1, // admin
            User=users[0], // admin
            UrgencyLevel = urgencyLevels.FirstOrDefault(u=>u.UrgencyLevelName == "בינונית"), // בינונית
            UrgencyLevelId = 2, // בינונית
            TaskDescription = "להתכונן לפגישת צוות",
		TaskDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
		TaskImage = "https://picsum.photos/seed/meeting/300/200"
	},
	new UserTaskDTO
	{
		UserId = 2, // user1
            User=users[1], // user1
            UrgencyLevel = urgencyLevels.FirstOrDefault(u=>u.UrgencyLevelName == "נמוכה"), // נמוכה
            UrgencyLevelId = 1, // נמוכה
            TaskDescription = "לעדכן את תיעוד הפרויקט",
		TaskDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
		TaskActualDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), // משימה שהושלמה
            TaskImage = "https://picsum.photos/seed/docs/300/200"
	}
};

		await Init();
		try
		{
			await database.DeleteAllAsync<User>();
			await database.DeleteAllAsync<UrgencyLevel>();
			await database.DeleteAllAsync<UserTaskDTO>();
			await database.DeleteAllAsync<TaskCommentDTO>();
		}
		catch (Exception ex)
		{
			// טיפול בשגיאה במחיקת נתונים קיימים
			Console.WriteLine($"Error deleting existing data: {ex.Message}");
			throw;
		}
		// Load sample data into the database if needed
		// טעינת נתוני דוגמה למסד הנתונים במידת הצורך

		try
		{
			if ((await database.Table<User>().CountAsync()) == 0)
			{
				await database.InsertAllAsync(users);
			}
			if (await database.Table<UrgencyLevel>().CountAsync() == 0)
			{
				await database.InsertAllAsync(urgencyLevels);
			}

			if (await database.Table<UserTaskDTO>().CountAsync() == 0)
			{
				try
				{
					await database.InsertAllWithChildrenAsync(tasks, false);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error inserting tasks: {ex.Message}");
					// שגיאה בהוספת משימות
					throw;
				}
			}
			if (await database.Table<TaskCommentDTO>().CountAsync() == 0)
			{
				var comments = new List<TaskCommentDTO>
			{
				new TaskCommentDTO
				{
					TaskId = tasks[0].TaskId, // משימה 1
                    Task=tasks[0],
					Comment = "התחלתי לעבוד על המשימה",
					CommentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2))
				},
				new TaskCommentDTO
				{
					TaskId = tasks[0].TaskId, // משימה 1
                    Task=tasks[0],
					Comment = "המשימה מתקדמת כמתוכנן",
					CommentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1))
				}
			};
				try
				{
					await database.InsertAllWithChildrenAsync(comments, false);
				}
				catch (Exception ex)
				{
					// שגיאה בהוספת תגובות
					Console.WriteLine($"Error inserting comments: {ex.Message}");
					throw;
				}
			}
		}
		catch (Exception ex)
		{
			// שגיאה בטעינת נתוני דוגמה
			Console.WriteLine($"Error loading sample data: {ex.Message}");
			throw;
		}
	}

		}
	#endregion











