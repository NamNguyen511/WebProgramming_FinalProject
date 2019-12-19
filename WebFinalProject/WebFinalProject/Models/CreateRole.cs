using System;
using System.ComponentModel.DataAnnotations;

namespace WebFinalProject.Models
{
    public class CreateRole
    {
        [Required]
        public string  RoleName { get; set; }
    }
}
