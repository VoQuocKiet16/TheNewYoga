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
