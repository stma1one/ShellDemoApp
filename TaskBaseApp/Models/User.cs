using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBaseApp.Models;

/// <summary>
/// מייצג את מודל הנתונים של משתמש באפליקציה.
/// </summary>
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
	public string? Password
	{
		get; set;
	}

	public string imageUrl
	{
		get; set;
	} 



 	/// <summary>
	/// האם המשתמש הוא מנהל מערכת.
	/// </summary>
		public bool? IsAdmin { get; set;  } 

	/// <summary>
	/// שם פרטי של המשתמש. 
	/// </summary>
	public string FirstName
	{
		get; set;
	} 

	/// <summary>
	///  שם משפחה של המשתמש.
	/// </summary>
	public string LastName
	{
		get; set;
	} 

	/// <summary>
	/// שם מלא של המשתמש, המורכב משם פרטי ושם משפחה.
	/// </summary>		
	public string FullName
	{
		get
		{
			return $"{FirstName} {LastName}";
		}
	}
}