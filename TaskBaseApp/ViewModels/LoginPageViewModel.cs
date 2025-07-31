using TaskBaseApp.Helper;
using TaskBaseApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskBaseApp.Views;
using System.Text.Json;

namespace TaskBaseApp.ViewModels;

/// <summary>
/// ה-ViewModel עבור דף ההתחברות. מנהל את הלוגיקה והמצב של הדף.
/// </summary>
public class LoginPageViewModel : ViewModelBase
{
	// שדה פרטי לשמירת שירות ההתחברות שהוזרק
	ITaskServices db;
	IServiceProvider provider;

	private string? _userName;
	private string? _password;

	/// <summary>
	/// שם המשתמש המוזן על ידי המשתמש ב-UI.
	/// </summary>
	public string? UserName
	{
		get => _userName;
		set
		{
			if (_userName != value)
			{
				_userName = value;
				OnPropertyChanged(); // מודיע ל-UI על שינוי כדי לעדכן את התצוגה
				(LoginCommand as Command)?.ChangeCanExecute(); // בודק מחדש אם ניתן להפעיל את כפתור ההתחברות
			}
		}
	}

	/// <summary>
	/// הסיסמה המוזנת על ידי המשתמש ב-UI.
	/// </summary>
	public string? Password
	{
		get => _password;
		set
		{
			if (_password != value)
			{
				_password = value;
				OnPropertyChanged(); // מודיע ל-UI על שינוי
				(LoginCommand as Command)?.ChangeCanExecute(); // בודק מחדש אם ניתן להפעיל את כפתור ההתחברות
			}
		}
	}

	private bool messageIsVisible;
	/// <summary>
	/// קובע אם הודעת המשוב (הצלחה/שגיאה) תוצג למשתמש.
	/// </summary>
	public bool MessageIsVisible
	{
		get => messageIsVisible;
		set
		{
			if (messageIsVisible != value)
			{
				messageIsVisible = value;
				OnPropertyChanged();
			}
		}
	}

	private Color? messageColor;
	/// <summary>
	/// קובע את צבע הודעת המשוב (למשל, ירוק להצלחה ואדום לשגיאה).
	/// </summary>
	public Color? MessageColor
	{
		get => messageColor;
		set
		{
			if (messageColor != value)
			{
				messageColor = value;
				OnPropertyChanged();
			}
		}
	}

	private bool isPassword;
	/// <summary>
	/// קובע האם שדה הסיסמה יוצג כמוסתר (true) או גלוי (false).
	/// </summary>
	public bool IsPassword
	{
		get => isPassword;
		set
		{
			if (isPassword != value)
			{
				isPassword = value;
				OnPropertyChanged();
			}
		}
	}

	//private string? showPasswordIcon;
	///// <summary>
	///// האייקון שיוצג עבור כפתור הצג/הסתר סיסמה.
	///// </summary>
	//public string? ShowPasswordIcon
	//{
	//	get => showPasswordIcon;
	//	set
	//	{
	//		if (showPasswordIcon != value)
	//		{
	//			showPasswordIcon = value;
	//			OnPropertyChanged();
	//		}
	//	}
	//}

	private string? loginMessage;
	/// <summary>
	/// טקסט הודעת המשוב שתוצג למשתמש לאחר ניסיון התחברות.
	/// </summary>
	public string? LoginMessage
	{
		get => loginMessage;
		set
		{
			if (loginMessage != value)
			{
				loginMessage = value;
				OnPropertyChanged();
			}
		}
	}

	private Task loadDataTask;

	/// <summary>
	/// בנאי של ה-ViewModel.
	/// </summary>
	/// <param name="service">שירות ההתחברות שיוזרק באמצעות Dependency Injection.</param>
	public LoginPageViewModel(ITaskServices service, IServiceProvider provider)
	{
		this.provider= provider;
		db = service;
		// אתחול הפקודות והגדרת ערכים ראשוניים
		ShowPasswordCommand = new Command(TogglePasswordVisiblity);
		LoginCommand = new Command(async()=>await Login(), CanLogin);
		//ShowPasswordIcon = FontHelper.CLOSED_EYE_ICON; // הגדרת אייקון ברירת מחדל
		IsPassword = true; // הגדרת שדה הסיסמה כמוסתר כברירת מחדל
		loadDataTask = GetUserNameAsync();
	}

	private async Task? GetUserNameAsync()
	{
		UserName = await SecureStorage.Default.GetAsync("userName");
	}

	/// <summary>
	/// תנאי הקובע אם ניתן להפעיל את פקודת ההתחברות.
	/// </summary>
	/// <returns>אמת אם גם שם המשתמש וגם הסיסמה אינם ריקים.</returns>
	public bool CanLogin()
	{
		return (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password));
	}

	/// <summary>
	/// פקודה להצגה והסתרה של הסיסמה.
	/// </summary>
	public ICommand ShowPasswordCommand
	{
		get;
	}

	/// <summary>
	/// פקודה לביצוע תהליך ההתחברות.
	/// </summary>
	public ICommand LoginCommand
	{
		get;
	}

	/// <summary>
	/// מחליף את מצב התצוגה של הסיסמה (מוסתר/גלוי) ומעדכן את האייקון בהתאם.
	/// </summary>
	private void TogglePasswordVisiblity()
	{
		IsPassword = !IsPassword; // הופך את הערך הבוליאני
		//if (IsPassword)
		//	ShowPasswordIcon = FontHelper.CLOSED_EYE_ICON;
		//else
		//	ShowPasswordIcon = FontHelper.OPEN_EYE_ICON;
	}

	/// <summary>
	/// מבצע את לוגיקת ההתחברות.
	/// </summary>
	private async Task Login()
	{
		IsBusy = true; // מסמן שהאפליקציה בתהליך (להצגת מחוון טעינה)
		MessageIsVisible = true; // מציג את אזור הודעת המשוב

		await Task.Delay(5000);// סימול של עיכוב (למשל, קריאה לרשת)
								 // קורא לשירות ההתחברות עם הפרטים שהוזנו
		if (db.Login(UserName!, Password!))
		{
			// במקרה של הצלחה
			LoginMessage = AppMessages.LoginMessage;
			MessageColor = Colors.Green;
			((App)Application.Current!).CurrentUser = db.GetCurrentUser(UserName!); // מאחסן את המשתמש הנוכחי באפליקציה
			
			await SecureStorage.Default.SetAsync("userName", UserName);


			var shellVm = provider.GetService<AppShellViewModel>()!;                                                                    // כאן ניתן להוסיף ניווט לדף הבא
		//דוגמה ליצירת SHELL אחר
			if (UserName != "user2")
			Application.Current.Windows[0].Page = new AppShell(shellVm);
			else
				Application.Current.Windows[0].Page = new MyAppShell(shellVm);


		}
		else
		{
			await Application.Current!.Windows[0].Page!.DisplayAlert("חיבור כושל", AppMessages.LoginErrorMessage, "אישור");
			// במקרה של כישלון
			//LoginMessage = AppMessages.LoginErrorMessage;

			//MessageColor = Colors.Red;
		}
		IsBusy = false; // מסיים את מצב "עסוק"
	}
}