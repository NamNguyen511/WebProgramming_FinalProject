using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebFinalProject.Utilities;

namespace WebFinalProject.Models

{
    public class Register 
    {
      
        [Required]
       [EmailAddress]
       [Remote(action:"IsEmailInUse",controller:"Account")]
       [ValidEmailDomainAttributes(allowedDomain: "gmail.com",ErrorMessage = "Email domain must be gmail.com")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string  Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage = "Password and Confirm Password does not match")]
        public string ConfirmPassword { get; set; }

    }
}
