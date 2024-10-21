using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace TheNewYoga
{
    public partial class AppShell : Shell
    {
        public ICommand LogoutCommand { get; }

        public AppShell()
        {
            InitializeComponent();

            // Register Routes for Navigation
            Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
            Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            // Initialize the Logout Command
            LogoutCommand = new Command(OnLogout);

            // Đảm bảo BindingContext được gán
            BindingContext = this;
        }

        private async void OnLogout()
        {
            // Clear session data (assuming you use Preferences for session management)
            Preferences.Clear();

            // Show a confirmation message
            await DisplayAlert("Logout", "You have been logged out successfully.", "OK");

            // Navigate to the LoginPage
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
