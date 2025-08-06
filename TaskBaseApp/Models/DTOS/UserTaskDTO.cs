using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Attributes;

namespace TaskBaseApp.Models.DTOS
{
	public class UserTaskDTO
	{
		[PrimaryKey]
		[AutoIncrement]
		public int TaskId
		{
			get; set;
		}

		/// <summary>
		/// המשתמש המשויך למשימה.
		/// </summary>
		[SQLiteNetExtensions.Attributes.ForeignKey(typeof(User))]
		public int? UserId
		{
			get; set;
		}
		[OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
		public User? User { get; set;
		}
		/// <summary>
		/// רמת הדחיפות של המשימה.
		/// </summary>
		[SQLiteNetExtensions.Attributes.ForeignKey(typeof(UrgencyLevel))]
		public int? UrgencyLevelId
		{
			get; set;
		}

		[OneToOne(CascadeOperations=CascadeOperation.CascadeRead)]
		public UrgencyLevel? UrgencyLevel { get; set;
		}

		/// <summary>
		/// תיאור המשימה.
		/// </summary>
		public string TaskDescription { get; set; } 

		/// <summary>
		/// תאריך היעד לביצוע המשימה.
		/// </summary>
		public DateTime TaskDueDate
		{
			get; set;
		}

		/// <summary>
		/// תאריך הביצוע בפועל של המשימה. יכול להיות ריק אם המשימה טרם הושלמה.
		/// </summary>
		public DateTime TaskActualDate
		{
			get; set;
		}

	
		public string TaskImage { get; set; } 

		
		[OneToMany(CascadeOperations = CascadeOperation.CascadeRead|CascadeOperation.CascadeDelete)]
		public List<TaskCommentDTO> TaskComments { get; set; }

	}
}
