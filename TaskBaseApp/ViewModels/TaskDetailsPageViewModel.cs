// --- START OF FILE TaskDetailsPageViewModel.cs ---

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TaskBaseApp.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace TaskBaseApp.ViewModels
{
	/// <summary>
	/// ViewModel עבור דף פרטי המשימה (במצב קריאה בלבד).
	/// מקבל אובייקט UserTask שלם דרך מנגנון הניווט של MAUI.
	/// </summary>

	[QueryProperty(nameof(CommentsTitle),"desc")]
	[QueryProperty(nameof(Id),"id")]
	//[QueryProperty(nameof(SelectedTask), "selectedTask")]
	public partial class TaskDetailsPageViewModel :ViewModelBase ,IQueryAttributable

	{
		#region שדות
		private UserTask? _selectedTask;
		private bool _hasComments;
		private string _commentsTitle = "אין תגובות";
		#endregion

		#region מאפיינים ל-Data Binding

		/// <summary>
		/// המשימה הנוכחית שמוצגת ב-UI.
		/// המאפיין שה-View מאזין לו ישירות.
		/// </summary>
		public UserTask? CurrentTask
		{
			get => _selectedTask;
			private set
			{
				_selectedTask = value;
				OnPropertyChanged(); // נודיע ל-UI שהמשימה התחלפה
			}
		}
		public int Id
		{
			get; set;
		}
		/// <summary>
		/// המאפיין שמקבל את אובייקט המשימה המלא מהניווט.
		/// כאשר הוא מתעדכן, אנו מאתחלים את כל הדף.
		/// </summary>
		public UserTask? SelectedTask
		{
			get => _selectedTask;
			set
			{
				// האובייקט שהתקבל מהניווט נשמר ומעדכן את המאפיין שה-UI מאזין לו
				CurrentTask = value;

				// לאחר קבלת המשימה, נעדכן את מאפייני העזר
				UpdateCommentStatus();
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// מאפיין בוליאני שמציין אם למשימה יש תגובות.
		/// שימושי כדי להציג או להסתיר את אזור התגובות ב-XAML.
		/// </summary>
		public bool HasComments
		{
			get => _hasComments;
			set
			{
				_hasComments = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// כותרת דינמית לאזור התגובות (למשל "תגובות (3)").
		/// </summary>
		public string CommentsTitle
		{
			get => _commentsTitle;
			set
			{
				_commentsTitle = value;
				OnPropertyChanged();
			}
		}
		#endregion

		#region פקודות
		/// <summary>
		///		
		///</summary>
		public ICommand UpdateImageCommand
		{
			get;
		}
		#endregion

		#region קונסטרוקטור (בנאי)
		/// <summary>
		/// בנאי. במצב קריאה בלבד, אין צורך להזריק שירותים או לאתחל פקודות.
		/// </summary>
		public TaskDetailsPageViewModel()
		{
			UpdateImageCommand=new Command(async ()=>await Shell.Current.DisplayAlert("עדכון תמונה", "המשימה עודכנה בהצלחה!", "אישור"));
		}
		#endregion

		#region מתודות פרטיות
		/// <summary>
		/// מתודה פרטית שמעדכנת את המאפיינים הקשורים לתגובות
		/// על סמך המשימה הנוכחית.
		/// </summary>
		private void UpdateCommentStatus()
		{
			if (CurrentTask != null && CurrentTask.TaskComments.Any())
			{
				HasComments = true;
				CommentsTitle = $"תגובות ({CurrentTask.TaskComments.Count})";
			}
			else
			{
				HasComments = false;
				CommentsTitle = "אין תגובות להצגה";
			}
		}

		public void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			SelectedTask = (UserTask)query["selectedTask"];
		}
		#endregion
	}
}