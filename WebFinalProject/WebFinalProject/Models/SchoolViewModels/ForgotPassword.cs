using System;
using System.ComponentModel.DataAnnotations;

namespace WebFinalProject.Models.SchoolViewModels
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
