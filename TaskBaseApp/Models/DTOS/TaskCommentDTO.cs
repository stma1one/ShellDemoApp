using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForeignKeyAttribute = SQLiteNetExtensions.Attributes.ForeignKeyAttribute;

namespace TaskBaseApp.Models.DTOS
{
	public class TaskCommentDTO
	{
		[PrimaryKey, AutoIncrement]
		public int CommentId
		{
			get; set;
		}

		/// <summary>
		/// המשימה המשויכת לתגובה.
		/// </summary>
		
		[ForeignKey(typeof(UserTaskDTO))]
		public int TaskId
		{
			get; set;
		}

		[ManyToOne(CascadeOperations =CascadeOperation.CascadeRead)]
		public UserTaskDTO? Task { get; set;
		}

		/// <summary>
		/// תוכן התגובה.
		/// </summary>
		public string Comment { get; set; } 

		/// <summary>
		/// תאריך יצירת התגובה.
		/// </summary>
		public DateTime CommentDate
		{
			get; set;
		}
	}
}
