using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebFinalProject.Models;

namespace WebFinalProject.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            //context.Database.EnsureCreated();
            using (var context = new UTEUniversityContext(serviceProvider.GetRequiredService<DbContextOptions<UTEUniversityContext>>()))
            {
                // Look for any students.
                if (context.Students.Any())
                {
                    return;   // DB has been seeded
                }

                var students = new Student[]
                {
                new Student { FirstName = "Tuan Anh",   LastName = "Vu",Birthday = DateTime.Parse("1999-01-01"),
                    EnrollmentDate = DateTime.Parse("2010-09-01") },
                new Student { FirstName = "Tuan Anh", LastName = "Bui",Birthday = DateTime.Parse("1999-01-02"),
                    EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstName = "Dang Khoa",   LastName = "Quach Nguyen",Birthday = DateTime.Parse("1999-01-03"),
                    EnrollmentDate = DateTime.Parse("2013-09-01") },
                new Student { FirstName = "Tuan Anh ",    LastName = "Nguyen",Birthday = DateTime.Parse("1999-01-03"),
                    EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstName = "Anh Duy",      LastName = "Mai Ngoc",Birthday = DateTime.Parse("1999-01-04"),
                    EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstName = "Nam",    LastName = "Nguyen Xuan",Birthday = DateTime.Parse("1999-01-05"),
                    EnrollmentDate = DateTime.Parse("2011-09-01") },
                new Student { FirstName = "Hoai Thuong",    LastName = "Nguyen Khac",Birthday = DateTime.Parse("1999-01-04"),
                    EnrollmentDate = DateTime.Parse("2013-09-01") },
                new Student { FirstName = "Thuy",     LastName = "Dinh Quoc",Birthday = DateTime.Parse("1999-01-05"),
                    EnrollmentDate = DateTime.Parse("2005-09-01") }
                };

                foreach (Student s in students)
                {
                    context.Students.AddRange(s);
                }
                context.SaveChanges();

                var instructors = new Instructor[]
                {
                new Instructor{FirstName ="Thi Van", LastName = "Nguyen Tran",
                    HireDate = DateTime.Parse("2001-11-09")},
                new Instructor{FirstName = "Quyen", LastName = "Nguyen Van",
                    HireDate = DateTime.Parse("1995-01-01")},
                new Instructor{FirstName = "Bao", LastName = "Nguyen Thien",
                    HireDate = DateTime.Parse("2002-05-03")},
                new Instructor{FirstName = "Quang", LastName ="Nguyen Dang",
                    HireDate = DateTime.Parse("1999-02-02")},
                };
                foreach (Instructor i in instructors)
                {
                    context.Instructors.Add(i);
                }
                context.SaveChanges();

                var departments = new Department[]
                {
                new Department{Name = "IT", Budget = 350000,
                StartDate = DateTime.Parse("2002-02-01"),
                InstructorID = instructors.Single(i => i.FirstName == "Thi Van").InstructorID},

                new Department{Name = " English", Budget = 100000,
                StartDate = DateTime.Parse("1995-02-02"),
                InstructorID = instructors.Single(i => i.FirstName == "Quyen").InstructorID},

                new Department{Name = "Mathematics", Budget = 120000,
                StartDate = DateTime.Parse("2002-05-26"),
                InstructorID = instructors.Single(i => i.FirstName == "Bao").InstructorID},

                new Department { Name = "Network", Budget = 200000,
                StartDate = DateTime.Parse("1999-02-17"),
                InstructorID = instructors.Single(i => i.FirstName == "Quang").InstructorID},
                };
                foreach (Department d in departments)
                {
                    context.Departments.Add(d);
                }
                context.SaveChanges();

                var courses = new Course[]
                {
                new Course {Title = "Information Security", Credits = 3,
                DepartmentID = departments.Single(s => s.Name == "IT").DepartmentID},

                new Course {Title ="Technical English", Credits = 2,
                DepartmentID = departments.Single(s => s.Name == "English").DepartmentID},

                new Course { Title = "Calculus 3", Credits = 3,
                DepartmentID = departments.Single(s => s.Name == "Mathematics").DepartmentID},

                new Course { Title = "Network Essential",Credits = 3,
                DepartmentID = departments.Single(s => s.Name == "Network").DepartmentID},

                new Course{ Title = "Literature", Credits = 2,
                DepartmentID = departments.Single(s => s.Name == "English").DepartmentID},

                new Course{ Title = "Calculus 2", Credits = 3,
                DepartmentID = departments.Single(s => s.Name == "Mathematics").DepartmentID}
                };
                foreach (Course c in courses)
                {
                    context.Courses.Add(c);
                }
                context.SaveChanges();

                var courseInstructors = new CourseAssignment[]
                {
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "Calculus 3").CourseID,
                    InstructorID = instructors.Single(i => i.FirstName == "Bao").InstructorID
                },

                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "Literature").CourseID,
                    InstructorID = instructors.Single(i => i.FirstName == "Quyen").InstructorID
                },

                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "Network Essential").CourseID,
                    InstructorID = instructors.Single(i => i.FirstName == "Quang").InstructorID
                },

                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "Calculus 2").CourseID,
                    InstructorID = instructors.Single(i => i.FirstName == "Bao").InstructorID
                },

                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "Information Security").CourseID,
                    InstructorID = instructors.Single(i => i.FirstName == "Thi Van").InstructorID
                },

                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "Technical English").CourseID,
                    InstructorID = instructors.Single(i => i.FirstName == "Quyen").InstructorID
                }

                };
                foreach (CourseAssignment ca in courseInstructors)
                {
                    context.CourseAssignments.Add(ca);
                }
                context.SaveChanges();

                var enrollments = new Enrollment[]
                {
                new Enrollment
                {
                    StudentID = students.Single(s => s.FirstName == "Tuan Anh" && s.LastName == "Bui").StudentID,
                    CourseID = courses.Single(c => c.Title == "Information Security").CourseID,
                    Grade = 10
                },

                new Enrollment
                {
                    StudentID = students.Single(s => s.FirstName == "Tuan Anh" && s.LastName == "Vu").StudentID,
                    CourseID = courses.Single(c => c.Title == "Calculus 3").CourseID,
                    Grade = 8
                },

                new Enrollment
                {
                    StudentID = students.Single(s => s.FirstName == "Dang Khoa" && s.LastName == "Quach Nguyen").StudentID,
                    CourseID = courses.Single(c => c.Title == "Technical English").CourseID,
                    Grade = 7

                },

                new Enrollment
                {
                    StudentID = students.Single(s => s.FirstName == "Tuan Anh" && s.LastName =="Nguyen").StudentID,
                    CourseID = courses.Single(c => c.Title == "Calculus 2").CourseID,
                    Grade = 9
                },

                new Enrollment
                {
                    StudentID = students.Single(s => s.FirstName == "Nam").StudentID,
                    CourseID = courses.Single(c => c.Title == "Calculus 3").CourseID,
                    Grade = 8
                },

                 new Enrollment
                {
                    StudentID = students.Single(s => s.FirstName == "Hoai Thuong").StudentID,
                    CourseID = courses.Single(c => c.Title == "Literature").CourseID,
                    Grade = 7
                },

                  new Enrollment
                {
                    StudentID = students.Single(s => s.FirstName == "Thuy").StudentID,
                    CourseID = courses.Single(c => c.Title == "Network Essential").CourseID,
                    Grade = 7
                },

                   new Enrollment
                {
                    StudentID = students.Single(s => s.FirstName == "Anh Duy").StudentID,
                    CourseID = courses.Single(c => c.Title == "Technical English").CourseID,
                    Grade = 8
                },
                };
                foreach (Enrollment e in enrollments)
                {
                    var EnrollmentInDatabase = context.Enrollments.Where
                        (s => s.Student.StudentID == e.StudentID &&
                        s.Course.CourseID == e.CourseID).SingleOrDefault();
                    if (EnrollmentInDatabase == null)
                    {
                        context.Enrollments.Add(e);
                    }
                }
                context.SaveChanges();

            }
        }
    }
}
