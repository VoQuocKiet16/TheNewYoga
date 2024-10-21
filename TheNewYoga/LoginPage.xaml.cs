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

         
            if (!IsValidEmail(email))
            {
                await DisplayAlert("Error", "Invalid email format.", "OK");
                return;
            }

            // Mã hóa mật khẩu nhập vào
            string hashedPassword = HashPassword(password);

            try
            {
                // Lấy tất cả người dùng từ bảng "yoga_users" trong Firebase
                var users = await firebaseClient
                    .Child("yoga_users")  // Fetch từ bảng "yoga_users"
                    .OnceAsync<YogaUser>();

                // Kiểm tra nếu có người dùng nào khớp với email và mật khẩu đã mã hóa
                var matchedUser = users.FirstOrDefault(u =>
                    u.Object.email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                    u.Object.password == hashedPassword);

                if (matchedUser != null)
                {
                    // Lấy thông tin người dùng từ kết quả
                    var user = matchedUser.Object;
                    user.firebaseKey = matchedUser.Key;

                    // Lưu thông tin người dùng hiện tại trong ứng dụng
                    App.CurrentUser = user;

                    // Lưu thông tin đăng nhập vào Preferences để sử dụng cho lần sau
                    Preferences.Set("UserEmail", user.email);

                    await DisplayAlert("Success", "Login successful!", "OK");

                    // Điều hướng đến AppShell (Bottom Navigation Bar)
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

        // Phương thức kiểm tra định dạng email
        private bool IsValidEmail(string email)
        {
            // Biểu thức regex đơn giản để kiểm tra định dạng email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        // Phương thức mã hóa mật khẩu bằng SHA256
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

        // Xử lý sự kiện khi nhấn nút Đăng ký
        private async void btnRegister_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
