using TaskBaseApp.ViewModels;

namespace TaskBaseApp.Views;

public partial class TaskDetailsPage : ContentPage
{
	public TaskDetailsPage(TaskDetailsPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}