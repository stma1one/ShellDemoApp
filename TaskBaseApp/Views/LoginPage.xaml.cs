
using TaskBaseApp.Helper;
using TaskBaseApp.Models;
using TaskBaseApp.Service;
using TaskBaseApp.ViewModels;

namespace TaskBaseApp.Views;

public partial class LoginPage : ContentPage
{
	

	public LoginPage(LoginPageViewModel vm)
	{
		
		InitializeComponent();
		
		
		BindingContext = vm ;
	}

	

	

	
}