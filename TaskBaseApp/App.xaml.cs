using TaskBaseApp.Models;
using TaskBaseApp.Views;

namespace TaskBaseApp
{
    public partial class App : Application
    {
		public User? CurrentUser { get; set; }
		Page? firstpage;
		public App(IServiceProvider provider)
		{
			InitializeComponent();
			// Initialize the first page of the application
			this.firstpage =provider.GetService<LoginPage>();
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			//return new Window(new MyAppShell());
		// return new Window(new AppShell());
			return new Window(firstpage!);
		}
	}
}