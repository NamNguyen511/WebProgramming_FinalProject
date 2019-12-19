using System;
using System.ComponentModel.DataAnnotations;

namespace WebFinalProject.Models.SchoolViewModels
{
    public class ResetPassword
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password does not match")]
        public string ConfirmPassword { get; set; }

        public string  token { get; set; }
    }
}
