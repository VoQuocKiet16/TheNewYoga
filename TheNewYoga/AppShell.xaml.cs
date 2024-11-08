using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace TheNewYoga
{
    public partial class AppShell : Shell
    {
        public ICommand LogoutCommand { get; }

        public string WelcomeMessage { get; set; }

        public AppShell()
        {
            InitializeComponent();

            // Register Routes for Navigation
            Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
            Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            // Initialize the Logout Command
            LogoutCommand = new Command(OnLogout);

            if (App.CurrentUser != null)
            {
                WelcomeMessage = $"Welcome, {App.CurrentUser.email}!";
            }
            else
            {
                WelcomeMessage = "Welcome!";
            }

            BindingContext = this;
        }

        private async void OnLogout()
        {
            Preferences.Clear();

            await DisplayAlert("Logout", "You have been logged out successfully.", "OK");

            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
