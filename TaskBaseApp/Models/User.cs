using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TaskBaseApp.Models;

/// <summary>
/// מייצג את מודל הנתונים של משתמש באפליקציה.
/// </summary>

//[SQLite.Table("UserTbl")]
public class User
{
	/// <summary>
	/// זיהוי משתמש
	/// </summary>
	[PrimaryKey]
	public int UserId
	{
		get; set;
	}
	/// <summary>
	/// שם המשתמש לצורך הזדהות.
	/// </summary>
	public string? Username
	{
		get; set;
	}

	/// <summary>
	/// סיסמת המשתמש.
	/// </summary>
	[Ignore]
	public string? Password
	{
		get; set;
	}

	public string imageUrl
	{
		get; set;
	} = "https://www.gravatar.com/avatar/";



 	/// <summary>
	/// האם המשתמש הוא מנהל מערכת.
	/// </summary>
		public bool? IsAdmin { get; set;  } = false;

	/// <summary>
	/// שם פרטי של המשתמש. 
	/// </summary>
	public string FirstName
	{
		get; set;
	} = string.Empty;

	/// <summary>
	///  שם משפחה של המשתמש.
	/// </summary>
	public string LastName
	{
		get; set;
	} = string.Empty;

	/// <summary>
	/// שם מלא של המשתמש, המורכב משם פרטי ושם משפחה.
	/// </summary>		
	/// 
	[Ignore]
	public string FullName
	{
		get
		{
			return $"{FirstName} {LastName}";
		}
	}
}