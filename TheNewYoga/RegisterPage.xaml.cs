using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace TheNewYoga
{
    public partial class RegisterPage : ContentPage
    {
        private readonly FirebaseClient firebaseClient = new FirebaseClient("https://thenewyoga-604c0-default-rtdb.asia-southeast1.firebasedatabase.app/");

        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void btnRegister_Clicked(object sender, EventArgs e)
        {
            string username = eUsername.Text?.Trim();
            string email = eEmail.Text?.Trim();
            string password = ePassword.Text?.Trim();
            string confirmPassword = eConfirmPassword.Text?.Trim();

            // Validate fields
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                await DisplayAlert("Error", "All fields are required.", "OK");
                return;
            }

            if (!IsValidEmail(email))
            {
                await DisplayAlert("Error", "Invalid email format.", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            // Check if email already exists in "yoga_users"
            var existingUsers = await firebaseClient
                .Child("yoga_users")
                .OnceAsync<YogaUser>();

            bool emailExists = existingUsers.Any(u => u.Object.email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (emailExists)
            {
                await DisplayAlert("Error", "Email already registered. Please use a different email.", "OK");
                return;
            }

            // Hash the password before saving
            string hashedPassword = HashPassword(password);

            // Create a new user object without the firebaseKey
            var user = new YogaUser
            {
                userId = Guid.NewGuid().ToString(),
                username = username,
                email = email,
                password = hashedPassword,
                role = "CUSTOMER",  // Default role
                roleAsString = "customer"
            };

            // Save user to Firebase and retrieve the Firebase ID
            var firebasePush = await firebaseClient
                .Child("yoga_users")
                .PostAsync(user);

            // Use the Firebase ID as firebaseKey and update the record
            string firebaseKey = firebasePush.Key;  // This is the Firebase-generated ID
            user.firebaseKey = firebaseKey;

            // Update the Firebase entry to include firebaseKey
            await firebaseClient
                .Child("yoga_users")
                .Child(firebaseKey)  // Use the Firebase ID to update the same entry
                .PutAsync(user);

            // Show success alert and navigate back to the login page
            await DisplayAlert("Success", "Registration successful!", "OK");
            await Navigation.PopAsync();  // Navigate back to the login page
        }


        private bool IsValidEmail(string email)
        {
            // Simple regex to validate email format
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private string HashPassword(string password)
        {
            // Use SHA256 to hash the password
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
