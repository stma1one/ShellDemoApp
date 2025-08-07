using TaskBaseApp.Models;
using TaskBaseApp.Service;
using TaskBaseApp.Views;

namespace TaskBaseApp
{
    public partial class App : Application
    {
		public User? CurrentUser { get; set; }
		Page? firstpage;
		LocalDBService db;
		Task loadSampleData;
		public App(IServiceProvider provider)
		{
			InitializeComponent();
			// Initialize the first page of the application
			this.firstpage =provider.GetService<LoginPage>();
			db= provider.GetService<LocalDBService>()!;
			loadSampleData = LoadSampleDataAsync();

		}

		private async Task? LoadSampleDataAsync()
		{
			try
			{
				await db.LoadSampleDataAsync().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				// Handle exceptions related to loading sample data
				Console.WriteLine($"Error loading sample data: {ex.Message}");
			}
			finally
			{
				// Ensure that the task is completed
				loadSampleData = Task.CompletedTask;
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