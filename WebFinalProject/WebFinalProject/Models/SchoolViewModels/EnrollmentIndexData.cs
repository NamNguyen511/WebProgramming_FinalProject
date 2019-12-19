using System;
using System.Collections.Generic;

namespace WebFinalProject.Models.SchoolViewModels
{
    public class EnrollmentIndexData
    {
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
