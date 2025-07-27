using TaskBaseApp.ViewModels;

namespace TaskBaseApp.Views;

public partial class AddTaskPage : ContentPage
{
	public AddTaskPage(AddTaskPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}