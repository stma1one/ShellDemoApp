using TaskBaseApp.Views;

namespace TaskBaseApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //רישום של דפים פנימיים
            Routing.RegisterRoute("TaskDetailsPage",typeof(TaskDetailsPage));
            Routing.RegisterRoute("UserProfilePage", typeof(UserProfilePage));
		}
    }
}
