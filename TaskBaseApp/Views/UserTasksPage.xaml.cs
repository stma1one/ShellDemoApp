using TaskBaseApp.ViewModels;

namespace TaskBaseApp.Views;

public partial class UserTasksPage : ContentPage
{
	public UserTasksPage(UserTasksPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}