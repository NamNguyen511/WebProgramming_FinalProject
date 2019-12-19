using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFinalProject.Data;
using WebFinalProject.Models;
using WebFinalProject.Models.SchoolViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebFinalProject.Controllers
{
    public class InstructorController : Controller
    {
        // GET: /<controller>/
        private readonly UTEUniversityContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        public InstructorController(UTEUniversityContext context,SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _context = context;
           
        }
        public async Task<IActionResult> Index(int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = await _context.Instructors
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Enrollments)
                            .ThenInclude(i => i.Student)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Department)
                .AsNoTracking()
                .OrderBy(o => o.LastName)
                .ToListAsync();

            if(id != null)
            {
                ViewData["InstructorID"] = id.Value;
                Instructor instructor = viewModel.Instructors.Where(
                    i => i.InstructorID == id.Value).Single();

                viewModel.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }
            if(courseID != null)
            {
                ViewData["CourseID"] = courseID.Value;
                /*var selectedCourse = viewModel.Courses.Where(x => x.CourseID == courseID.Value).Single();
                await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync();
                foreach(Enrollment enrollment in selectedCourse.Enrollments)
                {
                    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                }
                viewModel.Enrollments = selectedCourse.Enrollments;*/
                viewModel.Enrollments = viewModel.Courses.Where(
            x => x.CourseID == courseID).Single().Enrollments;
            }
            return View(viewModel);
        }
        //Method: PopulateAssignedCourseData
        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _context.Courses;
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(s => s.CourseID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    IsSelected = instructorCourses.Contains(course.CourseID)
                });
            }
            ViewData["Courses"] = viewModel;
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var instructors = await _context.Instructors
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.InstructorID == id);
            if(instructors == null)
            {
                return NotFound();
            }
            PopulateAssignedCourseData(instructors);
            return View(instructors);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id,string [] selectedCourse)
        {
            if (id == null)
            {
                return NotFound();
            }
            var instructorsUpdate = await _context.Instructors
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                .FirstOrDefaultAsync(s => s.InstructorID == id);
            if (await TryUpdateModelAsync<Instructor>(
                instructorsUpdate, " ", s => s.FirstName, s => s.LastName, s => s.HireDate,s => s.CourseAssignments))
            {
                UpdateInstructorCourses(selectedCourse, instructorsUpdate);

                try
                {

                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to edit changes" + "Please contact to  admin");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateInstructorCourses(selectedCourse, instructorsUpdate);
            PopulateAssignedCourseData(instructorsUpdate);
            return View(instructorsUpdate);
        }
        //Method : UpdateInstructorCourses
        private void UpdateInstructorCourses(string[] selectedCourse,Instructor instructorsUpdate)
        {
            if(selectedCourse == null)
            {
                instructorsUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }
            var selectedCourseHS = new HashSet<string>(selectedCourse);
            var instructorsCourse = new HashSet<int>(instructorsUpdate.CourseAssignments.Select(s => s.Course.CourseID));
            foreach(var course in _context.Courses)
            {
                if(selectedCourseHS.Contains(course.CourseID.ToString()))
                {
                    if(!instructorsCourse.Contains(course.CourseID))
                    {
                        instructorsUpdate.CourseAssignments.Add(
                            new CourseAssignment
                            {
                                InstructorID = instructorsUpdate.InstructorID,
                                CourseID = course.CourseID
                            });
                    }
                    else
                    {
                        if(instructorsCourse.Contains(course.CourseID))
                        {
                            CourseAssignment courseRemove = instructorsUpdate
                                .CourseAssignments.FirstOrDefault(i => i.CourseID == course.CourseID);
                            _context.Remove(courseRemove);
                        }
                    }
                }
            }
        }
        //Method : GET-POST Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var instructor = await _context.Instructors.AsNoTracking().FirstOrDefaultAsync(s => s.InstructorID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deleted(int? id)
        {
            Instructor instructor = await _context.Instructors
                .Include(i => i.CourseAssignments)
                .SingleAsync(i => i.InstructorID == id);
            var departments = await _context.Departments
                .Where(d => d.InstructorID == id)
                .ToListAsync();
            departments.ForEach(d => d.InstructorID = null);

            _context.Instructors.Remove(instructor);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        //Method : GET-POST Create
        [HttpGet]
        public IActionResult Create()
        {
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>();
            PopulateAssignedCourseData(instructor);

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName",  "LastName", "HireDate")] Instructor instructor,string[] selectedCourse)
        {
            if(selectedCourse != null)
            {
                instructor.CourseAssignments = new List<CourseAssignment>();
                foreach(var course in selectedCourse)
                {
                    var courseAdd = new CourseAssignment{
                        CourseID = int.Parse(course),
                        InstructorID = instructor.InstructorID
                    };
                    instructor.CourseAssignments.Add(courseAdd);
                }
            }
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(instructor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save change" + "Please contact to admin");
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }
        //Method : Instructor Details
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var instructor = await _context.Instructors.FirstOrDefaultAsync(s => s.InstructorID == id);
           var instructors = await _context.Instructors
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Department)
                .AsNoTracking().
                FirstOrDefaultAsync(s => s.InstructorID == id);
            if (instructors == null || instructor == null)
            {
                return NotFound();
            }
            if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
            {
                return View(instructors);
            }
            ViewBag.Message = "You need to login as Admin to see which course the instructors teaches";
            return View(instructor);
            
            
            

           

        }
    }
    
}
