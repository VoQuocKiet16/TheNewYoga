using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNewYoga
{
    public static class SessionManager
    {
        private const string UserEmailKey = "UserEmail";
        private const string IsLoggedInKey = "IsLoggedIn";

        // Save login session
        public static void SaveLoginSession(string email)
        {
            Preferences.Set(UserEmailKey, email);
            Preferences.Set(IsLoggedInKey, true);
        }

        // Check if user is logged in
        public static bool IsLoggedIn()
        {
            return Preferences.Get(IsLoggedInKey, false);
        }

        // Get the logged-in user's email
        public static string GetUserEmail()
        {
            return Preferences.Get(UserEmailKey, string.Empty);
        }

        // Clear session and logout
        public static void LogoutUser()
        {
            Preferences.Remove(UserEmailKey);
            Preferences.Remove(IsLoggedInKey);
        }
    }
}
