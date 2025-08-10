using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Microsoft.Maui.Graphics;
using TaskBaseApp.Models;
using TaskBaseApp.Services;

namespace TaskBaseApp.ViewModels;

/// <summary>
/// ViewModel לעמוד הוספת משתמש חדש.
/// </summary>
public class AddAccountPageViewModel : ViewModelBase
{
	#region שדות
	private string? username;
	private string? password;
	private string? firstName;
	private string? lastName;
	private string? email;
	private string? imageUrl;
	private bool isAdmin;
	private string? message;
	private Color messageColor = Colors.Black;
	private bool isMessageVisible;
	private readonly UserAndFileServices userService;
	#endregion

	#region תכונות
	public string? Username
	{
		get => username;
		set
		{
			username = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanCreateAccount));
		}
	}
	public string? Password
	{
		get => password;
		set
		{
			password = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanCreateAccount));
		}
	}
	public string? FirstName
	{
		get => firstName;
		set
		{
			firstName = value; OnPropertyChanged();
		}
	}
	public string? LastName
	{
		get => lastName;
		set
		{
			lastName = value; OnPropertyChanged();
		}
	}
	public string? Email
	{
		get => email;
		set
		{
			email = value; OnPropertyChanged();
		}
	}
	public string? ImageUrl
	{
		get => imageUrl;
		set
		{
			imageUrl = value; OnPropertyChanged();
		}
	}
	public bool IsAdmin
	{
		get => isAdmin;
		set
		{
			isAdmin = value; OnPropertyChanged();
		}
	}
	public string? Message
	{
		get => message;
		set
		{
			message = value; OnPropertyChanged();
		}
	}
	public Color MessageColor
	{
		get => messageColor;
		set
		{
			messageColor = value; OnPropertyChanged();
		}
	}
	public bool IsMessageVisible
	{
		get => isMessageVisible;
		set
		{
			isMessageVisible = value; OnPropertyChanged();
		}
	}
	public bool CanCreateAccount =>
		!string.IsNullOrWhiteSpace(Username) &&
		!string.IsNullOrWhiteSpace(Password);

	public ICommand CreateAccountCommand
	{
		get;
	}
	#endregion

	#region בנאי
	public AddAccountPageViewModel(UserAndFileServices userService)
	{
		this.userService = userService;
		CreateAccountCommand = new Command(async () => await CreateAccountAsync(), () => CanCreateAccount);
	}
	#endregion

	#region מתודות
	private async Task CreateAccountAsync()
	{
		IsMessageVisible = false;
		if (!CanCreateAccount)
		{
			Message = "יש למלא שם משתמש וסיסמה";
			MessageColor = Colors.Red;
			IsMessageVisible = true;
			return;
		}

		var user = new User
		{
			Username = Username,
			Password = Password,
			FirstName = FirstName ?? "",
			LastName = LastName ?? "",
			imageUrl = string.IsNullOrWhiteSpace(ImageUrl) ? "https://www.gravatar.com/avatar/" : ImageUrl,
			IsAdmin = IsAdmin
		};

		try
		{
			int id = await userService.CreateUserAsync(user);
			if (id > 0)
			{

				Message = $"המשתמש {id} נוצר בהצלחה!";
				MessageColor = Colors.Green;
			}
			else
			{
				Message = "יצירת המשתמש נכשלה.";
				MessageColor = Colors.Red;
			}
		}
		catch (Exception ex)
		{
			Message = $"שגיאה: {ex.Message}";
			MessageColor = Colors.Red;
		}
		IsMessageVisible = true;
	}
	#endregion
}