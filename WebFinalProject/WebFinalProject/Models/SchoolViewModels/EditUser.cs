using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFinalProject.Models.SchoolViewModels
{
    public class EditUser
    {
        public EditUser()
        {
            Roles = new List<string>();
        }
        public string  ID { get; set; }

        [Required]
        public string  UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public IList<string> Roles { get; set; }
    }
}
