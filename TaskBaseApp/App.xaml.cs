using TaskBaseApp.Models;
using TaskBaseApp.Service;
using TaskBaseApp.Views;

namespace TaskBaseApp
{
    public partial class App : Application
    {
		public User? CurrentUser { get; set; }
		Page? firstpage;
		LocalDBService _db;
		Task loadSampleData;
		public App(IServiceProvider provider)
		{
			InitializeComponent();
			// Initialize the first page of the application
			this.firstpage =provider.GetService<LoginPage>();
			_db = provider.GetService<LocalDBService>();
			loadSampleData = LoadSampleDataAsync();
		}

		private async Task? LoadSampleDataAsync()
		{
			try
			{
				await _db.LoadSampleDataAsync().ConfigureAwait(false);
			}
			catch (Exception ex)
			{

			}
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			//return new Window(new MyAppShell());
		// return new Window(new AppShell());
			return new Window(firstpage!);
		}
	}
}