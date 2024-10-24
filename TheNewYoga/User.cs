namespace TheNewYoga
{
    public class YogaUser
    {
        public string userId { get; set; } // ID của người dùng, dùng làm khóa chính
        public string firebaseKey { get; set; } // Khóa Firebase để đồng bộ với dữ liệu trên Firebase
        public string email { get; set; } // Email của người dùng
        public string password { get; set; } // Mật khẩu của người dùng (đã hash)
        public string username { get; set; } // Tên người dùng
        public string role { get; set; } // Role của người dùng (User, Teacher, Admin)
        public string roleAsString { get; set; }

        // Constructor mặc định
        public YogaUser() { }

        // Constructor khởi tạo đối tượng User với giá trị ban đầu
        public YogaUser(string userId, string firebaseKey, string email, string password, string username, string role, string roleAsString)
        {
            this.userId = userId;
            this.firebaseKey = firebaseKey;
            this.email = email;
            this.password = password;
            this.username = username;
            this.role = role;
            this.roleAsString = roleAsString;
        }
    }

    public class Role
    {
        public static readonly Role Admin = new Role("Admin");
        public static readonly Role Teacher = new Role("Teacher");
        public static readonly Role Customer = new Role("Customer");

        public string RoleName { get; private set; }

        private Role(string roleName)
        {
            RoleName = roleName;
        }

        // Phương thức để lấy tên của Role
        public string GetRoleName()
        {
            return RoleName;
        }

        // Phương thức chuyển từ string thành Role
        public static Role FromString(string roleName)
        {
            switch (roleName.ToLower())
            {
                case "admin":
                    return Admin;
                case "teacher":
                    return Teacher;
                case "customer":
                    return Customer;
                default:
                    throw new ArgumentException("Invalid role name: " + roleName);
            }
        }

        public override string ToString()
        {
            return RoleName;
        }
    }





    public class YogaCourse
    {
        public string Id { get; set; } // Course ID
        public string FirebaseKey { get; set; } // Firebase key for synchronization
        public string DayOfWeek { get; set; } // Day of the week when the course takes place
        public string Time { get; set; } // Course time
        public int Capacity { get; set; } // Capacity for the course
        public string Duration { get; set; } // Duration of the course
        public double PricePerClass { get; set; } // Price for each class
        public string ClassType { get; set; } // Type of class (e.g., Flow Yoga)
        public string Description { get; set; } // Description of the course
        public string NameCourse { get; set; } // Name of the course
        public bool IsSynced { get; set; } // Indicates if the course is synchronized with Firebase

        // Default constructor
        public YogaCourse() { }

        // Constructor with parameters
        public YogaCourse(string id, string firebaseKey, string dayOfWeek, string time, int capacity, string duration, double pricePerClass, string classType, string description, string nameCourse, bool isSynced)
        {
            Id = id;
            FirebaseKey = firebaseKey;
            DayOfWeek = dayOfWeek;
            Time = time;
            Capacity = capacity;
            Duration = duration;
            PricePerClass = pricePerClass;
            ClassType = classType;
            Description = description;
            NameCourse = nameCourse;
            IsSynced = isSynced;
        }
    }

    public class YogaClass
    {
        public string Id { get; set; } // ID of the class
        public string CourseId { get; set; } // ID of the associated course
        public string Date { get; set; } // Date when the class takes place
        public string Teacher { get; set; } // Name of the teacher for the class
        public string FirebaseKey { get; set; } // Firebase key for synchronization
        public double CoursePrice { get; set; } // Price of the course
        public bool IsSynced { get; set; } // Indicates if the class is synchronized with Firebase

        // Default constructor
        public YogaClass() { }

        // Parameterized constructor for easy instantiation
        public YogaClass(string id, string courseId, string date, string teacher, string firebaseKey, double coursePrice, bool isSynced)
        {
            Id = id;
            CourseId = courseId;
            Date = date;
            Teacher = teacher;
            FirebaseKey = firebaseKey;
            CoursePrice = coursePrice;
            IsSynced = isSynced;
        }
    }


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


}