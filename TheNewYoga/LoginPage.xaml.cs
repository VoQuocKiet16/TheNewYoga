using Firebase.Database;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace TheNewYoga
{
    public partial class LoginPage : ContentPage
    {

        private readonly FirebaseClient firebaseClient = new FirebaseClient("https://thenewyoga-604c0-default-rtdb.asia-southeast1.firebasedatabase.app/");

        public LoginPage()
        {
            InitializeComponent();
        }


        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            string email = eEmail.Text?.Trim();
            string password = ePassword.Text?.Trim();

            // Check if email or password is empty
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Error", "Email cannot be empty.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Password cannot be empty.", "OK");
                return;
            }

            // Validate email format
            if (!IsValidEmail(email))
            {
                await DisplayAlert("Error", "Invalid email format.", "OK");
                return;
            }

            // Hash the entered password
            string hashedPassword = HashPassword(password);

            try
            {
                // Fetch all users from the "yoga_users" table in Firebase
                var users = await firebaseClient
                    .Child("yoga_users")  // Fetch from the "yoga_users" table
                    .OnceAsync<YogaUser>();

                // Check if there is a user matching the email and hashed password
                var matchedUser = users.FirstOrDefault(u =>
                    u.Object.email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                    u.Object.password == hashedPassword);

                if (matchedUser != null)
                {
                    // Get user information from the result
                    var user = matchedUser.Object;
                    user.firebaseKey = matchedUser.Key;

                    // Save current user information in the application
                    App.CurrentUser = user;

                    // Save login information in Preferences for later use
                    Preferences.Set("UserEmail", user.email);

                    await DisplayAlert("Success", "Login successful!", "OK");

                    // Navigate to AppShell (Bottom Navigation Bar)
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await DisplayAlert("Error", "Invalid email or password. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error occurred during login. Please try again later.", "OK");
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private async void btnRegister_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
