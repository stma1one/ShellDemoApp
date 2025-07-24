using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskBaseApp.Views;

namespace TaskBaseApp.ViewModels;

public class AppShellViewModel : ViewModelBase
{
	IServiceProvider provider;

	public bool IsAdmin
	{
		get => (Application.Current as App)?.CurrentUser?.IsAdmin ?? false;
	}


	public ICommand LogoutCommand
	{
		get;
	}

	public AppShellViewModel(IServiceProvider provider)
	{
		this.provider = provider;

		LogoutCommand = new Command(Logout);

	}

	private void Logout()
	{
		(Application.Current as App)!.CurrentUser = null; // איפוס המשתמש הנוכחי לאפס
		OnPropertyChanged(nameof(IsAdmin));
		Page loginPage = provider.GetService<LoginPage>()!;
		Application.Current.Windows[0].Page = loginPage; // החלפת הדף הנוכחי לדף ההתחברות
	}
}
