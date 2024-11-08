namespace TheNewYoga
{
    public class YogaUser
    {
        public string userId { get; set; } 
        public string firebaseKey { get; set; } 
        public string email { get; set; }
        public string password { get; set; }
        public string username { get; set; } 
        public string role { get; set; }
        public string roleAsString { get; set; }

        public YogaUser() { }

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

        public string GetRoleName()
        {
            return RoleName;
        }
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





}