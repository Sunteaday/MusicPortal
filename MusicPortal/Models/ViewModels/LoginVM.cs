using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicPortal.Models.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "This field is required")]
        [MinLength(2, ErrorMessage = "Min length is 2 chars")]
        [MaxLength(100, ErrorMessage = "Max length is 100 chars")]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MinLength(4, ErrorMessage = "Min length is 4 chars")]
        [MaxLength(100, ErrorMessage = "Max length is 100 chars")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}