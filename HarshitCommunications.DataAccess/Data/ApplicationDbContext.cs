using HarshitCommunications.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HarshitCommunications.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Charger", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Cables", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Air Buds", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Neck Band", DisplayOrder = 4 },
                new Category { Id = 5, Name = "Car Charger", DisplayOrder = 5 },
                new Category { Id = 6, Name = "Power Bank", DisplayOrder = 6 },
                new Category { Id = 7, Name = "Smart Watch", DisplayOrder = 7 },
                new Category { Id = 8, Name = "Speakers", DisplayOrder = 8 },
                new Category { Id = 9, Name = "Earphone", DisplayOrder = 9 }
                );

            //modelBuilder.Entity<Product>().HasData(
            //    new Product
            //    {
            //        Id = 1,
            //        Name = "COOLPODS 9",
            //        Description = @"{
            //            ""Bluetooth Version"": ""5.1+EDR"",
            //            ""Music Time"": ""25 Hours in Total"",
            //            ""Earphone Battery Capacity"": ""26 mAh"",
            //            ""Earphone Talk Time"": ""4hr"",
            //            ""Earphone Charging Time"": ""1.5hr"",
            //            ""Speaker Size"": ""13mm"",
            //            ""Charging Case Battery Capacity"": ""300 mAh"",
            //            ""Charging Case Time"": ""2Hr"",
            //            ""Standby Time"": ""500 Hours"",
            //            ""Distance"": ""10M""
            //        }",
            //        Price = 3299,
            //        CategoryId = 3,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 2,
            //        Name = "COOLPODS 12",
            //        Description = @"{
            //            ""Bluetooth Version"": ""5.1+EDR"",
            //            ""Music Time"": ""40 Hours in Total"",
            //            ""Earphone Battery Capacity"": ""30 mAh"",
            //            ""Earphone Talk Time"": ""5hr"",
            //            ""Earphone Charging Time"": ""1.5hr"",
            //            ""Speaker Size"": ""8mm"",
            //            ""Charging Case Battery Capacity"": ""270 mAh"",
            //            ""Charging Case Time"": ""2Hr"",
            //            ""Standby Time"": ""500 Hours"",
            //            ""Distance"": ""10M""
            //        }",
            //        Price = 3299,
            //        CategoryId = 3,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 3,
            //        Name = "ROVER 3",
            //        Description = @"{
            //            ""Wireless Version"": ""5.0"",
            //            ""Talk Time"": ""40 Hours"",
            //            ""Music Time"": ""36 Hours"",
            //            ""Stand By Time"": ""400 Hours"",
            //            ""Charging Time"": ""2 Hours"",
            //            ""Charging Type"": ""Type-C"",
            //            ""Battery Capacity"": ""250mAh"",
            //            ""Transmission Distance"": ""10M"",
            //            ""Speaker Size"": ""10MM""
            //        }",
            //        Price = 3299,
            //        CategoryId = 4,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 4,
            //        Name = "ROVER 5",
            //        Description = @"{
            //            ""Wireless Version"": ""5.0"",
            //            ""Talk Time"": ""35 Hours"",
            //            ""Music Time"": ""30 Hours"",
            //            ""Stand By Time"": ""400 Hours"",
            //            ""Charging Time"": ""2 Hours"",
            //            ""Charging Type"": ""Micro USB"",
            //            ""Battery Capacity"": ""250mAh"",
            //            ""Transmission Distance"": ""10M"",
            //            ""Speaker Size"": ""10MM""
            //        }",
            //        Price = 3299,
            //        CategoryId = 4,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 5,
            //        Name = "HYDRO 2",
            //        Description = @"{
            //            ""Model"": ""HYDRO 2"",
            //            ""Bluetooth Version"": ""5.0"",
            //            ""Standby Time"": ""Up to 150 Hours"",
            //            ""Talk Time"": ""28 Hours"",
            //            ""Music Time"": ""20 Hours"",
            //            ""Charging Time"": ""2 Hours"",
            //            ""Battery Capacity"": ""400mAh"",
            //            ""Speaker Size"": ""40MM"",
            //            ""Use Distance"": ""Up to 10mm""
            //        }",
            //        Price = 3299,
            //        CategoryId = 9,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 6,
            //        Name = "HYDRO 3",
            //        Description = @"{
            //            ""Model"": ""HYDRO 3"",
            //            ""Bluetooth Version"": ""5.3+EDR"",
            //            ""Standby Time"": ""Up to 150 Hours"",
            //            ""Talk Time"": ""25 Hours"",
            //            ""Music Time"": ""20 Hours"",
            //            ""Charging Time"": ""2 Hours"",
            //            ""Battery Capacity"": ""200mAh"",
            //            ""Speaker Size"": ""40MM"",
            //            ""Use Distance"": ""Up to 10mm""
            //        }",
            //        Price = 3299,
            //        CategoryId = 9,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 7,
            //        Name = "POWERBOX 5",
            //        Description = @"{
            //            ""Battery"": ""Polymer Li-on Battery"",
            //            ""Battery Capacity="": ""10000mAh"",
            //            ""Input Ports"": ""Micro USB, Type-C"",
            //            ""Output Ports"": ""USB-A"",
            //            ""Portable Cable"": ""Lightning, Type-C, Micro USB"",
            //            ""Material"": ""PC+ABS""
            //        }",
            //        Price = 3999,
            //        CategoryId = 6,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 8,
            //        Name = "POWERBOX 8",
            //        Description = @"{
            //            ""Battery"": ""Polymer Li-on Battery"",
            //            ""Battery Capacity="": ""10000mAh"",
            //            ""Input Ports"": ""USB-A, Type-C"",
            //            ""Output Ports"": ""USB-A"",
            //            ""Portable Cable"": ""Lightning, Type-C, Micro, USB-A"",
            //            ""Material"": ""PC+ABS""
            //        }",
            //        Price = 4999,
            //        CategoryId = 6,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 9,
            //        Name = "Lancer 2",
            //        Description = @"{
            //            ""Model"": ""Lancer 2"",
            //            ""Screen Size"": ""2.0 inch"",
            //            ""Charging Mode"": ""Wireless Charger"",
            //            ""Battery Backup"": ""3-4 Days with BT"",
            //            ""Application"": ""FereFit"",
            //            ""Bluetooth Calling"": ""Yes"",
            //            ""AI Voice Assistant"": ""Yes"",
            //            ""Waterproof Level"": ""IP67"",
            //            ""BT Version"": ""3.0/5.0""
            //        }",
            //        Price = 3299,
            //        CategoryId = 7,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 10,
            //        Name = "Lancer 3",
            //        Description = @"{
            //            ""Model"": ""Lancer 3"",
            //            ""Screen Size"": ""2.0 inch"",
            //            ""Charging Mode"": ""Magnet Charge"",
            //            ""Battery Backup"": ""3-4 Days"",
            //            ""Application"": ""HryFine"",
            //            ""Bluetooth Calling"": ""Yes"",
            //            ""AI Voice Assistant"": ""Yes"",
            //            ""Waterproof Level"": ""IP67"",
            //            ""BT Version"": ""3.0/5.0""
            //        }",
            //        Price = 3299,
            //        CategoryId = 7,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 11,
            //        Name = "Chamber 4c",
            //        Description = @"{
            //            ""Output"": ""22W MAX"",
            //            ""Ports"": ""Type-C"",
            //            ""Protocols Supported"": ""QC3.0, PD, VOOC"",
            //            ""Fast Charging Supported"": ""Samsung OPPO, ONE+, VIVO, REALME, MI""
            //        }",
            //        Price = 3299,
            //        CategoryId = 1,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 12,
            //        Name = "Chamber 4i",
            //        Description = @"{
            //            ""Output"": ""22W MAX"",
            //            ""Ports"": ""Type-C"",
            //            ""Protocols Supported"": ""QC3.0, PD, VOOC"",
            //            ""Fast Charging Supported"": ""Samsung OPPO, ONE+, VIVO, REALME, MI""
            //        }",
            //        Price = 3299,
            //        CategoryId = 1,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 13,
            //        Name = "Flexy 3",
            //        Description = @"{
            //            ""Current"": ""4A Output"",
            //            ""Wire Material"": ""Braided"",
            //            ""Length"": ""1 Meter"",
            //            ""Shell"": ""TPE"",
            //            ""Core"": ""Tinned Copper Wire"",
            //            ""Color"": ""Black & Grey""
            //        }",
            //        Price = 3299,
            //        CategoryId = 2,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 14,
            //        Name = "Flexy 6",
            //        Description = @"{
            //            ""Current"": ""4A Output"",
            //            ""Wire Material"": ""Braided"",
            //            ""Length"": ""1 Meter"",
            //            ""Shell"": ""Alluminium Alloy + TPE"",
            //            ""Core"": ""Tinned Copper Wire"",
            //            ""Color"": ""Black, Brown, Grey""
            //        }",
            //        Price = 3299,
            //        CategoryId = 2,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 15,
            //        Name = "Photon 4",
            //        Description = @"{
            //            ""Driver Unit"": ""10mm"",
            //            ""Connector"": ""3.5mm audio jack"",
            //            ""Cord Length"": ""1.2m""
            //        }",
            //        Price = 3299,
            //        CategoryId = 9,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 16,
            //        Name = "Photon 5",
            //        Description = @"{
            //            ""Driver Unit"": ""10mm"",
            //            ""Connector"": ""3.5mm audio jack"",
            //            ""Cord Length"": ""1.2m""
            //        }",
            //        Price = 3299,
            //        CategoryId = 9,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 17,
            //        Name = "Piston 5L (Without Cable)",
            //        Description = @"{
            //            ""Model Name"": ""Piston 5L"",
            //            ""Input"": ""DC 12-24V"",
            //            ""Output"": ""5V = 2.4A (Max)"",
            //            ""Material"": ""ABS+PC""
            //        }",
            //        Price = 3299,
            //        CategoryId = 5,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 18,
            //        Name = "Piston 7",
            //        Description = @"{
            //            ""Model Name"": ""Piston 5L"",
            //            ""Input"": ""DC 12-24V"",
            //            ""Output"": ""DC5V/3.0A"",
            //            ""Port"": ""USB-A Port *2""
            //        }",
            //        Price = 3299,
            //        CategoryId = 5,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 19,
            //        Name = "JUKEBOX 3",
            //        Description = @"{
            //            ""Input Power"": ""5V = 1A"",
            //            ""Output"": ""10W(RWS)"",
            //            ""BT Version"": ""5.0"",
            //            ""Battery"": ""3.7V(1200mAh)"",
            //            ""Connectivity Options"": ""Wireless"",
            //            ""Dimensions"": ""64mm x 380mm x 56mm""
            //        }",
            //        Price = 3299,
            //        CategoryId = 8,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 20,
            //        Name = "JUKEBOX 4",
            //        Description = @"{
            //            ""Input Power"": ""5V = 1A"",
            //            ""Output"": ""20W(RWS)"",
            //            ""BT Version"": ""5.0"",
            //            ""Battery"": ""3.7V(3000mAh)"",
            //            ""Connectivity Options"": ""Wireless"",
            //            ""Dimensions"": ""212mm x 367mm x 158mm""
            //        }",
            //        Price = 3299,
            //        CategoryId = 8,
            //        ImageUrl = ""
            //    },

            //    new Product
            //    {
            //        Id = 21,
            //        Name = "Flame 1t",
            //        Description = @"{
            //            ""Cable Length"": ""1 meter"",
            //            ""Material"": ""Braided wire"",
            //            ""Color"": ""Black with Silver Alloy"",
            //            ""Interface"": ""Type-C to 3.5mm Aux"",
            //            ""Input"": ""USB Type-C"",
            //            ""Output"": ""3.5mm Aux""
            //        }",
            //        Price = 3299,
            //        CategoryId = 1,
            //        ImageUrl = ""
            //    }
            //    );


        }
    }
}