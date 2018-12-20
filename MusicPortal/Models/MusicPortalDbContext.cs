using MusicPortal.App_Start;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MusicPortal.Models
{
    public class MusicPortalDbContext : DbContext
    {
        static MusicPortalDbContext()
        {
            Database.SetInitializer(new MusicPortalDBInitializer());
        }

        public MusicPortalDbContext()
            : base("MusicPortalDb")
        {

        }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}