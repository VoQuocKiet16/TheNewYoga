using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace TheNewYoga
{
    public partial class CartPage : ContentPage
    {
        private readonly FirebaseClient firebaseClient = new FirebaseClient("https://thenewyoga-604c0-default-rtdb.asia-southeast1.firebasedatabase.app/");
        public ObservableCollection<CartItem> UserCart { get; set; }
        public double TotalPrice { get; set; }

        public CartPage()
        {
            InitializeComponent();
            UserCart = new ObservableCollection<CartItem>();
            BindingContext = this;
            _ = LoadUserCart(); // Initial load of cart items.
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = LoadUserCart(); // Reload cart items whenever the page appears.
        }

        private async Task LoadUserCart()
        {
            if (App.CurrentUser == null)
            {
                await DisplayAlert("Error", "You need to log in to view your cart.", "OK");
                return;
            }

            try
            {
                var cartItems = await firebaseClient
                    .Child("carts")
                    .Child(App.CurrentUser.userId)
                    .OnceAsync<CartItem>();

                UserCart.Clear();
                TotalPrice = 0;

                foreach (var cartItem in cartItems)
                {
                    var cart = cartItem.Object;
                    cart.FirebaseKey = cartItem.Key; // Store the key for updates or deletions.
                    UserCart.Add(cart);
                    TotalPrice += cart.TotalPrice; // Sum up the total price.
                }

                OnPropertyChanged(nameof(TotalPrice)); // Update the UI for TotalPrice.
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load cart: {ex.Message}", "OK");
            }
        }

        private async void OnCheckout(object sender, EventArgs e)
        {
            if (!UserCart.Any())
            {
                await DisplayAlert("Error", "Your cart is empty.", "OK");
                return;
            }

            try
            {
                // Loop through each cart item and convert it to a HistoryItem.
                foreach (var cartItem in UserCart)
                {
                    var historyItem = new HistoryItem
                    {
                        ClassId = cartItem.ClassId,
                        userId = cartItem.UserId,
                        PricePerClass = cartItem.PricePerClass,
                        TotalPrice = cartItem.TotalPrice,
                        ClassName = cartItem.ClassName,
                        Quantity = cartItem.Quantity,
                        CheckoutDate = DateTime.UtcNow,
                        email = App.CurrentUser.email
                    };

                    await firebaseClient
                        .Child("history")
                        .Child(App.CurrentUser.userId)
                        .PostAsync(historyItem);
                }

                // Clear the user's cart after storing it in the history.
                await firebaseClient
                    .Child("carts")
                    .Child(App.CurrentUser.userId)
                    .DeleteAsync();

                // Clear the local cart and update the UI.
                UserCart.Clear();
                TotalPrice = 0;
                OnPropertyChanged(nameof(TotalPrice));

                await DisplayAlert("Success", "Checkout completed! Your cart has been cleared and stored in your history.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Checkout failed: {ex.Message}", "OK");
            }
        }

        private async void OnRemoveItemClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var cartItem = (CartItem)button.CommandParameter;

            bool confirmDelete = await DisplayAlert("Confirm", "Do you want to remove this item from your cart?", "Yes", "No");
            if (!confirmDelete) return;

            try
            {
                await firebaseClient
                    .Child("carts")
                    .Child(App.CurrentUser.userId)
                    .Child(cartItem.FirebaseKey)
                    .DeleteAsync();

                UserCart.Remove(cartItem);
                TotalPrice -= cartItem.TotalPrice;
                OnPropertyChanged(nameof(TotalPrice));

                await DisplayAlert("Success", "Item removed from cart successfully!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to remove item: {ex.Message}", "OK");
            }
        }
    }
}
