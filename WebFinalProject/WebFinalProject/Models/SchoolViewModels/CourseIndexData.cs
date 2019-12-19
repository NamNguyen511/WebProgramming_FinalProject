using System;
using System.Collections;
using System.Collections.Generic;

namespace WebFinalProject.Models.SchoolViewModels
{
    public class CourseIndexData
    {
        public ICollection<Course> Courses { get; set; }
        public ICollection<Instructor> Instructors { get; set; }
    }
}
