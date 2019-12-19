using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFinalProject.Models
{
    public class Instructor
    {
        [Key]
        public int  InstructorID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{00:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        private ICollection<CourseAssignment> _courseAssignments;
        public  ICollection<CourseAssignment> CourseAssignments
        {
            get
            {
                return _courseAssignments ?? (_courseAssignments = new List<CourseAssignment>());
            }
            set
            {
                _courseAssignments = value;
            }

        }
        
    }
}
