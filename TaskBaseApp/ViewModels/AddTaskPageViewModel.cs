using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskBaseApp.Models;

namespace TaskBaseApp.ViewModels
{
	/// <summary>
	/// ViewModel עבור דף הוספת משימה.
	/// מנהל את הלוגיקה, הוולידציה והמצב של הדף.
	/// </summary>
	public class AddTaskPageViewModel : ViewModelBase
	{
		// שדה פרטי לתיאור המשימה
		private string? _taskDescription;
		// שדה פרטי לתאריך יעד
		private DateTime? _taskDueDate;
		// הודעת שגיאה עבור תיאור המשימה
		private string _taskDescriptionError = string.Empty;
		// הודעת שגיאה עבור תאריך יעד
		private string _taskDueDateError = string.Empty;
		// האם להציג שגיאה עבור תיאור המשימה
		private bool _isTaskDescriptionErrorVisible;
		// האם להציג שגיאה עבור תאריך יעד
		private bool _isTaskDueDateErrorVisible;

		/// רמת הדחיפות שנבחרה על ידי המשתמש.
		private UrgencyLevel? selectedUrgency;

		/// <summary>
		/// תיאור המשימה שהמשתמש מזין.
		/// </summary>
		public string? TaskDescription
		{
			get => _taskDescription;
			set
			{
				if (_taskDescription != value)
				{
					_taskDescription = value;
					ValidateTaskDescription(); // ולידציה לשדה
					(SaveTaskCommand as Command)?.ChangeCanExecute(); // עדכון מצב הכפתור
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// תאריך יעד שהמשתמש בוחר.
		/// </summary>
		public DateTime? TaskDueDate
		{
			get => _taskDueDate;
			set
			{
				if (_taskDueDate != value)
				{
					_taskDueDate = value;
					ValidateTaskDueDate(); // ולידציה לשדה
					(SaveTaskCommand as Command)?.ChangeCanExecute(); // עדכון מצב הכפתור
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// הודעת שגיאה עבור תיאור המשימה.
		/// </summary>
		public string TaskDescriptionError
		{
			get => _taskDescriptionError;
			set
			{
				if (_taskDescriptionError != value)
				{
					_taskDescriptionError = value;
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// האם להציג את הודעת השגיאה עבור תיאור המשימה.
		/// </summary>
		public bool IsTaskDescriptionErrorVisible
		{
			get => _isTaskDescriptionErrorVisible;
			set
			{
				if (_isTaskDescriptionErrorVisible != value)
				{
					_isTaskDescriptionErrorVisible = value;
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// הודעת שגיאה עבור תאריך יעד.
		/// </summary>
		public string TaskDueDateError
		{
			get => _taskDueDateError;
			set
			{
				if (_taskDueDateError != value)
				{
					_taskDueDateError = value;
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// האם להציג את הודעת השגיאה עבור תאריך יעד.
		/// </summary>
		public bool IsTaskDueDateErrorVisible
		{
			get => _isTaskDueDateErrorVisible;
			set
			{
				if (_isTaskDueDateErrorVisible != value)
				{
					_isTaskDueDateErrorVisible = value;
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// רמת הדחיפות שנבחרה על ידי המשתמש.
		/// </summary>
		public UrgencyLevel? SelectedUrgency
		{
			get => selectedUrgency;
			set
			{
				if (selectedUrgency != value)
				{
					selectedUrgency = value;
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// אוסף רמות הדחיפות הזמינות לבחירה.
		/// </summary>
		public ObservableCollection<UrgencyLevel> UrgencyLevels {
			get; init;
		} 

		/// <summary>
		/// פקודה לשמירת המשימה.
		/// הכפתור יהיה פעיל רק כאשר כל השדות תקינים.
		/// </summary>
		public ICommand SaveTaskCommand
		{
			get;
		}

		/// <summary>
		/// פקודה למעבר לדף פרופיל המשתמש.
		/// </summary>
		public ICommand GotoProfileCommand
		{
			get;
		}

		/// <summary>
		/// בנאי של ה-ViewModel.
		/// </summary>
		public AddTaskPageViewModel()
		{
			SaveTaskCommand = new Command(async () => await SaveTask(), CanSaveTask);
			GotoProfileCommand = new Command(async () => await Shell.Current.GoToAsync("/DetailsPage"));
			TaskDueDate = DateTime.Today; // ערך ברירת מחדל
			UrgencyLevels = new ()
		{
			new UrgencyLevel { UrgencyLevelId = 1,  UrgencyLevelName = "נמוכה" },
			new UrgencyLevel { UrgencyLevelId = 2,  UrgencyLevelName = "בינונית" },
			new UrgencyLevel { UrgencyLevelId = 3, UrgencyLevelName = "גבוהה" }
		};
		}

		/// <summary>
		/// ולידציה לשדה תיאור המשימה.
		/// </summary>
		private void ValidateTaskDescription()
		{
			if (string.IsNullOrWhiteSpace(TaskDescription))
			{
				TaskDescriptionError = "שדה תיאור משימה חובה";
				IsTaskDescriptionErrorVisible = true;
			}
			else
			{
				TaskDescriptionError = string.Empty;
				IsTaskDescriptionErrorVisible = false;
			}
		}

		/// <summary>
		/// ולידציה לשדה תאריך יעד.
		/// </summary>
		private void ValidateTaskDueDate()
		{
			if (TaskDueDate == null)
			{
				TaskDueDateError = "יש לבחור תאריך יעד";
				IsTaskDueDateErrorVisible = true;
			}
			else
			{
				TaskDueDateError = string.Empty;
				IsTaskDueDateErrorVisible = false;
			}
		}

		/// <summary>
		/// בודק אם ניתן לשמור את המשימה (כל השדות תקינים).
		/// </summary>
		private bool CanSaveTask()
		{
			return !string.IsNullOrWhiteSpace(TaskDescription) && TaskDueDate != null;
		}

		/// <summary>
		/// פעולה לשמירת המשימה.
		/// </summary>
		private async Task SaveTask()
		{
			IsBusy = true;
			// שמור את המשימה כאן
			await Task.Delay(2000);
			IsBusy = false;
			
		}
	}
}