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

        public static IQueryable<Bank> BuildQuery(this IQueryable<Bank> query, BankParams bankParams)
        {
            if (!string.IsNullOrEmpty(bankParams.Name))
            {
                query = query.Where(b => b.Name.Contains(bankParams.Name));
            }
            if (!string.IsNullOrEmpty(bankParams.Address))
            {
                var items = bankParams.Address.Split(" ");
                query = query.Where(b =>
                    items.Contains(b.Address.Area) ||
                    items.Contains(b.Address.City) ||
                    items.Contains(b.Address.State) ||
                    items.Contains(b.Address.Country) ||
                    items.Contains(b.Address.PostalCode));
            }

            query = bankParams.OrderBy switch
            {
                _ => query.OrderByDescending(b => b.LastUpdated)
            };
            return query;
        }
    }
}
