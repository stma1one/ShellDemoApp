using TaskBaseApp.ViewModels;

namespace TaskBaseApp.Views;

public partial class AddAccountPage : ContentPage
{
	public AddAccountPage(AddAccountPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}