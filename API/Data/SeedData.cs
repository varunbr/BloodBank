using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Data
{
    public class SeedData
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration config)
        {
            if (await userManager.Users.AnyAsync()) return;

            var data = await File.ReadAllTextAsync("Data/UserSeed.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(data);

            if (users == null) return;

            var roles = new List<AppRole>
            {
                new() {Name = "Member"},
                new() {Name = "Admin"},
                new() {Name = "Moderator"},
                new() {Name = "BankAdmin"},
                new() {Name = "BankModerator"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();

                if (user.UserName == "admin")
                {
                    await userManager.CreateAsync(user, config["AdminPassword"]);
                    await userManager.AddToRolesAsync(user, new[] { "Member", "Admin" });
                    continue;
                }

                await userManager.CreateAsync(user, "User@2021");
                await userManager.AddToRoleAsync(user, "Member");

                if (user.UserName == "lynn")
                    await userManager.AddToRoleAsync(user, "Moderator");
            }
        }

        public static async Task SeedBanks(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, DataContext context)
        {
            if (await context.Banks.AnyAsync()) return;

            var data = await File.ReadAllTextAsync("Data/BankSeed.json");
            var banks = JsonSerializer.Deserialize<List<Bank>>(data);

            var pat = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == "pat");
            var tia = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == "tia");

            if (banks == null) return;

            var i = 0;
            foreach (var bank in banks)
            {
                var user = await userManager.Users.FirstOrDefaultAsync(u =>
                    u.UserName == bank.Moderators.First().User.UserName.ToLower());
                if (user == null)
                    throw new Exception("User not found!");
                bank.Moderators = new List<Moderator>
                {
                    new() { Bank = bank, User = user, Type = "BankAdmin" },
                    new() { Bank = bank, User = i % 2 == 0 ? pat : tia, Type = "BankModerator" }
                };

                await context.Banks.AddAsync(bank);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                {
                    await userManager.AddToRoleAsync(user, "BankAdmin");
                }

                i++;
            }

            await userManager.AddToRoleAsync(pat, "BankModerator");
            await userManager.AddToRoleAsync(tia, "BankModerator");
        }
    }
}
