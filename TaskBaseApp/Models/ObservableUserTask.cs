
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskBaseApp.Models
{
	public class ObservableUserTask : INotifyPropertyChanged
	{
		UserTask task;
		public event PropertyChangedEventHandler? PropertyChanged;


		protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		
		public string TaskDescription
		{
			get => task.TaskDescription;
			set
			{
				task.TaskDescription = value;
				OnPropertyChanged();
			}
		}
		
		
		public User? User
		{
			get => task.User;
			set
			{
				task.User = value;
				OnPropertyChanged();
			}
		}
		public int  TaskId
		{
			get
			{
				return task.TaskId;
			}
			set
			{

				task.TaskId = value;
				OnPropertyChanged();
			}
		}

		public UrgencyLevel? UrgencyLevel
		{
			get => task.UrgencyLevel;
			set
			{
				task.UrgencyLevel = value;
				OnPropertyChanged();
			}
		}
		public DateOnly TaskDueDate
		{
			get => task.TaskDueDate;
			set
			{
				task.TaskDueDate = value;
			}
		}
		public DateOnly? TaskActualDate
		{
			get=>task.TaskActualDate; set {
				task.TaskActualDate = value;OnPropertyChanged(); }
		}
		public ObservableCollection<TaskComment> TaskComments
		{
			get=>new ObservableCollection<TaskComment>(task.TaskComments); set {
				task.TaskComments = value.ToList();
				OnPropertyChanged();

			}
		}
		public string TaskImage { get=>task.TaskImage; set { task.TaskImage = value;OnPropertyChanged(); } } 
		public ObservableUserTask(UserTask t)
		{
			task = t;
		}

		public UserTask ToUserTask()
		{
			return task;
		}
	}
}
