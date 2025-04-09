using Microsoft.EntityFrameworkCore;
using MuseumExhibitManager.Database;
using MuseumExhibitManager.Forms;
using MuseumExhibitManager.Models;
using MuseumExhibitManager.Services;

namespace MuseumExhibitManager
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Initialize the database
            using (var context = new MuseumContext())
            {
                context.Database.EnsureCreated();
                SeedInitialData(context);
            }

            Application.Run(new LoginForm());
        }

        private static void SeedInitialData(MuseumContext context)
        {
            // Only seed if the database is empty
            if (!context.Users.Any())
            {
                // Create admin and regular user
                var admin = new User
                {
                    Username = "admin",
                    Password = PasswordHasher.HashPassword("admin123"),
                    IsAdmin = true,
                    FullName = "Administrator"
                };

                var user = new User
                {
                    Username = "user",
                    Password = PasswordHasher.HashPassword("user123"),
                    IsAdmin = false,
                    FullName = "Regular User"
                };

                context.Users.Add(admin);
                context.Users.Add(user);

                // Create sample exhibit types
                var types = new List<ExhibitType>
                {
                    new ExhibitType { Name = "Painting", Description = "Artwork created using paint on a surface" },
                    new ExhibitType { Name = "Sculpture", Description = "Three-dimensional art created by shaping or combining materials" },
                    new ExhibitType { Name = "Artifact", Description = "Object made by a human being, typically of cultural or historical interest" },
                    new ExhibitType { Name = "Fossil", Description = "Preserved remains or traces of organisms from a past geologic age" },
                    new ExhibitType { Name = "Document", Description = "Historical or significant paper documents" }
                };

                context.ExhibitTypes.AddRange(types);

                // Create sample locations
                var locations = new List<Location>
                {
                    new Location { Name = "Gallery A", Floor = 1, Room = "101" },
                    new Location { Name = "Gallery B", Floor = 1, Room = "102" },
                    new Location { Name = "Gallery C", Floor = 2, Room = "201" },
                    new Location { Name = "Vault", Floor = 0, Room = "V01" },
                    new Location { Name = "Archive", Floor = -1, Room = "A01" }
                };

                context.Locations.AddRange(locations);
                
                context.SaveChanges();
            }
        }
    }
}
