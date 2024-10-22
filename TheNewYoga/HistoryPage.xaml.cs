using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace TheNewYoga
{
    public partial class HistoryPage : ContentPage
    {
        private readonly FirebaseClient firebaseClient = new FirebaseClient("https://thenewyoga-604c0-default-rtdb.asia-southeast1.firebasedatabase.app/");
        public ObservableCollection<HistoryItem> UserHistory { get; set; }

        public HistoryPage()
        {
            InitializeComponent();
            UserHistory = new ObservableCollection<HistoryItem>();
            BindingContext = this;
            _ = LoadUserHistory();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = LoadUserHistory(); // Reload cart items whenever the page appears.
        }

        private async Task LoadUserHistory()
        {
            Console.WriteLine("Loading user history...");
            if (App.CurrentUser == null)
            {
                await DisplayAlert("Error", "You need to log in to view your history.", "OK");
                return;
            }

            try
            {
                var historyItems = await firebaseClient
                    .Child("history")
                    .Child(App.CurrentUser.userId)
                    .OnceAsync<HistoryItem>();

                UserHistory.Clear();

                foreach (var historyItem in historyItems)
                {
                    var item = historyItem.Object;
                    item.FirebaseKey = historyItem.Key;

                    item.ClassName = item.ClassName ?? "Unknown";
                    item.Quantity = item.Quantity > 0 ? item.Quantity : 1;
                    item.PricePerClass = item.PricePerClass > 0 ? item.PricePerClass : 0;
                    item.TotalPrice = item.TotalPrice > 0 ? item.TotalPrice : item.Quantity * item.PricePerClass;

                    UserHistory.Add(item);
                }

                OnPropertyChanged(nameof(UserHistory));
                Console.WriteLine("History loaded successfully.");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load history: {ex.Message}", "OK");
                Console.WriteLine($"Error loading history: {ex.Message}");
            }
        }


    }
}
