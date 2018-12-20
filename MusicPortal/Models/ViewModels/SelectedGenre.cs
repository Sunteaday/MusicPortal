using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPortal.Models.ViewModels
{
    public class SelectedGenre
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }
}