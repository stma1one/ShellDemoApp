using TaskBaseApp.Views;

namespace TaskBaseApp
{
    public partial class MyAppShell : Shell
    {
        public MyAppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("TaskDetailsPage",typeof(TaskDetailsPage));
		}
    }
}
