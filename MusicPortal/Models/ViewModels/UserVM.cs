using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicPortal.Models.ViewModels
{
    public class UserVM
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        public IEnumerable<SelectedRole> Roles { get; set; }
    }
}