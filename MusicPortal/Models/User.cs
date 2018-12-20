using System;
using System.Collections.Generic;
using System.Linq;


namespace MusicPortal.Models
{
    public class User
    {
        public User() =>
            Roles = new List<UserRole>();

        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }
    }
}