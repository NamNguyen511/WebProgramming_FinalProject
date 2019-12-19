using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebFinalProject.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string  Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remmeber me")]
        public bool Rememberme { get; set; }

       
    }
}

