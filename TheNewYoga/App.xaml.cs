using Microsoft.Maui.Controls;

namespace TheNewYoga
{
    public partial class App : Application
    {
        public static YogaUser CurrentUser { get; set; }

        public App()
        {
            InitializeComponent();

            if (IsUserLoggedIn())
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        private bool IsUserLoggedIn()
        {
            if (Preferences.ContainsKey("UserEmail"))
            {
                var userEmail = Preferences.Get("UserEmail", string.Empty);

                if (!string.IsNullOrEmpty(userEmail))
                {
                    CurrentUser = new YogaUser
                    {
                        email = userEmail,
                    };
                    return true;
                }
            }

            return false; 
        }

        public static void LogoutUser()
        {

            Preferences.Clear();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
