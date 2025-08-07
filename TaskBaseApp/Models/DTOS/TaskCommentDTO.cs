//using System.ComponentModel.DataAnnotations.Schema;
using SQLite;
using System.ComponentModel.DataAnnotations;
using TaskBaseApp.Models;
using TableAttribute = SQLite.TableAttribute;
using SQLiteNetExtensions.Attributes;


namespace TaskBaseApp.Models.DTOS;

/// <summary>
/// מייצג תגובה למשימה.
/// </summary>
[Table("TaskComments")]
public class TaskCommentDTO
{
	/// <summary>
	/// מזהה ייחודי של התגובה.
	/// </summary>
	
	[PrimaryKey, AutoIncrement]
	public int CommentId
	{
		get; set;
	}

	[ForeignKey(typeof(UserTaskDTO))]
	public int TaskId
	{
		get; set;
	}


	/// <summary>
	/// המשימה המשויכת לתגובה.
	/// </summary>
	[ManyToOne(CascadeOperations =CascadeOperation.CascadeRead)]
	public UserTaskDTO? Task
	{
		get; set;
	}

	/// <summary>
	/// תוכן התגובה.
	/// </summary>
	public string Comment { get; set; } = null!;

	/// <summary>
	/// תאריך יצירת התגובה.
	/// </summary>
	public DateTime CommentDate
	{
		get; set;
	}

	/// <summary>
	/// בנאי ריק.
	/// </summary>
	public TaskCommentDTO()
	{
	}
	public TaskCommentDTO(TaskComment comment)
	{
		CommentId = comment.CommentId;
		Comment = comment.Comment;
		CommentDate = comment.CommentDate.ToDateTime(TimeOnly.MinValue);
		if (comment.Task != null)
		{
			TaskId = comment.Task.TaskId;
		
		}
	}

}