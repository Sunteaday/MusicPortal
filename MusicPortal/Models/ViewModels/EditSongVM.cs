using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicPortal.Models.ViewModels
{
    public class EditSongVM
    {
        public IEnumerable<SelectedGenre> Genres { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(100, ErrorMessage = "Max length is 100 chars")]
        [MinLength(4, ErrorMessage = "Min length is 4 chars")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase File { get; set; }
    }
}