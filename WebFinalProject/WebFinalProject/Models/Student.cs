using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFinalProject.Models

{
    public class Student
    {
        [Key]
        public int  StudentID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string  LastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{00:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime Birthday { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{00:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime EnrollmentDate { get; set; }

        public  ICollection<Enrollment> Enrollments { get; set; }
        
    }
}
