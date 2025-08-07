using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskBaseApp.Models;
using TaskBaseApp.Models.DTOS;

namespace TaskBaseApp.Models
{
	/// <summary>
	/// מייצג תגובה למשימה.
	/// </summary>
	public class TaskComment
	{
		/// <summary>
		/// מזהה ייחודי של התגובה.
		/// </summary>
		public int CommentId
		{
			get; set;
		}

		/// <summary>
		/// המשימה המשויכת לתגובה.
		/// </summary>
		public UserTask? Task
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
		public DateOnly CommentDate
		{
			get; set;
		}

		/// <summary>
		/// בנאי ריק.
		/// </summary>
		public TaskComment()
		{
		}

		/// <summary>
		/// בנאי הממיר אובייקט TaskCommentDTO ל-TaskComment.
		/// </summary>
		/// <param name="t">אובייקט TaskCommentDTO</param>
		public TaskComment(TaskCommentDTO t)
		{
			CommentId = t.CommentId;
			Comment = t.Comment;
			CommentDate = DateOnly.FromDateTime(t.CommentDate);
			
		}
	}
}