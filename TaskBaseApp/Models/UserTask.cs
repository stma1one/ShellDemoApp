using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskBaseApp.Models;


namespace TaskBaseApp.Models;

/// <summary>
/// מייצג משימה של משתמש.
/// </summary>
public class UserTask
{
	/// <summary>
	/// מזהה ייחודי של המשימה.
	/// </summary>
	public int TaskId
	{
		get; set;
	}

	/// <summary>
	/// המשתמש המשויך למשימה.
	/// </summary>
	public User? User
	{
		get; set;
	}

	/// <summary>
	/// רמת הדחיפות של המשימה.
	/// </summary>
	public UrgencyLevel? UrgencyLevel
	{
		get; set;
	}

	/// <summary>
	/// תיאור המשימה.
	/// </summary>
	public string TaskDescription { get; set; } = null!;

	/// <summary>
	/// תאריך היעד לביצוע המשימה.
	/// </summary>
	public DateOnly TaskDueDate
	{
		get; set;
	}

	/// <summary>
	/// תאריך הביצוע בפועל של המשימה. יכול להיות ריק אם המשימה טרם הושלמה.
	/// </summary>
	public DateOnly? TaskActualDate
	{
		get; set;
	}

	/// <summary>
	/// רשימת התגובות למשימה.
	/// </summary>
	public List<TaskComment> TaskComments { get; set; } = new List<TaskComment>();

	/// <summary>
	/// כתובת URL של תמונה המשויכת למשימה.
	/// </summary>
	public string TaskImage { get; set; } = null!;

	/// <summary>
	/// בנאי ריק.
	/// </summary>
	public UserTask()
	{
	}

	
}