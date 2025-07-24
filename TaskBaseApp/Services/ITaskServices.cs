using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBaseApp.Models;

namespace TaskBaseApp.Service;

/// <summary>
/// ממשק (Interface) המגדיר את החוזה עבור שירותי התחברות וניהול משימות.
/// </summary>
public interface ITaskServices
{

	/// <summary>
	/// פעולה לקבלת המשתמש הנוכחי.
	/// </summary>
	/// <param name="username"></param>
	/// <returns></returns>
	public User? GetCurrentUser(string username);
	
	/// <summary>
	/// פעולה לאימות פרטי משתמש.
	/// </summary>
	/// <param name="username">שם המשתמש.</param>
	/// <param name="password">סיסמת המשתמש.</param>
	/// <returns>מחזירה אמת (true) אם ההתחברות הצליחה, אחרת שקר (false).</returns>
	public bool Login(string username, string password);

	/// <summary>
	/// מחזיר את רשימת המשימות של משתמש.
	/// </summary>
	/// <param name="userId">מזהה המשתמש.</param>
	/// <returns>רשימת המשימות.</returns>
	public  Task<List<UserTask>> GetTasks(int userId);

	/// <summary>
	/// מוסיף משימה חדשה.
	/// </summary>
	/// <param name="task">המשימה להוספה.</param>
	/// <returns>אמת (true) אם ההוספה הצליחה, אחרת שקר (false).</returns>
	public bool AddTask(UserTask task);

	/// <summary>
	/// מוחק משימה.
	/// </summary>
	/// <param name="taskId">מזהה המשימה למחיקה.</param>
	/// <returns>אמת (true) אם המחיקה הצליחה, אחרת שקר (false).</returns>
	public bool DeleteTask(int taskId);

	/// <summary>
	/// מחזיר את רשימת התגובות למשימה.
	/// </summary>
	/// <param name="taskId">מזהה המשימה.</param>
	/// <returns>רשימת התגובות.</returns>
	public List<TaskComment> GetComments(int taskId);

	/// <summary>
	/// מוסיף תגובה למשימה.
	/// </summary>
	/// <param name="comment">התגובה להוספה.</param>
	/// <returns>אמת (true) אם ההוספה הצליחה, אחרת שקר (false).</returns>
	public bool AddComment(TaskComment comment);
}