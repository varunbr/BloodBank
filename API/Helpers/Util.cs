using System.Collections.Generic;

namespace API.Helpers
{
    public static class Util
    {
        public static IEnumerable<string> GetDonorGroups(string group)
        {
            switch (group)
            {
                case "Op":
                    return new[] { "On", "Op" };
                case "An":
                    return new[] { "On", "An" };
                case "Ap":
                    return new[] { "On", "Op", "An", "Ap" };
                case "Bn":
                    return new[] { "On", "Bn" };
                case "Bp":
                    return new[] { "On", "Op", "Bn", "Bp" };
                case "ABn":
                    return new[] { "On", "An", "Bn", "ABn" };
                case "ABp":
                    return new[] { "On", "Op", "An", "Ap", "Bn", "Bp", "ABn", "ABp" };
            }
            return new[] { group };
        }

        public static IEnumerable<string> GetGenderList()
        {
            return new[] { "male", "female" };
        }

        public static IEnumerable<string> GetBloodGroupList()
        {
            return new[] { "On", "Op", "An", "Ap", "Bn", "Bp", "ABn", "ABp" };
        }
    }
}