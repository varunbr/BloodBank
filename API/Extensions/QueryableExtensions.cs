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
            query = query.Where(u => u.UserName != userParams.CurrentUserName && u.Available);

            var genders = new[] { "male", "female" };
            if (genders.Contains(userParams.Gender, StringComparer.OrdinalIgnoreCase))
            {
                query = query.Where(u => u.Gender == userParams.Gender);
            }

            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
            var minDob = DateTime.Today.AddYears(-userParams.MaxAge);

            if (userParams.MaxAge > 0)
                query = query.Where(u => u.DateOfBirth >= minDob);
            if (userParams.MinAge > 0)
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
            if (bankParams.Id > 0)
            {
                query = query.Where(b => b.Id == bankParams.Id);
                return query;
            }

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

            if (!string.IsNullOrEmpty(bankParams.BloodGroup))
            {
                query = query.Where(b =>
                    b.BloodGroups.Any(bg =>
                        bg.Group == bankParams.BloodGroup && bg.Value > 0));
            }

            query = bankParams.OrderBy switch
            {
                _ => query.OrderByDescending(b => b.LastUpdated)
            };
            return query;
        }

        public static IQueryable<AppUserRole> BuildQuery(this IQueryable<AppUserRole> query, AdminRoleParams roleParams)
        {
            if (roleParams.UserId > 0) 
                query = query.Where(r => r.UserId == roleParams.UserId);

            if (!string.IsNullOrEmpty(roleParams.UserName))
                query = query.Where(r => r.User.UserName == roleParams.UserName);

            if (!string.IsNullOrEmpty(roleParams.Name))
                query = query.Where(r => r.User.Name.Contains(roleParams.Name));

            query = string.IsNullOrEmpty(roleParams.Role) ? 
                query.Where(r => r.Role.Name == "Admin" || r.Role.Name== "Moderator") : 
                query.Where(r => r.Role.Name == roleParams.Role);

            query = query.OrderByDescending(r => r.User.LastActive);
            return query;
        }
    }
}
