using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFinalProject.Data;
using WebFinalProject.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebFinalProject.Controllers
{
    public class CourseController : Controller
    {
        // GET: /<controller>/
        private readonly UTEUniversityContext _context;
        public CourseController(UTEUniversityContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var courses = _context.Courses
                .Include(c => c.Department)
                .AsNoTracking();
            return View( await courses.ToListAsync());
        }
        private void PopulateDepartmentsDropdownList(object selectedDepartment = null)
        {
            var departmentQuery = from d in _context.Departments
                                  orderby d.Name
                                  select d;
            ViewBag.DepartmentID = new SelectList(departmentQuery.AsNoTracking(), "DepartmentID", "Name", selectedDepartment);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }
            PopulateDepartmentsDropdownList(course.DepartmentID);
            return View(course);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var courseUpdate = await _context.Courses.FirstOrDefaultAsync(s => s.CourseID == id);
            if (await TryUpdateModelAsync<Course>(
                courseUpdate, " " ,s => s.Title, s => s.DepartmentID ,s => s.Credits))
            {
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
            PopulateDepartmentsDropdownList(courseUpdate.DepartmentID);
            return View(courseUpdate);
        }
        [HttpGet]
        public IActionResult Create()
        {
            PopulateDepartmentsDropdownList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("CourseID","Credits","DepartmentID","Title")]Course course)
        {
            if(ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDepartmentsDropdownList(course.DepartmentID);
            return View(course);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseID == id);

            if(course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if(course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var course = await _context.Courses.FindAsync(id);
            if(course == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Remove(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException)
            {
                return RedirectToAction(nameof(Delete));
            }
        }
    }
}
