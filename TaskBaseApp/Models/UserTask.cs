using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TaskBaseApp.Models;
using TaskBaseApp.Models.DTOS;



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
	public UserTask(UserTaskDTO t)
	{
		TaskId = t.TaskId;
		User = t.User;
		TaskDescription = t.TaskDescription;
		TaskDueDate = DateOnly.FromDateTime(t.TaskDueDate);
		TaskImage = t.TaskImage;
		TaskActualDate = DateOnly.FromDateTime(t.TaskActualDate);
		UrgencyLevel = t.UrgencyLevel;
		
		TaskComments = new List<TaskComment>(t.TaskComments.Select(t => new TaskComment(t)).ToList());

	}


}