
﻿using Microsoft.Extensions.Options;
using System;

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using TaskBaseApp.Models;
using TaskBaseApp.Service;

namespace TaskBaseApp.ViewModels
{
	/// <summary>
	/// ViewModel עבור דף הוספת משימה.
	/// מנהל את הלוגיקה, הוולידציה והמצב של הדף.
	/// </summary>
	public class AddTaskPageViewModel : ViewModelBase
	{
		// שדה פרטי לתיאור המשימה
		private string? _taskDescription=string.Empty;
		// שדה פרטי לתאריך יעד
		private DateTime? _taskDueDate;
		// הודעת שגיאה עבור תיאור המשימה
		private string _taskDescriptionError = string.Empty;
		// הודעת שגיאה עבור תאריך יעד
		private string _taskDueDateError = string.Empty;
		
		/// רמת הדחיפות שנבחרה על ידי המשתמש.
		private UrgencyLevel? selectedUrgency;

		// הודעת שגיאה עבור רמת דחיפות
		private string _taskUrgencyError=string.Empty;

		private LocalDBService _db;

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
					OnPropertyChanged();
					(SaveTaskCommand as Command)?.ChangeCanExecute(); // עדכון מצב הכפתור
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

					OnPropertyChanged();
					(SaveTaskCommand as Command)?.ChangeCanExecute(); // עדכון מצב הכפתור
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
			get =>  ValidateTaskDescription();
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
			get => ValidateTaskDueDate();
		}
		/// <summary>
		/// האם להציג את הודעת השגיאה עבור רמת דחיפות.
		/// </summary>
		public bool IsTaskUrgencyErrorVisible
		{
			get=>ValidateTaskUrgencyLevel();
			
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
					(SaveTaskCommand as Command)?.ChangeCanExecute(); // עדכון מצב הכפת
				}
			}
		}

		/// <summary>
		/// אוסף רמות הדחיפות הזמינות לבחירה.
		/// </summary>
		public ObservableCollection<UrgencyLevel> UrgencyLevels
		{
			get; set;
		}
		/// <summary>
		/// הודעת שגיאה עבור רמת דחיפות.
		/// </summary>
		public string TaskUrgencyError
		{
			get=> _taskUrgencyError;
			 set{
				if(_taskUrgencyError != value)
				{
					_taskUrgencyError = value;
					OnPropertyChanged();
				}
			}
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
		public AddTaskPageViewModel(LocalDBService db)
		{
			_db = db;
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
		private bool ValidateTaskDescription()
		{
			if (string.IsNullOrWhiteSpace(TaskDescription))
			{
				TaskDescriptionError = "שדה תיאור משימה חובה";
				
				return false;
			}
			
				TaskDescriptionError = string.Empty;
				
			return true;
		}

		/// <summary>
		/// ולידציה לשדה תאריך יעד.
		/// </summary>
		private bool ValidateTaskDueDate()
		{
			if (TaskDueDate == null)
			{
				TaskDueDateError = "יש לבחור תאריך יעד";
				
				return false;
			}
			
				TaskDueDateError = string.Empty;
				
				return true;
			
		}

		private bool ValidateTaskUrgencyLevel()
		{
			if (SelectedUrgency == null)
			{
				TaskUrgencyError = "יש לבחור רמת דחיפות";
				
				return false;
			}
			
				TaskUrgencyError = string.Empty;
				
				return true;

		}

		/// <summary>
		/// בודק אם ניתן לשמור את המשימה (כל השדות תקינים).
		/// </summary>
		private bool CanSaveTask()
		{
			OnPropertyChanged(nameof(IsTaskDescriptionErrorVisible));
			OnPropertyChanged(nameof(IsTaskDueDateErrorVisible));
			OnPropertyChanged(nameof(IsTaskUrgencyErrorVisible));
			return  ValidateTaskDescription()&& ValidateTaskDueDate() && ValidateTaskUrgencyLevel();
		}

		/// <summary>
		/// פעולה לשמירת המשימה.
		/// </summary>
		private async Task SaveTask()
		{
			IsBusy = true;
			// שמור את המשימה כאן
			await Task.Delay(5000);
			UserTask task = new UserTask()
			{
				TaskDueDate = DateOnly.FromDateTime((DateTime)this.TaskDueDate),
				TaskDescription = this.TaskDescription,
				UrgencyLevel = this.SelectedUrgency,
				User = ((App)Application.Current).CurrentUser
				 
			};
			try
			{
				
				await _db.InsertTaskAsync(task);
				await Shell.Current.DisplayAlert("הצלחה", "משימה נשמרה בהצלחה", "אישור");
				IsBusy = false;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("unable to save task to db" + ex.Message);
			}
			finally
			{
				IsBusy = false;
			}


		}
	}
}