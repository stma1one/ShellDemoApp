using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBaseApp.Helper;

/// <summary>
/// מחלקה סטטית המרכזת הודעות טקסט קבועות לשימוש בממשק המשתמש.
/// </summary>
public static class AppMessages
{
	/// <summary>
	/// הודעה שתוצג לאחר התחברות מוצלחת.
	/// </summary>
	public const string LoginMessage = "התחברת";

	/// <summary>
	/// הודעת שגיאה שתוצג במקרה של שם משתמש או סיסמה לא נכונים.
	/// </summary>
	public const string LoginErrorMessage = "שם משתמש וסיסמה לא תקינים";
}