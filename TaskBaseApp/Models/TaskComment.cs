using SQLite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskBaseApp.Models;

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
	}
}