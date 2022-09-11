using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helpdesk.Core.ViewModels.Account
{
    public class AccountViewModel
    {
        public class ExternalLoginListViewModel
        {
            public string ReturnUrl { get; set; }
        }

        public class Login
        {
            [Required(ErrorMessage = "Ju lutem vendosni një përdorues!")]
            [Display(Name = "Përdorues")]
            [DataType(DataType.Text)]
            public string Username { get; set; }

            [Required(ErrorMessage = "Ju lutem vendosni një fjalëkalim!")]
            [DataType(DataType.Password)]
            [Display(Name = "Fjalëkalimi")]
            public string Password { get; set; }
        }

        public class Register
        {
            [Required(ErrorMessage = "Ju lutem vendosni një përdorues!")]
            [DataType(DataType.Text)]
            [Display(Name = "Përdoruesi")]
            [StringLength(100, ErrorMessage = "{0} duhet të jetë te paktën {2} karaktere!", MinimumLength = 3)]

            public string Username { get; set; }

            [Required(ErrorMessage = "Ju lutem vendosni një fjalëkalim!")]
            [StringLength(100, ErrorMessage = "{0} duhet të jetë te paktën {2} karaktere!", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Fjalëkalimi")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Konfirmo fjalëkalimin")]
            [Compare("Password", ErrorMessage = "Fjalëkalimet nuk përputhen!")]
            public string ConfirmPassword { get; set; }
        }
    }
}