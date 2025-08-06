using TaskBaseApp.ViewModels;

namespace TaskBaseApp.Views;

public partial class JokePage : ContentPage
{
	public JokePage(JokePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}