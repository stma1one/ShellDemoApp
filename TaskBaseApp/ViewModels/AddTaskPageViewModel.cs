using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaskBaseApp.ViewModels
{
	public class AddTaskPageViewModel:ViewModelBase
	{
		public ICommand GotoProfileCommand
		{
			get;
		}
		public AddTaskPageViewModel()
		{
			GotoProfileCommand = new Command(async () => await Shell.Current.GoToAsync("/DetailsPage"));
			// Initialize properties or commands here if needed
		}
		// Add properties, commands, and methods for the AddTaskPage functionality
	}
	
}
