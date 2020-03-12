using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DynamicPhotoGallary.Models.ViewModel
{
    public class LoginModel
    {
        [Display(Name="User name")]
        [Required]
        public string Username { get; set; }
        [Display(Name="Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}