public class HistoryItem
{
    public string ClassId { get; set; }
    public string userId { get; set; }
    public double PricePerClass { get; set; }
    public double TotalPrice { get; set; }
    public string ClassName { get; set; }
    public int Quantity { get; set; }
    public DateTime CheckoutDate { get; set; } // Optional: Store the date and time when the user checked out.
    public string FirebaseKey { get; set; }
    public string email { get; set; }
}
