﻿using HarshitCommunications.DataAccess.Data;
using HarshitCommunications.Models;
using HarshitCommunications.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarshitCommunications.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }

            catch (Exception ex) { }

            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@HarshitCommunication.com",
                    Email = "admin@HarshitCommunication.com",
                    Name = "Harshit Lalwani",
                    PhoneNumber = "9709636213",
                    Address = "test 123 Ave",
                    City = "Godhra",
                    State = "Gujarat",
                    Pincode = "390009"
                }, "Admin123*").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@HarshitCommunication.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
