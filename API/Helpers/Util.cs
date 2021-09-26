using System;
using System.Collections.Generic;

namespace API.Helpers
{
    public static class Util
    {
        public static IEnumerable<string> GetDonorGroups(string group)
        {
            switch (group)
            {
                case "O+":
                    return new[] { "O-", "O+" };
                case "A-":
                    return new[] { "O-", "A-" };
                case "A+":
                    return new[] { "O-", "O+", "A-", "A+" };
                case "B-":
                    return new[] { "O-", "B-" };
                case "B+":
                    return new[] { "O-", "O+", "B-", "B+" };
                case "AB-":
                    return new[] { "O-", "A-", "B-", "AB-" };
                case "AB+":
                    return new[] { "O-", "O+", "A-", "A+", "B-", "B+", "AB-", "AB+" };
            }
            return new[] { group };
        }

        public static IEnumerable<string> GetGenderList()
        {
            return new[] { "male", "female" };
        }

        public static IEnumerable<string> GetBloodGroupList()
        {
            return new[] { "O-", "O+", "A-", "A+", "B-", "B+", "AB-", "AB+" };
        }

        public static IEnumerable<string> GetBankRoles()
        {
            return new[] { "BankModerator", "BankAdmin" };
        }

        public static IEnumerable<string> GetAdminRoles()
        {
            return new[] { "Moderator", "Admin" };
        }

        public static int GetAge(DateTime dob)
        {
            var today = DateTime.Now;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age).Date)
                age--;
            return age;
        }
    }
}