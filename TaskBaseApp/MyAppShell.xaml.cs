using TaskBaseApp.ViewModels;
using TaskBaseApp.Views;

namespace TaskBaseApp
{
    public partial class MyAppShell : Shell
    {
        public MyAppShell(AppShellViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            Routing.RegisterRoute("TaskDetailsPage",typeof(TaskDetailsPage));
		}
    }
}
