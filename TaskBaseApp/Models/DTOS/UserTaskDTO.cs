//using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskBaseApp.Models;
using SQLite;
using SQLiteNetExtensions.Attributes;


namespace TaskBaseApp.Models.DTOS;

/// <summary>
/// מייצג משימה של משתמש.
/// </summary>
[Table("UserTasks")]
public class UserTaskDTO
{
	/// <summary>
	/// מזהה ייחודי של המשימה.
	/// </summary>
	[PrimaryKey , AutoIncrement]
	public int TaskId
	{
		get; set;
	}

	[ForeignKey(typeof(User))]
	public int UserId
	{
		get; set;
	}
	/// <summary>
	/// המשתמש המשויך למשימה.
	/// </summary>
	[OneToOne (CascadeOperations=CascadeOperation.CascadeRead) ]
	public User? User
	{
		get; set;
	}

	[ForeignKey(typeof(UrgencyLevel))]
	public int UrgencyLevelId
	{
		get;
		set;
	}
	/// <summary>
	/// רמת הדחיפות של המשימה.
	/// </summary>
	/// 
	[OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
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
	/// 

	[OneToMany(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeDelete)]
	public List<TaskCommentDTO> TaskComments { get; set; } = new List<TaskCommentDTO>();

	/// <summary>
	/// כתובת URL של תמונה המשויכת למשימה.
	/// </summary>
	public string TaskImage { get; set; } = null!;

	/// <summary>
	/// בנאי ריק.
	/// </summary>
	public UserTaskDTO()
	{
	}

	public UserTaskDTO(UserTask t)
	{
		 TaskId=t.TaskId;
		 User = t.User;
		UserId = t.User.UserId;
		TaskDescription = t.TaskDescription;
		TaskDueDate = t.TaskDueDate;
		TaskImage = t.TaskImage;
		TaskActualDate = t.TaskActualDate;
		UrgencyLevel=t.UrgencyLevel;
		UrgencyLevelId = t.UrgencyLevel.UrgencyLevelId;
		//טעינת משימות
		TaskComments = new List<TaskCommentDTO>(t.TaskComments.Select(t => new TaskCommentDTO(t)).ToList());


	}

	
}