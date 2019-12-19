using System;
using System.ComponentModel.DataAnnotations;

namespace WebFinalProject.Models
{
    
    public class Enrollment
    {
        [Key]
        public int  EnrollmentID { get; set; }

        public int StudentID { get; set; }
        public int CourseID { get; set; }
        [DisplayFormat(NullDisplayText = "No Grade")]
        public int Grade { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }

    }
}
