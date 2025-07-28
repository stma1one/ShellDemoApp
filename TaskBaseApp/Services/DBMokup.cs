using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBaseApp.Models;

namespace TaskBaseApp.Service;

/// <summary>
/// מימוש של שירות ההתחברות המשתמש ברשימת משתמשים מקומית (Mock) לצורכי פיתוח ובדיקה.
/// </summary>
public class DBMokup : ITaskServices
{
	// רשימות המשמשות כמסד נתונים מדמה
	List<Models.User> users = new List<Models.User>();
	List<Models.UserTask> tasks = new List<Models.UserTask>();
	List<Models.TaskComment> comments = new List<Models.TaskComment>();
	List<Models.UrgencyLevel> urgencyLevels = new List<Models.UrgencyLevel>();

	/// <summary>
	/// בנאי המאתחל את "מסד הנתונים" עם נתונים לדוגמה.
	/// </summary>
	public DBMokup()
	{
		// אתחול משתמשים לדוגמה
		users.Add(new Models.User { UserId = 1, Username = "admin", Password = "admin", IsAdmin=true });
		users.Add(new Models.User { UserId = 2, Username = "user1", Password = "password1",IsAdmin=false });
		users.Add(new Models.User { UserId = 3, Username = "user2", Password = "password2", IsAdmin=false });

		// אתחול רמות דחיפות
		urgencyLevels.Add(new UrgencyLevel { UrgencyLevelId = 1, UrgencyLevelName = "נמוכה" });
		urgencyLevels.Add(new UrgencyLevel { UrgencyLevelId = 2, UrgencyLevelName = "בינונית" });
		urgencyLevels.Add(new UrgencyLevel { UrgencyLevelId = 3, UrgencyLevelName = "גבוהה" });

		// אתחול משימות לדוגמה
		tasks.Add(new UserTask
		{
			TaskId = 1,
			User = users.First(u => u.UserId == 1), // admin
			UrgencyLevel = urgencyLevels.First(ul => ul.UrgencyLevelId == 3), // גבוהה
			TaskDescription = "לסיים את הדוח השבועי",
			TaskDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
			TaskImage = "https://picsum.photos/seed/report/300/200"
		});

		tasks.Add(new UserTask
		{
			TaskId = 2,
			User = users.First(u => u.UserId == 1), // admin
			UrgencyLevel = urgencyLevels.First(ul => ul.UrgencyLevelId == 2), // בינונית
			TaskDescription = "להתכונן לפגישת צוות",
			TaskDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
			TaskImage = "https://picsum.photos/seed/meeting/300/200"
		});

		tasks.Add(new UserTask
		{
			TaskId = 3,
			User = users.First(u => u.UserId == 2), // user1
			UrgencyLevel = urgencyLevels.First(ul => ul.UrgencyLevelId == 1), // נמוכה
			TaskDescription = "לעדכן את תיעוד הפרויקט",
			TaskDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
			TaskActualDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), // משימה שהושלמה
			TaskImage = "https://picsum.photos/seed/docs/300/200"
		});

		// אתחול תגובות לדוגמה
		comments.Add(new TaskComment
		{
			CommentId = 1,
			Task = tasks.First(t => t.TaskId == 1),
			Comment = "אל תשכח לכלול את הנתונים מהרבעון האחרון.",
			CommentDate = DateOnly.FromDateTime(DateTime.Now)
		});

		comments.Add(new TaskComment
		{
			CommentId = 2,
			Task = tasks.First(t => t.TaskId == 1),
			Comment = "בטיפול.",
			CommentDate = DateOnly.FromDateTime(DateTime.Now)
		});

		// הוספת התגובות למשימה המתאימה
		foreach (var task in tasks)
		{
			task.TaskComments.AddRange(comments.Where(c => c.Task?.TaskId == task.TaskId));
		}
	}

	/// <summary>
	/// מבצע אימות פרטי משתמש מול רשימת המשתמשים המקומית.
	/// </summary>
	/// <param name="username">שם המשתמש לבדיקה.</param>
	/// <param name="password">הסיסמה לבדיקה.</param>
	/// <returns>אמת (true) אם נמצא משתמש עם פרטים תואמים, אחרת שקר (false).</returns>
	public bool Login(string username, string password)
	{
		// חיפוש המשתמש הראשון ברשימה שתואם לשם המשתמש והסיסמה שהתקבלו
		var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
		// אם נמצא משתמש (התוצאה אינה null), ההתחברות הצליחה
		return user != null;
	}

	/// <summary>
	/// מוסיף תגובה למשימה.
	/// </summary>
	/// <param name="comment">התגובה להוספה.</param>
	/// <returns>אמת (true) אם ההוספה הצליחה, אחרת שקר (false).</returns>
	public bool AddComment(TaskComment comment)
	{
		throw new NotImplementedException("This method is not implemented yet.");
	}

	/// <summary>
	/// מוסיף משימה חדשה.
	/// </summary>
	/// <param name="task">המשימה להוספה.</param>
	/// <returns>אמת (true) אם ההוספה הצליחה, אחרת שקר (false).</returns>
	public bool AddTask(UserTask task)
	{
		throw new NotImplementedException("This method is not implemented yet.");
	}

	/// <summary>
	/// מוחק משימה.
	/// </summary>
	/// <param name="taskId">מזהה המשימה למחיקה.</param>
	/// <returns>אמת (true) אם המחיקה הצליחה, אחרת שקר (false).</returns>
	public bool DeleteTask(int taskId)
	{
		throw new NotImplementedException("This method is not implemented yet.");
	}

	/// <summary>
	/// מחזיר את רשימת התגובות למשימה.
	/// </summary>
	/// <param name="taskId">מזהה המשימה.</param>
	/// <returns>רשימת התגובות.</returns>
	public List<TaskComment> GetComments(int taskId)
	{
		throw new NotImplementedException("This method is not implemented yet.");
	}

	/// <summary>
	/// מחזיר את רשימת המשימות של משתמש.
	/// </summary>
	/// <param name="userId">מזהה המשתמש.</param>
	/// <returns>רשימת המשימות.</returns>
	public async Task<List<UserTask>> GetTasks(int userId)
	{
		await Task.Delay(1000); // סימול של עיכוב בטעינת הנתונים (כמו קריאה לבסיס נתונים)
		return this.tasks.Where(x => x?.User?.UserId == userId).Select(x=> new UserTask
		{
			TaskId = x.TaskId,
			User = x.User,
			UrgencyLevel = x.UrgencyLevel,
			TaskDescription = x.TaskDescription,
			TaskDueDate = x.TaskDueDate,
			TaskActualDate = x.TaskActualDate,
			TaskComments = x.TaskComments.Select(c => new TaskComment
			{
				CommentId = c.CommentId,
				Task = c.Task,
				Comment = c.Comment,
				CommentDate = c.CommentDate
			}).ToList(),
			TaskImage = x.TaskImage // Assuming TaskImage is a property in UserTask
		}).ToList();
	}

	/// <summary>
	/// מחזיר את המשתמש הנוכחי על פי שם המשתמש שניתן.
	/// </summary>
	/// <param name="username"></param>
	/// <returns></returns>
	public User? GetCurrentUser(string username)
	{
		return users.FirstOrDefault(u => u.Username == username);
	}
}