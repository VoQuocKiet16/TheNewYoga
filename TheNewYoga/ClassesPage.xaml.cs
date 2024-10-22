using Firebase.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Firebase.Database.Query;

namespace TheNewYoga
{
    public partial class ClassesPage : ContentPage
    {
        private readonly FirebaseClient firebaseClient = new FirebaseClient("https://thenewyoga-604c0-default-rtdb.asia-southeast1.firebasedatabase.app/");
        private ObservableCollection<YogaClass> _allYogaClasses; // Store all fetched classes
        public ObservableCollection<YogaClass> YogaClasses { get; set; } // Filtered classes for UI display
        public string CourseName { get; set; }
        private double CoursePrice { get; set; }

        public ClassesPage(string courseId, string courseName, double coursePrice)
        {
            InitializeComponent();
            CourseName = courseName;
            CoursePrice = coursePrice; // Store the price of the course
            YogaClasses = new ObservableCollection<YogaClass>();
            _allYogaClasses = new ObservableCollection<YogaClass>();
            BindingContext = this;

            Title = $"Classes for {courseName}";
             _ = LoadYogaClasses(courseId); // Load classes when the page is initialized
        }

        private async Task LoadYogaClasses(string courseId)
        {
            try
            {
                var classes = await firebaseClient
                    .Child("yoga_classes")
                    .OnceAsync<YogaClass>();

                _allYogaClasses.Clear();
                YogaClasses.Clear();

                foreach (var classData in classes)
                {
                    var yogaClass = classData.Object;
                    if (yogaClass.CourseId == courseId)
                    {
                        yogaClass.FirebaseKey = classData.Key; // Store the Firebase key for later use if needed
                        _allYogaClasses.Add(yogaClass); // Add to the complete list
                        YogaClasses.Add(yogaClass); // Add to the filtered list for display
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load classes: {ex.Message}", "OK");
            }
        }

        private void FilterClasses(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Show all classes if search text is empty
                YogaClasses.Clear();
                foreach (var yogaClass in _allYogaClasses)
                {
                    YogaClasses.Add(yogaClass);
                }
            }
            else
            {
                var filteredClasses = _allYogaClasses
                    .Where(yogaClass =>
                        (yogaClass.Date?.ToLower().Contains(searchText.ToLower()) ?? false) ||
                        (yogaClass.Teacher?.ToLower().Contains(searchText.ToLower()) ?? false))
                    .ToList();

                YogaClasses.Clear();
                foreach (var yogaClass in filteredClasses)
                {
                    YogaClasses.Add(yogaClass);
                }
            }
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            // Filter classes based on the search text
            FilterClasses(e.NewTextValue);
        }

        private async Task AddClassToCart(YogaClass selectedClass, double pricePerClass, string courseName)
        {
            if (App.CurrentUser == null)
            {
                await DisplayAlert("Error", "You need to log in to add classes to your cart.", "OK");
                return;
            }

            try
            {
                // Find existing cart for the user
                var existingCart = await firebaseClient
                    .Child("carts")
                    .Child(App.CurrentUser.userId)
                    .OnceAsync<CartItem>();

                var cartItem = existingCart.FirstOrDefault(c => c.Object.ClassId == selectedClass.Id);

                if (cartItem != null)
                {
                    // Update the existing cart item
                    var updatedCart = cartItem.Object;
                    updatedCart.Quantity += 1;
                    updatedCart.TotalPrice = updatedCart.Quantity * updatedCart.PricePerClass;

                    await firebaseClient
                        .Child("carts")
                        .Child(App.CurrentUser.userId)
                        .Child(cartItem.Key)
                        .PutAsync(updatedCart);

                    await DisplayAlert("Success", "Class quantity updated in the cart!", "OK");
                }
                else
                {
                    // Add a new cart item
                    var newCart = new CartItem
                    {
                        UserId = App.CurrentUser.userId,
                        ClassId = selectedClass.Id,
                        ClassName = courseName,
                        PricePerClass = pricePerClass,
                        Quantity = 1,
                        TotalPrice = pricePerClass
                    };

                    await firebaseClient
                        .Child("carts")
                        .Child(App.CurrentUser.userId)
                        .PostAsync(newCart);

                    await DisplayAlert("Success", "Class added to cart successfully!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to add class to cart: {ex.Message}", "OK");
            }
        }

        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            var imageButton = (ImageButton)sender;
            var selectedClass = (YogaClass)imageButton.BindingContext;

            // Use the stored course price and name from this context
            double pricePerClass = CoursePrice;
            string courseName = CourseName;

            await AddClassToCart(selectedClass, pricePerClass, courseName);
        }
    }
}
