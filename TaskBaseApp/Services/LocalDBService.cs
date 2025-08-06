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

/// <summary>
/// שירות לניהול מסד נתונים מקומי.
/// </summary>
public class LocalDBService
{
    #region Constants אזור קבועים
    /// <summary>
    /// שם קובץ מסד הנתונים.
    /// </summary>
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

    #region Initialization אזור אתחול
    /// <summary>
    /// אתחול מסד הנתונים המקומי.
    /// </summary>
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
            // טיפול בשגיאות כגון כישלון ביצירת מסד הנתונים
            Console.WriteLine($"Unable to create database: {ex.Message}");
            throw;
        }
    }
    #endregion

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
                TaskDueDate = DateTime.Now.AddDays(1),
                TaskImage = "https://picsum.photos/seed/report/300/200"
            },
            new UserTaskDTO
            {
                UserId =1, // admin
                User=users[0], // admin
                UrgencyLevel = urgencyLevels.FirstOrDefault(u=>u.UrgencyLevelName == "בינונית"), // בינונית
                UrgencyLevelId = 2, // בינונית
                TaskDescription = "להתכונן לפגישת צוות",
                TaskDueDate = DateTime.Now.AddDays(3),
                TaskImage = "https://picsum.photos/seed/meeting/300/200"
            },
            new UserTaskDTO
            {
                UserId = 2, // user1
                User=users[1], // user1
                UrgencyLevel = urgencyLevels.FirstOrDefault(u=>u.UrgencyLevelName == "נמוכה"), // נמוכה
                UrgencyLevelId = 1, // נמוכה
                TaskDescription = "לעדכן את תיעוד הפרויקט",
                TaskDueDate = DateTime.Now.AddDays(7),
                TaskActualDate = DateTime.Now.AddDays(-1), // משימה שהושלמה
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
                    // שגיאה בהוספת משימות
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
                        TaskId = tasks[0].TaskId, // משימה 1
                        Task=tasks[0],
                        Comment = "המשימה מתקדמת כמתוכנן",
                        CommentDate = DateTime.Now.AddDays(-1)
                    }
                };
                try
                {
                    await database.InsertAllWithChildrenAsync(comments,false);
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
    #endregion

    #region CRUD אזור פעולות CRUD
    /// <summary>
    /// מחזיר את רשימת המשימות של משתמש לפי מזהה.
    /// </summary>
    /// <param name="userId">מזהה משתמש</param>
    /// <returns>רשימת משימות</returns>
    public async Task<List<UserTaskDTO>> GetUserTasksAsync(int userId)
    {
        await Init();
        try
        {
            var list = (await database.GetAllWithChildrenAsync<UserTaskDTO>(task=>task.UserId==userId));
            return list;
        }
        catch (Exception ex)
        {
            // שגיאה בקבלת משימות משתמש
            Console.WriteLine($"Error getting user tasks: {ex.Message}");
            return new List<UserTaskDTO>();
        }
    }

    /// <summary>
    /// שמירת משימה במסד הנתונים.
    /// </summary>
    /// <param name="task">אובייקט משימה</param>
    /// <returns>הצלחה/כישלון</returns>
    public async Task<bool> SaveTaskAsync(UserTaskDTO task)
    {
        await Init();
        int result = 0;
        UrgencyLevel level= (await database.QueryAsync<UrgencyLevel>("SELECT * FROM UrgencyLevel WHERE UrgencyLevelId = ?", task.UrgencyLevelId)).FirstOrDefault();
		task.UrgencyLevel = level; // עדכון רמת דחיפות מה DB
		try
        {
            if (task.TaskId != 0)
            {
                result = await database.UpdateAsync(task);
            }
            else
            {
               await database.InsertWithChildrenAsync(task);
            }
            return result == 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving task: {ex.Message}");
            // שגיאה בשמירת משימה
            return false;
        }
    }

    /// <summary>
    /// מחיקת משימה ממסד הנתונים.
    /// </summary>
    /// <param name="task">אובייקט משימה</param>
    /// <returns>הצלחה/כישלון</returns>
    public async Task<bool> DeleteTaskAsync(int taskId)
    {
        try
        {
            await Init();
            var task = await database.GetWithChildrenAsync<UserTaskDTO>(taskId);
            await database.DeleteAsync(task, true);
            var list = await database.Table<TaskCommentDTO>().ToListAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting task: {ex.Message}");
            // שגיאה במחיקת משימה
            return false;
        }
    }
    #endregion
}
