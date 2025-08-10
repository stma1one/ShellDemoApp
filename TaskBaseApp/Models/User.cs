using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
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
	
	[JsonPropertyName("id")]
	public int UserId
	{
		get; set;
	}
	/// <summary>
	/// שם המשתמש לצורך הזדהות.
	/// </summary>
	
	[JsonIgnore]
	public string? Username
	{
		get; set;
	}

	/// <summary>
	/// סיסמת המשתמש.
	/// </summary>
	[Ignore]
	[JsonPropertyName("password")]
	public string? Password
	{
		get; set;
	}

	[JsonPropertyName("avatar")]
	public string imageUrl
	{
		get; set;
	} = "https://www.gravatar.com/avatar/";



 	/// <summary>
	/// האם המשתמש הוא מנהל מערכת.
	/// </summary>
	/// 
	[JsonIgnore]
		public bool? IsAdmin { get; set;  } = false;

	/// <summary>
	/// שם פרטי של המשתמש. 
	/// </summary>
	[JsonIgnore]
	public string FirstName
	{
		get; set;
	} = string.Empty;

	/// <summary>
	///  שם משפחה של המשתמש.
	/// </summary>
	[JsonIgnore]
	public string LastName
	{
		get; set;
	} = string.Empty;

	/// <summary>
	/// שם מלא של המשתמש, המורכב משם פרטי ושם משפחה.
	/// </summary>		
	/// 
	[Ignore]

	[JsonPropertyName("name")]
	public string FullName
	{
		get
		{
			return $"{FirstName} {LastName}";
		}
	}

	[JsonPropertyName("email")]
	public string Email
	{
		get;
		set;
	} = "demo@demo.com";
}