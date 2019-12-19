using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFinalProject.Models
{
    public class EditRole
    {
        public EditRole()
        {
            Users = new List<string>();
        }
        public string ID { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
