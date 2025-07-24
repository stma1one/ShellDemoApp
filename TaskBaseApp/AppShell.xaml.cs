using TaskBaseApp.ViewModels;
using TaskBaseApp.Views;

namespace TaskBaseApp
{
    public partial class AppShell : Shell
    {
		// AppShell הוא ה-Shell הראשי של האפליקציה, המנהל את הניווט בין הדפים השונים
       
		public AppShell(AppShellViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            //רישום של דפים פנימיים
            Routing.RegisterRoute("TaskDetailsPage",typeof(TaskDetailsPage));
            Routing.RegisterRoute("UserProfilePage", typeof(UserProfilePage));
            
		}
		
    }
}
