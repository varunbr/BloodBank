using System;
using System.Linq;
using API.Entities;
using API.Helpers;

namespace API.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<AppUser> BuildQuery(this IQueryable<AppUser> query, UserParams userParams)
        {
            query = query.Where(u => u.UserName != userParams.CurrentUserName);

            var genders = new[] { "male", "female" };
            if (genders.Contains(userParams.Gender, StringComparer.OrdinalIgnoreCase))
            {
                query = query.Where(u => u.Gender == userParams.Gender);
            }

            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
            var minDob = DateTime.Today.AddYears(-userParams.MaxAge);

            if (userParams.MaxAge != 0)
                query = query.Where(u => u.DateOfBirth >= minDob);
            if (userParams.MinAge != 0)
                query = query.Where(u => u.DateOfBirth <= maxDob);

            if (!string.IsNullOrEmpty(userParams.BloodGroup))
            {
                query = query.Where(u => Util.GetDonorGroups(userParams.BloodGroup).Contains(u.BloodGroup));
            }

            if (!string.IsNullOrEmpty(userParams.Address))
            {
                var items = userParams.Address.Split(" ");
                query = query.Where(u =>
                    items.Contains(u.Address.Area) ||
                    items.Contains(u.Address.City) ||
                    items.Contains(u.Address.State) ||
                    items.Contains(u.Address.Country) ||
                    items.Contains(u.Address.PostalCode));
            }

            query = userParams.OrderBy switch
            {
                _ => query.OrderByDescending(u => u.LastActive)
            };
            return query;
        }
    }
}
