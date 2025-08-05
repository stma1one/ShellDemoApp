using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBaseApp.Models
{
	/// <summary>
	/// מייצג רמת דחיפות של משימה.
	/// </summary>
	public class UrgencyLevel
	{
		/// <summary>
		/// מזהה ייחודי של רמת הדחיפות.
		/// </summary>
	[PrimaryKey]
	
		public int UrgencyLevelId
		{
			get; set;
		}

		/// <summary>
		/// שם רמת הדחיפות.
		/// </summary>
		public string UrgencyLevelName { get; set; } = null!;

		/// <summary>
		/// מחזיר את שם רמת הדחיפות כמחרוזת.
		/// </summary>
		/// <returns>שם רמת הדחיפות.</returns>
		public override string ToString()
		{
			return UrgencyLevelName;
		}
	}
}