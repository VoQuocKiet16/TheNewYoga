using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace TheNewYoga
{
    public partial class MainPage : ContentPage
    {
        private readonly FirebaseClient firebaseClient = new FirebaseClient("https://thenewyoga-604c0-default-rtdb.asia-southeast1.firebasedatabase.app/");
        private ObservableCollection<YogaCourse> _allYogaCourses; // Lưu tất cả khóa học
        public ObservableCollection<YogaCourse> YogaCourses { get; set; } // Khóa học được hiển thị trên UI

        // Constructor mặc định
        public MainPage()
        {
            InitializeComponent();
            YogaCourses = new ObservableCollection<YogaCourse>();
            _allYogaCourses = new ObservableCollection<YogaCourse>();
            BindingContext = this;

            // Hiển thị thông báo chào mừng mặc định nếu không có username
            WelcomeLabel.Text = "Welcome to The New Yoga";

            // Tải khóa học
            _ = LoadYogaCourses();
        }

        private async Task LoadYogaCourses()
        {
            if (App.CurrentUser == null)
            {
                await DisplayAlert("Error", "No user is currently logged in.", "OK");
                return;
            }

            try
            {
                var courses = await firebaseClient
                    .Child("yoga_courses")
                    .OnceAsync<YogaCourse>();

                _allYogaCourses.Clear();
                YogaCourses.Clear();

                foreach (var course in courses)
                {
                    var newCourse = course.Object;
                    newCourse.FirebaseKey = course.Key;
                    _allYogaCourses.Add(newCourse);
                    YogaCourses.Add(newCourse);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK");
            }
        }

        // Lọc khóa học theo từ khóa tìm kiếm
        private void FilterCourses(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Nếu không có từ khóa, hiển thị tất cả khóa học
                YogaCourses.Clear();
                foreach (var course in _allYogaCourses)
                {
                    YogaCourses.Add(course);
                }
            }
            else
            {
                var filteredCourses = _allYogaCourses
                    .Where(course =>
                        (course.DayOfWeek?.ToLower().Contains(searchText.ToLower()) ?? false) ||
                        (course.Time?.ToLower().Contains(searchText.ToLower()) ?? false) ||
                        (course.ClassType?.ToLower().Contains(searchText.ToLower()) ?? false) ||
                        (course.Description?.ToLower().Contains(searchText.ToLower()) ?? false) ||
                        (course.NameCourse?.ToLower().Contains(searchText.ToLower()) ?? false))
                    .ToList();

                YogaCourses.Clear();
                foreach (var course in filteredCourses)
                {
                    YogaCourses.Add(course);
                }
            }
        }

        // Xử lý sự kiện thay đổi nội dung SearchBar
        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterCourses(e.NewTextValue);
        }

        // Khi người dùng chọn khóa học
        private async void OnFrameTapped(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var selectedCourse = (YogaCourse)frame.BindingContext;
            await Navigation.PushAsync(new ClassesPage(selectedCourse.Id, selectedCourse.NameCourse, selectedCourse.PricePerClass));
        }

        // Chuyển đến trang giỏ hàng
        private async void OnViewCartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartPage());
        }

        // Chuyển đến trang lịch sử
        private async void OnViewHistoryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HistoryPage());
        }
    }
}
