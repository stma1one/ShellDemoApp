using Microsoft.Extensions.Logging;
using TaskBaseApp.Service;
using TaskBaseApp.Views;
using TaskBaseApp.ViewModels;
using TaskBaseApp.Services;


namespace TaskBaseApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
					#region הוספת פונטים חדשים
					fonts.AddFont("MaterialSymbolsOutlined.ttf", "MaterialSymbols");
					#endregion
				});

			#region הזרקת דפים
			builder.AddPages().AddViewModels().AddServices();
			#endregion

#if DEBUG
			builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
	#region load Pages
	public static MauiAppBuilder AddPages(this MauiAppBuilder builder)
        {
			builder.Services.AddTransient<Views.LoginPage>();
            builder.Services.AddTransient<Views.AddTaskPage>();
            builder.Services.AddTransient<Views.UserTasksPage>();
            builder.Services.AddTransient<Views.TaskDetailsPage>();
            builder.Services.AddTransient<JokePage>();
            builder.Services.AddSingleton<AddAccountPage>();
            builder.Services.AddSingleton<AppShell>();


			return builder;
		}
		#endregion
		#region load ViewModels
        public static MauiAppBuilder AddViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<AddTaskPageViewModel>();
            builder.Services.AddTransient<UserTasksPageViewModel>();
			builder.Services.AddTransient<TaskDetailsPageViewModel>();
            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddSingleton<JokePageViewModel>();
            builder.Services.AddSingleton<AddAccountPageViewModel>();
          
            return builder;
		}
		#endregion

		#region load Services
        public static MauiAppBuilder AddServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<ITaskServices, DBMokup>();
            builder.Services.AddSingleton<LocalDBService>();
            builder.Services.AddSingleton<JokeServiceApi>();
            builder.Services.AddSingleton<UserAndFileServices>();

       
            return builder;
		}
		#endregion
	}
}
