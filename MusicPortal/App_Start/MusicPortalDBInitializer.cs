using MusicPortal.Infrastructure;
using MusicPortal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;


namespace MusicPortal.App_Start
{
    public class MusicPortalDBInitializer : CreateDatabaseIfNotExists<MusicPortalDbContext>
    {
        protected override void Seed(MusicPortalDbContext context)
        {
            context.Genres.AddRange(new List<Genre>()
            {
                new Genre() { Name = "Alternative" },
                new Genre() { Name = "Ambient" },
                new Genre() { Name = "Blues" },
                new Genre() { Name = "Background" },
                new Genre() { Name = "Chanson" },
                new Genre() { Name = "Classical" },
                new Genre() { Name = "Club" },
                new Genre() { Name = "Country" },
                new Genre() { Name = "Dance" },
                new Genre() { Name = "Disco" },
                new Genre() { Name = "Drum & Bass" },
                new Genre() { Name = "Electro" },
                new Genre() { Name = "Folk" },
                new Genre() { Name = "Funk" },
                new Genre() { Name = "Hardcore" },
                new Genre() { Name = "Hip-Hop" },
                new Genre() { Name = "House" },
                new Genre() { Name = "Industrial" },
                new Genre() { Name = "Instrumental" },
                new Genre() { Name = "Jazz" },
                new Genre() { Name = "Metal" },
                new Genre() { Name = "Minimal" },
                new Genre() { Name = "Pop-Rock" },
                new Genre() { Name = "Punk" },
                new Genre() { Name = "Rap" },
                new Genre() { Name = "Reggae" },
                new Genre() { Name = "Retro" },
                new Genre() { Name = "R&B" },
                new Genre() { Name = "Rock" },
                new Genre() { Name = "Soul" },
                new Genre() { Name = "Soundtrack" },
                new Genre() { Name = "Techno" },
                new Genre() { Name = "Trance" }
            });

            UserRole authorized = new UserRole() { Name = "Admin" };
            UserRole admin = new UserRole() { Name = "Authorized" };

            context.UserRoles.AddRange(new List<UserRole>() {
                authorized,
                admin,
                new UserRole() { Name = "Authenticated" }
            });

            var salt = CryptoService.GetRandomBytes(AppConstants.PASSOWORD_SALT_LENGTH).ToHexString();
            context.Users.Add(new User()
            {
                Login = "admin",
                Password = CryptoService.ComputeMD5Hash(Encoding.Unicode.GetBytes((salt + "admin"))).ToHexString(),
                Salt = salt,
                Roles = new List<UserRole>() { authorized, admin }
            });

            context.SaveChanges();

            base.Seed(context);
        }
    }
}