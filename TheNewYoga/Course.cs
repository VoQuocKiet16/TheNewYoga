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

