using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskBaseApp.ViewModels;

/// <summary>
/// מחלקת בסיס עבור כל ה-ViewModels באפליקציה.
/// מממשת את ממשק INotifyPropertyChanged כדי לאפשר עדכון אוטומטי של ה-UI.
/// </summary>
public class ViewModelBase : INotifyPropertyChanged
{
	private bool _isBusy;
	/// <summary>
	/// מציין אם מתבצעת פעולה ארוכה ברקע (למשל, קריאה לרשת).
	/// שימושי לקשירה (binding) למחוון טעינה (ActivityIndicator) ב-UI.
	/// </summary>
	public bool IsBusy
	{
		get => _isBusy;
		set
		{
			if (_isBusy != value)
			{
				_isBusy = value;
				OnPropertyChanged(); // מודיע ל-UI על השינוי
			}
		}
	}

	/// <summary>
	/// אירוע המופעל כאשר ערך של תכונה (Property) משתנה.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// פעולה להפעלת האירוע PropertyChanged.
	/// </summary>
	/// <param name="propertyName">שם התכונה שהשתנתה. יזוהה אוטומטית בזכות [CallerMemberName].</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}