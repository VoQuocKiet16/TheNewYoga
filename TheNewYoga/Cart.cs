public class Cart
{
    public string Id { get; set; } // Unique ID for the cart item
    public string UserId { get; set; } // ID of the user who owns the cart
    public string ClassId { get; set; } // ID of the class added to the cart
    public double TotalPrice { get; set; } // Total price of the cart
    public string FirebaseKey { get; set; } // Firebase key for updating/deleting cart data

    public Cart() { }

    public Cart(string userId, string classId, double totalPrice)
    {
        UserId = userId;
        ClassId = classId;
        TotalPrice = totalPrice;
    }
}

public class CartItem
{
    public string ClassId { get; set; } // Unique identifier for the class being added to the cart.
    public string UserId { get; set; } // ID of the user who owns this cart.
    public double PricePerClass { get; set; } // The price for a single class.
    public double TotalPrice { get; set; } // The total price for the selected class, calculated as PricePerClass * Quantity.
    public string ClassName { get; set; } // Name of the class.
    public int Quantity { get; set; } // The quantity of classes added to the cart (default 1 if not specified).

    public string FirebaseKey { get; set; }
}
