using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicPortal.Models
{
    public class Song
    {
        public Song() =>
            Genres = new List<Genre>();

        public int Id { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }
    }
}